### Overview
- This is an example Web App to demonstrate the use of Dotnet Core InMemoryCaching and using a distributed caching system "Redis".
- In this example, we run the redis cluster locally using Docker.
- For fetching the current weather, this App uses the OpenWeather APIs: https://openweathermap.org/
- We are creating a new Attribute class which also implements an `IAsyncActionFilter` interface to intercepts the requests and serve it from cache if it is present in cache. Else, this also caches the response which is used in subsequent API calls.

### Running Redis Cluster via docker locally
docker run -p 6379:6379 --name redis-master -e REDIS_REPLICATION_MODE=master -e ALLOW_EMPTY_PASSWORD=yes bitnami/redis:latest
