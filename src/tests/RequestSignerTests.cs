using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	[TestFixture]
	public class RequestSignerTests
	{
		/// <summary>
		/// see http://docs.amazonwebservices.com/Route53/latest/DeveloperGuide/index.html?RESTAuthentication.html#AuthorizationHeader
		/// </summary>
		/// <param name="date">Date to be hashed</param>
		/// <param name="key">Key used in hashing</param>
		/// <param name="hash">Expected hash</param>
		[TestCase("Thu, 14 Aug 2008 17:08:48 GMT", "/Ml61L9VxlzloZ091/lkqVV5X1/YvaJtI9hW4Wr9", "4cP0hCJsdCxTJ1jPXo7+e/YSu0g=")
		]
		public void Hash(string date, string key, string hash)
		{
			var signer = new RequestSigner(Helper.GetCredentials(key));

			Assert.AreEqual(hash, signer.GenerateSignature(date));
		}
	}
}