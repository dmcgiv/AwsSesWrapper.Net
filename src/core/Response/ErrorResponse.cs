using System;
using System.Xml;
using System.Xml.Linq;


namespace McGiv.AWS.SES
{

	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/DeveloperGuide/index.html?UnderstandingResponses.html
	/// </summary>
	public class ErrorResponse : Response
	{
		public string Type { get; set; }
		public string Code { get; set; }
		public string Message { get; set; }
		
	}

	/// <summary>
	/// Parses a response stream into a Error Response object
	/// </summary>
	public class ErrorResponseParser : ResponseParser<ErrorResponse>
	{


		/*
		 * expected XML format
		 * see http://docs.amazonwebservices.com/ses/latest/DeveloperGuide/index.html?UnderstandingResponses.html
		 * http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?CommonErrors.html
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

		public ErrorResponseParser()
			: base("ErrorResponse")
		{
		}


		protected override void InnerParse(XmlReader reader, ErrorResponse response)
		{
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
							response.Type = el.Value;
							break;
						}
					case "Code":
						{
							var el = (XElement)XNode.ReadFrom(reader);
							response.Code = el.Value;
							break;
						}
					case "Message":
						{
							var el = (XElement)XNode.ReadFrom(reader);
							response.Message = el.Value;
							break;
						}
					case "RequestId":
						{
							var el = (XElement)XNode.ReadFrom(reader);
							response.RequestID = el.Value;
							break;
						}

					default:
						{
							return;
						}

				}
			}
		}


		//public ErrorResponse Process(Stream input)
		//{
		//    var r = new StreamReader(input);
		//    Console.WriteLine(r.ReadToEnd());
		//    input.Position = 0;


		//    var data = new ErrorResponse();
		//    using (XmlReader reader = XmlReader.Create(input))
		//    {
		//        reader.MoveToContent();

				
		//    }


		//    return data;
		//}
	}


//    public enum ErrorType
//    {
//        /// <summary>
//        /// The request signature does not conform to AWS standards.
//        /// </summary>
//        IncompleteSignature= 400,

//        /// <summary>
//        /// The request processing has failed due to some unknown error, exception or failure.
//        /// </summary>
//InternalFailure=500,

//        /// <summary>
//        /// The action or operation requested is invalid.
//        /// </summary>
//InvalidAction

//400
//InvalidClientTokenId
//The X.509 certificate or AWS Access Key ID provided does not exist in our records.
//403
//InvalidParameterCombination
//Parameters that must not be used together were used together.
//400
//InvalidParameterValue
//A bad or out-of-range value was supplied for the input parameter.
//400
//InvalidQueryParameter
//AWS query string is malformed, does not adhere to AWS standards.
//400
//MalformedQueryString
//The query string is malformed.
//404
//MissingAction
//The request is missing an action or operation parameter.
//400
//MissingAuthenticationToken
//Request must contain either a valid (registered) AWS Access Key ID or X.509 certificate.
//403
//MissingParameter
//An input parameter that is mandatory for processing the request is not supplied.
//400
//OptInRequired
//The AWS Access Key ID needs a subscription for the service.
//403
//RequestExpired
//Request is past expires date or the request date (either with 15 minute padding), or the request date occurs more than 15 minutes in the future.
//400
//ServiceUnavailable
//The request has failed due to a temporary failure of the server.
//503
//Throttling
//Request was denied due to request throttling.
//400
//    }

	public class AwsSesException : Exception
	{
		public AwsSesException(ErrorResponse error, Exception exception)
			: base(error.Message, exception)
		{
			this.ErrorResponse = error;
		}
		public ErrorResponse ErrorResponse { get; private set; }

	}
}
