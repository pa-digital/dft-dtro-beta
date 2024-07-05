terraform {
  required_version = "~> 1.9.0"
  required_providers {
    google = {
      source  = "hashicorp/google"
      version = ">= 5.28.0"
    }
  }
}
