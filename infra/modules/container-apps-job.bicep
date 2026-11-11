param location string
param jobName string
param environmentResourceId string
param identityResourceId string
param registryLoginServer string
param containerImage string = 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest'
param tags object

var placeholderImage = 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest'
var isPlaceholder = containerImage == placeholderImage

resource job 'Microsoft.App/jobs@2026-07-01' = {
  name: jobName
  location: location
  tags: tags
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${identityResourceId}': {}
    }
  }
  properties: {
    environmentId: environmentResourceId
    configuration: {
      triggerType: 'Manual'
      replicaTimeout: 1800
      replicaRetryLimit: 0
      manualTriggerConfig: {
        parallelism: 1
        replicaCompletionCount: 1
      }
      registries: isPlaceholder ? [] : [
        {
          server: registryLoginServer
          identity: identityResourceId
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'main'
          image: containerImage
          resources: {
            cpu: 1
            memory: '2Gi'
          }
          env: [
            {
              name: 'SELENIUM_TestSettings__RemoteWebDriverUrl'
              value: 'http://localhost:4444'
            }
          ]
        }
        {
          name: 'selenium-chrome'
          image: 'selenium/standalone-chrome:latest'
          resources: {
            cpu: 2
            memory: '4Gi'
          }
        }
      ]
    }
  }
}

output jobId string = job.id