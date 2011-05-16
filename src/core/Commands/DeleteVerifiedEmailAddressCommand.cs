using System;
using System.Collections.Generic;

namespace McGiv.AWS.SES
{
	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_DeleteVerifiedEmailAddress.html
	/// </summary>
	[Serializable]
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

	[Serializable]
	public class DeleteVerifiedEmailAddressResponse : Response
	{
		
	}
	public class DeleteVerifiedEmailAddressResponseParser : ResponseParser<DeleteVerifiedEmailAddressResponse>
	{

		/*
<DeleteVerifiedEmailAddressResponse xmlns="http://ses.amazonaws.com/doc/2010-12-01/">
  <ResponseMetadata>
    <RequestId>adff1bc2-7e52-11e0-9786-7304214aba17</RequestId>
  </ResponseMetadata>
</DeleteVerifiedEmailAddressResponse>
		 * */
		public DeleteVerifiedEmailAddressResponseParser()
			: base("DeleteVerifiedEmailAddressResponse")
		{
		}
	}
}