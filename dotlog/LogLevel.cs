// #region License
// ========================================================================
// LogLevel.cs
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

namespace DotLog
{
	/// <summary>
	///   The logger uses this enumeration to determine which log entries are written to the log destination.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		///   Nothing will be logged.
		/// </summary>
		None = 0,

		/// <summary>
		///   Log critical errors that require the application to terminate.
		/// </summary>
		Critical = 1,

		/// <summary>
		///   Log exceptional errors
		///   (the application will continue to run although important functionality is unavailable).
		/// </summary>
		Error = 2,

		/// <summary>
		///   Log warnings that indicate issues that ought to be addressed.
		/// </summary>
		Warning = 3,

		/// <summary>
		///   Log informational events, such as successful initialization.
		/// </summary>
		Information = 4,

		/// <summary>
		///   Log details useful for debugging or tracing through code execution.
		/// </summary>
		Verbose = 5,
	}
}