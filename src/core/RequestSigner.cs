using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace McGiv.AWS.SES
{

	public enum RequestSignerAlgorithm
	{
		HmacSHA1 = 1,
		HmacSHA256 = 2

	}
	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/DeveloperGuide/index.html?HMACShaSignatures.html
	/// and http://docs.amazonwebservices.com/Route53/latest/DeveloperGuide/index.html?RESTAuthentication.html#AuthorizationHeader
	/// </summary>
	public class RequestSigner
	{
		private readonly AwsCredentials _credentials;
		private readonly KeyedHashAlgorithm _algorithm;
		readonly Func<string, byte[]> getBytes = Encoding.UTF8.GetBytes;

		public RequestSigner(AwsCredentials credentials)
			: this(credentials, RequestSignerAlgorithm.HmacSHA1)
		{
			
		}

		public RequestSigner(AwsCredentials credentials, RequestSignerAlgorithm algorithm)
		{
			if (credentials == null)
			{
				throw new ArgumentNullException("credentials");
			}


			

			switch (algorithm)
			{
				case RequestSignerAlgorithm.HmacSHA1:
					{
						_algorithm = new HMACSHA1();
						break;
					}

				case RequestSignerAlgorithm.HmacSHA256:
					{
						_algorithm = new HMACSHA256();
						break;
					}

				default:
					{
						throw new ArgumentException("Invalid value : " + algorithm, "algorithm");
					}
			}


			_credentials = credentials;
			_algorithm.Key = getBytes(_credentials.SecretAccessKey);
			RequestSignerAlgorithm = algorithm;


		}


		public RequestSignerAlgorithm RequestSignerAlgorithm { get; private set; }

		/// <summary>
		/// Signes the request by adding a X-Amzn-Authorization HTTP header to the request.
		/// </summary>
		/// <param name="request"></param>
		public void SignRequest(HttpWebRequest request)
		{
			// date header
			request.Date = DateTime.UtcNow;



			// sign request with request date
			var date = request.Date.ToString("ddd, dd MMM yyyy HH:mm:ss ") + "GMT"; // e.g Tue, 25 May 2010 23:05:27 GMT // todo make more generic
			var sb = new StringBuilder();
			sb.Append("AWS3-HTTPS AWSAccessKeyId=");
			sb.Append(this._credentials.AccessKeyID);


			sb.Append(", Algorithm=");
			switch(RequestSignerAlgorithm)
			{
				case RequestSignerAlgorithm.HmacSHA1:
					{
						sb.Append("HmacSHA1");
						break;
					}

				case RequestSignerAlgorithm.HmacSHA256:
					{
						sb.Append("HmacSHA256");
						break;
					}
			}
			

			sb.Append(", Signature=");
			sb.Append(GenerateSignature(date));

			request.Headers.Add("X-Amzn-Authorization", sb.ToString());
		}




		/// <summary>
		/// Generates a hash of the given data
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public string GenerateSignature(string data)
		{
			

			return Convert.ToBase64String(
				_algorithm.ComputeHash(
					getBytes(data)
					)
				);

		}
	}
}
