using System;
using System.Collections.Generic;
using System.Xml;

namespace McGiv.AWS.SES
{
	/// <summary>
	///  see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_ListVerifiedEmailAddresses.html
	/// </summary>
	[Serializable]
	public class ListVerifiedEmailAddressesCommand : ICommand
	{
		#region ICommand Members

		public string Action
		{
			get { return "ListVerifiedEmailAddresses"; }
		}

		public Dictionary<string, string> GetData()
		{
			return null;
		}

		#endregion
	}


	[Serializable]
	public class ListVerifiedEmailAddressesResponse : Response
	{
		public string[] Emails { get; internal set; }
	}



	public class ListVerifiedEmailAddressesResponseParser : ResponseParser<ListVerifiedEmailAddressesResponse>
	{

//<ListVerifiedEmailAddressesResponse xmlns="http://ses.amazonaws.com/doc/2010-12-01/">
//  <ListVerifiedEmailAddressesResult>
//    <VerifiedEmailAddresses>
//      <member>admin@nimtug.org</member>
//      <member>damien.mcgivern@aurion.co.uk</member>
//    </VerifiedEmailAddresses>
//  </ListVerifiedEmailAddressesResult>
//  <ResponseMetadata>
//    <RequestId>5942b90d-7f24-11e0-94eb-9168216feecf</RequestId>
//  </ResponseMetadata>
//</ListVerifiedEmailAddressesResponse>

		#region ICommandResponseParser<string[]> Members

		public ListVerifiedEmailAddressesResponseParser()
			: base("ListVerifiedEmailAddressesResponse")
		{
		}

		protected override void InnerParse(XmlReader reader, ListVerifiedEmailAddressesResponse response)
		{
			var emails = new List<string>();
			reader.ReadStartElement("ListVerifiedEmailAddressesResult");
			bool lastElementWasMember = false;
			while (reader.Read())
			{
				if (!lastElementWasMember && reader.NodeType == XmlNodeType.Element && reader.Name == "member")
				{
					lastElementWasMember = true;
				}
				else if (lastElementWasMember && reader.NodeType == XmlNodeType.Text)
				{
					emails.Add(reader.Value);
					lastElementWasMember = false;
				}
				else if(reader.Name == "ResponseMetadata")
				{
					goto completed;
				}
				else
				{
					lastElementWasMember = false;
				}
			}

			completed:
			response.Emails = emails.ToArray();
		}

		

		#endregion
	}
}