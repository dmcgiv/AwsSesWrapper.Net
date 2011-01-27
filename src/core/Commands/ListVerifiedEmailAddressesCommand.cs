using System.Collections.Generic;


namespace McGiv.AWS.SES
{

	/// <summary>
	///  see http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_ListVerifiedEmailAddresses.html
	/// </summary>
	public class ListVerifiedEmailAddressesCommand : ICommand
	{
		public string Action
		{
			get { return "ListVerifiedEmailAddresses"; }
		}

		public Dictionary<string, string> GetData()
		{
			return null;
		}
	}
}
