## <p align=center> Task-1 <br> <br> </p>

| **SL** | **Topic** |
| --- | --- |
| 01 | [Tools Selection](#01) |
| 02 | [Steps to Create Infrastructure](#02) |
| 03 | [Description of the mentioned topics from the task for the project perspective](#03) |


### <a name="01">:diamond_shape_with_a_dot_inside: &nbsp;Tools Selection</a>

In my opinion to resolve this task purpose I will use this following tools:

**For IAC:** To create Infrastructure I will use Terraform. Cause terraform is a opensource best Infrastructure as Code tool. We can easily prepare any size small or big infrastructure using this tools. Using terraform we can deploy infrastructure from any IaaS platform like AWS, Azure etc.

**For Configuration Management:** Now let assume that, We have completely setup the infrastructure. Now we need to configure the server. For example, install webserver, database server, others application or update existing application or services. To do this I will use Ansible/Chef. I will prefer ansible in this case cause it is matured tools which is used mostly in the world.

So to do this task I will use **Terraform** for IAC and **AWS** for a IaaS platform

### <a name="02">:diamond_shape_with_a_dot_inside: &nbsp;Steps to Create Infrastructure</a>

1. **AWS Authentication:** To authenticate I have used Programmatic API Keys (Access Key and Secret). This key I have created from AWS console.
2. **Terraform Files:** I have created two files. One is [main.tf](https://github.com/Shadikul-Islam/DevOps-Task/blob/master/Task-1/Main/main.tf) and other one is [provider.tf](https://github.com/Shadikul-Islam/DevOps-Task/blob/master/Task-1/Provider-Configuration/provider.tf). These two are my terraform configuration file to create infrastructure from aws.
3. **Terraform Files Apply:** 
   - Execute the command ```terraform init``` to initialize.
   - Execute the command ```terraform plan``` to check what change would be made.
   - If all things are good then execute ```terraform apply``` to commit and start the build.

4. After the apply command, We will get the **public ip** of the server. We can access that vm.

In this way we can deploy infrastructure via terraform.

### <a name="03">:diamond_shape_with_a_dot_inside: &nbsp;Description of the mentioned topics from the task for the project perspective</a>

**Microservice definition for the application:** In my opinion to do microservice for this application I will use docker and kubernetes. To containerized the application I will use **Docker**. Then to manage all of the container I will use orchestration tool **Kubernetes**. If my source code in the github then I can use **gitops** also. To CI/CD I will use **Jenkins** cause it is open source free tools, Otherwise I will use Azure DevOps which is a best ci/cd platform.

**Database solution for this application:** I will suggest to use **mysql** or **mongodb** to use as a database. I will tune the database to get extra performance. Also I will use replication of the database so that our database will divided by master and slave architecture so that it will give a better result. Also I will suggest **Redis** to use as a cache server and **kafka** or **Message Queue** for quing service.

**Hybrid Architecture:** Yes it's possible to store data on the on prem database. I will setup the on prem agent into cloud so that they will communicate each other and will stay in sync.

**Connector in Hybrid Architecture:** If our application in AWS then we can use **AWS Direct Connect** and if our application is in Azure then we can use **Microsoft Azure Express Route**.

**Loadbalancing, Single Point of Failure, Scalability, Fault Tolerance, Auto Recovery Considerations:** To handle all of these we can use **Kubernetes** service. Kubernetes can handle all of those things very easily. Kubernetes is an open source container orchestration platform that automates many of the manual processes involved in deploying, managing, and scaling containerized applications.

**Cost Effectiveness**: Most Cloud provider offer pay-as-you-go or subscription payment model. If you decide to subscribe, then you can realize economies of scale and, if you choose pay-as-you-go, then you do not need to pay a large amount of money in advance for your servers and racks.

**Security:** I will follow these steps for maintaining the security.
  - By using HTTPS by usning authenticate ssl
  - By using access and identity tokens
  - By Encrypting and protecting secrets
