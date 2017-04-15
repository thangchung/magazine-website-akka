> We are stopping the development for this project for a while due to some of the limitations in the actor model. If you have checked out this repository, please very care of using it in your project.

## Architecture
The Microservices + CQRS architecture are implemented by using Akkadotnet 
## Setup
- MongoDB at mongodb://127.0.0.1:27017
- Service Discovery using Lighthouse at akka.tcp://magazine-system@localhost:8090
- Category Service at akka.tcp://magazine-system@localhost:8092
- Web API at http://localhost:8091/swagger

## Install Windows Service
- Lighthouse.exe install -servicename "Magazine Website - Lighthouse" –autostart
- Cik.Magazine.CategoryService.exe install -servicename "Magazine Website - Category Service" –autostart

## Process Manager
![Category Publishing Approval](https://raw.githubusercontent.com/thangchung/magazine-website-akka/master/docs/CategoryPublishingApprovalFlow.png "Category Publishing Approval")

## Query with GraphQL
- Choose `/graphql`, then input `query { categories { id, status } }` for the `query` field

[![](https://codescene.io/projects/786/status.svg) Get more details at **codescene.io**.](https://codescene.io/projects/786/jobs/latest-successful/results)
