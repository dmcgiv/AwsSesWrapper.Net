using System;
using System.Collections.Generic;
using System.IO;

namespace McGiv.AWS.SES.Tests
{
	public class AsyncStreamReader
	{
		private const int DefaultBufferSize = 1024;
		private readonly List<byte> _all;
		private readonly byte[] _buffer;
		private readonly int _bufferSize;
		private readonly Action<byte[]> _completed;
		private readonly Stream _stream;
		//private int _offset;

		public AsyncStreamReader(Stream stream, Action<byte[]> completed)
			: this(stream, completed, DefaultBufferSize)
		{
		}

		public AsyncStreamReader(Stream stream, Action<byte[]> completed, int bufferSize)
		{
			_bufferSize = bufferSize;
			_buffer = new byte[_bufferSize];
			_completed = completed;
			_stream = stream;
			_all = new List<byte>();

			_stream.BeginRead(_buffer, 0, _bufferSize, Callback, this);
		}

		//public int BufferSize { get;private  set; }

		private static void Callback(IAsyncResult asyncResult)
		{
			var reader = (AsyncStreamReader) asyncResult.AsyncState;

			int bytesRead = reader._stream.EndRead(asyncResult);

			if (bytesRead > 0)
			{
				// get data that was read
				if (bytesRead == reader._bufferSize)
				{
					reader._all.AddRange(reader._buffer);
				}
				else
				{
					var tmp = new byte[bytesRead];
					Array.Copy(reader._buffer, tmp, bytesRead);

					reader._all.AddRange(tmp);
				}


				// read some more
				reader._stream.BeginRead(reader._buffer, 0, reader._bufferSize, Callback, reader);
				return;
			}

			// everything has been read

			reader._stream.Close();


			reader._completed(reader._all.ToArray());
		}
	}
}