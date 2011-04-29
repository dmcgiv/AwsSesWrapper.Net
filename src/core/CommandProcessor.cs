using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace McGiv.AWS.SES
{
	public class CommandProcessor
	{
		private readonly CommandRequestBuilder _builder;

		public CommandProcessor(CommandRequestBuilder builder)
		{
			if (builder == null)
			{
				throw new ArgumentNullException("builder");
			}

			_builder = builder;
		}


		public Task<T> CreateTask<T>(ICommand command, ICommandResponseParser<T> parser)
			//where T : CommandResponse
		{
			HttpWebRequest request = _builder.Build();
			return TaskHelper.CreatePostWebRequestTask(request, () => _builder.GetData(command), parser);
		}

		public T Process<T>(ICommand command, ICommandResponseParser<T> parser)
		{
			HttpWebRequest request = _builder.Build();

			Task<T> task = TaskHelper.CreatePostWebRequestTask(request, () => _builder.GetData(command), parser);

			return task.Result;


			//var response = Encoding.ASCII.GetString(task.Result);

			//Console.WriteLine(response);
		}
	}


	public static class TaskHelper
	{
		public static Task<T> CreatePostWebRequestTask<T>(HttpWebRequest request, Func<byte[]> dataGetter,
		                                                  ICommandResponseParser<T> commandResponseParser)
			//where T  : Task<CommandResponse>, new()
		{
			Task<byte[]> task = CreatePostWebRequestTask(request, dataGetter);

			try
			{
				task.Wait();
			}
			catch (AggregateException e)
			{
				var awse = e.InnerException as AwsSesException;

				if (awse != null)
				{
					Console.WriteLine("Code : " + awse.ErrorResponse.Code);
					Console.WriteLine("RequestID : " + awse.ErrorResponse.RequestID);
					Console.WriteLine("Type : " + awse.ErrorResponse.Type);
				}
				throw;
			}
			

			//if(task.Exception != null)
			//{
			//    return new Task<T>(()=> new T{ Exception = task.Exception});
			//}

			using (var debug = new StreamReader(new MemoryStream(task.Result)))
			{
				Console.WriteLine(debug.ReadToEnd());

			}

			return Task<T>.Factory.StartNew(() =>
			                                	{
			                                		using (var stream = new MemoryStream(task.Result))
			                                		{
			                                			return commandResponseParser.Process(stream);
			                                		}
			                                	});
		}

		public static Task<byte[]> CreatePostWebRequestTask(HttpWebRequest request, Func<byte[]> dataGetter)
		{
			return Task<byte[]>.Factory.StartNew(
				() =>
					{
						//Exception exception = null;
						byte[] responseData = null;

						Task getRequestStreamTask = Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream,
						                                                           request.EndGetRequestStream, null)
							.ContinueWith(task =>
							              	{
							              		if (task.Exception != null)
							              		{
													// usually DNS issue or server unresponsive
							              			//exception = task.Exception;
							              			throw task.Exception;
							              			return;
							              		}

							              		// write data to request stream
							              		Stream outStream = task.Result;
							              		byte[] data = dataGetter();

							              		if (data == null || data.Length == 0)
							              		{
							              			return;
							              		}


							              		Task writterTask = Task.Factory.FromAsync(outStream.BeginWrite, outStream.EndWrite, data, 0,
							              		                                          data.Length, outStream,
							              		                                          TaskCreationOptions.AttachedToParent)
							              			.ContinueWith(
							              				x => ((Stream) x.AsyncState).Close());

							              		writterTask.Wait();
							              	});

						getRequestStreamTask.Wait();


						//if(exception != null)
						//{
						//    return null;
						//}


						

						// get response
						Task requestTask = Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null,
						                                                       TaskCreationOptions.AttachedToParent)
							.ContinueWith(task =>
							              	{

												//HttpWebResponse response = null;


							              		if (task.Exception != null)
							              		{
							              			//exception = task.Exception;

													if (task.Exception.InnerException is WebException)
													{
														var webException = (WebException)task.Exception.InnerException;

														var response = webException.Response as HttpWebResponse;

														if (response != null && response.StatusCode == (HttpStatusCode)400)
														{
															//responseData = GetData(response);

															var error = new ErrorResponseParser().Process(response.GetResponseStream());

															throw new AwsSesException(error, task.Exception.InnerException);
														}
													}


							              			throw task.Exception;
							              		}

												responseData = GetData((HttpWebResponse)task.Result);
			
							              	}
							);

						requestTask.Wait();


						return responseData;
					}, TaskCreationOptions.AttachedToParent);
		}



		static byte[] GetData(WebResponse response)
		{
			using (response)
			{
				Stream inStream = response.GetResponseStream();

				if (inStream != null)
				{
					using (inStream)
					{
						return GetResponseData(inStream,
													   response.ContentLength > 0 ? response.ContentLength : 1024);
					}
				}
			}

			return null;
		}

		static byte[] GetResponseData(Stream inStream, long bufferSize)
		{
			// read response data into memory buffer

			var buffer = new byte[bufferSize];

			using (var output = new MemoryStream())
			{
				int bytesRead = 1;
				while (bytesRead > 0)
				{
					Task readTask = Task<int>.Factory.FromAsync(inStream.BeginRead, inStream.EndRead, buffer, 0, buffer.Length, null,
					                                            TaskCreationOptions.AttachedToParent)
						.ContinueWith(task =>
						              	{
						              		bytesRead = task.Result;
						              		if (bytesRead > 0)
						              		{
						              			output.Write(buffer, 0, bytesRead);
						              		}
						              	});

					readTask.Wait();
				}

				return output.ToArray();
			}
		}
	}
}