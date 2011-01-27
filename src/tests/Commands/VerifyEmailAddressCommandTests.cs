using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	[TestFixture]
	[Ignore("This will cause AWS to send an email to the email address used. Run test by commenting out this attribute.")]
	public class VerifyEmailTests
	{

		readonly CommandRequestBuilder builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));

		[Test]
		public void VerifyTest()
		{
			var cmd = new VerifyEmailAddressCommand
			{
				EmailAddress = Helper.GetSenderEmailAddress()
			};


			var request = builder.Build(cmd);

			Helper.ProcessRequest(request);

		}
	}
}
