using System;
using System.Net;
using System.Web;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	[TestFixture]
	public class DeleteVerifiedEmailAddressCommandTests
	{
		private readonly CommandRequestBuilder _builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));
		private readonly string _email = Helper.GetSenderEmailAddress();
		private readonly string _encodedDmail;

		public DeleteVerifiedEmailAddressCommandTests()
		{
			_encodedDmail = HttpUtility.UrlEncode(_email);
		}

		[Test]
		public void Format()
		{
			var cmd = new DeleteVerifiedEmailAddressCommand
			          	{
			          		EmailAddress = _email
			          	};


			string data = _builder.FormatData(cmd);

			Assert.AreEqual("Action=DeleteVerifiedEmailAddress&EmailAddress=" + _encodedDmail, data);
		}


		[Test]
		public void Run()
		{
			var cmd = new DeleteVerifiedEmailAddressCommand
			          	{
			          		EmailAddress = _email
			          	};


			

			var cp = new CommandProcessor(_builder);

			var resp = cp.Process(cmd, new DeleteVerifiedEmailAddressCommandResponseParser());

			Console.WriteLine(resp.Command + " : ID " + resp.RequestID);

		}
	}
}