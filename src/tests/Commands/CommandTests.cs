using System;
using System.IO;
using System.Net;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	// todo break up into individual command tests
	//[TestFixture]
	public class CommandTests
	{
		


		private static void ProcessRequest(HttpWebRequest request)
		{
			try
			{
				using (WebResponse response = request.GetResponse())
				{
					using (Stream dataStream = response.GetResponseStream())
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


	}
}