using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenTracksBETA
{
	// Token: 0x0200001D RID: 29
	public class YouTubeAudioService
	{
		// Token: 0x06000171 RID: 369 RVA: 0x0000AA47 File Offset: 0x00008C47
		public void SetApiKey(string apiKey)
		{
			this._youTubeApiKey = apiKey;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000AA51 File Offset: 0x00008C51
		public void GetAudioStreamUrl(string searchQuery, Action<string> onSuccess, Action<string> onError)
		{
			this.SearchAndGetAudioStream(searchQuery, onSuccess, onError);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000AA60 File Offset: 0x00008C60
		private void SearchAndGetAudioStream(string query, Action<string> onSuccess, Action<string> onError)
		{
			try
			{
				string text = string.Concat(new string[]
				{
					this._apiBaseUrl,
					"/get_search_videos.php?query=",
					Uri.EscapeDataString(query),
					"&count=1&apikey=",
					Uri.EscapeDataString(this._youTubeApiKey)
				});
				Debug.WriteLine("Search URL: " + text);
				WebClient webClient = new WebClient();
				webClient.Headers["User-Agent"] = "OpenTracksBETA/1.0";
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
				{
					bool flag = e.Error != null;
					if (flag)
					{
						Action<string> onError3 = onError;
						if (onError3 != null)
						{
							onError3.Invoke("Search failed: " + e.Error.Message);
						}
					}
					else
					{
						try
						{
							Debug.WriteLine("Search response: " + e.Result);
							string text2 = this.ExtractVideoIdFromResponse(e.Result);
							bool flag2 = string.IsNullOrEmpty(text2);
							if (flag2)
							{
								Action<string> onError4 = onError;
								if (onError4 != null)
								{
									onError4.Invoke("No video ID found in search results");
								}
							}
							else
							{
								Debug.WriteLine("Found video ID: " + text2);
								string text3 = this._apiBaseUrl + "/direct_audio_url?video_id=" + text2;
								Action<string> onSuccess2 = onSuccess;
								if (onSuccess2 != null)
								{
									onSuccess2.Invoke(text3);
								}
							}
						}
						catch (Exception ex2)
						{
							Action<string> onError5 = onError;
							if (onError5 != null)
							{
								onError5.Invoke("Search parse error: " + ex2.Message);
							}
						}
					}
				};
				webClient.DownloadStringAsync(new Uri(text));
			}
			catch (Exception ex)
			{
				Action<string> onError2 = onError;
				if (onError2 != null)
				{
					onError2.Invoke("Search request failed: " + ex.Message);
				}
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000AB50 File Offset: 0x00008D50
		private string ExtractVideoIdFromResponse(string jsonResponse)
		{
			string value;
			try
			{
				string text = jsonResponse.Trim();
				Debug.WriteLine("Extracting video ID from: " + text);
				bool flag = text.StartsWith("[");
				if (flag)
				{
					List<YtWebLegacyVideo> list = JsonConvert.DeserializeObject<List<YtWebLegacyVideo>>(text);
					bool flag2 = list != null && list.Count > 0 && !string.IsNullOrEmpty(list[0].video_id);
					if (flag2)
					{
						return list[0].video_id;
					}
				}
				bool flag3 = text.StartsWith("{");
				if (flag3)
				{
					JObject jobject = JObject.Parse(text);
					bool flag4 = jobject["error"] != null;
					if (flag4)
					{
						throw new Exception("API error: " + jobject["error"].ToString());
					}
					bool flag5 = jobject["video_id"] != null;
					if (flag5)
					{
						return jobject["video_id"].ToString();
					}
					bool flag6;
					if (jobject["videos"] != null)
					{
						JToken jtoken = jobject["videos"][0];
						flag6 = (((jtoken != null) ? jtoken["video_id"] : null) != null);
					}
					else
					{
						flag6 = false;
					}
					bool flag7 = flag6;
					if (flag7)
					{
						return jobject["videos"][0]["video_id"].ToString();
					}
					bool flag8;
					if (jobject["items"] != null)
					{
						JToken jtoken2 = jobject["items"][0];
						flag8 = (((jtoken2 != null) ? jtoken2["video_id"] : null) != null);
					}
					else
					{
						flag8 = false;
					}
					bool flag9 = flag8;
					if (flag9)
					{
						return jobject["items"][0]["video_id"].ToString();
					}
				}
				Match match = Regex.Match(text, "\"video_id\"\\s*:\\s*\"([^\"]+)\"");
				bool success = match.Success;
				if (!success)
				{
					throw new Exception("Could not extract video ID from response");
				}
				value = match.Groups[1].Value;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Video ID extraction error: " + ex.Message);
				throw;
			}
			return value;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000AD9C File Offset: 0x00008F9C
		public void TestAudioEndpoint(string endpointUrl, Action<bool> onResult)
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.Headers["User-Agent"] = "OpenTracksBETA/1.0";
				string text = endpointUrl.Contains("?") ? (endpointUrl + "&test=1") : (endpointUrl + "?test=1");
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
				{
					bool flag = e.Error != null || string.IsNullOrEmpty(e.Result);
					if (flag)
					{
						Action<bool> onResult3 = onResult;
						if (onResult3 != null)
						{
							onResult3.Invoke(false);
						}
					}
					else
					{
						bool flag2 = !e.Result.TrimStart(new char[0]).StartsWith("{") && !e.Result.TrimStart(new char[0]).StartsWith("<");
						Action<bool> onResult4 = onResult;
						if (onResult4 != null)
						{
							onResult4.Invoke(flag2);
						}
					}
				};
				webClient.DownloadStringAsync(new Uri(text));
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Audio test failed: " + ex.Message);
				Action<bool> onResult2 = onResult;
				if (onResult2 != null)
				{
					onResult2.Invoke(false);
				}
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000AE5C File Offset: 0x0000905C
		private bool IsLikelyAudioData(byte[] data)
		{
			bool flag = data == null || data.Length < 4;
			return !flag && data[0] != 123 && data[0] != 91;
		}

		// Token: 0x040000E4 RID: 228
		private string _apiBaseUrl = "https://yt.legacyprojects.ru";

		// Token: 0x040000E5 RID: 229
		private string _youTubeApiKey = "";
	}
}
