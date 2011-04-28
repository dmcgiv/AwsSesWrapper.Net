using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace McGiv.AWS.SES
{


	public class CommandResponseParser : ICommandResponseParser<CommandResponse>
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

		public CommandResponseParser(string command)
		{
			this.Command = command;
		}
		
		public string Command { get; private set; }

		public CommandResponse Process(Stream input)
		{
			var resp = new CommandResponse();
			resp.Command = this.Command;

			using (XmlReader reader = XmlReader.Create(input))
			{
				
				reader.MoveToContent();
				reader.ReadStartElement(this.Command);
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name == "RequestId")
					{
						var el = (XElement)XNode.ReadFrom(reader);
						resp.RequestID = el.Value;
					}
				}
			}

			return resp;
		}

		#endregion
	}


	/// <summary>
	/// Parses a response stream into a Error Response object
	/// </summary>
	public class ErrorResponseParser : ICommandResponseParser<ErrorResponse>
	{


		/*
		 * expected XML format
		 * see http://docs.amazonwebservices.com/ses/latest/DeveloperGuide/index.html?UnderstandingResponses.html
		 * 

<ErrorResponse>
   <Error>
      <Type>
         Sender
      </Type>
      <Code>
         ValidationError
      </Code>
      <Message>
         Value null at 'message.subject' failed to satisfy constraint: Member must not be null
      </Message>
   </Error>
   <RequestId>
      42d59b56-7407-4c4a-be0f-4c88daeea257
   </RequestId>
</ErrorResponse>
		 
		 * 
		 * 
		 * 
		 * */
		public ErrorResponse Process(Stream input)
		{
			var data = new ErrorResponse();
			using (XmlReader reader = XmlReader.Create(input))
			{
				reader.MoveToContent();

				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element)
					{
						continue;
					}

					switch (reader.Name)
					{
						case "Type":
							{
								var el = (XElement)XNode.ReadFrom(reader);
								data.Type = el.Value;
								break;
							}
						case "Code":
							{
								var el = (XElement)XNode.ReadFrom(reader);
								data.Code = el.Value;
								break;
							}
						case "Message":
							{
								var el = (XElement)XNode.ReadFrom(reader);
								data.Message = el.Value;
								break;
							}
						case "RequestId":
							{
								var el = (XElement)XNode.ReadFrom(reader);
								data.RequestID = el.Value;
								break;
							}

					}
				}
			}


			return data;
		}
	}


	public class ResponseParser
	{
		public string GetResponse(WebRequest request)
		{
			try
			{
				using (WebResponse response = request.GetResponse())
				{
					if (response == null)
					{
						return null;
					}

					using (Stream dataStream = response.GetResponseStream())
					{
						if (dataStream == null)
						{
							//Assert.Fail("GetResponseStream is null");
							return null;
						}

						using (var reader = new StreamReader(dataStream))
						{
							string responseFromServer = reader.ReadToEnd();

							Console.WriteLine(responseFromServer);
						}
					}
				}
			}
			catch (WebException e)
			{
				using (WebResponse response = e.Response)
				{
					var httpResponse = (HttpWebResponse) response;
					//if (httpResponse.StatusCode != (HttpStatusCode)400)
					//{

					//}
					using (Stream dataStream = httpResponse.GetResponseStream())
					{
						if (dataStream == null)
						{
							return null;
						}

						using (var reader = new StreamReader(dataStream))
						{
							string responseFromServer = reader.ReadToEnd();

							return responseFromServer;
						}
					}
				}
			}

			return null;
		}
	}
}