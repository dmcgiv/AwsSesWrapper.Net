using System.Collections.Generic;


namespace McGiv.AWS.SES
{

	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_SendEmail.html
	/// </summary>
	public class SendEmailCommand : ICommand
	{
		public SendEmailCommand()
		{
			this.Destination = new Destination();
			this.Message = new Message();
		}

		/// <summary>
		/// Sender's emails address
		/// </summary>
		public string Source { get; set; }

		public Destination Destination { get; set; }


		public Message Message { get; set; }

		public string Action
		{
			get { return "SendEmail"; }
		}

		public Dictionary<string, string> GetData()
		{
			
			var data = new Dictionary<string, string>();

			data.Add("Source", this.Source);
			int i = 1;
			foreach (var email in this.Destination.ToAddresses)
			{
				data.Add("Destination.ToAddresses.member" + (i++), email);
			}
			//data.Add("Destination.ToAddresses", string.Join("; ", this.Destination.ToAddresses));
			//data.Add("Destination.CcAddresses", string.Join("; ", this.Destination.CcAddresses));
			//data.Add("Destination.BccAddresses", string.Join("; ", this.Destination.BccAddresses));

			//data.Add("Message.Subject.Charset", this.Message.Subject.Charset);
			data.Add("Message.Subject.Data", this.Message.Subject.Data);
			data.Add("Message.Body.Html.Data", this.Message.Body.Html.Data);
			data.Add("Message.Body.Text.Data", this.Message.Body.Text.Data);

			return data;

		}
	}
}
