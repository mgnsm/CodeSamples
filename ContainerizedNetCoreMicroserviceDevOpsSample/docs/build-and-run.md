# Build and Run
You can build and run the sample microservice app in this example from the command-line on your development machine using either [Docker Desktop](https://docs.docker.com/desktop/) or the latest version of the [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1?WT.mc_id=DOP-MVP-5001077). 

You can also build and run the source code using Visual Studio 2019 or the latest version of Visual Studio for Mac that supports the .NET Core 3.1 SDK, with or without Docker support. The instructions under the **Visual Studio** section below apply primarily to Windows.

The first thing you should do is to [clone](https://docs.github.com/en/github/creating-cloning-and-archiving-repositories/cloning-a-repository) or download this repository to a local folder on your computer:

    git clone https://github.com/mgnsm/CodeSamples.git

## Docker

1. Build the Docker image using the `docker build` command:

        docker build -f "ContainerizedNetCoreMicroserviceDevOpsSample\src\Containerized.Microservice\Dockerfile.Linux"
          -t mgnsm/containerized.microservice:dev  
          ./ContainerizedNetCoreMicroserviceDevOpsSample/src 
          --build-arg version=1.0.0

    **Note:** If Docker is set up to use Windows containers, you should either switch to Linux containers before running the above command or change the name of the Dockerfile specified by the `-f` option to run the microservice in a Windows container:

        docker build -f "ContainerizedNetCoreMicroserviceDevOpsSample\src\Containerized.Microservice\Dockerfile.Windows"
          -t mgnsm/containerized.microservice:dev
          ./ContainerizedNetCoreMicroserviceDevOpsSample/src
          --build-arg version=1.0.0

2. Start the container using the `docker run` command:

        docker run -d --name containerized.microservice -p 5000:80 mgnsm/containerized.microservice:dev

3. Verify that the container is up and running by browsing to http://localhost:5000.

4. Run the `docker stop` command to shutdown the app:

        docker stop containerized.microservice

5. Remove the container and images to clean up resources:

        docker rm containerized.microservice
        docker rmi mgnsm/containerized.microservice:dev
        docker image prune -f

## .NET Core CLI

1. Build the solution using the `dotnet build` command:

        dotnet build ContainerizedNetCoreMicroserviceDevOpsSample/src/ContainerizedMicroserviceSample.sln -c release -p:Version=1.0.0

2. Optionally run the unit tests using `dotnet test` command:

        dotnet test ContainerizedNetCoreMicroserviceDevOpsSample/src/ContainerizedMicroserviceSample.sln -c release --no-build

3. Start the microservice using the `dotnet run` command:

        dotnet run -p ContainerizedNetCoreMicroserviceDevOpsSample/src/Containerized.Microservice/Containerized.Microservice.csproj
          -c release --no-build --urls=http://localhost:5000/

4. Open up a web browser and browse to http://localhost:5000 to verify that the microservice is running.

5. Press Ctrl + C to shutdown the app.

## Visual Studio

1. Open the `ContainerizedNetCoreMicroserviceDevOpsSample/src/ContainerizedMicroserviceSample.sln` solution file and build it using the **Build**->**Build Solution** option on the menu bar, or by right-clicking on the solution in the **Solution Explorer** and choose **Build Solution**.

2. Right-click on the `Containerized.Microservice` project in the **Solution Explorer** and choose **Set as Startup Project**.

3. Visual Studio automatically creates a *launchSettings.json* file that contains profile settings and is intended to be used on the local development machine only. By default, it uses the `IISExpress` profile with SSL enabled to launch the app that is set as the startup project. Since the sample microservice is not set up to use SSL, you should disable it by either editing the `Properties/launchSettings.json` file in the project folder (`ContainerizedNetCoreMicroserviceDevOpsSample/src/Containerized.Microservice`) directly, or by unchecking the **Enabled SSL* checkbox in the project properties **Debug** tab.

    Alternatively, you can select the `Containerized.Microservice` profile to launch the microservice using the Kestrel web server. This is the profile that the `dotnet run` CLI uses by default. 

    There is also a `Docker` profile that you can select to run the microservice in a Docker container (assuming you have installed Docker Desktop). The project is configured to run in Linux containers by default. If Docker is configured to use Windows containers, and you don't want to switch to Linux containers, you can edit the the `DockerDefaultTargetOS` and `DockerfileFile` properties in the `ContainerizedNetCoreMicroserviceDevOpsSample\src\Containerized.Microservice\Containerized.Microservice.csproj.csproj` project file before you run the app:

    <DockerDefaultTargetOS>Windows</DockerDefaultTargetOS>
    <DockerfileFile>Dockerfile.Windows</DockerfileFile>

    Visual Studio will create a `.csproj.user` file in the project folder to store your launch settings and apply them the next time you open the solution.

4. Press F5 (or choose **Debug**->**Start Debugging** on the menu bar) to run the app with the debugger attached. Subject to the value of the `launchBrowser` setting in the *launchSettings.json* file, a web browser should be launched automatically. It's `true` by default. 

    Press Ctrl + F5 (or choosing **Debug**->**Start Without Debugging** on the menu bar) to start without debugging.