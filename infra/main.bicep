targetScope = 'subscription'

@minLength(1)
@maxLength(64)
param environmentName string

@minLength(1)
param location string

param sessionId string
param deployedBy string
param createdAt string
param resourceGroupName string
param logAnalyticsWorkspaceName string
param identityName string
param registryName string
param registrySku string
param logAnalyticsSku string
param applicationInsightsName string
param containerAppsEnvironmentName string
param containerAppsJobName string
param entraAdminObjectId string = ''
param entraAdminName string = 'Truong Ho Dac'
param containerImage string = 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest'

var tags = {
  'app-onboard-skill': 'true'
  'app-onboard-session-id': sessionId
  'created-at': createdAt
  environment: environmentName
  'deployed-by': deployedBy
}

resource rg 'Microsoft.Resources/resourceGroups@2026-06-01' = {
  name: resourceGroupName
  location: location
  tags: tags
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2026-03-01' existing = {
  name: logAnalyticsWorkspaceName
  scope: rg
}

module identity './modules/managed-identity.bicep' = {
  name: 'seleniumIdentity'
  scope: rg
  params: {
    location: location
    identityName: identityName
    tags: tags
  }
}

module registry './modules/container-registry.bicep' = {
  name: 'seleniumRegistry'
  scope: rg
  params: {
    location: location
    registryName: registryName
    registrySku: registrySku
    tags: tags
  }
}

module logs './modules/log-analytics.bicep' = {
  name: 'seleniumLogs'
  scope: rg
  params: {
    location: location
    workspaceName: logAnalyticsWorkspaceName
    workspaceSku: logAnalyticsSku
    tags: tags
  }
}

module insights './modules/application-insights.bicep' = {
  name: 'seleniumInsights'
  scope: rg
  params: {
    location: location
    componentName: applicationInsightsName
    workspaceResourceId: logs.outputs.workspaceId
    tags: tags
  }
}

module environment './modules/container-apps-environment.bicep' = {
  name: 'seleniumEnvironment'
  scope: rg
  params: {
    location: location
    environmentResourceName: containerAppsEnvironmentName
    workspaceCustomerId: logs.outputs.customerId
    workspaceSharedKey: logAnalyticsWorkspace.listKeys().primarySharedKey
    tags: tags
  }
}

module job './modules/container-apps-job.bicep' = {
  name: 'seleniumJob'
  scope: rg
  dependsOn: [
    roleAssignments
  ]
  params: {
    location: location
    jobName: containerAppsJobName
    environmentResourceId: environment.outputs.environmentId
    identityResourceId: identity.outputs.identityId
    registryLoginServer: registry.outputs.loginServer
    containerImage: containerImage
    tags: tags
  }
}

module roleAssignments './modules/role-assignments.bicep' = {
  name: 'seleniumRoleAssignments'
  scope: rg
  dependsOn: [
    registry
  ]
  params: {
    registryName: registryName
    principalId: identity.outputs.principalId
  }
}

output resourceGroupId string = rg.id
output jobId string = job.outputs.jobId
output registryLoginServer string = registry.outputs.loginServer
output deploymentIdentity object = {
  objectId: entraAdminObjectId
  name: entraAdminName
}