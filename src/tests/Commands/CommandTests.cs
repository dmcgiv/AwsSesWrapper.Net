using System;
using System.IO;
using System.Net;
using NUnit.Framework;



namespace McGiv.AWS.SES.Tests
{

	// todo break up into individual command tests
	[TestFixture]
	public class CommandTests
	{
		readonly CommandRequestBuilder builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));




	

		private static void ProcessRequest(HttpWebRequest request)
		{
			try
			{
				using (WebResponse response = request.GetResponse())
				{

					//Console.WriteLine(((HttpWebResponse) response).StatusDescription);

					using (var dataStream = response.GetResponseStream())
					{

						if (dataStream == null)
						{
							Assert.Fail("GetResponseStream is null");
						}

						using (var reader = new StreamReader(dataStream))
						{

							string responseFromServer = reader.ReadToEnd();

							Console.WriteLine(responseFromServer);

						}

					}
				}
			}
			catch (Exception e)
			{
				Assert.Fail(e.Message);
				
			}

		

		}

		[Test]
		public void List()
		{
			var cmd = new ListVerifiedEmailAddressesCommand();

			



			var request = builder.Build(cmd);

			ProcessRequest(request);

		}


		[Test]
		public void SendQuota()
		{
			var cmd = new GetSendQuotaCommand();

			



			var request = builder.Build(cmd);

			ProcessRequest(request);

		}

		[Test]
		public void SendStatistics()
		{
			var cmd = new GetSendStatisticsCommand();

			



			var request = builder.Build(cmd);

			ProcessRequest(request);

		}


		[Test]
		public void Send()
		{
			var cmd = new SendEmailCommand
			          	{
							Source = Helper.GetSenderEmailAddress(),
							
			          	};

			cmd.Message.Subject.Data = "testing SES";
			cmd.Message.Body.Html.Data = "<b>this is bold text</b>";
			cmd.Message.Body.Text.Data = "this is not bold text";
			cmd.Destination.ToAddresses.Add(Helper.GetRecipientEmailAddress());

			



			var request = builder.Build(cmd);

			ProcessRequest(request);

		}
	}
}
