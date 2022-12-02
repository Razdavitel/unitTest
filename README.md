# devops-22-23-dotnet-t02

## Squads

## Installation
### Requirements
To send an email when inviting a new user some configuration needs to be added to the appsettings.json file:
```
"MailSettings": {
    "SmtpHost": "smtp-mail.outlook.com",
    "SmtpPort": 587,
    "FromEmail": "t02-squads@outlook.com",
    "LoginName": "t02-squads@outlook.com",
    "LoginPassword": "GeheimWachtwoord",
    "EmailTemplateLocation": "/path/to/devops-22-23-dotnet-t02/resources"
  }
```
The example above shows the setup for an outlook email adres. The EmailTemplateLocation needs to point to the location where the ActivationMail.cshtml is stored.

## Usage

## Tech-Stack

- [Dotnet 6](https://learn.microsoft.com/en-us/dotnet/fundamentals/)
- [EF-core](https://learn.microsoft.com/en-us/ef/core/)
- [JWT](https://jwt.io/introduction)
- [Postgres](https://www.postgresql.org/docs/current/app-psql.html)
- [Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0)
- [mudblazor](https://mudblazor.com/)
- [Radzen](https://www.radzen.com/documentation/blazor/)

## Commands

create a shared folder for a domain

```shell
sudo ../../scripts/create-shared-folder.sh -p /SportStore/src/Shared -n Category -f SportStore.Shared
```

run psql

```console
devops-22-23-dotnet-t02/infrastructure> docker-compose up -d
```

make a migration

```console
devops-22-23-dotnet-t02> dotnet ef migrations add [MigrationName] --project .\src\Persistence\Persistence.csproj --startup-project .\src\Server\Squads.Server.csproj --context SquadContext
```

update database

    via global tool:
```console
devops-22-23-dotnet-t02\src\Server> dotnet tool install --global dotnet-ef
devops-22-23-dotnet-t02\src\Server> dotnet ef database update --context SquadContext
```
    or via local tool:
```console
devops-22-23-dotnet-t02\src\Server> dotnet tool restore
devops-22-23-dotnet-t02\src\Server> dotnet ef database update --context SquadContext
```

list http endpoints

```console
\devops-22-23-dotnet-t02\src\Server> httprepl https://localhost:7045
(Disconnected)> connect https://localhost:7045
Using a base address of https://localhost:7045/
Using OpenAPI description at https://localhost:7045/swagger/v1/swagger.json
For detailed tool info, see https://aka.ms/http-repl-doc

https://localhost:7045/> ls
.      []
Todo   [GET|POST|PUT]
User   [GET|POST|PUT]
```

## Procedures

authorize Swagger

![auth-swagger](https://res.cloudinary.com/dpaf8dzoa/image/upload/v1667563984/auth-swagger_wskz5c.gif)

check JWT

![check-jwt](https://res.cloudinary.com/dpaf8dzoa/image/upload/v1667567545/verify-jwt_c2mrtx.gif)

## Links

- [diagrams](https://drive.google.com/file/d/1Rd8rksB0M92dD6q27DCM4SSG8IHTznwf/view?usp=sharing)
- [mock-ups](https://www.figma.com/file/OFhYF6I0QojofO0vs9JkI1/Squads)
