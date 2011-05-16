using System;
using System.Collections.Generic;
using System.Xml;


namespace McGiv.AWS.SES
{
	[Serializable]
	public class SendRawEmailCommand : ICommand
	{

		public SendRawEmailCommand()
		{
			Destination = new Destination();
		}

		public string Action
		{
			get { return "SendRawEmail"; }
		}

		public string Source { get; set; }

		public Destination Destination { get; set; }

		public string RawData { get; set; }

		public Dictionary<string, string> GetData()
		{
			var data = new Dictionary<string, string> { { "RawMessage.Data", RawData } };

			//int i = 1;
			//foreach (string email in Destination.ToAddresses)
			//{
			//    data.Add("Destinations.member." + (i++), email);
			//}

			//if (!string.IsNullOrEmpty(this.Source))
			//{
			//    data.Add("Source", this.Source);
			//}


			


			return data;
		}
	}


	public class SendRawEmailResponseParser : ResponseParser<SendEmailResponse>
	{
		public SendRawEmailResponseParser()
			: base("SendRawEmailResponse")
		{
		}

		protected override void InnerParse(XmlReader reader, SendEmailResponse response)
		{

			reader.ReadStartElement("SendRawEmailResult");
			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}

				switch (reader.Name)
				{
					case "MessageId":
						{
							response.MessageID = GetNextValue(reader);
							break;
						}
					case "ResponseMetadata":
						{
							return;
						}
				}
			}
		}

	}
}
