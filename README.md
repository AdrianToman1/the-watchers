# The Watchers

The Watchers is a simple, self-hosted, low cost / no-cost website availability monitor.

I’m sure that there are existing and better solutions out there, but I haven’t looked for them, and I don’t care. Being a consumer of somebody's else's solution holds no value for me.


## Backstory

During my recent job search, many of the job descriptions I read called for skills in serverless computer, containerisation, Kubernetes, cloud eventing services, Node.js & React. Since then, I had been looking for an idea where I could take a “learn by doing” approach to develop my skill in those areas.

I also found myself being the last dev standing to provide support for a small collection of websites which I am doing only on an “as need” basis. And after one incident where a couple of WordPress sites hosted in IIS stopped responding, accompanied by the usual whines to get them back online ASAP. I was over being reactive.

It didn’t take me long to see that I could hack together my own DYI website availability monitor in a way that could tick all of the skill development boxes. This approach had the added advantage that it could be no-cost (or very low cost) just by taking advantage of the free & development tiers in Azure.


## Prototypes

I am using a learn by doing approach to develop The Watches as I have little to no exposure to most the tech that I want to use.

### Prototype 1 - Simple URL Check

The Simple URL Check will provide an answer to the question: Can I use a serverless function to periodically call an URL and check it’s response status?

Of course, the answer to this question is: Yes. The functionality is so simple and fundamental that I don’t need the prototype to confidently answer the question. But, the code for this prototype will serve as the foundation for the more complex prototypes to come.

This prototype will only check a single hardcoded URL at a hardcode time interval and log the result. The value for the URL and time interval is not important, as is what exactly is logged. The log just needs to be descriptive enough to confirm that the check was performed and a response status was obtained.


### Prototype - Store Results Url Check

The Store Results Url Check prototype will extend the functionality of the Simple URL Check prototype by storing data about the check into a Cosmos DB.

The Store Results Url Check prototype allows me to learn what is required to store data into a Cosmos DB as I have not used it before. This will also allow me to explore the kind of data I will have and how it can be stored.

This prototype needs to connect to the Cosmos DB every time it checks the URL and then add a record to the database that describes the result of the check. What data is stored in the database, and the structure it is stored in, is not important. The stored results only need to be descriptive enough to confirm that the check was performed and a response status was obtained.


### Prototype - Trigger Via Event Grid

The Trigger Via Event Grid will extend the functionality of the Store Results Url Check prototype to use an event-based architecture via Azure Event Grid. The trigger for the existing URL Check function need to altered so it reacts to an event that will be generated periodically be a separate Azure function.

The Trigger Via Event Grid prototype allows me to learn what is required to leverage the Azure Event Grid. This prototype also allows me to explore implementing the URL Checking logic as a microservice.

This prototype only needs to use Azure Event Grid to trigger an URL check. The details of the event are not important.



Brad suggested leveraging an Azure Comos DB Trigger to invoke an Azure Function when URL check results is save to the Comos DB. This function will decide if the URL is offline and will generate an "offline" event for another function to handle.


### Prototype - Azure Cosmos DB Trigger

The Azure Cosmos DB Trigger will extend the event-base architecture of the Trigger Via Event Grid prototype by using an Azure Cosmos DB Trigger to react when the results of an URL check is save to the Cosmos DB. 

Azure Comos DB Trigger Prototype will give me my first expeirence of triggering events from NoSQL databases. Something that I have so little knowledge of, I feel unable to effectively describe it.

This prototype will build upon the previous prototype, but will use an Azure Comos DB Trigger to invoke an Azure Function that will log when URL check result is saved to the Comos DB. The what is logged isn't important, it just needs to be descriptive enough to confirm that it received the URL results.


### Prototype - Test web site

Thus far I have been using the URL to my blog to as the target URL, but things have reaching a stage where I need dedicated URLs for testing.

I have written a few Azure Functions before, but never one that had an HTTP trigger. This prototype will give me expeirence doing that.

This prototype will be a seperate application, and maybe even a seperate repo. It begin with it will be a two Azure functions with HTTP triggers, one that return a HTTP Status code of OK (200) and another the returns Internal Server Error (500).


### Prototype - Offline Event

The Offline Event Prototype will extend the Azure Cosmos DB Trigger Prototype by trigger as Offline Event when an "offline" URL result is saved to the Cosmos DB. Another Azure Function will subscribe to and react to the event.

This prototypes tests Brad's above suggestion of using the function trigger by the Azure Cosmos DB to decided what action is required and then triggering the apporiate event. 

Will use an Azure Comos DB Trigger to invoke an Azure Function when URL check results is save to the Comos DB. This function will trigger an "offline" event that will be subscribed to by new function which will log the result. What the new Azure Fuction logs isn't important it just needs to be descriptive enough to confirm that the URL is offline.

The prototypes are maturing and as a part of that the event data needs to become more formalised. The event data for all events will need to be refactored so that the subject URLs is only hard coded in the EventGridTrigger function and is pass around wthin event data. Therefore, the details of the "offline" event isn't import except that it must include the subject URL. 


### Prototype - ARM Template

Up to now I have been utilising the a local Azure Cosmos Emulator, but this project will need to be migrated to Azure at some point. Also, how can any reuse this project unless I share details about the required Azure resource and configuration.

I have only ever created Azure resource in an ad-hoc as need basis. Creating an ARM will give me familiarity with ARM templates and the maybe the Azure CLI, something I tend to avoid.
 
The prototype will only need to create an ARM template that could have the rest of the project deployed into. Not that concerned about harcoding an names etc.

