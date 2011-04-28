using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace McGiv.AWS.SES
{
	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_GetSendQuota.html
	/// </summary>
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
	public class GetSendQuoteResponse : CommandResponse
	{
		public float SentLast24Hours { get; set; }
		public float Max24HourSend { get; set; }
		public float MaxSendRate { get; set; }
	}


	public class GetSendQuoteResponseParser : ICommandResponseParser<GetSendQuoteResponse>
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

		public GetSendQuoteResponse Process(Stream input)
		{
			var quota = new GetSendQuoteResponse();
			quota.Command = "GetSendQuotaResponse";

			using (XmlReader reader = XmlReader.Create(input))
			{
				//reader.MoveToContent();
				reader.ReadStartElement(quota.Command);
				reader.ReadStartElement("GetSendQuotaResult");
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						switch (reader.Name)
						{
							case "SentLast24Hours":
								{
									quota.SentLast24Hours = GetNextValue(reader);
									break;
								}
							case "Max24HourSend":
								{
									quota.Max24HourSend = GetNextValue(reader);
									break;
								}
							case "MaxSendRate":
								{
									quota.MaxSendRate = GetNextValue(reader);
									break;
								}
						}
					}
				}
			}


			return quota;
		}

		#endregion

		private static float GetNextValue(XmlReader reader)
		{
			if (reader.Read() && reader.NodeType == XmlNodeType.Text)
			{
				return float.Parse(reader.Value);
			}

			throw new FormatException();
		}
	}
}