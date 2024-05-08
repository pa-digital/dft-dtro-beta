# Service availability – uptime check
resource "google_monitoring_uptime_check_config" "uptime_check" {
  display_name = "${var.service_name}-uptime-check"
  timeout      = "30s"

  period = "60s"

  content_matchers {
    content = "Healthy"
  }

  selected_regions = ["EUROPE", "USA", "SOUTH_AMERICA"]

  http_check {
    path         = "/health"
    port         = 443
    use_ssl      = true
    validate_ssl = true

    headers = {
      (var.uptime_check_credentials.header_name) = var.uptime_check_credentials.header_value
    }

    mask_headers = false

    accepted_response_status_codes {
      status_class = "STATUS_CLASS_2XX"
    }
  }

  monitored_resource {
    type = "uptime_url"
    labels = {
      project_id = var.project
      host       = var.service_url
    }
  }
}

# Service availability – uptime alerts
resource "google_monitoring_alert_policy" "uptime_alert" {
  display_name = "${var.service_name} Uptime Alert"

  notification_channels = var.notification_channels

  combiner = "OR"
  conditions {
    display_name = "${var.service_name} Uptime Health Check Failing"

    condition_threshold {
      filter = <<-EOT
        resource.type = "uptime_url" AND
        metric.type = "monitoring.googleapis.com/uptime_check/check_passed" AND
        metric.labels.check_id = "${google_monitoring_uptime_check_config.uptime_check.uptime_check_id}"
      EOT

      duration        = "0s"
      threshold_value = 1
      comparison      = "COMPARISON_GT"
      aggregations {
        alignment_period   = "1200s"
        per_series_aligner = "ALIGN_NEXT_OLDER"
        group_by_fields = [
          "resource.label.project_id",
          "resource.label.host",
        ]
        cross_series_reducer = "REDUCE_COUNT_FALSE"
      }

      trigger {
        count   = 1
        percent = 0
      }
    }
  }
}

# Application errors – alerts
resource "google_monitoring_alert_policy" "app_errors_alert" {
  display_name = "${var.service_name} App Errors Alert"

  notification_channels = var.notification_channels

  combiner = "OR"
  conditions {
    display_name = "Over 5 \"50x\" responses within 5 minutes"

    condition_threshold {
      filter = <<-EOT
        resource.type = "cloud_run_revision" AND
        resource.labels.service_name = "${var.service_name}"
        AND metric.type = "run.googleapis.com/request_count"
        AND metric.labels.response_code_class = "5xx"
      EOT

      duration        = "0s"
      threshold_value = 5
      comparison      = "COMPARISON_GT"
      aggregations {
        alignment_period   = "300s"
        per_series_aligner = "ALIGN_SUM"
        group_by_fields = [
          "resource.label.service_name",
        ]
        cross_series_reducer = "REDUCE_SUM"
      }

      trigger {
        count   = 1
        percent = 0
      }
    }
  }
}
