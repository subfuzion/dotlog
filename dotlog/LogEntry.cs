// #region License
// ========================================================================
// LogEntry.cs
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
	using System;
	using System.Text;

	public class LogEntry
	{
		public const string DefaultCategory = "General";

		private string _category;

		public DateTime Timestamp { get; set; }

		public LogLevel LogLevel { get; set; }

		public string Category
		{
			get { return _category ?? DefaultCategory; }
			set { _category = value; }
		}

		public string Message { get; set; }

		public string ToString(ILogFormatter formatter)
		{
			if (formatter == null) return ToString();

			try
			{
				return formatter.Format(this);
			}
			catch
			{
				return ToString();
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendFormat("[{0}][{1}][{2}] {3}",
				Timestamp.ToLocalTime(),
				LogLevel,
				Category,
				Message);

			return sb.ToString();
		}
	}
}