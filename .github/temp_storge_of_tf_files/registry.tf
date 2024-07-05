resource "google_artifact_registry_repository" "artifact_repository" {
  location      = var.region
  repository_id = var.repository_id
  description   = "Repository for housing prototype images"
  format        = "DOCKER"
}