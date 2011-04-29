using System.Collections.Generic;


namespace McGiv.AWS.SES.Commands
{
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


	public class SendRawEmailCommandResponseParser : CommandResponseParser
	{
		public SendRawEmailCommandResponseParser()
			: base("SendRawEmailResponse")
		{
		}
	}
}
