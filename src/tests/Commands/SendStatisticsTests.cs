using System;
using System.Net;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	[TestFixture]
	public class SendStatisticsTests
	{
		private readonly CommandRequestBuilder _builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));


		[Test]
		public void SendStatistics()
		{
			var cmd = new GetSendStatisticsCommand();


			HttpWebRequest request = _builder.Build(cmd);

			Helper.ProcessRequest(request);

			var cp = new CommandProcessor(_builder);

			var stats = cp.Process(cmd, new GetSendStatisticsResponseParser());

			foreach(var stat in stats.SendStatistics)
			{
				Console.WriteLine("DeliveryAttempts : " + stat.DeliveryAttempts);
				Console.WriteLine("Timestamp : " + stat.Timestamp);
				Console.WriteLine("Bounces : " + stat.Bounces);
				Console.WriteLine("Rejects : " + stat.Rejects);
				Console.WriteLine("Complaints : " + stat.Complaints);
			}
			
		}
	}
}