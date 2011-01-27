using System.Collections.Generic;


namespace McGiv.AWS.SES
{

	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_DeleteVerifiedEmailAddress.html
	/// </summary>
	public class DeleteVerifiedEmailAddressCommand : ICommand
	{
		public string Action
		{
			get { return "DeleteVerifiedEmailAddress"; }
		}

		public string EmailAddress { get; set; }


		public Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>
			       	{
			       		{"EmailAddress", this.EmailAddress}
			       	};
		}
	}
}
