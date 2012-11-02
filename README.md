# dotlog

## Minimalist logging library for .NET

There are a number of good logging options for .NET applications, and if your needs are
sophisticated, they can be indispensable.

On the other hand, there are times when a really simple logging library can be useful.
This library was designed to make it easy to log to the console or to a rolling file.

It provides a Common.Logging API adapter, which you can choose to use so that you're
not boxed in a corner if your need for a more powerful logging framework evolves.

This logging framework was designed to provide a balance between the following goals:
  - simple
  - flexible
  - lightweight
  - reasonably fast
  - reasonable defaults

To use the provided ConsoleLogger or FileLogger, there is no dependency on any other frameworks,
unless you choose to use the Common.Logging adapter.

Even without using the Common.Logging adapter, if it ever becomes necessary to use another logging
framework (such as Microsoft Enterprise Logging), a custom adapter can easily be created by
implementing the ILogger interface.

There is no static Log class or static log method. Ideally, applications should create concrete loggers via IOC
containers or factories. However, the Common.Logging API does provide a static LogManager class.

An example of using an IOC container or a factory class might look like the following:

var logger = Container.Resolve<ILogger>();

or

var logger = LogFactory.CreateLogger();

### Examples of explicitly creating a logger and using it:

```c#
// create a logger that outputs to the console using default log level (Information)
var logger = new Logger(Console.WriteLine);

// alternatively, use the provided ConsoleLogger
var logger = new ConsoleLogger();

// specify the log level (this example will write all entries
// from verbose through critical)
var logger = new ConsoleLogger {LogLevel = LogLevel.Verbose};

// create a file logger; by default this will create a rolling log file
// that will continue logging to the same file for the life of this logger
// (to support session based logging).
var logger = new FileLogger();

// provide a file name template. This example will create logs
// named testlog-001.log, testlog-002.log, etc.
logger.FileNameTemplate = "testlog-###.log"

// log something
logger.Log("test log message");

// the default log level is LogLevel.Verbose, so this does the same thing:
logger.Log("test log message", LogLevel.Verbose);

// Specifiy a category when logging (the default is "General")
logger.Log("test log message", LogLevel.Verbose, "Test Category");
```
