# The Watchers

The Watchers is a simple, self-hosted, low cost / no-cost website availability monitor.

I’m sure that there are existing and better solutions out there, but I haven’t looked for them, and I don’t care. Being a consumer of somebody's else's solution holds no value for me.


## Backstory

During my recent job search, many of the job descriptions I read called for skills in serverless computer, containerisation, Kubernetes, cloud eventing services, Node.js & React. Since then, I had been looking for an idea where I could take a “learn by doing” approach to develop my skill in those areas.

I also found myself being the last dev standing to provide support for a small collection of websites which I am doing only on an “as need” basis. And after one incident where a couple of WordPress sites hosted in IIS stopped responding, accompanied by the usual whines to get them back online ASAP. I was over being reactive.

It didn’t take me long to see that I could hack together my own DYI website availability monitor in a way that could tick all of the skill development boxes. This approach had the added advantage that it could be no-cost (or very low cost) just by taking advantage of the free & development tiers in Azure.


## Prototypes

I am using a learn by doing approach to develop The Watches as I have little to no exposure to most the tech that I want to use.

### Prototype - Simple URL Check

The Simple URL Check will provide an answer to the question: Can I use a serverless function to periodically call an URL and check it’s response status?

Of course, the answer to this question is: Yes. The functionality is so simple and fundamental that I don’t need the prototype to confidently answer the question. But, the code for this prototype will serve as the foundation for the more complex prototypes to come.

This prototype will only check a single hardcoded URL at a hardcode time interval and log the result. The value for the URL and time interval is not important, as is what exactly is logged. The log just needs to be descriptive enough to confirm that the check was performed and a response status was obtained.


### Prototype - Store Results Url Check

The Store Results Url Check prototype will extend the functionality of the Simple URL Check prototype by storing data about the check into a Cosmos DB.

The Store Results Url Check prototype allows me to learn what is required to store data into a Cosmos DB as I have not used it before. This will also allow me to explore the kind of data I will have and how it can be stored.

This prototype needs to connect to the Cosmos DB every time it checks the URL and then add a record to the database that describes the result of the check. What data is stored in the database, and the structure it is stored in, is not important. The stored results only need to be descriptive enough to confirm that the check was performed and a response status was obtained.


### Prototype - Trigger Via Event Grid

The Trigger Via Event Grid will extend the functionality of the Store Results Url Check prototype to using an event-based architecture via Azure Event Grid. The trigger for the existing URL Check function need to altered so it reacts to an event that will be generated periodically be a separate Azure function.

The Trigger Via Event Grid prototype allows me to learn what is required to leverage the Azure Event Grid. This prototype also allows me to explore implementing the URL Checking logic as a microservice.

This prototype only needs to use Azure Event Grid to trigger an URL check. The details of the event are not important.


### Prototype - Azure Comos DB Trigger

Brad suggested leveraging an Azure Comos DB Trigger to invoke an Azure Function when URL check results is save to the Comos DB. This function will decide if the URL is offline and will generate an "offline" event for another function to handle.

Azure Comos DB Trigger will give me my first expeirence of triggering events from NoSQL databases. Something that I have so little knowledge of, I feel unable to effectively describe it.

This prototype will build upon the previous prototype, but will use an Azure Comos DB Trigger to  invoke an Azure Function when URL check results is save to the Comos DB. This function will generate an "offline" even that will be subscribed to by another new function which will log the result. The details of the "offline" event and what is log isn't important, it just needs to be descriptive enough to confirm that the URL is offline.
 