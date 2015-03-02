# Code-Samples
##Snippets and Tidbits of Code I have developed

I selected this sample of code because it is code that I have personally written for a client.  It contains very little boilerplate code; and a client specific version of this code is currently being used in a production environment.

However, as a result I have removed sections of code that would be considered proprietary or client/implementation specific.  Also, the requirement that this code satisfies is rather niche.

This is a text line parser which accepts a string which contains search criteria and parses it out so that it can be used to query entity framework.  It also allows the user to specify field updates directly within the text.

Many of the details of what is searched and how are provided within the app/web config file (A sample of this is provided in the app.config associated with the tests project).
There are three custom sections in the config file:
	* EntityDefinitions - a section where you indicate what "entities" the parser is used for.  Each line corresponds to an entity framework model.  Entities are things like Orders, Sales Reps, Locations, Customers, Invoices.  Whatever you want to be able to search.  Often these correspond directly to tables or views in the database.
	* SearchFieldDefinitions - A collection of SearchFieldDefinition sections.  There should be one SearchFieldDefinition section for each EntityDefinition provided.  These contain the specific field definitions for each Entity.
	* DisplayDefinitions - These aren't used directly by the parser, but are passed through with the result to permit the result to be used by the front end for display purposes.
	
Interesting bits:
	* Custom Configuration Sections.
	* Use of clean poco object with Code First EF6
	* Creation of generic Linq Expressions (i.e. Expression<Func<T, bool>>) for use in creating dynamic expression trees for query purposes.
