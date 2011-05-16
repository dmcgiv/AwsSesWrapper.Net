using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	[TestFixture]
	public class SendQuotaTests
	{
		private readonly CommandProcessor _cp = CommandProcessor.Create(Helper.GetCredentials());
	

		[Test]
		public void SendQuota()
		{
			var cmd = new GetSendQuotaCommand();



			GetSendQuoteResponse resp = _cp.Process(cmd, new GetSendQuoteResponseParser());
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

				GetSendQuoteResponse response = _cp.Process(cmd, new GetSendQuoteResponseParser());

				Console.WriteLine(response.Command + " : ID " + response.RequestID);
				
			
			}

			sw.Stop();
			Console.WriteLine(sw.Elapsed);
		}

		[Test]
		public void BulkTask()
		{
			var sw = new Stopwatch();
			sw.Start();

			const int count = 10;
			var tasks = new Task<GetSendQuoteResponse>[count];
			for (int i = 0; i < count; i++)
			{
				var cmd = new GetSendQuotaCommand();

				tasks[i] = _cp.CreateTask(cmd, new GetSendQuoteResponseParser());

			}

			Task.WaitAll(tasks);
			sw.Stop();
			Console.WriteLine(sw.Elapsed);
		}
	}
}