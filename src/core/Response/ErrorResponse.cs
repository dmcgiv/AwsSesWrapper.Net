using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace McGiv.AWS.SES
{

	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/DeveloperGuide/index.html?UnderstandingResponses.html
	/// </summary>
	public class ErrorResponse
	{
		public string Type { get; set; }
		public string Code { get; set; }
		public string Message { get; set; }
		public string RequestID { get; set; }
	}


	public class AwsSesException : Exception
	{
		public AwsSesException(ErrorResponse error, Exception exception)
			: base(error.Message, exception)
		{
			
		}
		public ErrorResponse ErrorResponse { get; private set; }

	}
}
