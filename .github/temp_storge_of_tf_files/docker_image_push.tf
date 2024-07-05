resource "null_resource" "docker_build" {

  triggers = {
    always_run = timestamp()
  }

  provisioner "local-exec" {
    working_dir = ".."
    command     = "make docker-build-push"

    environment = {
      REGISTRY_URL = "${var.region}-docker.pkg.dev/${var.project}/${var.repository_id}"
    }
  }

  depends_on = [
    google_artifact_registry_repository.artifact_repository
  ]
}
