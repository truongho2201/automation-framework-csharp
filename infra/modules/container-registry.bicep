param location string
param registryName string
param registrySku string
param tags object

resource registry 'Microsoft.ContainerRegistry/registries@2026-11-01' = {
  name: registryName
  location: location
  tags: tags
  sku: {
    name: registrySku
  }
  properties: {
    adminUserEnabled: false
  }
}

output registryId string = registry.id
output loginServer string = registry.properties.loginServer