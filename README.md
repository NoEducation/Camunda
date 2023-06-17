---------------------------------
Uruchamianie środowiska w chmurze/SASS:

Przed uruchomieniem należy utworzyć klaster na https://console.cloud.camunda.io
oraz dodać do API Client credentials. Pobieramy Client credentials z zakładki Env Vars i zapisujemy w pliku CamundaCloud.env usuwając słowa export.

---------------------------------
Uruchamianie środowiska lokalnie:

https://github.com/camunda/camunda-platform
pobieramy repozytorium i z uruchamiamy komendą: docker-compose up -d
Należy także przestawić flage IsLocalConnection w appsettings na true.
Zabice środowiska: docker compose down -v
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
