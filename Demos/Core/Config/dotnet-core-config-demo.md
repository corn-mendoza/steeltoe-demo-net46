### Goal

Demonstrate how to use Steeltoe's Config Server with .NET Core applications.   


Prerequisites
--

1. Install .NET Core 2.0

    [.NET Core](https://www.microsoft.com/net/core)
    
    To validate the installation, run the following command from a command prompt or shell:

    ```bash
    dotnet  --version
    dotnet new console -o newapp
    cd newapp
    dotnet run
    ```    
    
    If successful, you should see a Hello World! in the output. 


2. Clone the Steeltoe Demo

	[Steeltoe Demo:  https://github.com/Pivotal-Field-Engineering/steeltoe-demo](https://github.com/Pivotal-Field-Engineering/steeltoe-demo)
	
	##### Get the source code for the demo

   ![](images/steeltoe-demo-git.png)

	For Linux/Mac:
	```bash
	$ git clone https://github.com/Pivotal-Field-Engineering/steeltoe-demo.git
	```

	For Windows
	```
	C:\<Some Directory to save code>\> git clone https://github.com/Pivotal-Field-Engineering/steeltoe-demo.git
	```
	
Steps
--

In this demo we are going to follow these steps to deploy a .NET Core application that communicates to the Configuration Server to retrieve its configuration.

***
## Deploying the Configuration Demo Application

### Step 1
##### Build the Configuration Demo Application

By this point, you should have cloned the [Steeltoe Demo:  https://github.com/Pivotal-Field-Engineering/steeltoe-demo](https://github.com/Pivotal-Field-Engineering/steeltoe-demo).  Let's prepare the application to deploy to Cloud Foundry. 

Change into the correct folder, which is Configuration/src/AspDotNetCore/SimpleCloudFoundry Notice that we are going down several levels to the SimpleCloudFoundry Folder:

Linux/Mac:

```bash
cd Configuration/src/AspDotNetCore/SimpleCloudFoundry
```

Windows:

```
cd Configuration\src\AspDotNetCore\SimpleCloudFoundry
```
  
### Step 2
##### Login into Pivotal Cloud Foundry (if necessary)

  cf login -a https://api.sys.cloud.rick-ross.com --skip-ssl-validation
  Email: myuserid
  Password: ••••••••

  Select a space (or press enter to skip):
  1. development
  2. test
  3. production

  Select any one and stick to that space for the rest of the demo.

Login to the App Console at https://app.cloud.rick-ross.com

   ![PCF App Console](images/pcf-console.png) 

### Step 3
##### Create a Config Server within Pivotal Cloud Foundry

Let's create a config server that the application will use to get configuration information from:

```
cf create-service p-config-server standard MyConfigServer -c ./config-server.json
```

Note that this step might take a bit to complete. Please wait until you see that the last operation resulted is "create succeeded". To do that:

```bash
cf services
Getting services in org pivotal / space development as admin...
OK
    
name              service           plan       bound apps   last operation
MyConfigServer   p-config-server   standard                create in progress
```
    
Once this is successful, you will see this:

```bash
cf services
Getting services in org pivotal / space development as admin...
OK
    
name              service           plan       bound apps   last operation
MyConfigServer   p-config-server   standard                create succeeded
```

You can now proced to the next step. 
    
### Step 4
#### Prepare the .NET Core application for deployment

```bash
dotnet restore --configfile nuget.config
dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64
```

### Step 5
##### Push the app

Push the Fortune Teller Service 

On Linux/Mac:

```bash
$ cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish 
```
  
On Windows: 
  
```bash
cf push -f manifest.yml -p bin\Debug\netcoreapp2.0\ubuntu.14.04-x64\publish
```

Which will result in output of

```bash
// This will give an output which is similar to this
requested state: started
instances: 1/1
usage: 1G x 1 instances
urls: rr-configapp.app.cloud.rick-ross.com
last uploaded: Sun Sep 3 21:16:55 UTC 2017
stack: cflinuxfs2
buildpack: ASP.NET Core (buildpack-1.0.25)
```

### Step 6
##### View the application in a Browser

Navigate the URL of the application in a browser. Then click on the CloudFoundry Settings menu / tab to view information from VCAP_APPLICATION and VCAP_SERVICES that are associated with this application running in Cloud Foundry. 

Here is the type of detail you see for the CloudFoundry Settings. Notice that you can see the instance ID and other information about this aplication.

   ![CloudFoundry Settings](images/cloud-foundry-settings.png)
   

Here is the type of detail you see for the Config Server Settings.  

   ![Config Server Settings](images/config-server-settings.png)

And these are the details, or the data retrieved from the Config Server repository. 

   ![Config Server Data](images/config-server-data.png)

To see what values it is pulling from files in the git repository, open a browser and navigate to the backing repo which is located here: [https://github.com/rossr3-pivotal/config-repo](https://github.com/rossr3-pivotal/config-repo). Look for properties files that have "foo" in their name. 

For additional information on Steeltoe Configuration please see the official documentation.  [http://steeltoe.io/docs/steeltoe-configuration/](http://steeltoe.io/docs/steeltoe-configuration/)

