// #region License
// ========================================================================
// Program.cs
// dotlog - Simple Logging for .NET
// https://github.com/tonypujals/dotlog
// 
// Copyright (C) 2012, Tony Pujals
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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