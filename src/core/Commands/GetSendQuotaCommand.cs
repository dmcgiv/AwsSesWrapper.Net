using System.Collections.Generic;


namespace McGiv.AWS.SES
{

	/// <summary>
	/// see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_GetSendQuota.html
	/// </summary>
	public class GetSendQuotaCommand : ICommand
	{
		public string Action
		{
			get { return "GetSendQuota"; }
		}

		public Dictionary<string, string> GetData()
		{
			return null;
		}
	}
}
