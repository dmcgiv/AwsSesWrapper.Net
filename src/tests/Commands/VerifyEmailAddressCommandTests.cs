using System;
using System.IO;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	[TestFixture]
	[Ignore("This will cause AWS to send an email to the email address used. Run test by commenting out this attribute.")]
	public class VerifyEmailTests
	{
		private readonly CommandRequestBuilder _builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));
		private readonly VerifierEmailAddressCommandResponseParser _parser = new VerifierEmailAddressCommandResponseParser();


		private static void FinishWebRequest(IAsyncResult result)
		{
			var response = (HttpWebResponse) ((HttpWebRequest) result.AsyncState).EndGetResponse(result);

			//using (var dataStream = response.GetResponseStream())
			Stream dataStream = response.GetResponseStream();
			{
				if (dataStream == null)
				{
					Assert.Fail("GetResponseStream is null");
				}

				new AsyncStreamReader(dataStream, data =>
				                                  	{
				                                  		response.Close();

				                                  		ProcessRequest(data);
				                                  	});
			}
		}

		private static void ProcessRequest(byte[] data)
		{
			string resp = Encoding.ASCII.GetString(data);

			Console.WriteLine(resp);
		}

		[Test]
		public void VerifyTest()
		{
			var cmd = new VerifyEmailAddressCommand
			          	{
			          		EmailAddress = Helper.GetSenderEmailAddress()
			          	};


			var cp = new CommandProcessor(_builder);

			var resp = cp.Process(cmd, _parser);


			Console.WriteLine(resp.Command + " : ID " +  resp.RequestID);
		}
	}
}