using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace McGiv.AWS.SES
{


	public abstract class ResponseParser<TResponse> : ICommandResponseParser<TResponse>
		where TResponse : Response, new()
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

		protected ResponseParser(string command)
		{
			this.Command = command;
		}
		
		public string Command { get; private set; }

		public virtual TResponse Process(Stream input)
		{
			var response = new TResponse
			           	{
			           		Command = this.Command
			           	};

			using (XmlReader reader = XmlReader.Create(input))
			{
				
				reader.MoveToContent();
				reader.ReadStartElement(this.Command);
				InnerParse(reader, response);
				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element || reader.Name != "RequestId")
					{
						continue;
					}
					var el = (XElement)XNode.ReadFrom(reader);
					response.RequestID = el.Value;
				}
			}

			return response;
		}

		protected virtual void InnerParse(XmlReader reader, TResponse response)
		{
			
		}

		protected float GetNextFloatValue(XmlReader reader)
		{
			if (reader.Read() && reader.NodeType == XmlNodeType.Text)
			{
				return float.Parse(reader.Value);
			}

			throw new FormatException();
		}

		protected static string GetNextValue(XmlReader reader)
		{
			if (reader.Read() && reader.NodeType == XmlNodeType.Text)
			{
				return reader.Value;
			}

			throw new FormatException();
		}

		#endregion
	}




}