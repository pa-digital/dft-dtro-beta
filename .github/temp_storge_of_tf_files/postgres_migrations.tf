resource "google_cloud_run_v2_job" "postgres_migrations" {
  name     = "${var.application_name}-postgres-migrations"
  location = var.region

  template {
    task_count  = 1
    parallelism = 1

    template {
      service_account = google_service_account.cloud_run.email

      vpc_access {
        connector = google_vpc_access_connector.serverless_connector.id
        egress    = "PRIVATE_RANGES_ONLY"
      }

      max_retries = 1

      containers {
        image = "${var.region}-docker.pkg.dev/${var.project}/dtro/dtro-prototype-postgres-migrations-job:${var.tag}"

        resources {
          limits = {
            cpu    = "2000m"
            memory = "2Gi"
          }
        }

        dynamic "env" {
          for_each = local.db_connection_envs
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

        dynamic "volume_mounts" {
          for_each = local.db_connection_secret_files
          content {
            name       = volume_mounts.key
            mount_path = volume_mounts.value.mount_point
          }
        }
      }

      dynamic "volumes" {
        for_each = local.db_connection_secret_files
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
  }

  depends_on = [
    null_resource.docker_build
  ]
}