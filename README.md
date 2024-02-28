# SEP490_G55_SPR24_BE

## Installation
### Step 1:
Install [Docker](https://www.docker.com/), [Azure Data Studio](https://learn.microsoft.com/en-us/azure-data-studio/download-azure-data-studio?view=sql-server-ver16&tabs=win-install%2Cwin-user-install%2Credhat-install%2Cwindows-uninstall%2Credhat-uninstall) or [Microsoft SQL Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16).

### Step 2:
Clone the project to your computer with:

```git clone https://github.com/quandhhe151315/SEP490_G55_SPR24_BE.git```

### Step 3:
Open a terminal within the SEP490_G55_SPR24_BE/KitchenDelights folder.
Either by ```Shift+Click``` > ```Open Powershell terminal here```, or ```cd SEP490_G55_SPR24_BE/KitchenDelights```.

### Step 4:
Build Docker image from source code with:

```docker compose build```

After that, create Docker container with:

```docker compose up -d```

### Step 5:
Use Azure Data Studio or Microsoft SQL Management Studio to connect to the database with "server" as ```localhost,1433```, "user" and "password" are provided within ```appsettings.json``` file.

Afterward, run the sql script provided within SEP490_G55_SPR24_BE/KitchenDelights/Resources folder.


### Step 6 (optional):
Run ```docker builder prune``` to clear unused image cache.
