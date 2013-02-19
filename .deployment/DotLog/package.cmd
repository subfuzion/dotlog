copy ..\..\dotlog\bin\Release\DotLog.dll lib\net40
set VERSION=1.3.0
nuget pack DotLog.nuspec
nuget push DotLog.%VERSION%.nupkg
