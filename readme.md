NuGet Packages for [Boost](http://boost.org).

[![NuGet package](https://img.shields.io/nuget/dt/boost.svg)](https://nuget.org/packages/boost)

# For Developers

## Building Boost

1. Download Boost library from [boost.org](http://boost.org/).
2. Unpack the Boost library in the parent folder of getboost. 
3. Rename it to `boost`.
4. Run [boost.bat](boost.bat). It may take several hours to complete.

## Building NuGet Packages

1. Open [builder\builder.sln](builder/builder.sln) in Visual Studio 2013 or higher.
2. Build and run the [builder](builder/builder/builder.csproj) project. It may take about half an hour to complete.
3. Find NuGet packages in the `builder\builder\bin\Debug\` folder.
