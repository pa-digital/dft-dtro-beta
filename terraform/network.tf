locals {
  serverless_subnet_name = "serverless-subnet"
}

module "network" {
  source  = "terraform-google-modules/network/google"
  version = "7.1.0"

  project_id   = var.project
  network_name = "${var.application_name}-network"

  subnets = [
    {
      subnet_name   = local.serverless_subnet_name
      subnet_ip     = var.serverless_connector_ip_range
      subnet_region = var.region
    }
  ]
}

module "cloudsql_private_service_access" {
  source  = "GoogleCloudPlatform/sql-db/google//modules/private_service_access"
  version = "15.1.0"

  project_id  = var.project
  vpc_network = module.network.network_name

  depends_on = [module.network]
}

resource "google_vpc_access_connector" "serverless_connector" {
  name    = "default-connector"
  project = var.project
  region  = var.region

  subnet {
    project_id = var.project
    name       = module.network.subnets["${var.region}/${local.serverless_subnet_name}"].name
  }

  machine_type  = var.serverless_connector_config.machine_type
  min_instances = var.serverless_connector_config.min_instances
  max_instances = var.serverless_connector_config.max_instances
}

resource "google_compute_region_network_endpoint_group" "publish_service_serverless_neg" {
  name                  = "${var.publish_service_image}-serverless-neg"
  network_endpoint_type = "SERVERLESS"
  region                = var.region

  cloud_run {
    service = var.publish_service_image
  }
}

resource "google_compute_region_network_endpoint_group" "search_service_serverless_neg" {
  name                  = "${var.search_service_image}-serverless-neg"
  network_endpoint_type = "SERVERLESS"
  region                = var.region

  cloud_run {
    service = var.search_service_image
  }
}
