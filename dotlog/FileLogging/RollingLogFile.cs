// #region License
// ========================================================================
// RollingLogFile.cs
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

namespace DotLog.FileLogging
{
	using System;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;

	public class RollingLogFile
	{
		public static readonly string DefaultFileNameTemplate = "logfile###.log";

		public static readonly string NumberedFileNamePattern = @"^([ .A-Za-z0-9_-]+)(#+)(\.[A-Za-z0-9_-]+)*";

		private string _basePath;
		private string _fileNameBasePartTemplate;
		private string _fileNameExtensionPartTemplate;
		private string _fileNameGlobPattern;
		private string _fileNameNumberPartTemplate;
		private string _fileNameTemplate;

		private string _glob;

		/// <summary>
		///   Gets or sets a rolling log file name template in the following form:
		///   "basename###[.ext]"
		/// 
		///   basename specifies the file name pattern to use, such as "logfile-"
		///   ext is optional
		/// 
		///   default: "logfile###.log"
		/// 
		///   For example, the following are valid templates:
		/// 
		///   "testlog#.log" will generate =>
		///   testlog1.log
		///   testlog2.log
		///   ...
		///   testlog10.log
		///   ...
		/// 
		///   "logfile-###.log" will generate =>
		///   testlog-001.log  (numeric part is padded with leading 0's)
		///   testlog-002.log
		///   ...
		///   testlog-999.log
		///   testlog-1000.log  (number is now bigger than specified width, so no more padding)
		///   ...
		/// </summary>
		public string FileNameTemplate
		{
			get { return _fileNameTemplate ?? (_fileNameTemplate = DefaultFileNameTemplate); }
			set { _fileNameTemplate = value; }
		}

		/// <summary>
		///   Gets or sets the base path to use for rolling log files.
		/// </summary>
		public string BasePath
		{
			get { return _basePath ?? (_basePath = "."); }
			set { _basePath = value; }
		}

		public void Validate()
		{
			if (!Regex.IsMatch(FileNameTemplate, NumberedFileNamePattern))
			{
				throw new Exception("Validation failed; ensure FileNameTemplate is specified correctly");
			}

			if (!Directory.Exists(BasePath))
			{
				throw new Exception(string.Format("Validation failed; the base path does not exist ({0})", BasePath));
			}
		}

		/// <summary>
		///   Returns the name to use for the next rolling log file. The name's numeric part is a value
		///   that is one greater than the highest number actually found in the <see cref="BasePath" /> location,
		///   or else is equal to 1. It is formatted according to <see cref="FileNameTemplate" />
		/// </summary>
		/// <returns> </returns>
		public string GetNextFileName()
		{
			Validate();

			CreateFileNameGlobPattern();

			var max = 0;

			var globbedFileNumberedPattern = Regex.Replace(
				FileNameTemplate,
				NumberedFileNamePattern,
				match => match.Groups[1] + @"(\d+)" + match.Groups[3]);

			foreach (var f in Directory.EnumerateFiles(BasePath, _fileNameGlobPattern, SearchOption.TopDirectoryOnly))
			{
				var filename = Path.GetFileName(f);

				// can't happen, but checking to suppress warning
				if (filename == null) continue;

				var match = Regex.Match(filename, globbedFileNumberedPattern);
				var numberedPart = match.Groups[1].Value;
				var value = 0;
				if (int.TryParse(numberedPart, out value))
				{
					max = Math.Max(max, value);
				}
			}

			// increment to next value to use
			max++;

			var formatSpecifier = "D" + _glob.Length;

			var nextFileName = string.Format("{0}{1}{2}",
				_fileNameBasePartTemplate,
				max.ToString(formatSpecifier),
				_fileNameExtensionPartTemplate);

			return nextFileName;
		}

		/// <summary>
		///   The glob pattern to use for searching for file names that match the current FileNameTemplate.
		///   Essentially, the '#' is simply replaced with '?' as used by the operating system for glob searching,
		///   but we use the regex to get the other file name parts and store them for later use.
		/// </summary>
		private void CreateFileNameGlobPattern()
		{
			string pattern = null;

			var match = Regex.Match(FileNameTemplate, NumberedFileNamePattern);

			if (match.Success)
			{
				_fileNameBasePartTemplate = match.Groups[1].Value;
				_fileNameNumberPartTemplate = match.Groups[2].Value;
				_fileNameExtensionPartTemplate = match.Groups[3].Value;


				var count = _fileNameNumberPartTemplate.Length;
				var glob = new StringBuilder();
				for (var i = 0; i < count; i++)
				{
					glob.Append('?');
				}

				_glob = glob.ToString();

				pattern = string.Format("{0}{1}{2}", _fileNameBasePartTemplate, _glob, _fileNameExtensionPartTemplate);
			}

			_fileNameGlobPattern = pattern;
		}
	}
}