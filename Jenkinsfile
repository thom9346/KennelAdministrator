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
        stage("Prepare Network") {
            steps {
                script {
                    def networkExists = bat(script: 'docker network ls | findstr my-network', returnStatus: true) == 0
                    if (!networkExists) {
                        bat 'docker network create my-network'
                    }
                }
            }
        }
        stage("Deploy to Docker Swarm") {
            steps {
                script {
                    def serviceExists = bat(script: 'docker service ls | findstr kennel-db', returnStatus: true) == 0
                    if (serviceExists) {
                        bat 'docker service update --network-add my-network --image mcr.microsoft.com/mssql/server:2019-latest kennel-db'
                    } else {
                        bat 'docker service create --name kennel-db --network my-network --env SA_PASSWORD=8zWt4!LmR6@kC3#eQb0 --env ACCEPT_EULA=Y --publish 1433:1433 mcr.microsoft.com/mssql/server:2019-latest'
                    }
                }
                script {
                    def serviceExists = bat(script: 'docker service ls | findstr kennelapi', returnStatus: true) == 0
                    if (serviceExists) {
                        bat "docker service update kennelapi --image thohol02/kennelapp-backendapi:\$BUILD_NUMBER"
                    } else {
                        bat "docker service create --name kennelapi --network my-network --env ASPNETCORE_ENVIRONMENT=Development --env ASPNETCORE_URLS=http://+:80 --env \"ConnectionStrings__MainDB=Server=kennel-db;Database=KennelDB;User Id=sa;Password=8zWt4!LmR6@kC3#eQb0;TrustServerCertificate=true;Connection Timeout=30\" --publish 5000:80 thohol02/kennelapp-backendapi:\$BUILD_NUMBER"
                    }
                }
                script {
                    def serviceExists = bat(script: 'docker service ls | findstr angular-app', returnStatus: true) == 0
                    if (serviceExists) {
                        bat "docker service update angular-app --image thohol02/kennelapp-frontend:\$BUILD_NUMBER"
                    } else {
                        bat "docker service create --name angular-app --network my-network --publish 4200:4200 thohol02/kennelapp-frontend:\$BUILD_NUMBER"
                    }
                }
            }
        }
    }
}
