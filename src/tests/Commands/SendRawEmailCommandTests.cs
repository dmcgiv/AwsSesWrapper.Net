using System;
using McGiv.AWS.SES.Commands;
using McGiv.AWS.SES.Util;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests.Commands
{
	[TestFixture]
	public class SendRawEmailCommandTests
	{
		private readonly CommandRequestBuilder _builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));

		[Test]
		public void T1()
		{
			var cmd = new SendRawEmailCommand
			{
				Source = Helper.GetSenderEmailAddress(),

				RawData = RawEmailGenerator.SendRawEmail(Helper.GetSenderEmailAddress(), Helper.GetRecipientEmailAddress(), "test email subject", "this is some text",
			             "<p>this is some html</p><p>asdasd asdasd</p>")
			};


			cmd.Destination.ToAddresses.Add(Helper.GetRecipientEmailAddress());
//            cmd.RawData =
//                @"From: admin@nimtug.org
//To: damien@mcgiv.com
//Subject: testing SES
//MIME-Version: 1.0
//Content-Type: multipart/alternative; 
//	boundary=""----=_Part_286008_14012783.1304034624483""
//Date: Thu, 28 Apr 2011 23:50:24 +0000
//Message-ID: <0000012f9e8543e1-174c6eb5-8338-4447-a79c-02b5acf61ae7-000000@email.amazonses.com>
//
//------=_Part_286008_14012783.1304034624483
//Content-Type: text/plain; charset=UTF-8
//Content-Transfer-Encoding: 7bit
//
//this is not bold text
//------=_Part_286008_14012783.1304034624483
//Content-Type: text/html; charset=UTF-8
//Content-Transfer-Encoding: 7bit
//
//<b>this is bold text</b>
//------=_Part_286008_14012783.1304034624483--
//";

			Console.WriteLine(cmd.RawData);

			var cp = new CommandProcessor(_builder);

			var resp = cp.Process(cmd, new SendRawEmailCommandResponseParser());


			Console.WriteLine(resp.Command + " : ID " + resp.RequestID);
		}
	}
}
