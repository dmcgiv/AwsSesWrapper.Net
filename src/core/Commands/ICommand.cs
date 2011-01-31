using System.Collections.Generic;
using System.IO;

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


	public interface ICommandResponseParser<out T>
	{
		T Process(Stream input);
	}
}