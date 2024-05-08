resource "google_bigquery_connection" "postgres_database_connection" {
  connection_id = "${var.application_name}-database"
  friendly_name = "${var.application_name}-database"
  description   = "Access to ${var.application_name} Postgres database"
  location      = var.region

  cloud_sql {
    instance_id = module.postgres_db.instance_connection_name
    database    = local.database_name
    type        = "POSTGRES"
    credential {
      username = var.application_name
      password = random_password.postgres_generated_password.result
    }
  }
}

resource "google_project_iam_member" "bigquery_allow_cloudsql_connection" {
  project = var.project
  role    = "roles/cloudsql.client"
  member  = "serviceAccount:${google_bigquery_connection.postgres_database_connection.cloud_sql[0].service_account_id}"
}
