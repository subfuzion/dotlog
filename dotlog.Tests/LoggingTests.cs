// #region License
// ========================================================================
// LoggingTests.cs
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

namespace DotLog.Tests
{
	using System.Diagnostics;
	using System.IO;
	using System.Text.RegularExpressions;
	using NUnit.Framework;

	[TestFixture]
	public class LoggingTests
	{
		[Test]
		public void Logger_WhenLogLevelIsHigher_ThenLogsOutput()
		{
			// stores log output
			string output = null;

			var logger = new Logger
			{
				LogAction = s => output = s,
				LogLevel = LogLevel.Verbose,
			};

			logger.Log("test", LogLevel.Information);

			Assert.IsTrue(output.EndsWith("test"));
		}

		[Test]
		public void Logger_WhenLogLevelIsLower_ThenDoesNotLogOutput()
		{
			// stores log output
			string output = null;

			var logger = new Logger
			{
				LogAction = s => output = s,
				LogLevel = LogLevel.Information,
			};

			logger.Log("test", LogLevel.Verbose);

			Assert.IsNull(output);
		}

		[Test]
		public void Logger_WhenLogLevelIsSame_ThenLogsOutput()
		{
			// stores log output
			string output = null;

			var logger = new Logger
			{
				LogAction = s => output = s,
				LogLevel = LogLevel.Information,
			};

			logger.Log("test", LogLevel.Information);

			Assert.IsTrue(output.EndsWith("test"));
		}

		[Test]
		public void Logger_WhenUsingDefaultFormat_ThenMatchesExpectedFormat()
		{
			// stores log output
			string output = null;

			var logger = new Logger
			{
				LogAction = s => output = s,
				LogLevel = LogLevel.Information,
			};

			logger.Log("test", LogLevel.Information);

			// should match entries in the form of:
			// "[MM/DD/YYYY HH:MM:SS (AM|PM)][Information][(category)] test"
			const string pattern = @"\[[^\]]*\]\[Information\]\[[^\]]*\]\s.*";

			Assert.IsTrue(Regex.IsMatch(output, pattern));
		}

		[Test]
		public void Logger_WhenUsingFile_ThenRollingFilesShouldIncrement()
		{
			// create first rolling log file
			var logger1 = new FileLogger {LogLevel = LogLevel.Verbose};
			logger1.Log("test");
			Assert.IsTrue(File.Exists(logger1.FullPathName));
			logger1.Close();

			// create a new file logger (effectively a new logging session)
			// and create the second rolling log file
			var logger2 = new FileLogger {LogLevel = LogLevel.Verbose};
			logger2.Log("test");
			Assert.IsTrue(File.Exists(logger2.FullPathName));
			logger2.Close();

			// get the number values from each file
			// used the default file name template, so the format is
			// "logfile###.log"
			var logNumber1 = int.Parse(logger1.CurrentLogFileName.Substring(7, 3));
			var logNumber2 = int.Parse(logger2.CurrentLogFileName.Substring(7, 3));

			Assert.IsTrue(logNumber2 == logNumber1 + 1);

			// clean up
			logger1.TryDeleteFile();
			logger2.TryDeleteFile();
		}

		[Test]
		public void Logger_WhenUsingFile_ThenWritesOutputToFile()
		{
			var logger = new FileLogger {LogLevel = LogLevel.Information};
			logger.Log("test", LogLevel.Information);
			logger.Close();

			Assert.IsTrue(File.Exists(logger.FullPathName));

			logger.TryDeleteFile();
		}

		/// <summary>
		///   This is a visual test. Look at the debug trace to verify success.
		/// </summary>
		[Test]
		public void Logger_WhenUsingLoggerWithDebugStream_ThenOutputsToConsole()
		{
			var logger = new Logger
			{
				LogAction = s => Debug.WriteLine(s),
				LogLevel = LogLevel.Information,
			};

			logger.Log("test", LogLevel.Information);

			Assert.Inconclusive(
				"This is a visual test. The test passed if you verify output in the following form: '[8/1/2012 9:19:08 PM][Information][] test'");
		}
	}
}