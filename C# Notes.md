# C# Notes

- How to run the program in C#?
    
    [Error Log](C#%20Notes%20f4923136476e4ab9b0c5797023901fb8/Error%20Log%20ac3bca70fe2046c89789379f33695bd9.md)
    
- How to run the tests in C#?

## General Code Fundamentals

- CLR
    - C# compiles into IL Code, and then translates this into Native Code
    - CLR runs our program from Main(string[] args)
- Class Library
- Namespace
    - is used to organise code and avoid naming conflicts. They are essentially capsules of information
        - These are then surrounded by DLL/EXE which are containers/assemblies for our namespaces

```csharp
using System
```

- Defining the namespace we’re using

```csharp
private TestEngine _testEngine;
private int _firstTaskId;
private int _secondTaskId;
```

- Private instance variables. This is a common convention in C# and other object-oriented programming languages, where it is considered good practice to prefix private fields with an underscore to distinguish them from local variables, method parameters, and other types of variables.
- By using the underscore prefix, developers can quickly identify which variables are private fields and which are not, which helps improve the readability and maintainability of the code.

```csharp
public class ClientsController : Controller
```

- Provides methods that respond to HTTP requests that are made to an [ASP.NET](http://asp.net/) MVC Web site.

```csharp
public sealed class UserService
```

- A sealed class, in C#, is **a class that cannot be inherited by any class but can be instantiated**

```csharp
new { Id = id } 
```

- syntax is an anonymous type used to pass in the id parameter as a parameter to the SQL query

## Program Code

```csharp
namespace ProjectKanban
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
```

- A namespace is used to organize code and avoid naming conflicts with other code.
- IHostBuilder takes the `args` array as input arguments and is used to configure and build a web host. ASP.NET Core apps configure and launch a *host*. The host is responsible for app startup and lifetime management
- This code sets up a basic web application using .NET Core and configures a web host to listen for incoming HTTP requests

```csharp
ProjectKanban.csproj
```

- This project file specifies the dependencies of the project, sets the target framework version, and includes the necessary files and resources to build a web application.

```csharp
[OneTimeSetUp]
```

- In NUnit, `[OneTimeSetUp]`is an attribute that marks a method to be run once before any of the tests in a test fixture are executed.