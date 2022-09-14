# Onion Architecture

> Original:
> https://codewithmukesh.com/blog/onion-architecture-in-aspnet-core/

![](https://codewithmukesh.com/wp-content/uploads/2020/06/Onion-Architecture-In-ASP.NET-Core.png)

## Domain and Application Layer
Domain and Application Layer will be at the center of the design. We can refer to these layers at the Core Layers. These layers will not depend on any other layers.

Domain Layer usually contains enterprise logic and entities. Application Layer would have Interfaces and types. The main difference is that The Domain Layer will have the types that are common to the entire enterprise, hence can be shared across other solutions as well. But the Application Layer has Application-specific types and interface.

As mentioned earlier, the Core Layers will never depend on any other layer. Therefore what we do is that we create interfaces in the Application Layer and these interfaces get implemented in the external layers. This is also known and DIP or Dependency Inversion Principle. 

For example, If your application want’s to send a mail, We define an IMailService in the Application Layer and Implement it outside the Core Layers. Using DIP, it is easily possible to switch the implementations. This helps build scalable applications.

## Presentation Layer
Presentation Layer is where you would Ideally want to put the Project that the User can Access. This can be a WebApi, Mvc Project, etc.

## Infrastructure Layer
Infrastructure Layer is a bit more tricky. It is where you would want to add your Infrastructure. Infrastructure can be anything. Maybe an Entity Framework Core Layer for Accessing the DB, or a Layer specifically made to generate JWT Tokens for Authentication or even a Hangfire Layer.