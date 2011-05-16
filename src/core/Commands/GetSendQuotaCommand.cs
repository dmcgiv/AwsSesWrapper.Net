using System;
using System.Collections.Generic;
using System.Xml;

namespace McGiv.AWS.SES
{


	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_GetSendQuota.html
	/// </summary>
	[Serializable]
	public class GetSendQuotaCommand : ICommand
	{
		#region ICommand Members

		public string Action
		{
			get { return "GetSendQuota"; }
		}

		public Dictionary<string, string> GetData()
		{
			return null;
		}

		#endregion
	}

	[Serializable]
	public class GetSendQuoteResponse : Response
	{
		/// <summary>
		/// The maximum number of emails the user is allowed to send in a 24-hour interval.
		/// </summary>
		public float SentLast24Hours { get; set; }

		/// <summary>
		/// The maximum number of emails the user is allowed to send per second.
		/// </summary>
		public float Max24HourSend { get; set; }

		/// <summary>
		/// The number of emails sent during the previous 24 hours.
		/// </summary>
		public float MaxSendRate { get; set; }
	}


	public class GetSendQuoteResponseParser : ResponseParser<GetSendQuoteResponse>
	{
		// example
		/*
		<GetSendQuotaResponse xmlns="http://ses.amazonaws.com/doc/2010-12-01/">
		  <GetSendQuotaResult>
			<SentLast24Hours>0.0</SentLast24Hours>
			<Max24HourSend>1000.0</Max24HourSend>
			<MaxSendRate>1.0</MaxSendRate>
		  </GetSendQuotaResult>
		  <ResponseMetadata>
			<RequestId>3abb51a6-2ca7-11e0-b33e-49f0802d0c9d</RequestId>
		  </ResponseMetadata>
		</GetSendQuotaResponse>
		 * */

		#region ICommandResponseParser<SendQuote> Members

		public GetSendQuoteResponseParser()
			: base("GetSendQuotaResponse")
		{
		}


		protected override void InnerParse(XmlReader reader, GetSendQuoteResponse response)
		{

			reader.ReadStartElement("GetSendQuotaResult");
			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}

				switch (reader.Name)
				{
					case "SentLast24Hours":
						{
							response.SentLast24Hours = GetNextFloatValue(reader);
							break;
						}
					case "Max24HourSend":
						{
							response.Max24HourSend = GetNextFloatValue(reader);
							break;
						}
					case "MaxSendRate":
						{
							response.MaxSendRate = GetNextFloatValue(reader);
							break;
						}
					case "ResponseMetadata":
						{
							return;
						}
				}

			}
			
		}


		#endregion

		
	}
}