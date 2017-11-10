# Fortune-Teller-UI - ASP.NET Core MVC Application
ASP.NET Core sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud) for registering micro services and [Spring Cloud Hystrix](http://cloud.spring.io/spring-cloud) for building resilient micro services applications. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup and the Fortune-Teller-UI uses a Hystrix Command with fallback ability when communicating with the Fortune service.  

In addition, the Fortune-Teller-UI also illustrates how to use a Hystrix Collapser to combine, or 'batch up', multiple requests to backend micro-services endpoints. 

This sample also illustrates how to use the [Hystrix Dashboard](http://cloud.spring.io/spring-cloud) to gather status and metrics of the Hystrix command used in communications.

# Pre-requisites - Local

This sample assumes that there is a running Spring Cloud Eureka Server on your machine and a Hystrix dashboard. To make this happen:

1. Install Java 8 JDK.
2. Install Maven 3.x.
3. Clone the Spring Cloud Samples Eureka repository. (https://github.com/spring-cloud-samples/eureka.git)
4. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
5. When running locally, this sample will default to looking for its eurka server on http://localhost:8761/eureka, so it should all connect.
6. Clone the Spring Cloud Samples Hystrix dashboard. (https://github.com/spring-cloud-samples/hystrix-dashboard.git)
7. Go to the hystrix dashboard directory (`hystix-dashboard`) and fire it up with `mvn spring-boot:run`
8. Install .NET Core SDK 

# Building & Running - Local

1. Clone this repo. (i.e. git clone https://github.com/SteeltoeOSS/Samples)
2. cd samples/CircuitBreaker/src/AspDotNetCore/Fortune-Teller/Fortune-Teller-UI
3. Set the `BUILD` environment variable to `LOCAL` (i.e. SET BUILD=LOCAL, export BUILD=LOCAL)
3. dotnet restore --configfile nuget.config
4. dotnet run -f netcoreapp2.0 --server.urls http://*:5555


# What to expect - Local
After building and running the app, you should see something like the following:
```
$ cd samples/CircuitBreaker/src/AspDotNetCore/Fortune-Teller/Fortune-Teller-UI
$ dotnet run -f netcoreapp2.0 --server.urls http://*:5555
Hosting environment: Production
Now listening on: http://*:5555
Application started. Press Ctrl+C to shut down.
```
At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit http://localhost:5555/ to see it!

# Using the Hystrix Dashboard - Local
Once you have the two applications communicating, you can make use of the Hystrix dashboard by following the instructions below.  Note: This assumes you have followed the steps above and already have the dashboard running.

1. Open a browser or browser window and connect to the dashboard. (e.g. http://localhost:7979)
2. In the first field, enter the endpoint (http://localhost:5555/hystrix/hystrix.stream) that is exposing the hystrix metrics.
3. Click the monitor button.
4. Go back to the Fortune-Teller-UI application and obtain several fortunes.  Observe the values changing in the Hystrix dashboard.  Click the refresh button on the UI app quickly to see the dashboard update.

# Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 
2. Installed Spring Cloud Services
3. Install .NET Core SDK

# Setup Service Registry on CloudFoundry
You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-service-registry standard myDiscoveryService
3. Wait for the service to become ready! (i.e. cf services) 

# Setup Circuit Breaker Dashboard service on CloudFoundry
You must first create an instance of the Circuit Breaker service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-circuit-breaker-dashboard standard myHystrixService
3. Wait for the service to become ready! (i.e. cf services) 

# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/CircuitBreaker/src/AspDotNetCore/Fortune-Teller/Fortune-Teller-UI
3. Make sure environment variable `BUILD` is not set to `LOCAL` (i.e. SET BUILD=, unset BUILD)
4. dotnet restore --configfile nuget.config
5. Publish app to a directory selecting the framework and runtime you want to run on. 
(e.g. `dotnet publish  -f netcoreapp2.0 -r ubuntu.14.04-x64`)
6. Push the app using the appropriate manifest.
 (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish`)


Note: If you are using self-signed certificates it is possible that you might run into SSL certificate validation issues when pushing this app. The simplest way to fix this:

1. Disable certificate validation for the Spring Cloud Discovery Client.  You can do this by editing `appsettings.json` and add `eureka:client:validate_certificates=false`.

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs fortuneui`

On a Windows cell, you should see something like this during startup:
```
2016-05-14T06:38:21.67-0600 [CELL/0]     OUT Successfully created container
2016-05-14T06:38:47.90-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:38:47.90-0600 [APP/0]      OUT       DoGetApplicationsAsync .....
2016-05-14T06:38:47.91-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:38:47.91-0600 [APP/0]      OUT       FetchFullRegistry returned: OK, Applications[Application[Name=FORTUNESERVICE ....
2016-05-14T06:38:47.91-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:38:47.91-0600 [APP/0]      OUT       FetchRegistry succeeded
2016-05-14T06:38:47.99-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[4]
2016-05-14T06:38:47.99-0600 [APP/0]      OUT       Hosting starting
2016-05-14T06:38:48.02-0600 [APP/0]      OUT info: Microsoft.Net.Http.Server.WebListener[0]
2016-05-14T06:38:48.02-0600 [APP/0]      OUT       Start
2016-05-14T06:38:48.07-0600 [APP/0]      OUT info: Microsoft.Net.Http.Server.WebListener[0]
2016-05-14T06:38:48.07-0600 [APP/0]      OUT       Listening on prefix: http://*:58442/
2016-05-14T06:38:48.12-0600 [APP/0]      OUT       Hosting started
2016-05-14T06:38:48.12-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[5]
2016-05-14T06:38:48.12-0600 [APP/0]      OUT Hosting environment: development
2016-05-14T06:38:48.12-0600 [APP/0]      OUT Now listening on: http://*:58442
2016-05-14T06:38:48.12-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```
At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit http://fortuneui.x.y.z/ to see it!

In addition to hitting http://fortuneui.x.y.z/, you can also hit: http://fortuneui.x.y.z/#/multiple to cause the UI to make use of a Hystrix Collapser to obtain multiple fortunes.

# Using the Hystrix Dashboard - Cloud Foundry

Once you have the two applications communicating, you can make use of the Hystrix dashboard by following the instructions below.  

1. Open a browser or browser window and connect to the Pivotal Apps Manager.  You will have to use a link that is specific to your Cloud Foundry setup. (e.g. https://apps.system.testcloud.com)
2. Follow [these instructions](http://docs.pivotal.io/spring-cloud-services/1-3/common/circuit-breaker/using-the-dashboard.html) to open the Hystrix dashboard service.
3. Go back to the Fortune-Teller-UI application and obtain several fortunes.  Observe the values changing in the Hystrix dashboard.  Click the refresh button on the UI app quickly to see the dashboard update.

# Enabling SSL usage on Cloud Foundry

If you wish to use SSL communications between the Fortune Teller UI and the Fortune Teller Service, have a look at the comments in the files listed below.  You will need to make modifications to one or more of those files. Also, you are encouraged to read the [Cloud Foundry documentation](https://docs.pivotal.io/pivotalcf/1-10/adminguide/securing-traffic.html) on how SSL is configured, used and implemented before trying to use it with the Fortune Teller app.

1. `FortuneService.cs` - Changes needed to enable SSL usage when the Fortune Teller Service (i.e. Kestrel) is presenting Self-Signed certificates to the client.
