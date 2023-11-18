#Краткое описание процесса использования миграций.
Есть два способа обновления базы - через dotnet-cli и через накат миграций в рантайме.

## Через dotnet-cli
Для автоматического создания файла миграции используется следующая команда запущенная из каталога солюшена:
\```
dotnet ef migrations add MIGRATION_NAME --project .\MatchingSystem.DataLayer\MatchingSystem.DataLayer.csproj  -- "data source=localhost\SQLEXPRESS;initial catalog=DiplomaMatching;Integrated Security=True;MultipleActiveResultSets=True; TrustServerCertificate=True;"
\```
где 
   -- MIGRATION_NAME - имя новой миграции
   -- project .\MatchingSystem.DataLayer\MatchingSystem.DataLayer.csproj - проект содержащий  каталог /Migrations/
   -- "data source=...." - строка подключения указанная в appsettings.Development.json

В результате будет сгенерирован файл вида дата_время_MIGRATION_NAME.cs, содержащий код обновления базы 
из состояния в котором она была до команды, в состояние отраженное в DbContext.cs.
Миграции применяются командой
\```
dotnet ef database update --project .\MatchingSystem.DataLayer\MatchingSystem.DataLayer.csproj -- "data source=localhost\SQLEXPRESS;initial catalog=DiplomaMatching;Integrated Security=True;MultipleActiveResultSets=True; TrustServerCertificate=True;"
\```

При этом применятся все непримененные (pending) миграции которые есть в папке Migrations.
Можно указать конкретную миграцию которую нужно применить или к которой нужно вернуть состояние базы указав ее как аргумент для update
\```
dotnet ef database update --project .\MatchingSystem.DataLayer\MatchingSystem.DataLayer.csproj -- "data source=localhost\SQLEXPRESS;initial catalog=DiplomaMatching;Integrated Security=True;MultipleActiveResultSets=True; TrustServerCertificate=True;" 20231021105557_Baseline
\```
Посмотреть какие миграции применены к конкретной БД можно в таблице __EFMigrationsHistory.
Посмотреть статусы миграций в папке Migrations можно командой
\```
dotnet ef migrations list --project .\MatchingSystem.DataLayer\MatchingSystem.DataLayer.csproj  -- "data source=localhost\SQLEXPRESS;initial catalog=DiplomaMatching;Integrated Security=True;MultipleActiveResultSets=True; TrustServerCertificate=True;"
\```


## Через накат в рантайме

Процесс созданий миграции аналогичен, разница лишь в том что вызов команды dotnet ef database update выполнится автоматически 
при запуске приложения. Для этого необходимо в appsettings.json параметр UseMigrations выставить в True.
Код выполняющий эти действия определен в MatchingSystem.UI.Startup.ApplyMigrations


## Процесс установки на чистый сервер из бэкапа с созданием нового baseline
Предположим необходимо развернуть БД на чистой СУБД поверх нового бэкапа нежели имевшийся.
Это может понадобиться на случай, если бейзлайном нужно сделать бэкап, содержащий обновленные данные с продакшена.

!!! В теории это не является необходимым, и делать это нежелательно !!!
Если база данных на проде имеет аналогичную историю миграций, то ее можно просто накатить на локальном сервере 
без конфликтов по созданию таблиц. Однако на всякий случай оставлю описание процесса.

Это может быть необходимо в случае, если нужно отразить в нем изменения состояния продовых данных.
1. В первую очередь на сервак вручную накатывается бэкап базового состояния БД.
2. Из __EFMigrationsHistory удаляются записи о старых миграциях
3. Удаляются (желательно с бэкапом в другую папку) все старые миграции в папке Migrations,
4. Выполняется dotnet ef migrations add NewBaseline
5. При этом будет сгенерирован файл XXX_NewBaseline.cs и DiplomaMatchingContextModelSnapshot.cs.
XXX_NewBaseline.cs будет содержать изменения отражена структура базы бэкапа, при попытке применения он поймает конфликты, 
так как все отраженные в нем таблицы уже существуют.
6. Чтобы этого избежать в XXX_NewBaseline.cs содержимое функций Up и Down заменяется на пустую строку или комментарится.
DiplomaMatchingContextModelSnapshot оставляем без изменений
7.  Выполнить dotnet ef database update или запустить приложение

Все, после этого можно создвать и применять новые миграции, конфликтов по существующим таблицам не будет