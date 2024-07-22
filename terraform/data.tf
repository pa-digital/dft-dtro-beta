locals {
  name_prefix = "${var.application_name}-${var.environment}"
}

data "google_project" "project" {}

data "google_sql_database_instance" "postgres_db" {
  name = "${local.name_prefix}-postgres"
}

data "google_vpc_access_connector" "serverless_connector" {
  name = "${local.name_prefix}-connector"
}

data "google_secret_manager_secret_version" "postgres_password_value" {
  secret = "${local.name_prefix}-postgres-password"
  version = "latest"
}
