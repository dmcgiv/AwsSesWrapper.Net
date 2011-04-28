using System.Collections.Generic;

namespace McGiv.AWS.SES
{
	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_DeleteVerifiedEmailAddress.html
	/// </summary>
	public class DeleteVerifiedEmailAddressCommand : ICommand
	{
		public string EmailAddress { get; set; }

		#region ICommand Members

		public string Action
		{
			get { return "DeleteVerifiedEmailAddress"; }
		}


		public Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>
			       	{
			       		{"EmailAddress", EmailAddress}
			       	};
		}

		#endregion
	}

	public class DeleteVerifiedEmailAddressCommandResponseParser : CommandResponseParser
	{
		public DeleteVerifiedEmailAddressCommandResponseParser()
			: base("DeleteVerifiedEmailAddressResponse")
		{
		}
	}
}