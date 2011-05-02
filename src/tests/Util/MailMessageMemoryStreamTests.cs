using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using McGiv.AWS.SES.Util;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests.Util
{

	[TestFixture]
	public class MailMessageMemoryStreamTests
	{

		[Test]
		public void T1()
		{
			RawEmailGenerator.SendRawEmail(Encoding.ASCII, "damien@mcgiv.com", "admin@nimtug.org", "test email subject", "this is some text",
			             "<p>this is some html</p><p>asdasd asdasd</p>");

		}

		
	}
}
