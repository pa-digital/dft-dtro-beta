# Publish service
resource "google_cloud_run_service_iam_policy" "publish_service_noauth" {
  location = google_cloud_run_v2_service.publish_service.location
  project  = google_cloud_run_v2_service.publish_service.project
  service  = google_cloud_run_v2_service.publish_service.name

  policy_data = data.google_iam_policy.noauth.policy_data
}

resource "google_cloud_run_v2_service" "publish_service" {
  name     = var.publish_service_image
  location = var.region
  ingress  = "INGRESS_TRAFFIC_INTERNAL_LOAD_BALANCER"

  template {
    service_account = google_service_account.cloud_run.email

    scaling {
      min_instance_count = 0
      max_instance_count = local.max_instance_count
    }

    vpc_access {
      connector = google_vpc_access_connector.serverless_connector.id
      egress    = "PRIVATE_RANGES_ONLY"
    }

    containers {
      image = "${var.region}-docker.pkg.dev/${var.project}/dtro/${var.publish_service_image}:${var.tag}"

      dynamic "env" {
        for_each = local.common_service_envs
        content {
          name  = env.key
          value = env.value
        }
      }

      env {
        name = local.db_password_env_name
        value_source {
          secret_key_ref {
            secret  = google_secret_manager_secret.postgres_password.secret_id
            version = "latest"
          }
        }
      }

      dynamic "env" {
        for_each = var.feature_enable_redis_cache ? [1] : []

        content {
          name = local.redis_auth_string_env_name
          value_source {
            secret_key_ref {
              secret  = one(google_secret_manager_secret.redis_auth_string).secret_id
              version = "latest"
            }
          }
        }
      }

      dynamic "volume_mounts" {
        for_each = local.common_secret_files
        content {
          name       = volume_mounts.key
          mount_path = volume_mounts.value.mount_point
        }
      }

      startup_probe {
        period_seconds    = 4
        failure_threshold = 5

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

    dynamic "volumes" {
      for_each = local.common_secret_files
      content {
        name = volumes.key
        secret {
          secret       = volumes.value.secret
          default_mode = 0444
          items {
            version = "latest"
            path    = "value"
            mode    = 0400
          }
        }
      }
    }
  }

  depends_on = [
    null_resource.docker_build,
    # Access to secrets is required to start the container
    google_secret_manager_secret_iam_member.cloud_run_secrets,
  ]
}
