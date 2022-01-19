# YAOS
Yet Another Online Store or YAOS is a simple Ecommerce application demonstrating on how to build and implement .NET Core based Microservice Application with various industry wide used architecture patterns, libraries and best practices.

## Solution Item
- Order service - for orders placed by customers
- Product service - for our exisiting products in catalog
- Customer service - for managing our customers data
- Serch service - for searching customer and thier orders
- Docker compose file to quickly run these services 
- Unit test projects for each services above

## Concepts demonstrated
- ASP.NET Core Web Api Development
- Entityframework Core for data access
- Devops Pipeline and release implementation 
  - A build pipeline to build and test our microservices
  - A release pipeline to build and push the images to Azure Container Registry
  - A another release pipeline to publish the container images to Azure Service Fabric
- Popular libraries including
  - polly - for resiliency
  - Automapper
  - XUnit for testing
  - Moq for mocking dependencies for testing


# TODOs:
- Refactor code to follow DDD clean architecture pattern
- Implement CQRS using MediatR
