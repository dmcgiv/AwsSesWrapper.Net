using System.Collections.Generic;

namespace McGiv.AWS.SES
{

	/// <summary>
	/// Base interface for all SES commands.
	/// </summary>
	public interface ICommand
	{
		string Action { get; }
		Dictionary<string, string> GetData();
	}

}
