using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

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


	public class VerifierEmailAddressCommandResponseParser : ICommandResponseParser<CommandResponse>
	{
		/*
		 * 
		 * example of response
		 
<VerifyEmailAddressResponse xmlns="http://ses.amazonaws.com/doc/2010-12-01/">
  <ResponseMetadata>
    <RequestId>6e27aa01-2c94-11e0-bb3e-d505e2ebe18d</RequestId>
  </ResponseMetadata>
</VerifyEmailAddressResponse>
		 
		 * */

		#region ICommandResponseParser<CommandResponse> Members

		public CommandResponse Process(Stream input)
		{
			var resp = new CommandResponse();

			using (XmlReader reader = XmlReader.Create(input))
			{
				reader.MoveToContent();
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name == "RequestId")
					{
						var el = (XElement) XNode.ReadFrom(reader);
						resp.RequestID = el.Value;
					}
				}
			}

			return resp;
		}

		#endregion
	}
}