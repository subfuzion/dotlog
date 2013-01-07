// #region License
// ========================================================================
// Program.cs
// DotLog - Simple Logging for .NET
// https://github.com/tonypujals/dotlog
// 
// Copyright © 2012, 2013 Tony Pujals and Subfuzion, Inc.
// 
// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ========================================================================
// #endregion

namespace DotLog.Example
{
	using System;

	internal class Program
	{
		private static void Main(string[] args)
		{
			DemoSimpleLogger();
			DemoConsoleLogger();
			DemoFileLogger();

			Console.WriteLine("press any key to exit");
			Console.ReadKey();
		}

		private static void DemoSimpleLogger()
		{
			// create a logger that outputs to the console using default log level (Verbose)
			var logger = new Logger(Console.WriteLine);
			logger.Log("using custom logger");
		}

		private static void DemoConsoleLogger()
		{
			// Use the ConsoleLogger and set minimum logging level
			var logger = new ConsoleLogger {LogLevel = LogLevel.Information};
			logger.Log("this won't show up...");
			logger.Log("using standard ConsoleLogger", LogLevel.Information);
		}

		private static void DemoFileLogger()
		{
			Console.Write("using file logger ... ");
			var logger = new FileLogger
			{
				FileNameTemplate = "dotlog-##.log",
				LogLevel = LogLevel.Verbose, // this is the default
				LogPath = "Logs", // this is the default
			};

			logger.Log("test entry 1");

			logger.Log("test entry 2", LogLevel.Verbose);

			logger.Log("test entry 3", LogLevel.Verbose, "example category");

			Console.WriteLine("log saved to: " + logger.FullPathName);
		}
	}
}