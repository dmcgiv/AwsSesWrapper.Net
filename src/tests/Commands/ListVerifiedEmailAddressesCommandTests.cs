using System;
using System.IO;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests.Commands
{
	[TestFixture]
	public class ListVerifiedEmailAddressesCommandTests
	{
		private readonly CommandRequestBuilder builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));
		private readonly ListVerifiedEmailAddressesResponseParser parser = new ListVerifiedEmailAddressesResponseParser();


		[Test]
		public void List()
		{
			var cmd = new ListVerifiedEmailAddressesCommand();

			var cp = new CommandProcessor(builder);


			foreach (string email in cp.Process(cmd, new ListVerifiedEmailAddressesResponseParser()))
			{
				Console.WriteLine(email);
			}
		}
	}

	public class ConsoleOutputParser : ICommandResponseParser<string>
	{
		#region ICommandResponseParser<string> Members

		public string Process(Stream input)
		{
			var reader = new StreamReader(input);
			return reader.ReadToEnd();
		}

		#endregion
	}
}