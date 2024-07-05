output "load_balancer_ip" {
  value = module.loadbalancer.external_ip
}

output "monitoring_dashboard_url" {
  value = "https://console.cloud.google.com/monitoring/dashboards/builder/${split("/", google_monitoring_dashboard.dashboard.id)[3]}"
}

output "publish_service_url" {
  description = "Link to the prototype publish service"
  value       = "https://${var.publish_service_domain}"
}

output "search_service_url" {
  description = "Link to the prototype search service"
  value       = "https://${var.search_service_domain}"
}
