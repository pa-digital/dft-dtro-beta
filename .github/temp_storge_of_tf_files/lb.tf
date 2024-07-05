resource "google_compute_global_address" "service_lb_ip" {
  name = "${var.application_name}-prototype-lb-ip"

  lifecycle {
    prevent_destroy = true
  }
}

resource "google_compute_url_map" "default" {
  name            = "${var.application_name}-url-map"
  description     = "Routes traffic to services"
  default_service = module.loadbalancer.backend_services["publish"].id

  host_rule {
    hosts        = [var.search_service_domain]
    path_matcher = "search"
  }

  path_matcher {
    name            = "search"
    default_service = module.loadbalancer.backend_services["search"].id
  }

  test {
    host    = var.publish_service_domain
    path    = "/health"
    service = module.loadbalancer.backend_services["publish"].id
  }

  test {
    host    = var.search_service_domain
    path    = "/health"
    service = module.loadbalancer.backend_services["search"].id
  }
}

resource "google_compute_ssl_policy" "default" {
  name            = "${var.application_name}-ssl-policy"
  profile         = "MODERN"
  min_tls_version = "TLS_1_2"
}

module "loadbalancer" {
  source  = "GoogleCloudPlatform/lb-http/google//modules/serverless_negs"
  version = "9.0.0"
  name    = "${var.application_name}-services"
  project = var.project

  address        = google_compute_global_address.service_lb_ip.address
  create_address = false

  ssl                             = true
  ssl_policy                      = google_compute_ssl_policy.default.id
  https_redirect                  = true
  managed_ssl_certificate_domains = [var.publish_service_domain, var.search_service_domain]

  create_url_map = false
  url_map        = google_compute_url_map.default.id

  backends = {
    publish = {
      description = "${var.publish_service_image} service"

      groups = [
        {
          group = google_compute_region_network_endpoint_group.publish_service_serverless_neg.id
        }
      ]

      enable_cdn              = false
      protocol                = null
      port_name               = null
      security_policy         = null
      custom_request_headers  = null
      custom_response_headers = null
      compression_mode        = null
      security_policy         = google_compute_security_policy.security_policy.id
      edge_security_policy    = null

      iap_config = {
        enable               = false
        oauth2_client_id     = ""
        oauth2_client_secret = ""
      }

      log_config = {
        enable      = false
        sample_rate = null
      }
    }

    search = {
      description = "${var.search_service_image} service"

      groups = [
        {
          group = google_compute_region_network_endpoint_group.search_service_serverless_neg.id
        }
      ]

      enable_cdn              = false
      protocol                = null
      port_name               = null
      security_policy         = null
      custom_request_headers  = null
      custom_response_headers = null
      compression_mode        = null
      security_policy         = google_compute_security_policy.security_policy.id
      edge_security_policy    = null

      iap_config = {
        enable               = false
        oauth2_client_id     = ""
        oauth2_client_secret = ""
      }

      log_config = {
        enable      = false
        sample_rate = null
      }
    }
  }
}
