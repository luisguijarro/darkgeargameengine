using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace dge.Tools
{
	/// <summary>
	/// Description of MD5FromFile.
	/// </summary>
	public static class FileTools
	{
		public static string MD5FromFile(string fileName)
		{
			FileStream file = new FileStream(fileName, FileMode.Open);
			return MD5FromStream(file);
		}

		public static string MD5FromBytes(byte[] bytes)
		{
			Stream file = new MemoryStream(bytes);
			return MD5FromStream(file);
		}

		public static string MD5FromStream(Stream filestream)
		{
			//DateTime initdt = DateTime.Now;
			//FileStream file = new FileStream(fileName, FileMode.Open);
			MD5 md5 = MD5.Create();//new MD5CryptoServiceProvider();
			byte[] retVal = md5.ComputeHash(filestream);
			filestream.Close();
			filestream.Dispose();
			
			StringBuilder sb = new StringBuilder();
			for (int i=0; i< retVal.Length;i++)
			{
				sb.Append(retVal[i]);
			}
			//Console.WriteLine("Calcular MD5: "+(DateTime.Now-initdt).TotalMilliseconds+" milisegundos");
			return sb.ToString();
		}
	}
}