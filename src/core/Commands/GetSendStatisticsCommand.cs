using System;
using System.Collections.Generic;
using System.Xml;

namespace McGiv.AWS.SES
{
	/// <summary>
	/// http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_GetSendStatistics.html
	/// </summary>
	[Serializable]
	public class GetSendStatisticsCommand : ICommand
	{
		#region ICommand Members

		public string Action
		{
			get { return "GetSendStatistics"; }
		}

		public Dictionary<string, string> GetData()
		{
			return null;
		}

		#endregion
	}


	[Serializable]
	public class GetSendStatisticsResponse : Response
	{
		public SendStatistic[] SendStatistics { get; set; }

	}

	[Serializable]
	public class SendStatistic
	{

		/// <summary>
		/// Number of emails that have been enqueued for sending.
		/// </summary>
		public int DeliveryAttempts { get; set; }

		/// <summary>
		/// Time of the data point.
		/// </summary>
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Number of emails rejected by Amazon SES.
		/// </summary>
		public int Rejects { get; set; }

		/// <summary>
		/// Number of emails that have bounced.
		/// </summary>
		public int Bounces { get; set; }

		/// <summary>
		/// Number of unwanted emails that were rejected by recipients.
		/// </summary>
		public int Complaints { get; set; }
	}

	public class GetSendStatisticsResponseParser : ResponseParser<GetSendStatisticsResponse>
	{
		/*
SendStatisticsTests.SendStatistics : Passed<GetSendStatisticsResponse xmlns="http://ses.amazonaws.com/doc/2010-12-01/">
  <GetSendStatisticsResult>
    <SendDataPoints>
      <member>
        <DeliveryAttempts>41</DeliveryAttempts>
        <Timestamp>2011-01-31T22:46:00Z</Timestamp>
        <Rejects>0</Rejects>
        <Bounces>0</Bounces>
        <Complaints>0</Complaints>
      </member>
      <member>
        <DeliveryAttempts>1</DeliveryAttempts>
        <Timestamp>2011-01-27T09:46:00Z</Timestamp>
        <Rejects>0</Rejects>
        <Bounces>0</Bounces>
        <Complaints>0</Complaints>
      </member>
      <member>
        <DeliveryAttempts>1</DeliveryAttempts>
        <Timestamp>2011-01-27T09:31:00Z</Timestamp>
        <Rejects>0</Rejects>
        <Bounces>0</Bounces>
        <Complaints>0</Complaints>
      </member>
      <member>
        <DeliveryAttempts>2</DeliveryAttempts>
        <Timestamp>2011-01-31T22:31:00Z</Timestamp>
        <Rejects>0</Rejects>
        <Bounces>0</Bounces>
        <Complaints>0</Complaints>
      </member>
      <member>
        <DeliveryAttempts>1</DeliveryAttempts>
        <Timestamp>2011-01-26T03:01:00Z</Timestamp>
        <Rejects>0</Rejects>
        <Bounces>0</Bounces>
        <Complaints>0</Complaints>
      </member>
      <member>
        <DeliveryAttempts>1</DeliveryAttempts>
        <Timestamp>2011-02-02T21:16:00Z</Timestamp>
        <Rejects>0</Rejects>
        <Bounces>0</Bounces>
        <Complaints>0</Complaints>
      </member>
    </SendDataPoints>
  </GetSendStatisticsResult>
  <ResponseMetadata>
    <RequestId>be1dc1e7-2f13-11e0-ad63-3d16cb3e2173</RequestId>
  </ResponseMetadata>
</GetSendStatisticsResponse>


		 * */

		public GetSendStatisticsResponseParser()
			: base("GetSendStatisticsResponse")
		{
		}

		protected override void InnerParse(XmlReader reader, GetSendStatisticsResponse response)
		{
			var list = new List<SendStatistic>();

			reader.ReadStartElement("GetSendStatisticsResult");
			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}

				switch (reader.Name)
				{
					case "GetSendStatisticsResponse":
					case "GetSendStatisticsResult":
					case "SendDataPoints":
						{

							break;
						}

					case "member":
						{
							list.Add(GetStat(reader));
							break;
						}
					case "ResponseMetadata":
						{
							goto completed;
						}
				}
			}

			completed:
			response.SendStatistics = list.ToArray();
		}



		static SendStatistic GetStat(XmlReader reader)
		{
			/*
<member>
    <DeliveryAttempts>1</DeliveryAttempts>
    <Timestamp>2011-02-02T21:16:00Z</Timestamp>
    <Rejects>0</Rejects>
    <Bounces>0</Bounces>
    <Complaints>0</Complaints>
</member>
			 * 
			 */

			var stat = new SendStatistic();
			
			
			reader.Read();
			reader.Read();
			reader.Read();
			stat.DeliveryAttempts = int.Parse(reader.Value);

			
			
			
			reader.Read();
			reader.Read();
			reader.Read();
			reader.Read();
			stat.Timestamp = DateTime.Parse(reader.Value);

			
			reader.Read();
			reader.Read();
			reader.Read();
			reader.Read();
			stat.Rejects = int.Parse(reader.Value);

			
			reader.Read();
			reader.Read();
			reader.Read();
			reader.Read();
			stat.Bounces = int.Parse(reader.Value);


			
			reader.Read();
			reader.Read();
			reader.Read();
			reader.Read();
			stat.Complaints = int.Parse(reader.Value);

			return stat;


		}
	}


}