locals {
  cloud_run_service_name = "${local.name_prefix}-${var.dtro_service_image}"
  Service_ui_cloud_run_service_name = "${local.name_prefix}-${var.service_ui_image}"
  # At most `database_max_connections` in total can be opened
  max_instance_count   = floor(var.database_max_connections / var.db_connections_per_cloud_run_instance)
  db_password_env_name = "POSTGRES_PASSWORD"

  dtro_service_envs = merge(
    {
      DEPLOYED               = timestamp()
      PROJECTID              = data.google_project.project.project_id
      EnableRedisCache       = var.feature_enable_redis_cache
      POSTGRES_DB            = "${local.name_prefix}-database"
      POSTGRES_USER          = var.application_name
      POSTGRES_HOST          = var.postgres_host
      POSTGRES_PORT          = var.postgres_port
      POSTGRES_SSL           = var.postgres_use_ssl
      POSTGRES_MAX_POOL_SIZE = var.db_connections_per_cloud_run_instance
  })

  service_ui_envs = merge(
    {
      DEPLOYED       = timestamp()
      PROJECTID      = data.google_project.project.project_id
      BASE_URL       = var.dtro_api_url[var.environment]
      VERSION_URL    = var.dtro_version
      TOKEN_ENDPOINT = var.oauth_gen_url[var.environment]
      CLIENT_ID      = var.cloud_run_service_ui_client_id
      CLIENT_SECRET  = var.cloud_run_service_ui_client_secret
    })

  project_id             = data.google_project.project.project_id
}

resource "google_cloud_run_v2_service" "dtro_service" {
  name     = local.cloud_run_service_name
  location = var.region
  ingress  = "INGRESS_TRAFFIC_INTERNAL_ONLY"

  template {
    service_account = var.execution_service_account

    scaling {
      min_instance_count = 1
      max_instance_count = local.max_instance_count
    }

    volumes {
      name = "cloudsql"
      cloud_sql_instance {
        instances = [data.google_sql_database_instance.postgres_db.connection_name]
      }
    }

    vpc_access {
      connector = data.google_vpc_access_connector.serverless_connector.id
      egress    = "PRIVATE_RANGES_ONLY"
    }

    containers {
      image = "${var.artifact_registry_dtro_image_path}:${var.dtro_tag}"
      ports {
        container_port = 8080
      }

      dynamic "env" {
        for_each = local.dtro_service_envs
        content {
          name  = env.key
          value = env.value
        }
      }

      env {
        name = local.db_password_env_name
        value_source {
          secret_key_ref {
            secret  = data.google_secret_manager_secret_version.postgres_password_value.secret
            version = "latest"
          }
        }
      }

      startup_probe {
        timeout_seconds   = 3
        period_seconds    = 15
        failure_threshold = 10
        http_get {
          path = "/health"
          port = 8080
        }
      }

      liveness_probe {
        http_get {
          path = "/health"
          port = 8080
        }
      }
    }

    containers {
      name  = "cloud-sql-proxy"
      image = "gcr.io/cloud-sql-connectors/cloud-sql-proxy:latest"
      args  = ["--private-ip", data.google_sql_database_instance.postgres_db.connection_name]
    }
  }
}

resource "google_cloud_run_v2_service" "service_portal_service" {
  name     = local.Service_ui_cloud_run_service_name
  location = var.region
  ingress  = "INGRESS_TRAFFIC_INTERNAL_LOAD_BALANCER"

  template {
    service_account = var.execution_service_account

    scaling {
      min_instance_count = 1
      max_instance_count = 1
    }

    containers {
      image = "${var.artifact_registry_service_ui_image_path}:${var.service_ui_tag}"
      ports {
        container_port = 8080
      }

      dynamic "env" {
        for_each = local.service_ui_envs
        content {
          name  = env.key
          value = env.value
        }
      }

      startup_probe {
        timeout_seconds   = 3
        period_seconds    = 15
        failure_threshold = 10
        http_get {
          path = "/health"
          port = 8080
        }
      }

      liveness_probe {
        http_get {
          path = "/health"
          port = 8080
        }
      }
    }
  }
}
