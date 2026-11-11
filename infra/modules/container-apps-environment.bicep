param location string
param environmentResourceName string
param workspaceCustomerId string
@secure()
param workspaceSharedKey string
param tags object

resource environment 'Microsoft.App/managedEnvironments@2026-07-01' = {
  name: environmentResourceName
  location: location
  tags: tags
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: workspaceCustomerId
        sharedKey: workspaceSharedKey
      }
    }
  }
}

output environmentId string = environment.id