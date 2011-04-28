using System.Collections.Generic;


namespace McGiv.AWS.SES
{
	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_VerifyEmailAddress.html
	/// </summary>
	public class VerifyEmailAddressCommand : ICommand
	{
		public string EmailAddress { get; set; }

		#region ICommand Members

		public string Action
		{
			get { return "VerifyEmailAddress"; }
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


	public class VerifierEmailAddressCommandResponseParser : CommandResponseParser
	{
		public VerifierEmailAddressCommandResponseParser()
			: base("VerifyEmailAddressResponse")
		{
		}
	}
}