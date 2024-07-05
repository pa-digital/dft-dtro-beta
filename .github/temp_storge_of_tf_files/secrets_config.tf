# Postgres
resource "google_secret_manager_secret" "postgres_password" {
  secret_id = "${var.application_name}-postgres-password"

  replication {
    user_managed {
      # Single region for prototype configuration
      replicas {
        location = var.region
      }
    }
  }
}

resource "random_password" "postgres_generated_password" {
  length  = 16
  special = false
}

resource "google_secret_manager_secret_version" "postgres_password_value" {
  secret      = google_secret_manager_secret.postgres_password.id
  secret_data = random_password.postgres_generated_password.result
}

resource "google_secret_manager_secret" "postgres_client_certificate" {
  secret_id = "${var.application_name}-postgres-client-certificate"

  replication {
    user_managed {
      # Single region for prototype configuration
      replicas {
        location = var.region
      }
    }
  }
}

resource "google_secret_manager_secret_version" "postgres_certificate_value" {
  secret      = google_secret_manager_secret.postgres_client_certificate.id
  secret_data = google_sql_ssl_cert.db_client_cert.cert
}

resource "google_secret_manager_secret" "postgres_client_key" {
  secret_id = "${var.application_name}-postgres-client-key"

  replication {
    user_managed {
      # Single region for prototype configuration
      replicas {
        location = var.region
      }
    }
  }
}

resource "google_secret_manager_secret_version" "postgres_key_value" {
  secret      = google_secret_manager_secret.postgres_client_key.id
  secret_data = google_sql_ssl_cert.db_client_cert.private_key
}

# Redis
resource "google_secret_manager_secret" "redis_auth_string" {
  count = var.feature_enable_redis_cache ? 1 : 0

  secret_id = "${var.application_name}-redis-auth-string"

  replication {
    user_managed {
      # Single region for prototype configuration
      replicas {
        location = var.region
      }
    }
  }
}

resource "google_secret_manager_secret_version" "redis_auth_string_value" {
  count = var.feature_enable_redis_cache ? 1 : 0

  secret      = one(google_secret_manager_secret.redis_auth_string).id
  secret_data = one(google_redis_instance.redis_cache).auth_string
}

resource "google_secret_manager_secret" "redis_server_ca" {
  count = var.feature_enable_redis_cache ? 1 : 0

  secret_id = "${var.application_name}-redis-server-ca"

  replication {
    user_managed {
      # Single region for prototype configuration
      replicas {
        location = var.region
      }
    }
  }
}

resource "google_secret_manager_secret_version" "redis_server_ca_value" {
  count = var.feature_enable_redis_cache ? 1 : 0

  secret      = one(google_secret_manager_secret.redis_server_ca).id
  secret_data = element(one(google_redis_instance.redis_cache).server_ca_certs, length(one(google_redis_instance.redis_cache).server_ca_certs) - 1).cert
}