copy ..\..\dotlog\bin\Release\DotLog.dll lib\net40
copy ..\..\dotlog\bin\Release\DotLog.Sql.dll lib\net40
set VERSION=1.3.2
nuget pack DotLog.nuspec
nuget push DotLog.%VERSION%.nupkg
