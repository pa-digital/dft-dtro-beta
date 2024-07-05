resource "google_project_service" "firestore" {
  project = var.project
  service = "firestore.googleapis.com"
}

resource "google_firestore_database" "database" {
  project                     = var.project
  name                        = "(default)"
  location_id                 = var.firestore_region
  type                        = "FIRESTORE_NATIVE"
  concurrency_mode            = "PESSIMISTIC"
  app_engine_integration_mode = var.firestore_app_engine_integration_enabled

  depends_on = [google_project_service.firestore]
}
