using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Cryptography
{
	/// <summary>
	/// Summary description for Cryption.
	/// </summary>
	public sealed class Cryption
	{
		private RijndaelManaged Algorithm;
		private MemoryStream memStream;
		private ICryptoTransform EncryptorDecryptor;
		private CryptoStream crStream;
		private StreamWriter strWriter;
		private StreamReader strReader;

		private string m_key;
		private string m_iv;

		private byte[] key;
		private byte[] iv;

		private string pwd_str;
		private byte[] pwd_byte;

		public Cryption(string key_val, string iv_val)
		{
			key = new byte[32];
			iv = new byte[32];

			int i;
			m_key = key_val;
			m_iv = iv_val;

			for(i=0;i<m_key.Length;i++)
			{
				key[i] = Convert.ToByte(m_key[i]);
			}
			for(i=0;i<m_iv.Length;i++)
			{
				iv[i] = Convert.ToByte(m_iv[i]);
			}

		}

		public string Encrypt(string s)
		{
			Algorithm = new RijndaelManaged();

			Algorithm.BlockSize = 256;
			Algorithm.KeySize = 256;

			memStream = new MemoryStream();

			EncryptorDecryptor = Algorithm.CreateEncryptor(key,iv);

			crStream = new CryptoStream(memStream, EncryptorDecryptor, CryptoStreamMode.Write);

			strWriter = new StreamWriter(crStream);

			strWriter.Write(s);

			strWriter.Flush();
			crStream.FlushFinalBlock();

			pwd_byte = new byte[memStream.Length];
			memStream.Position = 0;
			memStream.Read(pwd_byte,0,(int)pwd_byte.Length);

			pwd_str = new UnicodeEncoding().GetString(pwd_byte);

			return pwd_str;
		}

		public string Decrypt(string s)
		{
			Algorithm = new RijndaelManaged();

			Algorithm.BlockSize = 256;
			Algorithm.KeySize = 256;

			MemoryStream memStream = new MemoryStream(new UnicodeEncoding().GetBytes(s));

			ICryptoTransform EncryptorDecryptor = Algorithm.CreateDecryptor(key,iv);
			memStream.Position = 0;
			CryptoStream crStream = new CryptoStream(memStream,EncryptorDecryptor,CryptoStreamMode.Read);
			strReader = new StreamReader(crStream);

			return strReader.ReadToEnd();
		}
		
	}
}