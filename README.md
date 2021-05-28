# The Watchers

## Prototypes

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
