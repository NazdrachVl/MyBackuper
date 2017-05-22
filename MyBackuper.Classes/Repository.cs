using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MyBackuper.Classes
{
	[JsonObject]
	public class Repository : IEnumerable<KeyValuePair<string, Backup>>
	{
		#region Private members

		[JsonProperty(PropertyName = "Backups")]
		private Dictionary<string, Backup> _backups;

		#endregion

		#region Properties

		[JsonProperty]
		public string DirectoryPath { get; set; }

		[JsonProperty]
		public string BackupDirectoryPath { get; set; }

		[JsonProperty]
		public BackupTrigger Trigger { get; set; }

		public Backup this[string date]
		{
			get
			{
				return _backups[date];
			}
		}

		[JsonProperty]
		public int DaysPassed { get; set; }

		#endregion

		#region Constructors

		public Repository(string directory, string backupDirectory)
		{
			DirectoryPath = directory;
			BackupDirectoryPath = backupDirectory;
			_backups = new Dictionary<string, Backup>();
			DaysPassed = 0;
		}

		#endregion

		#region Public methods

		public void MakeBackup()
		{
			Backup bak = new Backup();
			DateTime time = DateTime.Now;
			var backupDirectory = new DirectoryInfo(BackupDirectoryPath);
			var directory = new DirectoryInfo(DirectoryPath);
			string backupDir2 = Path.Combine(backupDirectory.FullName, time.ToString("yyyy-MM-ddTHH_mm_ss"));
			System.IO.Directory.CreateDirectory(backupDir2);
			foreach (var file in directory.EnumerateFilesRecursive())
			{
				string filename = Path.Combine(backupDir2, file.FullName.Substring(directory.FullName.Length + 1));
				if (System.IO.File.Exists(file.FullName))
				{
					System.IO.Directory.CreateDirectory(new FileInfo(filename).DirectoryName);
					file.CopyTo(filename, true);
					bak.AddFile(file.FullName.Substring(directory.FullName.Length + 1));
				}
			}
			_backups.Add(time.ToString("yyyy-MM-ddTHH_mm_ss"), bak);
		}

		public void RemoveBackup(string date)
		{
			_backups.Remove(date);
			Directory.Delete(Path.Combine(new DirectoryInfo(BackupDirectoryPath).FullName, date), true);
		}

		public void RestoreBackup(string date)
		{
			if (Directory.Exists(DirectoryPath))
			{
				Directory.Delete(DirectoryPath, true);
			}
			foreach (var item in _backups[date])
			{
				Directory.CreateDirectory(new FileInfo(Path.Combine(DirectoryPath, item)).DirectoryName);
				File.Copy(Path.Combine(BackupDirectoryPath, date, item), Path.Combine(DirectoryPath, item));
			}
		}

		public void Clear()
		{
			_backups.Clear();
			Directory.Delete(BackupDirectoryPath, true);
			SaveConfig();
		}

		public void SaveConfig()
		{
			var ser = new JsonSerializer();
			using (var sw = new StreamWriter(Path.Combine(new DirectoryInfo(BackupDirectoryPath).FullName, "global.json")))
			{
				using (var jsonWriter = new JsonTextWriter(sw))
				{
					ser.Serialize(jsonWriter, this);
				}
			}
		}

		#endregion

		#region Static methods

		public static Repository FromConfig(string filename)
		{
			var ser = new JsonSerializer();
			using (var sr = new StreamReader(filename))
			{
				using (var jsonReader = new JsonTextReader(sr))
				{
					return ser.Deserialize<Repository>(jsonReader);
				}
			}
		}

		#endregion

		#region Interfaces

		public IEnumerator<KeyValuePair<string, Backup>> GetEnumerator()
		{
			return _backups.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}