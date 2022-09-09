# use azure
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.21.1"
    }
  }
}

# prevent errors on azure
provider "azurerm" {
  features {
  }
}

# create a new resource group
resource "azurerm_resource_group" "arg" {
  name     = "allan-terra-rg"
  location = "australiaeast"
}

# make new app service plan
resource "azurerm_service_plan" "asp" {
  name                = "allanterraasp"
  resource_group_name = azurerm_resource_group.arg.name
  location            = azurerm_resource_group.arg.location
  os_type             = "Linux"
  sku_name            = "B2"
}

# create app service to put the app onto
resource "azurerm_linux_web_app" "apf" {
  name                = "allanterrafrontend"
  resource_group_name = azurerm_resource_group.arg.name
  location            = azurerm_service_plan.asp.location
  service_plan_id     = azurerm_service_plan.asp.id

  site_config {
    always_on = "true"
  }

  app_settings = {
    "DOCKER_REGISTRY_SERVER_URL"      = "https://ghcr.io"
    "DOCKER_REGISTRY_SERVER_USERNAME" = "ccamel55"
    "DOCKER_REGISTRY_SERVER_PASSWORD" = ""
  }

  lifecycle {
    ignore_changes = [
      app_settings["DOCKER_REGISTRY_SERVER_PASSWORD"],
    ]
  }
}

# create app service to put the app onto
resource "azurerm_linux_web_app" "apb" {
  name                = "allanterrabackend"
  resource_group_name = azurerm_resource_group.arg.name
  location            = azurerm_service_plan.asp.location
  service_plan_id     = azurerm_service_plan.asp.id

  site_config {
    always_on = "true"
  }

  app_settings = {
    "DOCKER_REGISTRY_SERVER_URL"      = "https://ghcr.io"
    "DOCKER_REGISTRY_SERVER_USERNAME" = "ccamel55"
    "DOCKER_REGISTRY_SERVER_PASSWORD" = ""
  }

  lifecycle {
    ignore_changes = [
      app_settings["DOCKER_REGISTRY_SERVER_PASSWORD"],
    ]
  }
}

# you still need to manually add your registry server password which is just your github auth token