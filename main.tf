# Define o fornecedor de nuvem (AWS) e a região.
provider "aws" {
  region = "us-east-2"
}

# Repositório para a imagem do UserService
resource "aws_ecr_repository" "user_service_repo" {
  name = "user-service"
}

# Repositório para a imagem do TaskService
resource "aws_ecr_repository" "task_service_repo" {
  name = "task-service"
}

# Repositório para a imagem do ApiGateway
resource "aws_ecr_repository" "api_gateway_repo" {
  name = "api-gateway"
}


# # Recurso que queremos criar.
# resource "aws_sqs_queue" "task_manager_queue" {
#   # O nome da nossa fila de mensagens.
#   name = "task-manager-events-prod"

#   tags = {
#     Environment = "production"
#     Project     = "TaskManager"
#   }
# }