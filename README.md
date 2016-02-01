# cf-towns-dotnet
#Introduction
This is a basic sample c# ASP.NET app demonstrating the functionaliy of retrieving the Cloud Foundry environment variable. If there is no MySQL service bound it will display a note stating this, If a MySQL service has been bound to the app it will populate the database with UK Town data and then display the results in a simple table.

#Getting Started
Once your PCF instance has a Windows Cell available (see [here](http://docs.pivotal.io/pivotalcf/opsguide/deploying-diego.html) for more information) you can simply clone this repo and then push the app as follows:
```
cf push uktowns -s windows2012R2 -b binary_buildpack 
```
If diego is not enabled by default then you will need to add --no-start to the end of the above command and then run the following
```
cf enable-diego uktowns
cf start uktowns
```
Note: this will require the enable-diego CLI plug [available here](https://github.com/cloudfoundry-incubator/diego-enabler)

You can then go to the app URL and you will see a message highlighting that you need to bind a MySQL Service, so go ahead and create and bind the service as follows
```
cf create-service service-name plan-name mysqlServiceName
cf bind-service uktowns mysqlServiceName
cf restage uktowns
```
Then re-visit the URL to see the sample app showing a list of UK Towns



