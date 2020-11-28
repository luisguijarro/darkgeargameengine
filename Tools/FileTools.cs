using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace dge
{
	/// <summary>
	/// Description of MD5FromFile.
	/// </summary>
	public static class FileTools
	{
		public static string MD5FromFile(string fileName)
		{
			//DateTime initdt = DateTime.Now;
			FileStream file = new FileStream(fileName, FileMode.Open);
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] retVal = md5.ComputeHash(file);
			file.Close();
			file.Dispose();
			
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