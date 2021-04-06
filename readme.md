NuGet Packages for [Boost](http://boost.org).

- [![NuGet package](https://img.shields.io/nuget/v/boost.svg?label=boost)](https://nuget.org/packages/boost)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc142.svg?label=boost-vc142)](https://nuget.org/packages/boost-vc142)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc141.svg?label=boost-vc141)](https://nuget.org/packages/boost-vc141)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc140.svg?label=boost-vc140)](https://nuget.org/packages/boost-vc140)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc120.svg?label=boost-vc120)](https://nuget.org/packages/boost-vc120)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc110.svg?label=boost-vc110)](https://nuget.org/packages/boost-vc110)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc100.svg?label=boost-vc100)](https://nuget.org/packages/boost-vc100)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc90.svg?label=boost-vc90)](https://nuget.org/packages/boost-vc90)
- [![NuGet package](https://img.shields.io/nuget/v/boost-vc80.svg?label=boost-vc80)](https://nuget.org/packages/boost-vc80)
- [![NuGet package](https://img.shields.io/nuget/v/boost-src.svg?label=boost-src)](https://nuget.org/packages/boost-src)

# Releases

- [1.75](releases/1.75.md)
- [1.74](releases/1.74.md)
- [1.73](releases/1.73.md)
- [1.72](releases/1.72.md)
- [1.71](releases/1.71.md)
- [1.70](releases/1.70.md)
- [1.69](releases/1.69.md)
- [1.68](releases/1.68.md)
- [1.67](releases/1.67.md)
- [1.66](releases/1.66.md)
- [1.65.1](releases/1.65.1.md)
- [1.65](releases/1.65.md)
- [1.64](releases/1.64.md)
- [1.63](releases/1.63.md)
- [1.62](releases/1.62.md)
- [1.61](releases/1.61.md)

# For Developers

## Building Boost

1. Download Boost library from [boost.org](http://boost.org/).
2. Unpack the Boost library in the parent folder of getboost.
3. Rename it to `boost`.
4. Run [boost.bat](boost.bat). It may take several hours to complete.

## Building NuGet Packages

1. Open [builder\builder.sln](builder/builder.sln) in Visual Studio 2017 or higher.
2. Build and run the [builder](builder/builder/builder.csproj) project. It may take about half an hour to complete.
3. Find NuGet packages in the `builder\builder\bin\Debug\` folder.
