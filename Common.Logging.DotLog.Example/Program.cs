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

namespace Common.Logging.DotLog.Example
{
	using System;
	using System.Collections.Specialized;

	internal class Program
	{
		private static void Main(string[] args)
		{
			// create properties
			var properties = new NameValueCollection();

			properties["fileNameTemplate"] = "demo-##.log";
			properties["logPath"] = "Logs"; // this is the default value
			properties["logLevel"] = "all"; // this is the default value
			properties["showDateTime"] = "true";

			// set Adapter
			LogManager.Adapter = new DotLogFactoryAdapter(properties);

			// get logger
			var log = LogManager.GetLogger("FileLogger");

			// start logging
			log.Info(m => m("|demo category|this is a {0} log entry", "cool"));

			Console.WriteLine("Log saved to: " + @"Logs\demo-##.log");
			Console.WriteLine("press any key to exit");
			Console.ReadKey();
		}
	}
}