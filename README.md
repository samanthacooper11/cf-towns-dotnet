# cf-towns-dotnet
#Introduction
This is a basic sample dotnet app that binds to a MySQL service running in Cloud Foundry, Inserts UK Town data and then displays results

#Getting Started
Once your PCF instance has a Windows Cell available: See [here](http://docs.pivotal.io/pivotalcf/opsguide/deploying-diego.html) for more information you can simply clone this repo and then push the app as follows:
```
cf push uktowns -s windows2012R2 -b binary_buildpack --no-start
```
If diego is enabled by default then you won't need to have the --no-start and you can skip the next step
```
cf enable-diego uktowns
cf start uktowns
```
You can then go to the apps URL and you will see a message highlighting that you need to bind a MySQL Service, so go ahead and create and bind the service 
```
cf bind-service uktowns name-of-mysql-service
cf restage
```
Then re-visit the URL to see the sample app showing a list of UK Towns



