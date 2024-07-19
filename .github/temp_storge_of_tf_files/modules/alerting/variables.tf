# variable "project" {
#   type        = string
#   description = "GCP project ID where alerts will be created,"
# }

variable "service_name" {
  type        = string
  description = "Name of the monitored service."
}

variable "service_url" {
  type        = string
  description = "URL of the monitored service."
}

variable "notification_channels" {
  type        = set(string)
  description = "GCP Notifications Channels that should receive the alerts."
}

variable "uptime_check_credentials" {
  type        = object({ header_name = string, header_value = string })
  description = "Credentials for the uptime checker."
}
