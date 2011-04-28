using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests.Commands
{
	[TestFixture]
	public class SendQuotaTests
	{
		private readonly CommandRequestBuilder _builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));

		[Test]
		public void SendQuota()
		{
			var cmd = new GetSendQuotaCommand();


			//HttpWebRequest request = builder.Build(cmd);

			//Helper.ProcessRequest(request);


			var cp = new CommandProcessor(_builder);

			GetSendQuoteResponse resp = cp.Process(cmd, new GetSendQuoteResponseParser());
			Console.WriteLine(resp.Command + " : ID " + resp.RequestID);

			Console.WriteLine("Max24HourSend = " + resp.Max24HourSend);
			Console.WriteLine("MaxSendRate = " + resp.MaxSendRate);
			Console.WriteLine("SentLast24Hours = " + resp.SentLast24Hours);
		}


		[Test]
		public void Bulk()
		{
			var sw = new Stopwatch();
			sw.Start();
			
			for(int i=0; i<10; i++)
			{
				var cmd = new GetSendQuotaCommand();
				var cp = new CommandProcessor(_builder);
				GetSendQuoteResponse quote = cp.Process(cmd, new GetSendQuoteResponseParser());
			
			}

			sw.Stop();
			Console.WriteLine(sw.Elapsed);
		}

		[Test]
		public void BulkTask()
		{
			var sw = new Stopwatch();
			sw.Start();

			int count = 10;
			var tasks = new Task<GetSendQuoteResponse>[count];
			for (int i = 0; i < count; i++)
			{
				var cmd = new GetSendQuotaCommand();
				var cp = new CommandProcessor(_builder);
				tasks[i] = cp.CreateTask(cmd, new GetSendQuoteResponseParser());

			}

			Task.WaitAll(tasks);
			sw.Stop();
			Console.WriteLine(sw.Elapsed);
		}
	}
}