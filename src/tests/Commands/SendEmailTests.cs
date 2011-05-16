using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	[TestFixture]
	public class SendEmailTests
	{
		private readonly CommandRequestBuilder _builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));


		[Test]
		public void Bulk()
		{
			var sw = new Stopwatch();
			sw.Start();

			for (int i = 0; i < 10; i++)
			{
				var cmd = new SendEmailCommand
				          	{
				          		Source = Helper.GetSenderEmailAddress(),
				          	};

				
				cmd.Message.Subject.Data = "testing SES";
				cmd.Message.Body.Html.Data = "<b>this is bold text</b>";
				cmd.Message.Body.Text.Data = "this is not bold text";
				cmd.Destination.ToAddresses.Add(Helper.GetRecipientEmailAddress());

				var cp = new CommandProcessor(_builder);
				SendEmailResponse response = cp.Process(cmd, new SendEmailResponseParser());

				Console.WriteLine(response.Command + " : ID " + response.RequestID);
				Console.WriteLine(response.Command + " : MessageID " + response.MessageID);
			}

			sw.Stop();
			Console.WriteLine(sw.Elapsed);
		}

		[Test]
		public void BulkTask()
		{
			var sw = new Stopwatch();
			sw.Start();

			const int count = 20;
			var tasks = new Task<SendEmailResponse>[count];
			for (int i = 0; i < count; i++)
			{
				var cmd = new SendEmailCommand
				          	{
				          		Source = Helper.GetSenderEmailAddress(),
				          	};

				cmd.Message.Subject.Data = "testing SES";
				cmd.Message.Body.Html.Data = "<b>this is bold text</b>";
				cmd.Message.Body.Text.Data = "this is not bold text";
				cmd.Destination.ToAddresses.Add(Helper.GetRecipientEmailAddress());

				var cp = new CommandProcessor(_builder);
				tasks[i] = cp.CreateTask(cmd, new SendEmailResponseParser());
			}

			Task.WaitAll(tasks);
			sw.Stop();
			Console.WriteLine(sw.Elapsed);
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
			cmd.Destination.ToAddresses.Add( Helper.GetRecipientEmailAddress());


			
			var cp = new CommandProcessor(_builder);
			var resp = cp.Process(cmd, new SendEmailResponseParser());

			Console.WriteLine(resp.Command + " : ID " + resp.RequestID);
			Console.WriteLine(resp.Command + " : MessageID " + resp.MessageID);

			
		}
	}
}