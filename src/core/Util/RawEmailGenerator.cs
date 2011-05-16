using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace McGiv.AWS.SES.Util
{
	
	
		public static class RawEmailGenerator
		{
			public static byte[] SendRawEmail(Encoding encoding, string from, string to, string subject, string text = null, string html = null, string replyTo = null, string returnPath = null)
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
	
}
