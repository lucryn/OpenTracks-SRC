using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace MiniJSON
{
	// Token: 0x02000002 RID: 2
	public static class Json
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static object Deserialize(string json)
		{
			bool flag = json == null;
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = Json.Parser.Parse(json);
			}
			return result;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002078 File Offset: 0x00000278
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x0200002C RID: 44
		private sealed class Parser : IDisposable
		{
			// Token: 0x06000260 RID: 608 RVA: 0x000104A8 File Offset: 0x0000E6A8
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x06000261 RID: 609 RVA: 0x000104D8 File Offset: 0x0000E6D8
			public static bool IsHexDigit(char c)
			{
				return "0123456789ABCDEFabcdef".IndexOf(c) != -1;
			}

			// Token: 0x06000262 RID: 610 RVA: 0x000104FB File Offset: 0x0000E6FB
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x06000263 RID: 611 RVA: 0x00010514 File Offset: 0x0000E714
			public static object Parse(string jsonString)
			{
				object result;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x06000264 RID: 612 RVA: 0x00010550 File Offset: 0x0000E750
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x06000265 RID: 613 RVA: 0x00010568 File Offset: 0x0000E768
			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				Json.Parser.TOKEN nextToken;
				for (;;)
				{
					nextToken = this.NextToken;
					if (nextToken <= Json.Parser.TOKEN.CURLY_CLOSE)
					{
						break;
					}
					if (nextToken != Json.Parser.TOKEN.COMMA)
					{
						if (nextToken != Json.Parser.TOKEN.STRING)
						{
							goto Block_5;
						}
						string text = this.ParseString();
						bool flag = text == null;
						if (flag)
						{
							goto Block_6;
						}
						bool flag2 = this.NextToken != Json.Parser.TOKEN.COLON;
						if (flag2)
						{
							goto Block_7;
						}
						this.json.Read();
						Json.Parser.TOKEN nextToken2 = this.NextToken;
						object obj = this.ParseByToken(nextToken2);
						bool flag3 = obj == null && nextToken2 != Json.Parser.TOKEN.NULL;
						if (flag3)
						{
							goto Block_9;
						}
						dictionary[text] = obj;
					}
				}
				if (nextToken == Json.Parser.TOKEN.NONE)
				{
					return null;
				}
				if (nextToken == Json.Parser.TOKEN.CURLY_CLOSE)
				{
					return dictionary;
				}
				Block_5:
				goto IL_C1;
				Block_6:
				return null;
				Block_7:
				return null;
				Block_9:
				return null;
				IL_C1:
				return null;
			}

			// Token: 0x06000266 RID: 614 RVA: 0x00010648 File Offset: 0x0000E848
			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					Json.Parser.TOKEN token = nextToken;
					if (token != Json.Parser.TOKEN.NONE)
					{
						if (token != Json.Parser.TOKEN.SQUARED_CLOSE)
						{
							if (token != Json.Parser.TOKEN.COMMA)
							{
								object obj = this.ParseByToken(nextToken);
								bool flag2 = obj == null && nextToken != Json.Parser.TOKEN.NULL;
								if (flag2)
								{
									return null;
								}
								list.Add(obj);
							}
						}
						else
						{
							flag = false;
						}
						continue;
					}
					return null;
				}
				return list;
			}

			// Token: 0x06000267 RID: 615 RVA: 0x000106D0 File Offset: 0x0000E8D0
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x06000268 RID: 616 RVA: 0x000106F0 File Offset: 0x0000E8F0
			private object ParseByToken(Json.Parser.TOKEN token)
			{
				switch (token)
				{
				case Json.Parser.TOKEN.CURLY_OPEN:
					return this.ParseObject();
				case Json.Parser.TOKEN.SQUARED_OPEN:
					return this.ParseArray();
				case Json.Parser.TOKEN.STRING:
					return this.ParseString();
				case Json.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case Json.Parser.TOKEN.TRUE:
					return true;
				case Json.Parser.TOKEN.FALSE:
					return false;
				case Json.Parser.TOKEN.NULL:
					return null;
				}
				return null;
			}

			// Token: 0x06000269 RID: 617 RVA: 0x00010778 File Offset: 0x0000E978
			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					bool flag2 = this.json.Peek() == -1;
					if (flag2)
					{
						break;
					}
					char nextChar = this.NextChar;
					char c = nextChar;
					if (c != '"')
					{
						if (c != '\\')
						{
							stringBuilder.Append(nextChar);
						}
						else
						{
							bool flag3 = this.json.Peek() == -1;
							if (flag3)
							{
								flag = false;
							}
							else
							{
								nextChar = this.NextChar;
								char c2 = nextChar;
								if (c2 <= '\\')
								{
									if (c2 == '"' || c2 == '/' || c2 == '\\')
									{
										stringBuilder.Append(nextChar);
									}
								}
								else if (c2 <= 'f')
								{
									if (c2 != 'b')
									{
										if (c2 == 'f')
										{
											stringBuilder.Append('\f');
										}
									}
									else
									{
										stringBuilder.Append('\b');
									}
								}
								else if (c2 != 'n')
								{
									switch (c2)
									{
									case 'r':
										stringBuilder.Append('\r');
										break;
									case 't':
										stringBuilder.Append('\t');
										break;
									case 'u':
									{
										char[] array = new char[4];
										for (int i = 0; i < 4; i++)
										{
											array[i] = this.NextChar;
											bool flag4 = !Json.Parser.IsHexDigit(array[i]);
											if (flag4)
											{
												return null;
											}
										}
										stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
										break;
									}
									}
								}
								else
								{
									stringBuilder.Append('\n');
								}
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x0600026A RID: 618 RVA: 0x0001092C File Offset: 0x0000EB2C
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				bool flag = nextWord.IndexOf('.') == -1 && nextWord.IndexOf('E') == -1 && nextWord.IndexOf('e') == -1;
				object result;
				if (flag)
				{
					long num;
					long.TryParse(nextWord, 511, CultureInfo.InvariantCulture, ref num);
					result = num;
				}
				else
				{
					double num2;
					double.TryParse(nextWord, 511, CultureInfo.InvariantCulture, ref num2);
					result = num2;
				}
				return result;
			}

			// Token: 0x0600026B RID: 619 RVA: 0x000109A8 File Offset: 0x0000EBA8
			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					bool flag = this.json.Peek() == -1;
					if (flag)
					{
						break;
					}
				}
			}

			// Token: 0x17000084 RID: 132
			// (get) Token: 0x0600026C RID: 620 RVA: 0x000109EC File Offset: 0x0000EBEC
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x17000085 RID: 133
			// (get) Token: 0x0600026D RID: 621 RVA: 0x00010A10 File Offset: 0x0000EC10
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x17000086 RID: 134
			// (get) Token: 0x0600026E RID: 622 RVA: 0x00010A34 File Offset: 0x0000EC34
			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (!Json.Parser.IsWordBreak(this.PeekChar))
					{
						stringBuilder.Append(this.NextChar);
						bool flag = this.json.Peek() == -1;
						if (flag)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x17000087 RID: 135
			// (get) Token: 0x0600026F RID: 623 RVA: 0x00010A8C File Offset: 0x0000EC8C
			private Json.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					bool flag = this.json.Peek() == -1;
					Json.Parser.TOKEN result;
					if (flag)
					{
						result = Json.Parser.TOKEN.NONE;
					}
					else
					{
						char peekChar = this.PeekChar;
						if (peekChar <= '[')
						{
							switch (peekChar)
							{
							case '"':
								return Json.Parser.TOKEN.STRING;
							case '#':
							case '$':
							case '%':
							case '&':
							case '\'':
							case '(':
							case ')':
							case '*':
							case '+':
							case '.':
							case '/':
								break;
							case ',':
								this.json.Read();
								return Json.Parser.TOKEN.COMMA;
							case '-':
							case '0':
							case '1':
							case '2':
							case '3':
							case '4':
							case '5':
							case '6':
							case '7':
							case '8':
							case '9':
								return Json.Parser.TOKEN.NUMBER;
							case ':':
								return Json.Parser.TOKEN.COLON;
							default:
								if (peekChar == '[')
								{
									return Json.Parser.TOKEN.SQUARED_OPEN;
								}
								break;
							}
						}
						else
						{
							if (peekChar == ']')
							{
								this.json.Read();
								return Json.Parser.TOKEN.SQUARED_CLOSE;
							}
							if (peekChar == '{')
							{
								return Json.Parser.TOKEN.CURLY_OPEN;
							}
							if (peekChar == '}')
							{
								this.json.Read();
								return Json.Parser.TOKEN.CURLY_CLOSE;
							}
						}
						string nextWord = this.NextWord;
						if (!(nextWord == "false"))
						{
							if (!(nextWord == "true"))
							{
								if (!(nextWord == "null"))
								{
									result = Json.Parser.TOKEN.NONE;
								}
								else
								{
									result = Json.Parser.TOKEN.NULL;
								}
							}
							else
							{
								result = Json.Parser.TOKEN.TRUE;
							}
						}
						else
						{
							result = Json.Parser.TOKEN.FALSE;
						}
					}
					return result;
				}
			}

			// Token: 0x0400016F RID: 367
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x04000170 RID: 368
			private const string HEX_DIGIT = "0123456789ABCDEFabcdef";

			// Token: 0x04000171 RID: 369
			private StringReader json;

			// Token: 0x02000070 RID: 112
			private enum TOKEN
			{
				// Token: 0x04000222 RID: 546
				NONE,
				// Token: 0x04000223 RID: 547
				CURLY_OPEN,
				// Token: 0x04000224 RID: 548
				CURLY_CLOSE,
				// Token: 0x04000225 RID: 549
				SQUARED_OPEN,
				// Token: 0x04000226 RID: 550
				SQUARED_CLOSE,
				// Token: 0x04000227 RID: 551
				COLON,
				// Token: 0x04000228 RID: 552
				COMMA,
				// Token: 0x04000229 RID: 553
				STRING,
				// Token: 0x0400022A RID: 554
				NUMBER,
				// Token: 0x0400022B RID: 555
				TRUE,
				// Token: 0x0400022C RID: 556
				FALSE,
				// Token: 0x0400022D RID: 557
				NULL
			}
		}

		// Token: 0x0200002D RID: 45
		private sealed class Serializer
		{
			// Token: 0x06000270 RID: 624 RVA: 0x00010BDD File Offset: 0x0000EDDD
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x06000271 RID: 625 RVA: 0x00010BF4 File Offset: 0x0000EDF4
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x06000272 RID: 626 RVA: 0x00010C20 File Offset: 0x0000EE20
			private void SerializeValue(object value)
			{
				bool flag = value == null;
				if (flag)
				{
					this.builder.Append("null");
				}
				else
				{
					string str;
					bool flag2 = (str = (value as string)) != null;
					if (flag2)
					{
						this.SerializeString(str);
					}
					else
					{
						bool flag3 = value is bool;
						if (flag3)
						{
							this.builder.Append(((bool)value) ? "true" : "false");
						}
						else
						{
							IList anArray;
							bool flag4 = (anArray = (value as IList)) != null;
							if (flag4)
							{
								this.SerializeArray(anArray);
							}
							else
							{
								IDictionary obj;
								bool flag5 = (obj = (value as IDictionary)) != null;
								if (flag5)
								{
									this.SerializeObject(obj);
								}
								else
								{
									bool flag6 = value is char;
									if (flag6)
									{
										this.SerializeString(new string((char)value, 1));
									}
									else
									{
										this.SerializeOther(value);
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x06000273 RID: 627 RVA: 0x00010D0C File Offset: 0x0000EF0C
			private void SerializeObject(IDictionary obj)
			{
				bool flag = true;
				this.builder.Append('{');
				foreach (object obj2 in obj.Keys)
				{
					bool flag2 = !flag;
					if (flag2)
					{
						this.builder.Append(',');
					}
					this.SerializeString(obj2.ToString());
					this.builder.Append(':');
					this.SerializeValue(obj[obj2]);
					flag = false;
				}
				this.builder.Append('}');
			}

			// Token: 0x06000274 RID: 628 RVA: 0x00010DC4 File Offset: 0x0000EFC4
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				for (int i = 0; i < anArray.Count; i++)
				{
					object value = anArray[i];
					bool flag2 = !flag;
					if (flag2)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(value);
					flag = false;
				}
				this.builder.Append(']');
			}

			// Token: 0x06000275 RID: 629 RVA: 0x00010E34 File Offset: 0x0000F034
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				int i = 0;
				while (i < array.Length)
				{
					char c = array[i];
					char c2 = c;
					switch (c2)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					case '\v':
						goto IL_EB;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					default:
						if (c2 != '"')
						{
							if (c2 != '\\')
							{
								goto IL_EB;
							}
							this.builder.Append("\\\\");
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					}
					IL_149:
					i++;
					continue;
					IL_EB:
					int num = Convert.ToInt32(c);
					bool flag = num >= 32 && num <= 126;
					if (flag)
					{
						this.builder.Append(c);
					}
					else
					{
						this.builder.Append("\\u");
						this.builder.Append(num.ToString("x4"));
					}
					goto IL_149;
				}
				this.builder.Append('"');
			}

			// Token: 0x06000276 RID: 630 RVA: 0x00010FAC File Offset: 0x0000F1AC
			private void SerializeOther(object value)
			{
				bool flag = value is float;
				if (flag)
				{
					this.builder.Append(((float)value).ToString("R", CultureInfo.InvariantCulture));
				}
				else
				{
					bool flag2 = value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong;
					if (flag2)
					{
						this.builder.Append(value);
					}
					else
					{
						bool flag3 = value is double || value is decimal;
						if (flag3)
						{
							this.builder.Append(Convert.ToDouble(value).ToString("R", CultureInfo.InvariantCulture));
						}
						else
						{
							this.SerializeString(value.ToString());
						}
					}
				}
			}

			// Token: 0x04000172 RID: 370
			private StringBuilder builder;
		}
	}
}
