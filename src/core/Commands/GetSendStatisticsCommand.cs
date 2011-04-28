using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace McGiv.AWS.SES
{
	/// <summary>
	/// http://docs.amazonwebservices.com/ses/latest/APIReference/index.html?API_GetSendStatistics.html
	/// </summary>
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
	public class SendStatistics
	{
		public int DeliveryAttempts { get; set; }
		public DateTime Timestamp { get; set; }
		public int Rejects { get; set; }
		public int Bounces { get; set; }
		public int Complaints { get; set; }
	}

	public class SendStatisticsResponseParser : ICommandResponseParser<SendStatistics[]>
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
		public SendStatistics[] Process(Stream input)
		{
			// SendDataPoints


			var list = new List<SendStatistics>();


			using (XmlReader reader = XmlReader.Create(input))
			{
				reader.MoveToContent();

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
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
						}
					}
				}
			}


			return list.ToArray();

		}

		static SendStatistics GetStat(XmlReader reader)
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

			var stat = new SendStatistics();
			
			
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