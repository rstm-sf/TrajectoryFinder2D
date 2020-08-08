![License](https://img.shields.io/github/license/rstm-sf/TrajectoryFinder2D.svg) ![netcoreapp3.1](https://img.shields.io/badge/.NET%20Core-3.1-brightgreen.svg) ![repo-size](https://img.shields.io/github/repo-size/rstm-sf/TrajectoryFinder2D.svg)

# TrajectoryFinder2D

<img src="./assets/sample.gif" width="600">

## About

A cross-platform application for finding the 2D trajectory of an object, sending a signal to 3 points.

We know the following:
- the period of message sending;
- the velocity of signal travel;
- the time it took for the signal to reach the points.

## How build it?

An easy way to build from source code is to use the [Git](https://git-scm.com/downloads) and the [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) as follows:

```sh
$ git clone https://github.com/rstm-sf/TrajectoryFinder2D.git && cd TrajectoryFinder2D

$ dotnet publish -c=Release src/TrajectoryFinder2D/TrajectoryFinder2D.csproj
$ dotnet src/TrajectoryFinder2D/bin/Release/netcoreapp3.1/publish/TrajectoryFinder2D.dll
```
