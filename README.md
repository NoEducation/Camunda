---------------------------------
Uruchamianie środowiska w chmurze/SASS:

Przed uruchomieniem należy utworzyć klaster na https://console.cloud.camunda.io
oraz dodać do API Client credentials. Pobieramy Client credentials z zakładki Env Vars i aktualizuje sekcje
CamundaEnvironment odpowiednimi wartościami w pliku appsettings.json
``` 
"CamundaEnvironment": {
    "ZEEBE_ADDRESS": "***",
    "ZEEBE_CLIENT_ID": "***",
    "ZEEBE_CLIENT_SECRET": "***",
    "ZEEBE_AUTHORIZATION_SERVER_URL": "https://login.cloud.camunda.io/oauth/token",
    "ZEEBE_TOKEN_AUDIENCE": "zeebe.camunda.io",
    "CAMUNDA_CLUSTER_ID": "***",
    "CAMUNDA_CLUSTER_REGION": "***",
    "CAMUNDA_CREDENTIALS_SCOPES": "***",
    "CAMUNDA_OAUTH_URL": "https://login.cloud.camunda.io/oauth/token"
  },
```
---------------------------------
Uruchamianie środowiska lokalnie:

https://github.com/camunda/camunda-platform
pobieramy repozytorium i z uruchamiamy komendą:
```
docker-compose up -d
```
Należy także przestawić flagę IsLocalConnection w pliku appsettings.json na true.
Zabicie środowiska: 
```
docker compose down -v
```
Adresy:
- Operate: http://localhost:8081
- Tasklist: http://localhost:8082
- Optimize: http://localhost:8083
- Identity: http://localhost:8084
- Elasticsearch: http://localhost:9200

---------------------------------
Przydatne tutoriale:
- https://unsupported.docs.camunda.io/0.26/docs/guides/setting-up-development-project/
- https://github.com/Hafflgav/camunda-8-dotnet-guide
- https://github.com/camunda/camunda-platform-get-started/tree/main/csharp