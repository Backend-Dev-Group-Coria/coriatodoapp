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

@description('Azure Database for PostgreSQL compute capacity in vCores (2,4,8,16,32)')
param skuCapacity int = 1

@description('Azure Database for PostgreSQL sku name ')
param skuName string = 'B_Gen5_1'

@description('Azure Database for PostgreSQL Sku Size ')
param skuSizeMB int = 5120

@description('Azure Database for PostgreSQL pricing tier')
@allowed([
  'Basic'
  'GeneralPurpose'
  'MemoryOptimized'
])
param skuTier string = 'Basic'

@description('Azure Database for PostgreSQL sku family')
param skuFamily string = 'Gen5'

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
param postgresqlVersion string = '11'

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

resource server 'Microsoft.DBforPostgreSQL/servers@2017-12-01' = {
  name: serverName
  location: location
  sku: {
    name: skuName
    tier: skuTier
    capacity: skuCapacity
    size: '${skuSizeMB}'
    family: skuFamily
  }
  properties: {
    createMode: 'Default'
    version: postgresqlVersion
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
    storageProfile: {
      storageMB: skuSizeMB
      backupRetentionDays: backupRetentionDays
      geoRedundantBackup: geoRedundantBackup
    }
    publicNetworkAccess: 'Enabled'
  }
}

resource connectionstrings 'Microsoft.Web/sites/config@2022-03-01' = {
  name: 'connectionstrings'
  kind: 'string'
  parent: webAppPortal
  properties: {
      PostgresDefaultConnection: {
        value: 'Database=postgres; Data Source=backend-dev-group-postgres.postgres.database.azure.com; User Id=pr54rAdmin@backend-dev-group-postgres; Password=Password1'
        type: 'Custom'
      }
  }
}
