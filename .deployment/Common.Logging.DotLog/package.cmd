copy ..\..\Common.Logging.DotLog\bin\Release\Common.Logging.DotLog.dll lib\net40
set VERSION=1.2.0
nuget pack Common.Logging.DotLog.nuspec
nuget push Common.Logging.DotLog.%VERSION%.nupkg
