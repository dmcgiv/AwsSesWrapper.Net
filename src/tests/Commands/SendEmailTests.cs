using System;
using System.Diagnostics;
using System.Net;
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
				SendInfo quote = cp.Process(cmd, new SendEmailResponseParser());


			}

			sw.Stop();
			Console.WriteLine(sw.Elapsed);
		}

		[Test]
		public void BulkTask()
		{
			var sw = new Stopwatch();
			sw.Start();

			int count = 20;
			var tasks = new Task<SendInfo>[count];
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

			
		}
	}
}