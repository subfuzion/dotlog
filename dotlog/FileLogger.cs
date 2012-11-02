// #region License
// ========================================================================
// FileLogger.cs
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
	using System.IO;
	using FileLogging;

	public class FileLogger : Logger, IDisposable
	{
		public static readonly string DefaultFileNameTemplate = @"logfile###.log";
		private string _currentLogFileName;
		private string _fileNameTemplate;
		private string _logPath;
		private FileStream _logStream;

		private StreamWriter _logWriter;

		public FileLogger()
		{
			LogAction = WriteOutput;
		}

		public string FileNameTemplate
		{
			get { return _fileNameTemplate ?? (_fileNameTemplate = DefaultFileNameTemplate); }
			set { _fileNameTemplate = value; }
		}

		public FileDeletionPolicy FileDeletionPolicy { get; set; }

		public string LogPath
		{
			get
			{
				if (_logPath == null)
				{
					var root = AppDomain.CurrentDomain.BaseDirectory;
					_logPath = Path.Combine(root ?? ".", "logs");
				}

				return _logPath;
			}
			set { _logPath = value; }
		}

		public string CurrentLogFileName
		{
			get
			{
				if (_currentLogFileName == null)
				{
					var rollingLogFile = new RollingLogFile
					{
						BasePath = LogPath,
						FileNameTemplate = FileNameTemplate,
					};

					_currentLogFileName = rollingLogFile.GetNextFileName();
				}

				return _currentLogFileName;
			}

			set { _currentLogFileName = value; }
		}

		public string FullPathName
		{
			get { return Path.Combine(LogPath, CurrentLogFileName); }
		}

		public bool Exists()
		{
			return File.Exists(FullPathName);
		}

		public void TryDeleteFile()
		{
			if (Exists())
			{
				File.Delete(FullPathName);
			}
		}

		private void WriteOutput(string output)
		{
			if (_logStream == null)
			{
				if (!Directory.Exists(LogPath))
				{
					Directory.CreateDirectory(LogPath);
				}

				var pathname = Path.Combine(LogPath, CurrentLogFileName);
				_logStream = File.Open(pathname, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
				_logWriter = new StreamWriter(_logStream);
			}

			_logWriter.Write(output);
			_logWriter.Write(Environment.NewLine);
			_logWriter.Flush();
		}

		public void Close()
		{
			Dispose();
		}

		public void CleanUp()
		{
			// TODO
		}

		#region IDisposable

		public void Dispose()
		{
			Dispose(true);

			// the following is just an optimization to improve performance
			GC.SuppressFinalize(this);
		}

		~FileLogger()
		{
			Dispose(false);
		}

		private void Dispose(bool isDisposingExplicit)
		{
			if (isDisposingExplicit && _logWriter != null)
			{
				_logWriter.Close();
			}
		}

		#endregion
	}
}