namespace McGiv.AWS.SES
{
	/// <summary>
	/// Stores the public and private keys 
	/// 
	/// see http://docs.amazonwebservices.com/ses/latest/DeveloperGuide/YourAWSAccount.html
	/// </summary>
	public class AwsCredentials
	{
		public AwsCredentials(string accessKeyID, string secretAccessKey)
		{
			AccessKeyID = accessKeyID;
			SecretAccessKey = secretAccessKey;
		}


		/// <summary>
		/// Used to identify requests
		/// </summary>
		public string AccessKeyID { get; private set; }


		/// <summary>
		/// Used to sign requests
		/// </summary>
		public string SecretAccessKey { get; private set; }
	}
}