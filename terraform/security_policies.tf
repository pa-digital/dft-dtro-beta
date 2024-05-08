resource "random_password" "uptime_check_token" {
  length = 16
}

locals {
  uptime_check_token_header = "x-uptime-check-token"
  chunked_allowed_ips       = chunklist(distinct(var.allowed_ips), 10)
}

resource "google_compute_security_policy" "security_policy" {
  name = "${var.application_name}-security"

  # Ignore IP allow lists after first deployment. This is required as IPs have been considered sensitive so must be
  # manually added to the NEG security policy.
  lifecycle {
    ignore_changes = [
      rule,
    ]
  }

  dynamic "rule" {
    for_each = local.chunked_allowed_ips

    content {
      action   = "allow"
      priority = 1000 + rule.key
      match {
        versioned_expr = "SRC_IPS_V1"
        config {
          src_ip_ranges = rule.value
        }
      }
      description = "Allow access to authorized IPs only (chunk ${rule.key + 1} of ${length(local.chunked_allowed_ips)})"
    }
  }

  rule {
    action   = "allow"
    priority = "1100"
    match {
      expr {
        expression = <<-EOT
        request.path == '/health'
          && has(request.headers['${local.uptime_check_token_header}'])
          && request.headers['${local.uptime_check_token_header}'] == '${random_password.uptime_check_token.result}'
        EOT
      }
    }
    description = "Allow access to uptime checker"
  }

  rule {
    action   = "deny(403)"
    priority = "2147483647"
    match {
      versioned_expr = "SRC_IPS_V1"
      config {
        src_ip_ranges = ["*"]
      }
    }
    description = "default deny rule"
  }
}
