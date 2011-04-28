using System;
using System.IO;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests.Commands
{
	[TestFixture]
	public class ListVerifiedEmailAddressesCommandTests
	{
		private readonly CommandRequestBuilder _builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));
		private readonly ListVerifiedEmailAddressesResponseParser _parser = new ListVerifiedEmailAddressesResponseParser();


		[Test]
		public void List()
		{
			var cmd = new ListVerifiedEmailAddressesCommand();

			var cp = new CommandProcessor(_builder);
			var resp = cp.Process(cmd, _parser);
			Console.WriteLine(resp.Command + " : ID " +  resp.RequestID);

			foreach (string email in resp.Emails)
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