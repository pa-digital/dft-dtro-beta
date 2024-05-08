resource "google_redis_instance" "redis_cache" {
  count = var.feature_enable_redis_cache ? 1 : 0

  name   = "${var.application_name}-memory-cache"
  region = var.region

  tier           = "BASIC" # Single instance for prototype configuration
  memory_size_gb = var.redis_memory_size
  replica_count  = 0

  connect_mode       = "PRIVATE_SERVICE_ACCESS"
  authorized_network = module.network.network_id

  redis_version = "REDIS_7_0"
  display_name  = "${var.application_name} Redis Memory Cache"

  auth_enabled            = true
  transit_encryption_mode = "SERVER_AUTHENTICATION"

  maintenance_policy {
    weekly_maintenance_window {
      day = "SUNDAY"

      start_time {
        hours   = 2
        minutes = 0
      }
    }
  }
}