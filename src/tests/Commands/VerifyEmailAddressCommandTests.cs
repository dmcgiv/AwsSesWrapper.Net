using System;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	[TestFixture]
	//[Ignore("This will cause AWS to send an email to the email address used. Run test by commenting out this attribute.")]
	public class VerifyEmailTests
	{
		private readonly CommandRequestBuilder _builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));
		private readonly VerifyEmailAddressResponseParser _parser = new VerifyEmailAddressResponseParser();



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