using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace McGiv.AWS.SES.DKIM
{
	public enum CanonicalizationAlgorithm
	{
		Simple,
		Relaxed
	}

	public static class Canonicalization
	{
		private static char[] _whiteSpaceChars = new char[] {' ', '\t'};

		public static bool IsWhiteSpace(char c)
		{
			return c == ' ' || c == '\t';
		}



		/// <summary>
		/// Reduces all adjacent white space characters to a single space character.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string ReduceWitespace(string text)
		{
			if (text.IndexOfAny(new char[] { ' ', '\t' }) == -1)
			{
				return text;
			}

			var sb = new StringBuilder();
			bool hasWhiteSpace = true;
			foreach (var c in text)
			{
				switch (c)
				{
					case ' ':
					case '\t':
						{
							hasWhiteSpace = true;
							break;
						}
					default:
						{
							if (hasWhiteSpace)
							{
								sb.Append(' ');
							}

							sb.Append(c);
							hasWhiteSpace = false;
							break;
						}
				}
			}

			return sb.ToString();



		}
		public static string CanonicalizationHeaders(string headers, CanonicalizationAlgorithm type)
		{
			

			switch (type)
			{
				case CanonicalizationAlgorithm.Simple:
					{
						return headers;
					}
				case CanonicalizationAlgorithm.Relaxed:
					{
						var sb = new StringBuilder(headers.Length);

						//using (var reader = new StringReader(headers))
						//{
						//    string line = null;
						//}
						//var h = new NameValueCollection();

						using (var reader = new StringReader(headers))
						{
							string line;
							string lastKey = null;
							while ((line = reader.ReadLine()) != null)
							{

								// check if line is unfolded value
								if (lastKey != null && line.Length > 0 && IsWhiteSpace(line[0]))
								{
									//h[lastKey] += ReduceWitespace(line);
									sb.Length -= 2; // remove previous new-line
									sb.Append(' ');
									sb.AppendLine(ReduceWitespace(line.Trim()));
									continue;
								}

								int sep = line.IndexOf(':');

								if (sep == -1)
								{
									// todo error
									throw new FormatException();
								}
								var key = line.Substring(0, sep).TrimEnd().ToLower();
									lastKey = key;

								var value = ReduceWitespace(line.Substring(sep + 1).Trim());

									//h.Add(key, value);

								sb.Append(key);
								sb.Append(':');
								sb.AppendLine(value);

							}
						}

						//sb.AppendLine();
						return sb.ToString();
					}
			}

			return null;
		}




		public static string CanonicalizationBody(string body, CanonicalizationAlgorithm type)
		{

			//var whitespace = new[] { ' ', '\t' };

			var sb = new StringBuilder(body.Length);

			switch (type)
			{
				case CanonicalizationAlgorithm.Relaxed:
					{
						using (var reader = new StringReader(body))
						{
							string line = null;
							bool isWSP = false;
							int emptyLineCount = 0;

							while ((line = reader.ReadLine()) != null)
							{

								if (line == string.Empty)
								{
									emptyLineCount++;
									continue;
								}

								for (int i = 0; i < emptyLineCount; i++)
								{
									sb.AppendLine();
								}


								line = line.TrimEnd();


								foreach (var c in line)
								{
									switch (c)
									{
										case ' ':
										case '\t':
											{
												isWSP = true;
												break;
											}
										default:
											{

												if (isWSP)
												{
													sb.Append(' ');
												}
												sb.Append(c);
												isWSP = false;
												break;
											}
									}
								}

								sb.AppendLine();

							}
						}

						break;
					}


				case CanonicalizationAlgorithm.Simple:
					{
						using (var reader = new StringReader(body))
						{
							string line = null;
							int emptyLineCount = 0;

							while ((line = reader.ReadLine()) != null)
							{

								if (line == string.Empty)
								{
									emptyLineCount++;
									continue;
								}

								for (int i = 0; i < emptyLineCount; i++)
								{
									sb.AppendLine();
								}

								sb.AppendLine(line);
							}
						}

						if (sb.Length == 0)
						{
							sb.AppendLine();
						}

						break;
					}
			}

			return sb.ToString();

		}
	}
}
