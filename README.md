# DotLog
Minimalist logging library for .NET

- <a href="https://nuget.org/packages/DotLog/">Install via Nuget</a>
- <a href="https://github.com/tonypujals/dotlog">Source on Github</a>

## Overview

There are a number of great logging options for .NET applications, but sometimes all you need is a quick and easy way to add basic log file support to a prototype application or service without a lot of extra baggage.

The goal for DotLog is to provide a simple, lightweight solution that makes it easy to add file logging to your project while not making it difficult to switch to another logging framework as your application's requirements evolve.

DotLog only provides two loggers: a trivial console logger and a session-based file logger. There is no dependency on any other framework, unless you choose to use the available <a href="http://netcommon.sourceforge.net/">Common.Logging</a> adapter to make it easier to switch to another logging framework in the future. Commons.Logging is a logging facade that supports a number of .NET logging solutions, such as Microsoft Enterprise Logging, NLog, and log4net.

Even without using the Common.Logging adapter, if it ever becomes necessary to use another logging
framework, a custom adapter can easily be created by implementing the ILogger interface.

## Console Logger
The main advantage of DotLog is that it is trivial to quickly add file logging support to an application under development. However, for convenience, DotLog also includes a simple console logger.

```javascript
using DotLog;
var logger = new ConsoleLogger();
```

## File Logger 
Each new instance of a file logger creates a new sequentially numbered log file in a <code>logs</code> subdirectory of the executing process. If you want the same log file used during process execution, ensure that all logging calls go through the same logger instance.

There is no static Log class or static log method. Ideally, applications should create concrete logger instances via a factory or dependency injection. However, if you choose to use the Common.Logging API adapter, it does provide a static LogManager class that returns the same singleton instance.

An example of using an IOC container or a factory class might look like the following:

<code>var logger = Container.Resolve<ILogger>();</code>

or

<code>var logger = LogFactory.CreateLogger();</code>

```javascript
// create a file logger; by default this will create a serial log file
// that will continue logging to the same file for the life of this logger
// (to support session based logging).
var logger = new FileLogger();

// provide a file name template. This example will create logs
// named testlog-001.log, testlog-002.log, etc.
logger.FileNameTemplate = "testlog-###.log"
```

## Creating your own logger
A quick way to create a logger is to create an instance of the Logger class. The constructor takes an <code>Action&lt;string&gt;</code>, which is invoked for the final formatted output of any log message that has not been filtered out by log level.

For example, here is how a console logger can be created:

```javascript
// create a logger that outputs to the console using
// default log level (Information)
var logger = new Logger(Console.WriteLine);
```


## More Examples

```javascript
// specify the log level (this example will write all entries
// from verbose through critical)
var logger = new ConsoleLogger { LogLevel = LogLevel.Verbose };

// log something
logger.Log("test log message");

// the default log level is LogLevel.Verbose, so this does the same thing:
logger.Log("test log message", LogLevel.Verbose);

// Specifiy a category when logging (the default is "General")
logger.Log("test log message", LogLevel.Verbose, "Test Category");
```

&copy; 2012, 2013, Tony Pujals. <a href="http://tonypujals.github.com/dotlog/">DotLog</a> is <a href="http://opensource.org/">Open Source</a> and available under the <a href="license.html">MIT License</a>
