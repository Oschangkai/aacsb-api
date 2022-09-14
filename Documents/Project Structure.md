# Project Sturcture

> Original:  
> https://codewithmukesh.com/project/aspnet-core-webapi-clean-architecture%E2%80%8B/  
> https://www.3chy2.com.tw/3c資訊/在-asp-net-core-項目中使用-mediatr-實現中介者模式/

## 1. Core

The Core Layers will never depend on any other layer.  

Therefore what we do is that we create interfaces in the Application Layer and these interfaces get implemented in the external layers.
This is also known and DIP or Dependency Inversion Principle.

It is also important to note that these layers are .NET Standard 2.1 Libraries. This is so because we need to make these layers as comptatible as possible with other projects. Let’s say tommorow your client wants a .NET 4.7 Solutions (quite unlikely though) with the near-same business logics. At such scenarios, .NET Standard Libraries will be of great help.

### 1.1 Domain
All the Entities and the most common models are available here.
Note that this Layer will **NEVER** depend on anything else.

### 1.2 Application

Interfaces, CQRS Features, Exceptions, Behaviors are available here.  

## 2. Infrastructure
Whenever there is a requirement to communicate with an external source, we implement it on the Infrastructure Layer.
For example, Database or other Services will be included here.

To make the separation more visible, We will maintain further sub projects with the naming convention as ‘Infrastructure.xxxxxx’ where xxxxxx is the actual Point of Concern.

基礎架構層，這層會包含一些對於基礎元件的配置或是幫助類的程式碼。
對於每個新建的服務來說，該層的程式碼幾乎都是差不多的，所以對於基礎架構層的程式碼其實最好是發布到 Nuget 倉庫中，然後我們直接在專案中透過 Nuget 去引用。

### 2.1 Infrastructure.Identity
In this implementation, we will make use of the already Microsoft Identity.
Let’s separate the User Management Database from the Main Application Database.  

This is made possible by multiple – DbContext Classes in each of the required Infrastructure Project

### 2.2 Infrastructure.Persistence
An Application Specific Database will be maintained.  
This is to ensure that there is no relation between the DBContext classes of Application and the Identity.

### 2.3 Infrastructure.Shared
Now, there are some services that are common to the other Infrastructure Layers and has the possibility of use in nearly all the Infrastructure Layers.  
This includes Mail Service, Date Time Service and so on. Thus it is a better Idea to have a shared Infrastructure project as well.

## 3. WebApi
This is also known as the Presentation Layer, where you would put in the project that the user can interact with.  
In our case it is the WebAPI Project.