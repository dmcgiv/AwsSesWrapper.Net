﻿using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace McGiv.AWS.SES
{
	/// <summary>
	/// Hashing algorithms supported by SES
	/// </summary>
	public enum RequestSignerAlgorithm
	{
		HmacSha1 = 1,
		HmacSha256 = 2
	}

	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/DeveloperGuide/index.html?HMACShaSignatures.html
	/// and http://docs.amazonwebservices.com/Route53/latest/DeveloperGuide/index.html?RESTAuthentication.html#AuthorizationHeader
	/// </summary>
	public class RequestSigner
	{
		private readonly KeyedHashAlgorithm _algorithm;
		private readonly AwsCredentials _credentials;
		private readonly Func<string, byte[]> _getBytes = Encoding.ASCII.GetBytes;

		public RequestSigner(AwsCredentials credentials, RequestSignerAlgorithm algorithm = RequestSignerAlgorithm.HmacSha1)
		{
			if (credentials == null)
			{
				throw new ArgumentNullException("credentials");
			}


			switch (algorithm)
			{
				case RequestSignerAlgorithm.HmacSha1:
					{
						_algorithm = new HMACSHA1();
						break;
					}

				case RequestSignerAlgorithm.HmacSha256:
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
			_algorithm.Key = _getBytes(_credentials.SecretAccessKey);
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
			string date = request.Date.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss ") + "GMT";

			// e.g Tue, 25 May 2010 23:05:27 GMT // todo make more generic
			var sb = new StringBuilder();
			sb.Append("AWS3-HTTPS AWSAccessKeyId=");
			sb.Append(_credentials.AccessKeyID);


			sb.Append(", Algorithm=");
			switch (RequestSignerAlgorithm)
			{
				case RequestSignerAlgorithm.HmacSha1:
					{
						sb.Append("HmacSHA1");
						break;
					}

				case RequestSignerAlgorithm.HmacSha256:
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
					_getBytes(data)
					)
				);
		}
	}
}