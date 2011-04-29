using System;
using System.Collections.Generic;

namespace McGiv.AWS.SES
{
	/// <summary>
	/// http://docs.amazonwebservices.com/ses/latest/APIReference/API_Destination.html
	/// </summary>
	[Serializable]
	public class Destination
	{
		public Destination()
		{
			BccAddresses = new List<string>();
			CcAddresses = new List<string>();
			ToAddresses = new List<string>();
		}

		public List<string> BccAddresses { get; private set; }
		public List<string> CcAddresses { get; private set; }
		public List<string> ToAddresses { get; private set; }
	}


	[Serializable]
	public class RawMessage
	{
		public string Data { get; set; }
	}


	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/API_Message.html
	/// </summary>
	[Serializable]
	public class Message
	{
		public Message()
		{
			Body = new Body();
			Subject = new Content();
		}

		/// <summary>
		/// Required
		/// </summary>
		public Body Body { get; set; }

		/// <summary>
		/// Required
		/// </summary>
		public Content Subject { get; set; }
	}


	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/API_Body.html
	/// </summary>
	[Serializable]
	public class Body
	{
		public Body()
		{
			Html = new Content();
			Text = new Content();
		}

		public Content Html { get; set; }
		public Content Text { get; set; }
	}


	/// <summary>
	/// http://docs.amazonwebservices.com/ses/latest/APIReference/API_Content.html
	/// </summary>
	[Serializable]
	public class Content
	{
		public string Charset { get; set; }


		/// <summary>
		/// Required
		/// </summary>
		public string Data { get; set; }
	}
}