using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace McGiv.AWS.SES
{
	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_SendEmail.html
	/// </summary>
	public class SendEmailCommand : ICommand
	{
		public SendEmailCommand()
		{
			Destination = new Destination();
			Message = new Message();
		}

		/// <summary>
		/// Sender's emails address
		/// </summary>
		public string Source { get; set; }

		public Destination Destination { get; set; }


		public Message Message { get; set; }

		#region ICommand Members

		public string Action
		{
			get { return "SendEmail"; }
		}

		public Dictionary<string, string> GetData()
		{
			var data = new Dictionary<string, string>();

			data.Add("Source", Source);
			int i = 1;
			foreach (string email in Destination.ToAddresses)
			{
				data.Add("Destination.ToAddresses.member" + (i++), email);
			}
			//data.Add("Destination.ToAddresses", string.Join("; ", this.Destination.ToAddresses));
			//data.Add("Destination.CcAddresses", string.Join("; ", this.Destination.CcAddresses));
			//data.Add("Destination.BccAddresses", string.Join("; ", this.Destination.BccAddresses));

			//data.Add("Message.Subject.Charset", this.Message.Subject.Charset);
			data.Add("Message.Subject.Data", Message.Subject.Data);
			data.Add("Message.Body.Html.Data", Message.Body.Html.Data);
			data.Add("Message.Body.Text.Data", Message.Body.Text.Data);

			return data;
		}

		#endregion
	}


	public class SendInfo
	{
		public string MessageID { get; set; }
	}


	public class SendEmailResponseParser : ICommandResponseParser<SendInfo>
	{
		// example
		/*
<SendEmailResponse xmlns="http://ses.amazonaws.com/doc/2010-12-01/">
  <SendEmailResult>
    <MessageId>0000012dde363baf-4b325872-d719-4a10-b7ff-df1c8f7ad435-000000</MessageId>
  </SendEmailResult>
  <ResponseMetadata>
    <RequestId>39c456cc-2d8a-11e0-9e85-4157ce6dd0c8</RequestId>
  </ResponseMetadata>
</SendEmailResponse>

		 * */

		#region ICommandResponseParser<SendQuote> Members

		public SendInfo Process(Stream input)
		{
			var info = new SendInfo();
			using (XmlReader reader = XmlReader.Create(input))
			{
				reader.MoveToContent();

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						switch (reader.Name)
						{
							case "MessageId":
								{
									info.MessageID = GetNextValue(reader);
									break;
								}
		
						}
					}
				}
			}


			return info;
		}

		#endregion

		private static string GetNextValue(XmlReader reader)
		{
			if (reader.Read() && reader.NodeType == XmlNodeType.Text)
			{
				return reader.Value;
			}

			throw new FormatException();
		}
	}

}