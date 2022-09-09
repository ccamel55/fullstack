# Fullstack calculator
Code for a calculator that is connected to an API. Client is responsible for doing most of the grunt work, results are stored on the backend.

## Why?
This project finally gave me a reason to learn about devops and everything in that realm. Therefore the code for both the frontend and backend is only enough to make the web app functional.

## Devops stuff
Github actions does most of the CI/CD work using `.yml` configs.

- both the frontend and the backend are built into Docker images and then deployed onto thier respective Azure application services.
- Terraform is also run so that Azure can be automatically structured and provisioned whenever we want to change something about our infrastructure. 

## Moving infrastructures 
Because of how everything is setup, getting everything running on a clean Azure environment or even moving to a different infrastructure can be done with relatively low effort.

- The Terraform `main.tf` file will automatically create all the needed resources and set **most** of the settings up for you. **Github personal access token still needs to be uploaded manually.**

Note your github personal access token must have `repo` and `read:packages` enabled. 

### Github Secrets
Alongside the personal access token, if your moving to a new environment you will also need to update 6 of the repo's secrets 

- `AZURE_AD_CLIENT_ID`: used by Terraform, appId from your Terraform service principle

- `AZURE_AD_CLIENT_SECRET`: used by Terraform, password from your Terraform service principle

- `AZURE_AD_TENANT_ID`: used by Terraform, tenant from your Terraform service principle

- `AZURE_SUBSCRIPTION_ID`: used by Terraform, id from your Azure subscription

- `AZURE_WEBAPP_PUBLISH_PROFILE_BACKEND`: publish profile of the frontend app service, allows up to deploy our docker image to the app service.

- `AZURE_WEBAPP_PUBLISH_PROFILE_FRONTEND`: same thing as above, but this time for the backend app service.
