using System;
using System.Configuration;
using System.IO;
using System.Net;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	public static class Helper
	{
		/// <summary>
		/// Generates credentials from app settings
		/// </summary>
		/// <returns></returns>
		public static AwsCredentials GetCredentials()
		{
			return new AwsCredentials(ConfigurationManager.AppSettings["AccessKeyID"],
			                          ConfigurationManager.AppSettings["SecretAccessKey"]);
		}

		public static AwsCredentials GetCredentials(string key)
		{
			return new AwsCredentials(ConfigurationManager.AppSettings["AccessKeyID"],
			                          key);
		}


		public static string GetSenderEmailAddress()
		{
			return ConfigurationManager.AppSettings["SenderEmail"];
		}


		public static string GetRecipientEmailAddress()
		{
			return ConfigurationManager.AppSettings["RecipientEmail"];
		}


		public static void ProcessRequest(HttpWebRequest request)
		{
			try
			{
				using (WebResponse response = request.GetResponse())
				{
					//Console.WriteLine(((HttpWebResponse) response).StatusDescription);

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