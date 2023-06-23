//Web Application Parameters

@description('Base name of the resource such as web app name and app service plan ')
@minLength(2)
param webAppName string = 'CoriaToDo'

@description('The SKU of App Service Plan ')
param sku string = 'B1'

@description('The Runtime stack of current web app')
param linuxFxVersion string = 'DOTNETCORE|7.0'

@description('Location for all resources.')
param location string = resourceGroup().location

var webAppPortalName = '${webAppName}-webapp'
var appServicePlanName = 'AppServicePlan-${webAppName}'

//Database Parameters

@description('Server Name for Azure Database for PostgreSQL')
param serverName string = 'backend-dev-group-postgres'

@description('Database administrator login name')
@minLength(1)
param administratorLogin string

@description('Database administrator password')
@minLength(8)
@secure()
param administratorLoginPassword string

@description('Azure Database for PostgreSQL sku name ')
param skuName string = 'Standard_B1ms'

@description('Azure Database for PostgreSQL pricing tier')
param skuTier string = 'Burstable'

@description('Storage Size for Postgres Database')
param postgresStorageSizeGB int = 32

@description('PostgreSQL version')
@allowed([
  '9.5'
  '9.6'
  '10'
  '10.0'
  '10.2'
  '11'
  '14'
])
param postgresqlVersion string = '14'

@description('PostgreSQL Server backup retention days')
param backupRetentionDays int = 7

@description('Geo-Redundant Backup setting')
param geoRedundantBackup string = 'Disabled'

// WebApp resources

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: sku
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webAppPortal 'Microsoft.Web/sites@2022-03-01' = {
  name: webAppPortalName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      ftpsState: 'FtpsOnly'
    }
    httpsOnly: true
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource server 'Microsoft.DBforPostgreSQL/flexibleServers@2022-12-01' = {
  name: serverName
  location: location
  sku: {
    name: skuName
    tier: skuTier
  }
  properties: {
    createMode: 'Default'
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
    storage: {
      storageSizeGB: postgresStorageSizeGB
    }
    backup: {
      backupRetentionDays: backupRetentionDays
      geoRedundantBackup: geoRedundantBackup
    }
    version: postgresqlVersion
  }
}

resource connectionstrings 'Microsoft.Web/sites/config@2022-03-01' = {
  name: 'connectionstrings'
  kind: 'string'
  parent: webAppPortal
  properties: {
      PostgresDefaultConnection: {
        value: 'Database=postgres; Server=${serverName}.postgres.database.azure.com; User Id=${administratorLogin}; Password=${administratorLoginPassword}; SslMode=Require; Trust Server Certificate=true;'
        type: 'Custom'
      }
  }
}

resource symbolicname 'Microsoft.Web/sites/config@2022-03-01' = {
  name: 'appsettings'
  kind: 'string'
  parent: webAppPortal
  properties: {
      ASPNETCORE_ENVIRONMENT: 'Staging'
  }
}

resource allowAccessToAzureServices 'Microsoft.DBforPostgreSQL/flexibleServers/firewallRules@2022-12-01' = {
  name: 'allow-access-to-azure-services'
  parent: server
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}
