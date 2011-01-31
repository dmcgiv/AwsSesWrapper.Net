using System.Net;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests.Commands
{
	[TestFixture]
	public class SendStatisticsTests
	{
		private readonly CommandRequestBuilder builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));


		[Test]
		public void SendStatistics()
		{
			var cmd = new GetSendStatisticsCommand();


			HttpWebRequest request = builder.Build(cmd);

			Helper.ProcessRequest(request);
		}
	}
}