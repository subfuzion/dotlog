// #region License
// ========================================================================
// LogFormatter.cs
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
	using System.Text;

	public class LogFormatter : ILogFormatter
	{
		private static readonly string DefaultTemplate = @"[{timestamp}][{loglevel}][{category}] {message}";

		public FormatSpecification FormatSpecification { get; set; }

		#region ILogFormatter Members

		public string Format(LogEntry logEntry)
		{
			if (FormatSpecification == null)
			{
				FormatSpecification = new FormatSpecification
				{
					Template = DefaultTemplate,
				};
			}

			// TODO add real support for using the template
			var sb = new StringBuilder();

			sb.AppendFormat("[{0}][{1}][{2}] {3}",
				logEntry.Timestamp.ToUniversalTime(),
				logEntry.LogLevel,
				logEntry.Category,
				logEntry.Message);

			return sb.ToString();
		}

		#endregion
	}
}