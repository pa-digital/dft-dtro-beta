resource "google_project_iam_audit_config" "audit_logs" {
  project = var.project
  service = "allServices"

  audit_log_config {
    log_type = "ADMIN_READ"
  }

  audit_log_config {
    log_type = "DATA_READ"
  }

  audit_log_config {
    log_type = "DATA_WRITE"
  }
}

resource "google_logging_project_bucket_config" "default" {
  location       = "global"
  project        = var.project
  bucket_id      = "_Default"
  retention_days = var.logs_retention_in_days
}

resource "google_monitoring_notification_channel" "infrastructure_alerts_receivers" {
  for_each = var.emails_to_notify

  display_name = split("@", each.value)[0]
  type         = "email"
  labels = {
    email_address = each.value
  }
  force_delete = true
}

locals {
  notification_receivers = [
    for receiver in google_monitoring_notification_channel.infrastructure_alerts_receivers : receiver.name
  ]

  services_with_alerting = zipmap(
    [var.publish_service_image, var.search_service_image],
    [var.publish_service_domain, var.search_service_domain]
  )
}

# Alerting
module "publish_service_alerting" {
  for_each = local.services_with_alerting
  source   = "moduleslerting"

  project      = var.project
  service_name = each.key
  service_url  = each.value
  uptime_check_credentials = {
    header_name  = local.uptime_check_token_header
    header_value = random_password.uptime_check_token.result
  }
  notification_channels = local.notification_receivers
}

# Application metrics – DTROs count
resource "google_logging_metric" "created_dtros_count" {
  name        = "${var.publish_service_image}/created-dtros-counter"
  description = "Count of created DTROs reported in application logs"
  bucket_name = google_logging_project_bucket_config.default.name

  filter = <<-EOT
    resource.labels.service_name="${var.publish_service_image}" AND
    jsonPayload.properties.method="dtro.create" AND
    jsonPayload.properties.EventId.Id="201"
  EOT

  label_extractors = {}
}

# Monitoring Dashboard
data "local_file" "dashboard_template" {
  filename = var.feature_enable_redis_cache ? "${path.module}/configuration/monitoring/dashboard-with-memorystore.json" : "${path.module}/configuration/monitoring/dashboard.json"
}

resource "random_id" "dashboard_update_trigger" {
  keepers = {
    dashboard_hash = data.local_file.dashboard_template.id
  }

  byte_length = 4
}

resource "google_monitoring_dashboard" "dashboard" {
  # Suggested workflow to change the dashboard
  # 1. Update the dashboard in GCP
  # 2. Copy JSON and paste to `dashboard.json`
  # 3. Run `terraform apply`
  dashboard_json = data.local_file.dashboard_template.content

  lifecycle {
    # GCP applies sets own properties after updating the dashboard
    ignore_changes = [dashboard_json]

    # Trigger to handle update when dashboard template file is changed
    replace_triggered_by = [random_id.dashboard_update_trigger.id]
  }
}
