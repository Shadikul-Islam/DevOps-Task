## <p align=center> Task-3 <br> <br> </p>

| **SL** | **Topic** |
| --- | --- |
| 01 | [Introduction](#01) |
| 02 | [Application Setup and Dockerize the Application & Database](#02) |
| 03 | [Create Image from Container and Push into Dockerhub](#03) |
| 04 | [Deploy into Kubernetes Cluster](#04) |
| 05 | [Network Connection Within the Cluster](#05) |

### <a name="01">:diamond_shape_with_a_dot_inside: &nbsp;Introduction</a>
We will first dockerize an ASP.NET Core Application with Microsoft SQL Server Database then we will push the images into dockerhub repository. And finally we will Deploy ASP.NET Core Application with Microsoft SQL Server Database in Kubernetes Cluster in **Azure Kubernetes Service**. So let's begin.

### <a name="02">:diamond_shape_with_a_dot_inside: &nbsp;Application Setup and Dockerize the Application & Database</a>
**Application Setup:** 
- I  have prepared a sample project for this task. [Click Here](https://github.com/Shadikul-Islam/DevOps-Task/tree/master/Task-3/Sample-Project) for the sample project files.
- I have added the database credentials in the code base of this project. So my project is completely ready to dockerize.

**Dockerize the Application:** 
- This portion of the lines will create an intermediate image from the base image and expose the ports.
```
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base 
WORKDIR /app
EXPOSE 80
EXPOSE 443
```
- This portion of the lines will copy the code into the working source directory, restore the nugets packages, build the code and publish the code into Publish directory.
````
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./K8STestApp.csproj", "./"]
RUN dotnet restore "./K8STestApp.csproj"
COPY . .
WORKDIR "/src/K8STestApp"
RUN dotnet build "/src/K8STestApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/K8STestApp.csproj" -c Release -o /app/publish
````
- This portion of the lines will copy the publish directory into the final directory and it will create the final production-ready image which runs when the container is started running.
````
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "K8STestApp.dll"]
````
Let's see the full Dockerfile.
````
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./K8STestApp.csproj", "./"]
RUN dotnet restore "./K8STestApp.csproj"
COPY . .
WORKDIR "/src/K8STestApp"
RUN dotnet build "/src/K8STestApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/K8STestApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "K8STestApp.dll"]
````

**Dockerize the Database:** 
Now we need to setup the Database Dockerfile to build the database.
- Run this command ````vi dbDockerfile```` to open my Dockerfile that was created for the database.
- This Dockerfile will create an **MSSQL Server** image, set **home** as a working directory, and copy the database backup file into that directory.
````
FROM mcr.microsoft.com/mssql/server
WORKDIR /home
COPY ./dbbackup.bak .
````

**Docker-Compose File Setup:**
By running this command: ````vi docker-compose.yml```` you can open the docker-compose and you can see my docker-compose file of this project. Let's discuss in detail what the docker-compose.yml file will do.

- This portion of the lines is for the web application. It will build the **Dockerfile** and open the port **80** and keep the application container under a network named **app-network**. It will be dependent on **db**.
````
version: "3.9"
services:
    web:
        build: .
        ports:
            - "80:80"
        depends_on:
            - db
        networks:
            - app-network
````
- This portion of the lines is for the database. It will build the **dbDockerfile**. Set the Database **sa** password. Open port **1433** and keep the database container under a network named **app-network**.

````
    db:
        build:
           context: ./
           dockerfile: dbDockerfile
        environment:
            SA_PASSWORD: ${SA_PASSWORD}
            ACCEPT_EULA: ${ACCEPT_EULA}
        ports:
         - "1433:1433"
        networks:
            - app-network
````

- This portion of the lines is defying the Network as a **bridge network** for the container of the **app-network**.

````
networks:
  app-network:
    driver: bridge
````

**Build and Up the Docker-Compose:**
Now all the necessary files are ready. It's time to build and up our docker-compose file to create the image and run the container. 
- Run this command to do that: ````docker-compose up -d --build````.
- Run this command to show the list of running containers: ````docker ps````. There are two containers that will be running, One is the web app container and the other one is the database.
- Now go to your browser and enter your server IP or localhost then hit enter button. You will see the application.
Our web application is running successfully.

### <a name="03">:diamond_shape_with_a_dot_inside: &nbsp;Create Image from Container and Push into Dockerhub</a>
**Create Image from Container:**
We have the application and database containers which is perfectly working for our project. Now we will create our own application image from those containers. To do that we need to follow the following steps:
- First, run this command ````docker ps```` to check the running containers.
- Run this command to create an image from the container: ````docker commit ContainerName ImageName````. We need to do this for application and db container. Here our application image name will be **dot-net-core-app** and the database image name will be **dot-net-core-db**.
- Our application image has been created. We can check it by running this command: ````docker images````.
- Now we have to give the tag of both images. Run this command to give a tag: ````docker image tag ImageName DockerHubUserName/ImageTag:Version````. In our case it will be for the web image: ````docker image tag dot-net-core-app shadikul/dot-net-core-app:v1````. For DB image: ````docker image tag dot-net-core-db shadikul/dot-net-core-db:v1````. After running this command, Images will be created with the tag. You can see this by running the ````docker images```` command

We have successfully created our containers image and upload it into my dockerhub repository. Now it's time to add them into kubernetes cluster. Let's do this.

### <a name="04">:diamond_shape_with_a_dot_inside: &nbsp;Deploy into Kubernetes Cluster</a>
- As we will use AKS, so let's setup our AKS first.
  - Deploy Azure Kubernetes Service from Azure portal. Install kubectl locally using the ```az aks install-cli``` command.
  - Configure kubectl to connect to your Kubernetes cluster using this ```az aks get-credentials --resource-group myResourceGroup --name myAKSCluster``` command.
  - Verify the connection to the cluster using the ```kubectl get nodes``` command. This command returns a list of the cluster nodes.
- Our k8s environment is ready. Now We will prepare a deployment manifest to create pods. We need to create two pods, the first one is for the application and the second one is for the database.

  **Deployment-App.yml:** 

````
apiVersion: apps/v1
kind: Deployment
metadata:
  name: dotnetapp
spec:
  selector:
    matchLabels:
      name: myapp
  replicas: 1
  template:
    metadata:
      labels:
        name: myapp
    spec:
      containers:
      - name: dotnetappcon
        image: shadikul/dot-net-core-app:v1
        resources:
        ports:
        - containerPort: 80
````
- **Deployment-DB.yml:**

````
apiVersion: apps/v1
kind: Deployment
metadata:
  name: dotnetdb
spec:
  selector:
    matchLabels:
      name: myapp
  replicas: 1
  template:
    metadata:
      labels:
        name: myapp
    spec:
      containers:
      - name: db
        image: shadikul/dot-net-core-db:v1
        resources:
        ports:
        - containerPort: 1433
````
- Now run this command to apply this manifest into Kubernetes: ````kubectl apply -f Deployment-App.yml```` and ````kubectl apply -f Deployment-DB.yml````. We can see the pods by running this ```kubectl get pods``` command.
  <br> <br> <img src= "https://github.com/Shadikul-Islam/DevOps-Task/blob/master/Task-3/Images/Image-Pods.png" alt="List of pods"> <br>
- **Prepare a NodePort YAML File**
  Now our second step is to prepare a NodePort manifest file. NodePort will create a network connection between pods and give the advantage to browse it from the internet using its defined port.

   **NodePort.yml:**
````
apiVersion: v1
kind: Service
metadata:
  name: dotnet-nodeport
spec:
  selector:
    name: myapp
  type: NodePort
  ports:
  - name: http
    port: 80
    targetPort: 80
    nodePort: 30000
    protocol: TCP
  - name: db
    protocol: TCP
    port: 1433
    targetPort: 1433
    nodePort: 30001
````
- Now run this command to apply this manifest into Kubernetes: ````kubectl apply -f NodePort.yml````.
- You can see your application from the browser. Open Chrome or any browser and visit your IP address with NodePort. In my case, my IP address is _**20.127.49.237**_, and I have defined Nodeport ports: _**30000**_. So my full address will be _http://20.127.49.237:30000/_. You see that browser is loading our application successfully.
<br> <br> <img src= "https://github.com/Shadikul-Islam/DevOps-Task/blob/master/Task-3/Images/Image-Nodeport.png" alt="Application in NodePort"> <br>
- **Prepare a Ingress YAML File:**
  Setup the Ingress in Kubernetes: ````minikube addons enable ingress````.
  
  **Ingress.yml**
````
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: dotnetingress
  labels:
    name: myapp
spec:
  rules:
  - host:
    http:
      paths:
      - pathType: Prefix
        path: "/"
        backend:
          service:
            name: dotnet-nodeport
            port:
              number: 80
````
- Now run this command to apply this manifest into Kubernetes: ````kubectl apply -f Ingress.yml````.
- Open your browser and visit your IP address without any port you can see your application which is running default port 80. In my case, it appeared like this:
<br> <br> <img src= "https://github.com/Shadikul-Islam/DevOps-Task/blob/master/Task-3/Images/Image-Ingress.png" alt="Application in Ingress">
- **Prepare Database Pod:** 
  Open SSMS and connect with database. Restore the database that I already stored in the container image.
- After successfully restoring the database we need to login to check whether it works or not. On the application login page provide the **Username: _Sadik_** and **Password: _admin_**. 
<br> <br> <img src= "https://github.com/Shadikul-Islam/DevOps-Task/blob/master/Task-3/Images/Image-Before-DB-Connect.png" alt="Main Page"> <br>
- These credentials will fetch and check from the database and redirect you to the next page.
<br> <br> <img src= "https://github.com/Shadikul-Islam/DevOps-Task/blob/master/Task-3/Images/Image-After-DB-Connect.png" alt="Main Page">

This is the main page of this sample application.

### <a name="05">:diamond_shape_with_a_dot_inside: &nbsp;Network Connection Within the Cluster</a> 
Networking is a central part of Kubernetes, but it can be challenging to understand exactly how it is expected to work. There are 4 distinct networking problems to address:

**Highly-coupled container-to-container communications:** this is solved by Pods and **localhost** communications.

**Pod-to-Pod communications:** Every pod has its own IP. So they communicate with each other with the help of IP.

**Pod-to-Service communications:** this is covered by services. Service mean Nodeport, ClusterIP etc.

**External-to-Service communications:** this is covered by services. Service mean Ingress.

<br> <img src= "https://github.com/Shadikul-Islam/DevOps-Task/blob/master/Task-3/Images/Cluster-Network.png" alt="Cluster-Network">
