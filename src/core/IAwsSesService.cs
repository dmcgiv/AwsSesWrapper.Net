using System;
using System.Threading.Tasks;

namespace McGiv.AWS.SES
{
	public class Response
	{
		public string Command { get; internal set; }
		public string RequestID { get; internal set; }
	}


	//public class CommandResponse<T> : CommandResponse
	//{

	//    public T Data { get; internal set; }
	//}


	public interface ISesService
	{
		Task<ListVerifiedEmailAddressesResponse> ListVerifiedEmailAddresses();

		Task<VerifyEmailAddressResponse> VerifyEmailAddress(string address);

		Task<DeleteVerifiedEmailAddressResponse> DeleteVerifiedEmailAddress(string address);

		Task<GetSendQuoteResponse> GetSendQuote();

		Task<GetSendStatisticsResponse> GetSendStatistics();
	}


	public class SesService : ISesService
	{
		//private readonly CommandRequestBuilder _builder = new CommandRequestBuilder(new RequestSigner());
		private readonly CommandProcessor _processor;

		public SesService(CommandProcessor processor)
		{
			if (processor == null)
			{
				throw new ArgumentNullException("processor");
			}
			_processor = processor;
		}

		//public AwsCredentials Credentials
		//{
		//    get { return _builder.Signer.Credentials; }
		//    set{_builder.Signer.Credentials = value;}
		//}

		#region ISesService Members

		public Task<ListVerifiedEmailAddressesResponse> ListVerifiedEmailAddresses()
		{
			throw new NotImplementedException();
		}

		public Task<VerifyEmailAddressResponse> VerifyEmailAddress(string address)
		{
			var cmd = new VerifyEmailAddressCommand
						{
							EmailAddress = address
						};


			return _processor.CreateTask(cmd, new VerifyEmailAddressResponseParser());

			//new VerifierEmailAddressCommandResponseParser();
		}

		public Task<DeleteVerifiedEmailAddressResponse> DeleteVerifiedEmailAddress(string address)
		{
			throw new NotImplementedException();
		}

		public Task<GetSendQuoteResponse> GetSendQuote()
		{
			throw new NotImplementedException();
		}

		public Task<GetSendStatisticsResponse> GetSendStatistics()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}