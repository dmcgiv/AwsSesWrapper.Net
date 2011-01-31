using System;
using System.Threading.Tasks;

namespace McGiv.AWS.SES
{
	public class CommandResponse
	{
		//internal CommandResponse(string requestID, Exception exception)
		//{
		//    this.RequestID = requestID;
		//    this.Exception = exception;
		//}

		public Exception Exception { get; internal set; }
		public string RequestID { get; internal set; }
	}


	public class CommandResponse<T> : CommandResponse
	{
		//internal CommandResponse(string requestID, Exception exception, T data)
		//    : base(requestID, exception)
		//{
		//    this.Data = data;
		//}

		public T Data { get; internal set; }
	}


	public interface ISesService
	{
		Task<CommandResponse<string[]>> GetVerifiedEmailAddresses();

		Task<CommandResponse> VerifyEmailAddress(string address);

		Task<CommandResponse> DeleteVerifiedEmailAddress(string address);

		Task<CommandResponse<SendQuote>> GetSendQuote();

		Task<CommandResponse<SendStatistics>> GetSendStatistics();
	}


	public class SesService : ISesService
	{
		private readonly CommandProcessor _processor;

		public SesService(CommandProcessor processor)
		{
			if (processor == null)
			{
				throw new ArgumentNullException("processor");
			}
			_processor = processor;
		}

		#region ISesService Members

		public Task<CommandResponse<string[]>> GetVerifiedEmailAddresses()
		{
			throw new NotImplementedException();
		}

		public Task<CommandResponse> VerifyEmailAddress(string address)
		{
			var cmd = new VerifyEmailAddressCommand
			          	{
			          		EmailAddress = address
			          	};


			return _processor.CreateTask(cmd, new VerifierEmailAddressCommandResponseParser());

			//new VerifierEmailAddressCommandResponseParser();
		}

		public Task<CommandResponse> DeleteVerifiedEmailAddress(string address)
		{
			throw new NotImplementedException();
		}

		public Task<CommandResponse<SendQuote>> GetSendQuote()
		{
			throw new NotImplementedException();
		}

		public Task<CommandResponse<SendStatistics>> GetSendStatistics()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}