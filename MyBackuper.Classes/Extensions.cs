using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace MyBackuper.Classes
{
	static class Extensions
	{
		public static IEnumerable<FileInfo> EnumerateFilesRecursive(this DirectoryInfo directory)
		{
			foreach (var dir in directory.EnumerateDirectories())
			{
				foreach (var file in dir.EnumerateFilesRecursive())
				{
					yield return file;
				}
			}
			foreach (var file in directory.EnumerateFiles())
			{
				yield return file;
			}
		}

		public static FileInfo[] GetFilesRecursive(this DirectoryInfo directory)
		{
			return new List<FileInfo>(directory.EnumerateFilesRecursive()).ToArray();
		}

		public static string GetMD5Hash(this FileInfo file)
		{
			using (var md5 = MD5.Create())
			{
				using (var stream = file.OpenRead())
				{
					return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
				}
			}
		}

		public static bool CheckMD5Hash(this FileInfo file, string hash)
		{
			return file.GetMD5Hash() == hash;
		}
	}
}