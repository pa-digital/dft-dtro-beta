variable "region" {
  type        = string
  description = "GCP region to which resources will be deployed."
  default     = "europe-west2"
}

variable "project" {
  type        = string
  description = "GCP project ID to which resources will be deployed."
  default     = "pa-dft-dtro-sandbox"
}

variable "firestore_region" {
  type        = string
  description = "GCP region to which Firestore will be deployed."
  default     = "eur3"
}

variable "firestore_app_engine_integration_enabled" {
  type        = string
  description = "Controls whether app engine integration is enabled for firestore"
  default     = "ENABLED"
}

variable "repository_id" {
  type        = string
  description = "Repository name for artifact repository."
  default     = "dtro"
}

variable "application_name" {
  type        = string
  description = "The name the application."
  default     = "dtro"
}

variable "publish_service_image" {
  type        = string
  description = "The name of an image being pushed for publish service."
  default     = "dtro-prototype-publish"
}

variable "search_service_image" {
  type        = string
  description = "The name of an image being pushed for search service."
  default     = "dtro-prototype-search"
}

variable "db_connections_per_cloud_run_instance" {
  type        = number
  default     = 2
  description = "Maximum size of DB connection pool for each Cloud Run instance"
}

variable "tag" {
  type        = string
  description = "The tag of the image to run."
  default     = "latest"
}

variable "allowed_ips" {
  description = "IPs permitted to access the prototype"
  type        = list(any)
  default     = []
}

variable "publish_service_domain" {
  type        = string
  description = "Name of the domain where the prototype is published"
  default     = "dtro-alpha-publishing-prototype.dft.gov.uk"
}

variable "search_service_domain" {
  type        = string
  description = "Name of the domain where the prototype is published"
  default     = "dtro-alpha-access-prototype.dft.gov.uk"
}

variable "logs_retention_in_days" {
  type        = number
  description = "Retention time of the application logs"
  default     = 180
}

variable "emails_to_notify" {
  type        = set(string)
  description = "Emails to notify about infrastructure issues"
}

variable "feature_write_to_bucket" {
  type        = bool
  description = "Feature flag, when enabled data is written to Cloud Storage bucket"
}

variable "feature_enable_redis_cache" {
  type        = bool
  description = "Feature flag, when enabled MemoryStore (Redis) cache instance is configured and used by the app"
  default     = false
}

variable "database_zone" {
  type        = string
  description = "Primary zone for the Postgres database"
  default     = "europe-west2-a"
}

variable "database_availability_type" {
  type        = string
  description = "Availability of Postgres database instance"
  default     = "ZONAL"
}

variable "database_instance_type" {
  type        = string
  description = "Type of Postgres database instance"
  default     = "db-f1-micro"
}

variable "database_disk_initial_size" {
  type        = string
  description = "Initial size of the Postgres databases disk"
  default     = 10
}

variable "database_disk_autoresize_limit" {
  type        = string
  description = "Upper limit for Postgres database disk auto resize"
  default     = 30
}

variable "database_max_connections" {
  type        = number
  description = "Maximum number of connections allowed by the Postgres database"
  # 25 is Cloud SQL's default value for tiny instance (https://cloud.google.com/sql/docs/postgres/flags#postgres-m)
  default = 25
}

variable "database_backups_pitr_enabled" {
  type        = bool
  description = "Enables point-in-time recovery for Postgres database"
  default     = true
}

variable "database_backups_pitr_days" {
  type        = string
  description = "Retention policy that determines how many days of transaction logs are stored for point-in-time recovery"
  default     = 2
}

variable "database_backups_number_of_stored_backups" {
  type        = string
  description = "Retention policy that determines how many daily backups of Postgres database are stored"
  default     = 14
}

variable "serverless_connector_config" {
  type = object({
    machine_type  = string
    min_instances = number
    max_instances = number
  })
  description = "Configuration of Serverless VPC Access connector"
  default = {
    machine_type  = "e2-micro"
    min_instances = 2
    max_instances = 3
  }

  validation {
    condition     = var.serverless_connector_config.min_instances > 1 && var.serverless_connector_config.max_instances > var.serverless_connector_config.min_instances
    error_message = "At least 2 instances must be configured and max instances count must be greater than min instances count."
  }
}

variable "serverless_connector_ip_range" {
  type        = string
  description = "IP range for Serverless VPC Access Connector"
  default     = "10.64.0.0/28" # CIDR block with "/28" netmask is required
}

variable "redis_memory_size" {
  type        = string
  description = "Redis memory size in GiB"
  default     = 1
}