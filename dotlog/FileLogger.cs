// #region License
// ========================================================================
// FileLogger.cs
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