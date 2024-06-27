# System Of River Stations
## Příkazy
#### vytvoření databáze
- Add-Migration SORS_DB -Context ApplicationDbContext
- Update-Database -Context ApplicationDbContext
#### NuGet
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
Install-Package Swashbuckle.AspNetCore
Install-Package Microsoft.EntityFrameworkCore.Design -Version 7.0.4
Install-Package Hangfire -Version 1.7.31
Install-Package Hangfire.AspNetCore -Version 1.7.31
Install-Package Hangfire.SqlServer -Version 1.7.31

## Funkčnost
Záznamy ze stanic jsou řazeny takto, nahoře hodnoty přesahující LVLmax, řazeny od nejvyšších po nejnižší, po nich hodnoty nižší LVLmin, řazeny vzestupně (od největšího sucha dál), poté ostatní řazeny dle stáří, od nejmladších.

maximální stáří uchovaného reportu lze měnit v Services/ReportCleanupService





