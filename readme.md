NuGet Packages for [Boost](http://boost.org).

- [![NuGet package](https://img.shields.io/nuget/v/boost.svg?label=boost)](https://nuget.org/packages/boost)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc140.svg?label=boost-vc140)](https://nuget.org/packages/boost-vc140)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc120.svg?label=boost-vc120)](https://nuget.org/packages/boost-vc120)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc110.svg?label=boost-vc110)](https://nuget.org/packages/boost-vc110)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc100.svg?label=boost-vc100)](https://nuget.org/packages/boost-vc100)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc90.svg?label=boost-vc90)](https://nuget.org/packages/boost-vc90)

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
