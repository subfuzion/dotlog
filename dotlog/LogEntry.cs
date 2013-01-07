// #region License
// ========================================================================
// LogEntry.cs
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