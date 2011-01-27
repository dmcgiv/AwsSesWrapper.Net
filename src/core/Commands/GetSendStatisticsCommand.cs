using System.Collections.Generic;

namespace McGiv.AWS.SES
{

	/// <summary>
	/// http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_GetSendStatistics.html
	/// </summary>
	public class GetSendStatisticsCommand : ICommand
	{
		public string Action
		{
			get { return "GetSendStatistics"; }
		}

		public Dictionary<string, string> GetData()
		{
			return null;
		}
	}
}
