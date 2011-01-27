using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SES.Testing
{

	public class AwsCredentials
	{
		public AwsCredentials(string accessKeyID, string secretAccessKey)
		{
			AccessKeyID = accessKeyID;
			SecretAccessKey = secretAccessKey;
		}

		public string AccessKeyID { get; private set; }
		public string SecretAccessKey { get; private set; }
	}


	public interface ICommand
	{
		string Action { get; }
		Dictionary<string, string> GetData();
	}



	/// <summary>
	/// Builds a HTTP request for SES commands
	/// </summary>
	public class CommandRequestBuilder
	{
		private readonly AwsCredentials _credentials;
		private readonly string _address;

		public CommandRequestBuilder(AwsCredentials credentials)
		{
			if (credentials == null)
			{
				throw new ArgumentNullException("credentials");
			}

			this._credentials = credentials;
			this._address = "https://email.us-east-1.amazonaws.com";

		}

		public CommandRequestBuilder(AwsCredentials credentials, string address)
		{
			if (credentials == null)
			{
				throw new ArgumentNullException("credentials");
			}

			if (address == null)
			{
				throw new ArgumentNullException("address");
			}

			this._credentials = credentials;
			this._address = address;

		}




		public string GenerateSignature(string key, string data)
		{
			Func<string, byte[]> getBytes = Encoding.UTF8.GetBytes;


			var encoder = new /*HMACSHA256*/HMACSHA1(getBytes(key));

			var hash = encoder.ComputeHash(
				getBytes(data)
				);

			var sign = Convert.ToBase64String(hash);

			//Console.WriteLine(data + " : " + sign);

			return sign;

		}

		public HttpWebRequest Build(ICommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}




			var request = (HttpWebRequest)WebRequest.Create(this._address);

			request.Method = "POST";
			// Create POST data and convert it to a byte array.

			var sb = new StringBuilder();


			// setup headers


			// date header
			request.Date = DateTime.UtcNow;//.ToString("ddd, dd MMM yyyy hh:mm:ss +0000");  // e.g Tue, 25 May 2010 23:05:27 +0000



			// sign request with request date
			var date = request.Date.ToString("ddd, dd MMM yyyy HH:mm:ss ") + "GMT";
			sb.Append("AWS3-HTTPS AWSAccessKeyId=");
			sb.Append(this._credentials.AccessKeyID);
			sb.Append(", Algorithm=HmacSHA1, Signature="); //HMACSHA1 or HmacSHA1 
			sb.Append(GenerateSignature(this._credentials.SecretAccessKey, date));

			request.Headers.Add("X-Amzn-Authorization", sb.ToString());


			request.ContentType = "application/x-www-form-urlencoded";


			// data 
			sb.Length = 0;
			sb.Append("Action=");
			sb.Append(command.Action);

			var d = command.GetData();
			if (d != null)
			{
				foreach (string key in d.Keys)
				{
					sb.Append('&');
					sb.Append(HttpUtility.UrlEncode(key));
					sb.Append('=');
					sb.Append(HttpUtility.UrlEncode(d[key]));
				}

			}
			byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());


			request.ContentLength = byteArray.Length;

			using (Stream dataStream = request.GetRequestStream())
			{
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();
			}


			return request;



		}
	}
}
