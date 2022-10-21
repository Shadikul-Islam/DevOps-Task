## <p align=center> Task-2 <br> <br> </p>

| **SL** | **Topic** |
| --- | --- |
| 01 | [My Analysis with the Existing Pipeline Design of this Task](#01) |
| 02 | [My Proposed Pipeline Design to Speed Up the Process](#02) |
| 03 | [Choose the Right Tools/Platform for CI/CD](#03) |

### <a name="01">:diamond_shape_with_a_dot_inside: &nbsp;My Analysis with the Existing Pipeline Design of this Task </a>

In this mentioned pipeline design in short what it desribing, we can see here code will be pulled out from git --> OWASP coding standard checking --> Analyzing the quality of the code --> Build the code and also run a unit testing --> Run an Integration testing --> And finally deploy it into target environment.

In this pipeline design I noticed that there is no **Notification System** allowed during the time of building, testing and others steps. Also I didn't not notice the **Containerization** system, **Orchestration** system. Moreover, there is no **Monitoring** system to monitor all of the things from one place. So let's describe all of those things in shortly.

### <a name="02">:diamond_shape_with_a_dot_inside: &nbsp;My Proposed Pipeline Design to Speed Up the Process </a>

1. **Notification System:** We can add notification system in necessery steps to checkout the result of that step. We can easily findout the errors/issues where it is generated and what the actual problem is. So it will help us to send a simple build or test result notification of our CI/CD pipeline behaviour to us via email, sms or any notification tools. So I want to add this steps in my CI/CD pipeline design.
2. **Containerization:** In the current time, microservice is a popular solution of medium and large scale applications. My suggestion will be use docker to containerize the application and make image of that application differnet services. Keep it in a central repository. I want to add all of this steps in my CI/CD pipeline design.
3. **Orchestration:** For Orchestration, I want to use kubernetes. Create pods using those services image. We can create replication of our pods to use them for high availability. We can rollback if anything happens wrong and many more by using kubernetes. So I want to add this steps too in my CI/CD pipeline design.
4. **Monitoring:** To keep our application running 24/7 without failure then we need to monitor our not only application but also our whole system/infrastructure. It will be added in my CI/CD pipeline design. 

Here is my sample ci/cd pipeline design prototype:

<img src= "https://github.com/Shadikul-Islam/DevOps-Task/blob/master/Task-2/Images/CI-CD%20Image.png" alt="CI/CD Image"> 

### <a name="03">:diamond_shape_with_a_dot_inside: &nbsp;Choose the Right Tools/Platform for CI/CD </a>

In my opinion, It's not enough to design a good ci/cd pipeline and execution. We also need to chose a good ci/cd tools or platform. Now this time there are a lot of tools and platform for ci/cd. My suggestion will be Jenkins, Gitlab CI/CD and Azure DevOps. I will prefer Azure DevOps platform to use CI/CD execution cause It has lot of feature and best support from microsoft.

So If we can prepare a good pipeline design with a good ci/cd plaform/tool, I hope we can easily maintain our applicaiton's faster continuous integration and continuous deployment without the mentioned problems of this task.
