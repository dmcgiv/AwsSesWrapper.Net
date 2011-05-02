using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace McGiv.AWS.SES.Util
{



	/// <summary>
	/// Use memory stream with dummy Close method as MailWriter writes final CRLF when closing the stream. This allows us to read the stream and close it manually.
	/// </summary>
	class ClosableMemoryStream : MemoryStream
	{
		public override void Close()
		{
			
		}

		public void ReallyClose()
		{
			base.Close();
		}
	}


	public class RawEmailGenerator
	{
		public static byte[] SendRawEmail(Encoding encoding , string from, string to, string subject, string text = null, string html = null, string replyTo = null, string returnPath = null)
		{
			//var encoding = Encoding.UTF8;

			//AlternateView plainView = AlternateView.CreateAlternateViewFromString(text, encoding, "text/plain");
			//AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, encoding, "text/html");

			var mailMessage = new MailMessage();
			mailMessage.Headers.Set("Message-Id", "<12343245." + from + ">");
			//mailMessage.Headers.Set("Content-Language", "en-UK");

			
			
			mailMessage.From = new MailAddress(from);

			List<string> toAddresses = to.Replace(", ", ",").Split(',').ToList();
			foreach (string toAddress in toAddresses)
			{
				mailMessage.To.Add(new MailAddress(toAddress));
			}


			//foreach (string ccAddress in ccAddresses)
			//{
			//    mailMessage.CC.Add(new MailAddress(ccAddress));
			//}

			//foreach (string bccAddress in bccAddresses)
			//{
			//    mailMessage.Bcc.Add(new MailAddress(bccAddress));
			//}

			mailMessage.Subject = subject;
			mailMessage.SubjectEncoding = encoding;

			mailMessage.Body = text;
			mailMessage.IsBodyHtml = false;
			mailMessage.BodyEncoding = encoding;
			


			
		

			if (replyTo != null)
			{
				mailMessage.ReplyTo = new MailAddress(replyTo);
			}

			//if (text != null)
			//{
			//    mailMessage.AlternateViews.Add(plainView);
			//}

			//if (html != null)
			//{
			//    mailMessage.AlternateViews.Add(htmlView);
			//}

			

			return MailMessageMemoryStream.ConvertMailMessageToMemoryStream(mailMessage);


			//return Convert.ToBase64String(data, 0, data.Length);



		}

	}

	/// <summary>
	/// see http://neildeadman.wordpress.com/2011/02/01/amazon-simple-email-service-example-in-c-sendrawemail/
	/// see http://stackoverflow.com/questions/2423617/save-system-net-mail-mailmessage-as-msg-file
	/// see http://www.codeproject.com/KB/IP/smtpclientext.aspx
	/// </summary>
	public class MailMessageMemoryStream
	{

		public static byte[] ConvertMailMessageToMemoryStream(MailMessage message)
		{
			Assembly assembly = typeof(SmtpClient).Assembly;

			Type mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");
			//var outStream = new ClosableMemoryStream();
			using (var internalStream = new ClosableMemoryStream())
			{

				ConstructorInfo mailWriterContructor = mailWriterType.GetConstructor(
					BindingFlags.Instance | BindingFlags.NonPublic, null, new[] {typeof (Stream)}, null);

				object mailWriter = mailWriterContructor.Invoke(new object[] { internalStream });

				MethodInfo sendMethod = typeof (MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);

				sendMethod.Invoke(message, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] {mailWriter, true}, null);



				//internalStream.Write(new byte[] { 13, 10 }, 0, 2);
				//internalStream.Position = 0;
				//string data = null;
				//using (var reader = new StreamReader(internalStream))
				//{
				//    data = reader.ReadToEnd();
				//    //internalStream.Close();
				//}

				


				MethodInfo closeMethod = mailWriter.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);

				closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] {}, null);


				//CRLF = new byte[] { 13, 10 };

				//internalStream.Write(new byte[] { 13, 10 }, 0, 2);
				internalStream.Position = 0;

				var buffer = new byte[internalStream.Length];
				var i = internalStream.Read(buffer, 0, (int)internalStream.Length);
				return buffer;
				//return Convert.ToBase64String(buffer, 0, i);
				//string data = null;
				//using (var reader = new StreamReader(internalStream))
				//{
				//    //data = reader.ReadToEnd();
				//    //internalStream.Close();

				//    return reader.re
				//}

				//return data;



			}

			//return data;
		}

	}
}
