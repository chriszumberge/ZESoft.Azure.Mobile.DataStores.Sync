# ZESoft.Azure.Mobile.DataStores.Sync
Interfaces and abstractions for performing off-line Data Syncs with Azure Easy Tables and WebApi Table Controllers.

## Documentation
Get started by reading through the [Azure Mobile Sync Data Stores Documentation](docs/README.md)

## NuGet
Available on NuGet: (Coming soon)

## Builds and Tests
### Master Branch
[![Build status](https://ci.appveyor.com/api/projects/status/fns9eh9vdlaj7cg3/branch/master?svg=true)](https://ci.appveyor.com/project/chriszumberge/zesoft-azure-mobile-datastores-sync/branch/master)

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/bd69175718a448079095ff4e5c30c84a)](https://www.codacy.com/app/chriszumberge/ZESoft.Azure.Mobile.DataStores.Sync?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=chriszumberge/ZESoft.Azure.Mobile.DataStores.Sync&amp;utm_campaign=Badge_Grade)

CI NuGet Feed

### Develop Branch
[![Build status](https://ci.appveyor.com/api/projects/status/fns9eh9vdlaj7cg3/branch/develop?svg=true)](https://ci.appveyor.com/project/chriszumberge/zesoft-azure-mobile-datastores-sync/branch/develop)

### Platform Support
|Platform       | Version  |
|---------------|:--------:|
|.Net           |   4.5+   |
|.Net Core      |   2.0+   |
|.Net Standard  |   2.0+   |
|Xamarin.iOS    |  iOS 7+  |
|Xamarin.Android|  API 14+ |

## Setup
Install into PCL/.Net project
- Available on NuGet
- Download NuGet package in distribution (dist) folder
- Download project and reference the .csproj

## API Usage
[ZESoft.Azure.Mobile.DataStores.Sync API Documentation](docs/ApiDocumentation.md)

### Developer Tips

## Contribute
The library is open source so pull requests are encouraged.
- Report bugs by opening an issue
- Submit feature requests by opening an issue
- Fix bugs or add features by sending a pull request

### ToDos 

### Project Roadmap

### Coding Conventions
In general, follow the style used by the [.Net Foundation](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md)
with the following exceptions:
- Preference to use [expression bodied functions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#methods)
and [properties](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#property-get-statements)
where applicable
- Do not use the ```private``` keyword as it is the default accessibility level
- Hard tabs over spaces, *always*

Taking special note of:
- [Allman style](https://en.wikipedia.org/wiki/Indent_style#Allman_style) braces
- Use _camelCase for internal/private fields
- Use ```readonly``` where possible
- Prefix instance fields with ```_```
- Static fields start with ```s_```
- Fields should be specified at the top within type declarations
- Use ```nameof(...)``` whenever possible
- Avoid ```this.``` whenever possible
- Only use ```var``` when it's obvious what the variable type is
- Use PascalCasing to name constant variables and fields

### Architecture Conventions
- All services must implement an IService interface for both dependency injection and easier testing
- Where possible, services should have a configuration interface that a developer can implement to customize the service
- Where possible, projects should target as many platforms as possible (via .Net Standard or PCL)

### Project Conventions
- Sort and remove all assembly references