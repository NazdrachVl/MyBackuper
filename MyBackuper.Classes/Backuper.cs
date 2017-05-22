using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Newtonsoft.Json;

namespace MyBackuper.Classes
{
	[JsonObject]
	public class Backuper : IEnumerable<KeyValuePair<string, Repository>>
	{
		#region Private members

		private static string _configFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyBackuper", "config.json");

		[JsonProperty(PropertyName = "Repositories")]
		private Dictionary<string, Repository> _repositories;

		#endregion

		#region Properties

		public Repository this[string name]
		{
			get
			{
				return _repositories[name];
			}
		}

		[JsonIgnore]
		public int Count
		{
			get
			{
				return _repositories.Count;
			}
		}

		#endregion

		#region Public methods

		public Backuper()
		{
			_repositories = new Dictionary<string, Repository>();
		}

		public void AddRepository(string name, string directory, string backupDirectory)
		{
			_repositories.Add(name, new Repository(directory, backupDirectory));
			SaveConfig();
		}

		public void RemoveRepository(string name)
		{
			RemoveRepository(name, false);
		}

		public void RemoveRepository(string name, bool fullRemove)
		{
			if (fullRemove)
			{
				_repositories[name].Clear();
			}
			_repositories.Remove(name);
			SaveConfig();
		}

		public void MakeBackup(string name)
		{
			_repositories[name].MakeBackup();
			SaveConfig();
		}

		public void RemoveBackup(string repoName, string backupName)
		{
			_repositories[repoName].RemoveBackup(backupName);
			SaveConfig();
		}

		public void SaveConfig()
		{
			Directory.CreateDirectory(new FileInfo(_configFileName).DirectoryName);
			var ser = new JsonSerializer();
			using (var stream = new FileStream(_configFileName, FileMode.Create, FileAccess.Write))
			using (var sw = new StreamWriter(stream))
			using (var jsonWriter = new JsonTextWriter(sw))
			{
				ser.Serialize(jsonWriter, this);
			}
		}

		#endregion

		#region Static methods

		public static Backuper FromConfig()
		{
			var ser = new JsonSerializer();
			using (var sr = new StreamReader(_configFileName))
			{
				using (var jsonReader = new JsonTextReader(sr))
				{
					return ser.Deserialize<Backuper>(jsonReader);
				}
			}
		}

		#endregion

		#region Interfaces

		public IEnumerator<KeyValuePair<string, Repository>> GetEnumerator()
		{
			return _repositories.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}