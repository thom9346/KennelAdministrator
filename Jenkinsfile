pipeline {
    agent any

    triggers {
        pollSCM("* * * * *")
    }
    stages {
        stage("Build") {
            steps {
                bat "docker-compose build"
            }
        }
        stage("Deliver") {
            steps {
                withCredentials([usernamePassword(credentialsId: 'DockerHub', usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD')]) {
                    bat 'docker login -u %USERNAME% -p %PASSWORD%'
                    bat "docker-compose push"
                }
            }
        }
        stage("Deploy to Docker Swarm") {
            steps {
               // Update the kennel-db service
                bat 'docker service update --network-add my-network --image mcr.microsoft.com/mssql/server:2019-latest kennel-db || docker service create --name kennel-db --network my-network --env SA_PASSWORD=8zWt4!LmR6@kC3#eQb0 --env ACCEPT_EULA=Y --publish 1433:1433 mcr.microsoft.com/mssql/server:2019-latest'

                bat "docker service update --network-add my-network --image thohol02/kennelapp-backendapi:$BUILD_NUMBER kennelapi || docker service create --name kennelapi --network my-network --env ASPNETCORE_ENVIRONMENT=Development --env ASPNETCORE_URLS=http://+:80 --env \"ConnectionStrings__MainDB=Server=kennel-db;Database=KennelDB;User Id=sa;Password=8zWt4!LmR6@kC3#eQb0;TrustServerCertificate=true;Connection Timeout=30\" --publish 5000:80 --replicas 1 thohol02/kennelapp-backendapi:$BUILD_NUMBER"

                // Update the angular-app service
                bat "docker service update --network-add my-network --image thohol02/kennelapp-frontend:$BUILD_NUMBER angular-app || docker service create --name angular-app --network my-network --publish 4200:4200 --replicas 1 thohol02/kennelapp-frontend:$BUILD_NUMBER"
            }
        }

    }
}
