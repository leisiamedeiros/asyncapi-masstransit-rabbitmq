# Console application example with RabbitMQ MassTransit

An example of a dotnet console application with a usage of [MassTransit](https://masstransit-project.com/). 

You can see theses examples on [documentation](https://masstransit-project.com/usage/configuration.html#console-app).

To run this application just execute `dotnet run --project MassTransitExample.csproj`
  - You must have the dotnet and docker installed. 

# Docker compose

To run the rabbit container execute `docker-compose up -d`, the docker-compose file is on the root folder.

Access the default host:port `http://localhost:15672/` and login with the default `guest` user and password. 

## Docker image

Preconfigured Docker image, based on default management-alpine image, maintained by MassTransit, including the delayed exchange plug-in, as well as the Management interface enabled.

- [masstransit/rabbitmq](https://hub.docker.com/r/masstransit/rabbitmq)

References default image base:

- [rabbitmq](https://hub.docker.com/_/rabbitmq/)