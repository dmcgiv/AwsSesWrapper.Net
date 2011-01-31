using System.Collections.Generic;
using System.Web;
using Moq;
using NUnit.Framework;

namespace McGiv.AWS.SES.Tests
{
	[TestFixture]
	public class RequestBuilderFormatDataTests
	{
		private readonly CommandRequestBuilder _builder = new CommandRequestBuilder(new RequestSigner(Helper.GetCredentials()));


		[Test]
		public void FormatActionRequireEncoding()
		{
			var cmd = new Mock<ICommand>();

			const string action = "Action%$";
			string encoding = HttpUtility.UrlEncode(action);

			cmd.Setup(x => x.Action).Returns(action);
			cmd.Setup(x => x.GetData()).Returns(new Dictionary<string, string>
			                                    	{
			                                    		{"key1", "value1"},
			                                    		{"key2", "value2"},
			                                    		{"key3", "value3"},
			                                    	});

			string data = _builder.FormatData(cmd.Object);


			Assert.AreEqual("Action=" + encoding + "&key1=value1&key2=value2&key3=value3", data);
		}


		[Test]
		public void FormatKeyRequireEncoding()
		{
			var cmd = new Mock<ICommand>();

			const string value = "key1%$";
			string encoding = HttpUtility.UrlEncode(value);

			cmd.Setup(x => x.Action).Returns("ActionName");
			cmd.Setup(x => x.GetData()).Returns(new Dictionary<string, string>
			                                    	{
			                                    		{value, "value1"}
			                                    	});

			string data = _builder.FormatData(cmd.Object);


			Assert.AreEqual("Action=ActionName&" + encoding + "=value1", data);
		}

		[Test]
		public void FormatNotEncodingRequired()
		{
			var cmd = new Mock<ICommand>();

			cmd.Setup(x => x.Action).Returns("ActionName");
			cmd.Setup(x => x.GetData()).Returns(new Dictionary<string, string>
			                                    	{
			                                    		{"key1", "value1"},
			                                    		{"key2", "value2"},
			                                    		{"key3", "value3"},
			                                    	});

			string data = _builder.FormatData(cmd.Object);


			Assert.AreEqual("Action=ActionName&key1=value1&key2=value2&key3=value3", data);
		}


		[Test]
		public void FormatValueRequireEncoding()
		{
			var cmd = new Mock<ICommand>();

			const string value = "value1%$";
			string encoding = HttpUtility.UrlEncode(value);

			cmd.Setup(x => x.Action).Returns("ActionName");
			cmd.Setup(x => x.GetData()).Returns(new Dictionary<string, string>
			                                    	{
			                                    		{"key1", value}
			                                    	});

			string data = _builder.FormatData(cmd.Object);


			Assert.AreEqual("Action=ActionName&key1=" + encoding, data);
		}
	}
}