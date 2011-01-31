using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace McGiv.AWS.SES
{
	/// <summary>
	/// Builds a web request for SES commands
	/// </summary>
	public class CommandRequestBuilder
	{
		private readonly string _address;
		private readonly RequestSigner _signer;

		public CommandRequestBuilder(RequestSigner signer)
			: this(signer, "https://email.us-east-1.amazonaws.com")
		{
		}

		public CommandRequestBuilder(RequestSigner signer, string address)

		{
			if (signer == null)
			{
				throw new ArgumentNullException("signer");
			}


			if (address == null)
			{
				throw new ArgumentNullException("address");
			}


			_address = address;
			_signer = signer;
		}


		/// <summary>
		/// Formats the command data so that it can be used in a POST web request
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public string FormatData(ICommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}


			var sb = new StringBuilder();
			sb.Append("Action=");
			sb.Append(HttpUtility.UrlEncode(command.Action));

			Dictionary<string, string> d = command.GetData();
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

			return sb.ToString();
		}


		/// <summary>
		/// Creates a web request based on the given command.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public HttpWebRequest Build(ICommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}

			var request = (HttpWebRequest) WebRequest.Create(_address);

			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";


			// sign
			_signer.SignRequest(request);


			// add data
			byte[] byteArray = GetData(command);
			request.ContentLength = byteArray.Length;

			// possible WebException when resolving domain
			using (Stream dataStream = request.GetRequestStream())
			{
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();
			}


			return request;
		}


		public byte[] GetData(ICommand command)
		{
			return Encoding.ASCII.GetBytes(FormatData(command));
		}


		/// <summary>
		/// Creates a web request but with no command data.
		/// </summary>
		/// <returns></returns>
		public HttpWebRequest Build()
		{
			var request = (HttpWebRequest) WebRequest.Create(_address);

			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";


			// sign
			_signer.SignRequest(request);


			return request;
		}
	}
}