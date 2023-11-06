# Kennel Indexer API
This is the API portion of a web application that allows for basic CRUD operations using entity framework with a code-first approach

# How To use 
1. Create a file called `appsettings.json` in the KennelIndexer.API directory.
In this file, use following template and use a connection string to a SQL database:

```
{
  "ConnectionStrings": {
    "MainDB": "<CONNECTION_STRING_LINK>" 
  }
}
```

2. Go to the `Package Manager Console` and run the command `Update-Database` to apply the migrations from Entity Framework

3. Run KennelIndexer.API with IIS Express

The API is now running and you can test the functionality using an API-platform such as [Postman](https://www.postman.com/)

# Front-end
This repo only includes the backend API. To use the front-end (Created with Angular) please visit the following [Repository](https://github.com/thom9346/KennelIndexer)

# Author
[Thomas H](https://github.com/thom9346)