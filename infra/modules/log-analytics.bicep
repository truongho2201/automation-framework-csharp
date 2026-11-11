param location string
param workspaceName string
param workspaceSku string
param tags object

resource workspace 'Microsoft.OperationalInsights/workspaces@2026-03-01' = {
  name: workspaceName
  location: location
  tags: tags
  properties: {
    retentionInDays: 30
    sku: {
      name: workspaceSku
    }
  }
}

output workspaceId string = workspace.id
output customerId string = workspace.properties.customerId