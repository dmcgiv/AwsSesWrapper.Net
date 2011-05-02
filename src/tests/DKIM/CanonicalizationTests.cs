using McGiv.AWS.SES.DKIM;
using McGiv.AWS.SES.Tests.Util;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests.DKIM
{

	[TestFixture]
	public class CanonicalizationTests
	{
		[TestCase(@"A: X
B:Y	
	Z  

 C 
D 	 E


", @"a:X
b:Y Z
", @" C
D E
", CanonicalizationAlgorithm.Relaxed)]
		[TestCase(@"A: X
B:Y	
	Z  

 C 
D 	 E


", @"A: X
B:Y	
	Z  
", @" C 
D 	 E
", CanonicalizationAlgorithm.Simple)]
		public void Canonicalization2(string emailText, string canonicalizedHeaders, string canonicalizedBody, CanonicalizationAlgorithm type)
		{

			var email = DKIMSigner.ParseEmail(emailText);

			Assert.AreEqual(canonicalizedBody, Canonicalization.CanonicalizationBody(email.Body, type), "body " + type);
			Assert.AreEqual(canonicalizedHeaders, Canonicalization.CanonicalizationHeaders(email.Headers, type), "headers " + type);
		}
	}
}
