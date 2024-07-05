terraform {
  required_version = ">= 1.4.6"

  required_providers {
    google = "~> 4.65.2"
  }

  backend "gcs" {
    prefix = "terraform/state"
  }
}
