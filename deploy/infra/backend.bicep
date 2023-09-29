//Web Application Parameters

@description('Base name of the resource such as web app name and app service plan ')
@minLength(2)
param backendWebAppName string = 'coriatodo-backend'
param frontendWebAppName string = 'coriatodo-frontend'

@description('The SKU of App Service Plan ')
param sku string = 'B1'

@description('The SKU for the static site. https://docs.microsoft.com/en-us/azure/templates/microsoft.web/staticsites?tabs=bicep#skudescription')
param staticSku object = {
  name: 'Standard'
  tier: 'Standard'
}

@description('The Runtime stack of current web app')
param linuxFxVersion string = 'DOTNETCORE|7.0'
param frontendFxVersion string = 'node|16-lts'

@description('Allowed locations for backend webapp')
@allowed([
  'westeurope'
])

param location string = 'westeurope'

@description('Allowed locations for static website')
@allowed([
  'westus2'
  'centralus'
  'eastus2'
  'westeurope'
  'eastasia'
  'eastasiastage'
])
param staticLocation string = 'westeurope'

var backendWebAppPortalName = '${backendWebAppName}-webapp'
var backendAppServicePlanName = 'backendAppServicePlan-${backendWebAppName}'

var frontendWebAppPortalName = '${frontendWebAppName}-webapp'
var frontendAppServicePlanName = 'frontendAppServicePlan-${frontendWebAppName}'

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

// Backend WebApp resources

resource backendAppServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: backendAppServicePlanName
  location: location
  sku: {
    name: sku
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource backendWebAppPortal 'Microsoft.Web/sites@2022-03-01' = {
  name: backendWebAppPortalName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: backendAppServicePlan.id
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

// Frontend WebApp resources

resource frontendAppServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: frontendAppServicePlanName
  location: location
  sku: {
    name: sku
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource frontendWebAppPortal 'Microsoft.Web/sites@2022-03-01' = {
  name: frontendWebAppPortalName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: frontendAppServicePlan.id
    siteConfig: {
      linuxFxVersion: frontendFxVersion
      ftpsState: 'FtpsOnly'
      appCommandLine: 'node .output/server/index.mjs'      
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
  parent: backendWebAppPortal
  properties: {
      PostgresDefaultConnection: {
        value: 'Database=postgres; Server=${serverName}.postgres.database.azure.com; User Id=${administratorLogin}; Password=${administratorLoginPassword}; SslMode=Require; Trust Server Certificate=true;'
        type: 'Custom'
      }
  }
}

resource backendAppSettings 'Microsoft.Web/sites/config@2022-03-01' = {
  name: 'appsettings'
  kind: 'string'
  parent: backendWebAppPortal
  properties: {
      ASPNETCORE_ENVIRONMENT: 'Staging'
  }
}

resource frontendAppSettings 'Microsoft.Web/sites/config@2022-03-01' = {
  name: 'appsettings'
  kind: 'string'
  parent: frontendWebAppPortal
  properties: {
    WEBSITES_PORT: '8081'
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
