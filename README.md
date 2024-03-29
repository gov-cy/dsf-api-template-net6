<img src=https://github.com/gov-cy/govdesign/blob/main/RESTful-API.png height=75> 
 
# DSF API Template
## A quickstart template for creating a RESTful API based on the DSF API standards
This project is an example of an API developed in .NET6. A demo installations is available at: [api-template-demo].

## ASP.NET Web API
ASP.NET Web API is a framework for building HTTP services that can be accessed from any client including browsers and mobile devices. It is an ideal platform for building RESTful applications on the . NET Framework.

## DSF Components  
Generally, DSF-Ready Services are composed of a Web application (including the DSF Design System and CyLogin integration) and a RESTful API that gets data from and posts data to the Back-end system(s).  The connectivity from the service (which is on the cloud) to the API is implemented through an API Gateway.

![alt text](https://github.com/gov-cy/govdesign/blob/main/dsf-service-diagram.png)

### DSF templates to serve as a quickstart guide for developing DSF-ready services:
- **RESTful API**: This repository provides a template project for developing a RESTful API [git-api-template]
- **DSF Design System**: [git-design-system].  Detailed documentation for the DSF Design System is available at: [git-design-system-docs]
- **Service template**: A service example to provide a template for a dsf-ready service including the DSF Design system and basic functionality [git-service-template]
- **Sample OIDC Web Client**: to simulate the CyLogin functionality [git-oidc-web-client]. The client is using a mock identity server [dsf-idsrv-dev]

## API Features
This api is a demo implementation that contains the following:
* Basic API Configuration based on the **DSF API Standards** [dsf-api-standard]
* OIDC Configuration using a mock identity server [dsf-idsrv-dev]
* A proposed project structure for RESTful API development based on DSF API Standards [dsf-api-standard]
* API Key handling and validation in configuration (No DB required)
* Implementation of GET, POST, PUT, and DELETE endpoints for demostrating CRUD operations using temporary In-Memory storage.
* Sample email and phone number validation endpoints (Validation Controller)
* Implementation of a sample endpoint to return the claims of an IdentityServer access token (Identity Controller)
* Endpoint Versioning

## Integrations

### Mock API Calls
The project also simulates various API calls to demostrate how a client should get data from and post data to a fictional back-end system.
The endpoints can be tested via the Swagger UI, a custom client, or a third-party tool (e.g. Postman).

## How it Works
### Installation
```
git clone https://github.com/gov-cy/dsf-api-template-net6.git
cd src\dsf-api-template-net6
dotnet build
dotnet run
```
### Configuration (appsettings.json)
All configuration settings can be set in the appsettings.json file

**Identity server used to validate the jwt tokens**
```
"IdentityServer": {
    "Authority": "https://dsf-idsrv-dev.dmrid.gov.cy"
}
```

**API Keys ans API Key Authorizations**  
For all permissions use \*.  
For specific endpoints, use the endpoint URI(s) (e.g. "api/v1/Identity/identity-echo").  
```
"ApiKeys": {
    "client-keys": [
      "12345678901234567890123456789000",
      "B1234567890123456789012345678900"
    ]
  },

  "ApiKeyAuthorizations": {
    "A1234567890123456789012345678900": [
      "*"
    ],
    "B1234567890123456789012345678900": [
      "api/v1/Identity/identity-echo",
      "api/v1/email-validation"
    ]
  }
```

**Rate Limits**  
Rate limits restrict the number of requests that the API will handle in a specific time period.  
Set the limits in the ClientRateLimiting GeneralRules section.  
The client-key, is the key (not value) that is being sent in the request headers by the client. 
For all Endpoints, use \* in the GeneralRules/Endpoint setting.

```
"ClientRateLimiting": {
 ...
 "ClientIdHeader": "client-key",
 ... 
 "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1s",
        "Limit": 100
      }
 ...
}
```

Client specific limits can be set in the ClientRateLimitPolicies section of the configuration.  
The client-key, is the value (not key) that is being sent in the request headers by the client.

```
"ClientRateLimitPolicies": {
    "ClientRules": [
      {
        "ClientId": "client-id-1",
        "Rules": [
          {
            "Endpoint": "*",
            "Period": "1s",
            "Limit": 10
          },
          {
            "Endpoint": "*",
            "Period": "15m",
            "Limit": 200
          }
        ]
      },
      ...
    ]
  }
```

## Tech
* Oidc Authentication (Mock CyLogin) [dsf-idsrv-dev]
* Authorized Endpoints (access_token)
* API Endpoint Versioning
* Dependency Injection
* Middleware
* Filters
* API Key support
* Swagger
* Logging
* Rate Limiting
* API Documentation (Swagger)
* Sample endpoints
* Base Response
* Http Status Codes [http-status-codes]
* In-Memory Database (used only for testing):

## NuGet Packages
* AspNetCoreRateLimit
* Microsoft.AspNetCore.Authentication.JwtBearer
* Microsoft.AspNetCore.Authentication.OpenIdConnect
* Microsoft.AspNetCore.Mvc.Versioning
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.InMemory
* Microsoft.Identity.Web
* Swashbuckle.AspNetCore

## License

[MIT License]

**Free Software, Hell Yeah!**

#### Non-production Usage. This Software is provided for evaluation, testing, demonstration and other similar purposes. Any license and rights granted hereunder are valid only for such limited non-production purposes.

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job.)
   
   [git-api-template]: <https://github.com/gov-cy/dsf-api-template-net6.git>
   [api-template-demo]: <https://dsf-api-test.dmrid.gov.cy/swagger/index.html>
   [git-service-template]: <https://github.com/gov-cy/dsf-service-template-net6>
   [git-design-system]: <https://github.com/gov-cy/govcy-design-system>
   [git-design-system-docs]: <https://gov-cy.github.io/govcy-design-system-docs/>
   [git-oidc-web-client]: <https://github.com/gov-cy/dsf-oidc-web-client>
   [dsf-idsrv-dev]: <https://dsf-idsrv-dev.dmrid.gov.cy>
   [dsf-api-standard]: <https://dsf.dmrid.gov.cy/2022/05/17/technical-policy/>   
   [http-status-codes]: <https://learn.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode?view=net-7.0>
   [MIT License]: <https://github.com/gov-cy/dsf-api-template-net6/blob/master/LICENSE.md>
