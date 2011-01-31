using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace McGiv.AWS.SES
{
	/// <summary>
	///  see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_ListVerifiedEmailAddresses.html
	/// </summary>
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


	public class ListVerifiedEmailAddressesResponseParser : ICommandResponseParser<string[]>
	{
		#region ICommandResponseParser<string[]> Members

		public string[] Process(Stream input)
		{
//<ListVerifiedEmailAddressesResponse xmlns="http://ses.amazonaws.com/doc/2010-12-01/">
//  <ListVerifiedEmailAddressesResult>
//    <VerifiedEmailAddresses>
//      <member>admin@nimtug.org</member>
//    </VerifiedEmailAddresses>
//  </ListVerifiedEmailAddressesResult>
//  <ResponseMetadata>
//    <RequestId>56c399a3-2c9a-11e0-a410-ab87b662d75f</RequestId>
//  </ResponseMetadata>
//</ListVerifiedEmailAddressesResponse>


			var emails = new List<string>();
			using (XmlReader reader = XmlReader.Create(input))
			{
				reader.MoveToContent();
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
					else
					{
						lastElementWasMember = false;
					}
				}
			}

			return emails.ToArray();
		}

		#endregion
	}
}