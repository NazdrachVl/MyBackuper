using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MyBackuper.Classes
{
	[JsonObject]
	public class Backup : IEnumerable<string>
	{
		[JsonProperty(PropertyName = "Files")]
		private List<string> _files;

		public Backup()
		{
			_files = new List<string>();
		}

		public string this[int index]
		{
			get
			{
				return _files[index];
			}
		}

		[JsonIgnore]
		public int Count
		{
			get
			{
				return _files.Count;
			}
		}

		public void AddFile(string fileName)
		{
			_files.Add(fileName);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return _files.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}