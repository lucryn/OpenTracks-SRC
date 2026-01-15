using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenTracksBETA
{
	// Token: 0x02000015 RID: 21
	public class SpotifyPage : PhoneApplicationPage
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00006D88 File Offset: 0x00004F88
		public SpotifyPage()
		{
			this.InitializeComponent();
			base.Loaded += new RoutedEventHandler(this.MainPage_Loaded);
			this._youTubeService = new YouTubeAudioService();
			this._audioPlayer = this.AudioPlayer;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006DF4 File Offset: 0x00004FF4
		private void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			this.LoadCredentials();
			bool flag = IsolatedStorageSettings.ApplicationSettings.Contains("SpotifyAccessToken") && IsolatedStorageSettings.ApplicationSettings.Contains("SpotifyCredentialsSet");
			if (flag)
			{
				this._accessToken = (IsolatedStorageSettings.ApplicationSettings["SpotifyAccessToken"] as string);
				this._refreshToken = (IsolatedStorageSettings.ApplicationSettings["SpotifyRefreshToken"] as string);
				bool flag2 = IsolatedStorageSettings.ApplicationSettings.Contains("YouTubeApiKey");
				if (flag2)
				{
					this._youTubeApiKey = (IsolatedStorageSettings.ApplicationSettings["YouTubeApiKey"] as string);
					this._youTubeService.SetApiKey(this._youTubeApiKey);
				}
				this.ShowMainApp();
				this.NowPlayingText.Text = "Welcome back!";
			}
			else
			{
				this.ShowCredentialsEntry();
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006ED0 File Offset: 0x000050D0
		private void PlayYouTubeButton_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this._currentTrack == null;
			if (flag)
			{
				MessageBox.Show("Please select a track first");
			}
			else
			{
				this.PlayTrackViaYouTube(this._currentTrack);
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006F08 File Offset: 0x00005108
		private void PlayTrackViaYouTube(Track track)
		{
			try
			{
				this.YouTubeStatusText.Visibility = 0;
				this.YouTubeStatusText.Text = "Searching YouTube...";
				this.PlayYouTubeButton.IsEnabled = false;
				string artistName = "Unknown Artist";
				bool flag = track.Artists != null && track.Artists.Count > 0;
				if (flag)
				{
					artistName = track.Artists[0].Name;
				}
				string searchQuery = artistName + " - " + track.Name;
				this._youTubeService.GetAudioStreamUrl(searchQuery, delegate(string audioEndpoint)
				{
					Deployment.Current.Dispatcher.BeginInvoke(delegate()
					{
						Debug.WriteLine("Audio endpoint: " + audioEndpoint);
						this.PlayAudioStreamDirectly(audioEndpoint, track, artistName);
					});
				}, delegate(string error)
				{
					Deployment.Current.Dispatcher.BeginInvoke(delegate()
					{
						Debug.WriteLine("Search failed: " + error);
						this.YouTubeStatusText.Text = "YouTube failed";
						MessageBox.Show("YouTube error: " + error);
						this.PlayYouTubeButton.IsEnabled = true;
					});
				});
			}
			catch (Exception ex)
			{
				this.YouTubeStatusText.Text = "Playback failed";
				MessageBox.Show("Error: " + ex.Message);
				this.PlayYouTubeButton.IsEnabled = true;
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00007058 File Offset: 0x00005258
		private void PlayAudioStreamDirectly(string streamUrl, Track track, string artistName)
		{
			try
			{
				this.YouTubeStatusText.Text = "Loading audio stream...";
				this._audioPlayer.Stop();
				this._audioPlayer.Source = null;
				Debug.WriteLine("Playing audio stream directly from: " + streamUrl);
				this._audioPlayer.Source = new Uri(streamUrl);
				this._isPlaying = true;
				this.PlayButton.Content = "Pause";
				this.YouTubeStatusText.Text = "Buffering audio... (may take a minute)";
				this.PlayYouTubeButton.IsEnabled = true;
			}
			catch (Exception ex)
			{
				this.YouTubeStatusText.Text = "Stream play failed";
				MessageBox.Show("Stream play error: " + ex.Message);
				this.PlayYouTubeButton.IsEnabled = true;
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00007138 File Offset: 0x00005338
		private void ExtractStreamUrlFromEndpoint(string endpointUrl, Track track, string artistName)
		{
			try
			{
				this.YouTubeStatusText.Text = "Extracting stream URL...";
				WebClient webClient = new WebClient();
				webClient.Headers["User-Agent"] = "OpenTracksBETA/1.0";
				Exception ex;
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
				{
					bool flag = e.Error != null;
					if (flag)
					{
						Deployment.Current.Dispatcher.BeginInvoke(delegate()
						{
							this.YouTubeStatusText.Text = "URL extraction failed";
							MessageBox.Show("URL extraction error: " + e.Error.Message);
							this.PlayYouTubeButton.IsEnabled = true;
						});
					}
					else
					{
						try
						{
							string text = e.Result.Trim();
							Debug.WriteLine("Endpoint JSON response: " + text);
							string streamUrl = this.ExtractUrlFromJson(text);
							bool flag2 = !string.IsNullOrEmpty(streamUrl);
							if (flag2)
							{
								Deployment.Current.Dispatcher.BeginInvoke(delegate()
								{
									Debug.WriteLine("Extracted stream URL: " + streamUrl);
									this.PlayAudioStreamDirectly(streamUrl, track, artistName);
								});
							}
							else
							{
								Deployment.Current.Dispatcher.BeginInvoke(delegate()
								{
									this.YouTubeStatusText.Text = "No stream URL found";
									MessageBox.Show("Could not extract stream URL from response");
									this.PlayYouTubeButton.IsEnabled = true;
								});
							}
						}
						catch (Exception ex3)
						{
							Exception ex4 = ex3;
							Exception ex = ex4;
							Deployment.Current.Dispatcher.BeginInvoke(delegate()
							{
								this.YouTubeStatusText.Text = "JSON parse failed";
								MessageBox.Show("JSON parse error: " + ex.Message);
								this.PlayYouTubeButton.IsEnabled = true;
							});
						}
					}
				};
				webClient.DownloadStringAsync(new Uri(endpointUrl));
			}
			catch (Exception ex)
			{
				Exception ex;
				Exception ex;
				Exception ex2 = ex;
				ex = ex2;
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.YouTubeStatusText.Text = "Extraction failed";
					MessageBox.Show("Extraction error: " + ex.Message);
					this.PlayYouTubeButton.IsEnabled = true;
				});
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000071F8 File Offset: 0x000053F8
		private string ExtractUrlFromJson(string jsonResponse)
		{
			string result;
			try
			{
				bool flag = jsonResponse.StartsWith("{");
				if (flag)
				{
					JObject jobject = JObject.Parse(jsonResponse);
					string[] array = new string[]
					{
						"url",
						"audio_url",
						"video_url",
						"stream_url",
						"direct_url"
					};
					foreach (string propertyName in array)
					{
						bool flag2 = jobject[propertyName] != null;
						if (flag2)
						{
							string text = jobject[propertyName].ToString();
							bool flag3 = text.StartsWith("http");
							if (flag3)
							{
								return text;
							}
						}
					}
				}
				result = null;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("URL extraction error: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000072E0 File Offset: 0x000054E0
		private void TryAlternativeApproach(string searchQuery, Track track, string artistName)
		{
			try
			{
				this.YouTubeStatusText.Text = "Trying alternative approach...";
				string text = "https://yt.legacyprojects.ru/get_search_videos.php?query=" + Uri.EscapeDataString(searchQuery) + "&count=1&apikey=" + Uri.EscapeDataString(this._youTubeApiKey);
				Debug.WriteLine("Alternative search URL: " + text);
				WebClient webClient = new WebClient();
				webClient.Headers["User-Agent"] = "OpenTracksBETA/1.0";
				Exception ex;
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
				{
					bool flag = e.Error != null;
					if (flag)
					{
						Deployment.Current.Dispatcher.BeginInvoke(delegate()
						{
							this.YouTubeStatusText.Text = "Alternative search failed";
							MessageBox.Show("Alternative search error: " + e.Error.Message);
							this.PlayYouTubeButton.IsEnabled = true;
						});
					}
					else
					{
						try
						{
							Debug.WriteLine("Alternative search response: " + e.Result);
							string text2 = this.ExtractVideoIdFromSearchResponse(e.Result);
							bool flag2 = string.IsNullOrEmpty(text2);
							if (flag2)
							{
								Deployment.Current.Dispatcher.BeginInvoke(delegate()
								{
									this.YouTubeStatusText.Text = "No video found";
									MessageBox.Show("Could not find video ID in search results");
									this.PlayYouTubeButton.IsEnabled = true;
								});
							}
							else
							{
								Debug.WriteLine("Found video ID: " + text2);
								this.GetAudioStreamWithVideoId(text2, track, artistName);
							}
						}
						catch (Exception ex3)
						{
							Exception ex4 = ex3;
							Exception ex = ex4;
							Deployment.Current.Dispatcher.BeginInvoke(delegate()
							{
								this.YouTubeStatusText.Text = "Search parse failed";
								MessageBox.Show("Search parse error: " + ex.Message);
								this.PlayYouTubeButton.IsEnabled = true;
							});
						}
					}
				};
				webClient.DownloadStringAsync(new Uri(text));
			}
			catch (Exception ex)
			{
				Exception ex;
				Exception ex;
				Exception ex2 = ex;
				ex = ex2;
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.YouTubeStatusText.Text = "Alternative approach failed";
					MessageBox.Show("Alternative approach error: " + ex.Message);
					this.PlayYouTubeButton.IsEnabled = true;
				});
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000073D4 File Offset: 0x000055D4
		private string ExtractVideoIdFromSearchResponse(string response)
		{
			string result;
			try
			{
				string text = response.Trim();
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
					bool flag4 = jobject["video_id"] != null;
					if (flag4)
					{
						return jobject["video_id"].ToString();
					}
					bool flag5;
					if (jobject["videos"] != null)
					{
						JToken jtoken = jobject["videos"][0];
						flag5 = (((jtoken != null) ? jtoken["video_id"] : null) != null);
					}
					else
					{
						flag5 = false;
					}
					bool flag6 = flag5;
					if (flag6)
					{
						return jobject["videos"][0]["video_id"].ToString();
					}
					bool flag7;
					if (jobject["items"] != null)
					{
						JToken jtoken2 = jobject["items"][0];
						flag7 = (((jtoken2 != null) ? jtoken2["video_id"] : null) != null);
					}
					else
					{
						flag7 = false;
					}
					bool flag8 = flag7;
					if (flag8)
					{
						return jobject["items"][0]["video_id"].ToString();
					}
				}
				Match match = Regex.Match(text, "\"video_id\"\\s*:\\s*\"([^\"]+)\"");
				bool success = match.Success;
				if (success)
				{
					result = match.Groups[1].Value;
				}
				else
				{
					result = null;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Video ID extraction error: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000075E8 File Offset: 0x000057E8
		private void GetAudioStreamWithVideoId(string videoId, Track track, string artistName)
		{
			try
			{
				string text = "https://yt.legacyprojects.ru/direct_audio_url?video_id=" + videoId;
				Debug.WriteLine("Trying direct audio URL: " + text);
				WebClient webClient = new WebClient();
				webClient.Headers["User-Agent"] = "OpenTracksBETA/1.0";
				Exception ex;
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
				{
					bool flag = e.Error != null;
					if (flag)
					{
						Deployment.Current.Dispatcher.BeginInvoke(delegate()
						{
							this.YouTubeStatusText.Text = "Direct audio failed";
							MessageBox.Show("Direct audio error: " + e.Error.Message);
							this.PlayYouTubeButton.IsEnabled = true;
						});
					}
					else
					{
						try
						{
							string text2 = e.Result.Trim();
							Debug.WriteLine("Direct audio response: " + text2);
							string streamUrl = text2;
							bool flag2 = text2.StartsWith("{");
							if (flag2)
							{
								JObject jobject = JObject.Parse(text2);
								bool flag3 = jobject["url"] != null;
								if (flag3)
								{
									streamUrl = jobject["url"].ToString();
								}
								else
								{
									bool flag4 = jobject["audio_url"] != null;
									if (flag4)
									{
										streamUrl = jobject["audio_url"].ToString();
									}
									else
									{
										bool flag5 = jobject["video_url"] != null;
										if (flag5)
										{
											streamUrl = jobject["video_url"].ToString();
										}
									}
								}
							}
							bool flag6 = streamUrl.StartsWith("http");
							if (flag6)
							{
								Deployment.Current.Dispatcher.BeginInvoke(delegate()
								{
									Debug.WriteLine("Alternative method success! Audio URL: " + streamUrl);
									this.TestAndPlayAudio(streamUrl, track, artistName);
								});
							}
							else
							{
								Deployment.Current.Dispatcher.BeginInvoke(delegate()
								{
									this.YouTubeStatusText.Text = "Invalid stream URL";
									MessageBox.Show("Invalid stream URL: " + streamUrl);
									this.PlayYouTubeButton.IsEnabled = true;
								});
							}
						}
						catch (Exception ex3)
						{
							Exception ex4 = ex3;
							Exception ex = ex4;
							Deployment.Current.Dispatcher.BeginInvoke(delegate()
							{
								this.YouTubeStatusText.Text = "Audio parse failed";
								MessageBox.Show("Audio parse error: " + ex.Message);
								this.PlayYouTubeButton.IsEnabled = true;
							});
						}
					}
				};
				webClient.DownloadStringAsync(new Uri(text));
			}
			catch (Exception ex)
			{
				Exception ex;
				Exception ex;
				Exception ex2 = ex;
				ex = ex2;
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.YouTubeStatusText.Text = "Audio stream failed";
					MessageBox.Show("Audio stream error: " + ex.Message);
					this.PlayYouTubeButton.IsEnabled = true;
				});
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000076B8 File Offset: 0x000058B8
		private void PlayYouTubeAudio(string audioUrl, Track track, string artistName)
		{
			try
			{
				this.YouTubeStatusText.Text = "Loading audio stream...";
				this._audioPlayer.Stop();
				this._audioPlayer.Source = null;
				Debug.WriteLine("Final audio URL: " + audioUrl);
				bool flag = audioUrl.Contains("googlevideo.com");
				if (flag)
				{
					this.TryDirectPlayback(audioUrl, track, artistName);
				}
				else
				{
					this.ExtractFinalStreamUrl(audioUrl, track, artistName);
				}
			}
			catch (Exception ex)
			{
				this.YouTubeStatusText.Text = "Playback failed";
				MessageBox.Show("Audio playback error: " + ex.Message);
				this.PlayYouTubeButton.IsEnabled = true;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007778 File Offset: 0x00005978
		private void TryAlternativePlayback(string audioUrl, Track track, string artistName)
		{
			try
			{
				this.YouTubeStatusText.Text = "Streaming audio...";
				WebClient webClient = new WebClient();
				webClient.Headers["User-Agent"] = "Mozilla/5.0 (Windows Phone 8.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.85 Mobile Safari/537.36";
				webClient.Headers["Range"] = "bytes=0-";
				Action <>9__2;
				Exception ex;
				webClient.OpenReadCompleted += delegate(object sender, OpenReadCompletedEventArgs e)
				{
					bool flag = e.Error != null;
					if (flag)
					{
						Deployment.Current.Dispatcher.BeginInvoke(delegate()
						{
							this.YouTubeStatusText.Text = "Stream failed";
							MessageBox.Show("Stream error: " + e.Error.Message);
							this.PlayYouTubeButton.IsEnabled = true;
						});
					}
					else
					{
						try
						{
							Dispatcher dispatcher = Deployment.Current.Dispatcher;
							Action action;
							if ((action = <>9__2) == null)
							{
								action = (<>9__2 = delegate()
								{
									this.TryDirectPlayback(audioUrl, track, artistName);
								});
							}
							dispatcher.BeginInvoke(action);
						}
						catch (Exception ex2)
						{
							Exception ex3 = ex2;
							Exception ex = ex3;
							Deployment.Current.Dispatcher.BeginInvoke(delegate()
							{
								this.YouTubeStatusText.Text = "Playback failed";
								MessageBox.Show("Playback error: " + ex.Message);
								this.PlayYouTubeButton.IsEnabled = true;
							});
						}
					}
				};
				webClient.OpenReadAsync(new Uri(audioUrl));
			}
			catch (Exception ex)
			{
				this.YouTubeStatusText.Text = "Alternative playback failed";
				Exception ex;
				MessageBox.Show("Alternative playback error: " + ex.Message);
				this.PlayYouTubeButton.IsEnabled = true;
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00007860 File Offset: 0x00005A60
		private void TryDirectPlayback(string audioUrl, Track track, string artistName)
		{
			try
			{
				this.YouTubeStatusText.Text = "Initializing audio player...";
				this._audioPlayer.Stop();
				this._audioPlayer.Source = null;
				Thread.Sleep(100);
				this._audioPlayer.Source = new Uri(audioUrl);
				this._audioPlayer.Play();
				this._isPlaying = true;
				this.PlayButton.Content = "Pause";
				this.YouTubeStatusText.Text = "Playing: " + track.Name;
				this.StartProgressSimulation();
				this.NowPlayingText.Text = "YouTube: " + track.Name + " by " + artistName;
				this.PlayYouTubeButton.IsEnabled = true;
			}
			catch (Exception ex)
			{
				this.YouTubeStatusText.Text = "Direct playback failed";
				MessageBox.Show("Direct playback error: " + ex.Message);
				this.PlayYouTubeButton.IsEnabled = true;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00007978 File Offset: 0x00005B78
		private void ExtractFinalStreamUrl(string apiUrl, Track track, string artistName)
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.Headers["User-Agent"] = "OpenTracksBETA/1.0";
				Exception ex;
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
				{
					bool flag = e.Error != null;
					if (flag)
					{
						Deployment.Current.Dispatcher.BeginInvoke(delegate()
						{
							this.YouTubeStatusText.Text = "Stream extraction failed";
							MessageBox.Show("Could not extract audio stream: " + e.Error.Message);
							this.PlayYouTubeButton.IsEnabled = true;
						});
					}
					else
					{
						try
						{
							string text = e.Result.Trim();
							Debug.WriteLine("Final stream extraction response: " + text);
							string streamUrl = text;
							bool flag2 = text.StartsWith("{") || text.StartsWith("[");
							if (flag2)
							{
								JObject jobject = text.StartsWith("[") ? (Enumerable.FirstOrDefault<JToken>(JArray.Parse(text)) as JObject) : JObject.Parse(text);
								bool flag3 = jobject != null;
								if (flag3)
								{
									string[] array = new string[]
									{
										"url",
										"audio_url",
										"video_url",
										"stream_url"
									};
									foreach (string propertyName in array)
									{
										bool flag4 = jobject[propertyName] != null;
										if (flag4)
										{
											streamUrl = jobject[propertyName].ToString();
											break;
										}
									}
								}
							}
							bool flag5 = streamUrl.StartsWith("http");
							if (flag5)
							{
								Deployment.Current.Dispatcher.BeginInvoke(delegate()
								{
									Debug.WriteLine("Final stream URL: " + streamUrl);
									this.TryDirectPlayback(streamUrl, track, artistName);
								});
							}
							else
							{
								Deployment.Current.Dispatcher.BeginInvoke(delegate()
								{
									this.YouTubeStatusText.Text = "Invalid stream URL";
									MessageBox.Show("The API returned an invalid stream URL: " + streamUrl);
									this.PlayYouTubeButton.IsEnabled = true;
								});
							}
						}
						catch (Exception ex3)
						{
							Exception ex4 = ex3;
							Exception ex = ex4;
							Deployment.Current.Dispatcher.BeginInvoke(delegate()
							{
								this.YouTubeStatusText.Text = "Stream parse failed";
								MessageBox.Show("Error parsing stream response: " + ex.Message);
								this.PlayYouTubeButton.IsEnabled = true;
							});
						}
					}
				};
				webClient.DownloadStringAsync(new Uri(apiUrl));
			}
			catch (Exception ex)
			{
				Exception ex;
				Exception ex;
				Exception ex2 = ex;
				ex = ex2;
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.YouTubeStatusText.Text = "Stream extraction error";
					MessageBox.Show("Stream extraction error: " + ex.Message);
					this.PlayYouTubeButton.IsEnabled = true;
				});
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007A28 File Offset: 0x00005C28
		private void ExtractActualStreamUrl(string apiUrl, Track track, string artistName)
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.Headers["User-Agent"] = "Mozilla/5.0 (Windows Phone 8.1) AppleWebKit/537.36";
				Exception ex;
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
				{
					bool flag = e.Error != null;
					if (flag)
					{
						Deployment.Current.Dispatcher.BeginInvoke(delegate()
						{
							this.YouTubeStatusText.Text = "Stream extraction failed";
							MessageBox.Show("Could not extract audio stream: " + e.Error.Message);
							this.PlayYouTubeButton.IsEnabled = true;
						});
					}
					else
					{
						try
						{
							string result = e.Result;
							Debug.WriteLine("Stream extraction response: " + result);
							string streamUrl = result.Trim();
							bool flag2 = streamUrl.StartsWith("http");
							if (flag2)
							{
								Deployment.Current.Dispatcher.BeginInvoke(delegate()
								{
									this.TryDirectPlayback(streamUrl, track, artistName);
								});
							}
							else
							{
								Deployment.Current.Dispatcher.BeginInvoke(delegate()
								{
									this.YouTubeStatusText.Text = "Invalid stream URL";
									MessageBox.Show("The API returned an invalid stream URL");
									this.PlayYouTubeButton.IsEnabled = true;
								});
							}
						}
						catch (Exception ex3)
						{
							Exception ex4 = ex3;
							Exception ex = ex4;
							Deployment.Current.Dispatcher.BeginInvoke(delegate()
							{
								this.YouTubeStatusText.Text = "Stream parse failed";
								MessageBox.Show("Error parsing stream response: " + ex.Message);
								this.PlayYouTubeButton.IsEnabled = true;
							});
						}
					}
				};
				webClient.DownloadStringAsync(new Uri(apiUrl));
			}
			catch (Exception ex)
			{
				Exception ex;
				Exception ex;
				Exception ex2 = ex;
				ex = ex2;
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.YouTubeStatusText.Text = "Stream extraction error";
					MessageBox.Show("Stream extraction error: " + ex.Message);
					this.PlayYouTubeButton.IsEnabled = true;
				});
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007AD8 File Offset: 0x00005CD8
		private void AudioPlayer_MediaOpened(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Media opened successfully - audio stream is working!");
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				TextBox youTubeStatusText = this.YouTubeStatusText;
				string text = "Playing: ";
				Track currentTrack = this._currentTrack;
				youTubeStatusText.Text = text + (((currentTrack != null) ? currentTrack.Name : null) ?? "Unknown track");
				this._isPlaying = true;
				this.PlayButton.Content = "Pause";
				this.StartProgressSimulation();
			});
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007B04 File Offset: 0x00005D04
		private void AudioPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			Debug.WriteLine("Media failed: " + e.ErrorException.Message);
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				this.YouTubeStatusText.Text = "Playback failed";
				string text = "Audio playback failed.\n\n";
				text += "The audio endpoint returned data but MediaElement couldn't play it.\n";
				text += "This could be due to:\n";
				text += "• Unsupported audio codec\n";
				text += "• Missing audio headers\n";
				text += "• Network issues\n\n";
				text = text + "Error: " + e.ErrorException.Message;
				MessageBox.Show(text);
				this._isPlaying = false;
				this.PlayButton.Content = "Play";
				this.PlayYouTubeButton.IsEnabled = true;
				this.StopProgressSimulation();
			});
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007B62 File Offset: 0x00005D62
		private void AudioPlayer_MediaEnded(object sender, RoutedEventArgs e)
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				this.YouTubeStatusText.Text = "Playback finished";
				this._isPlaying = false;
				this.PlayButton.Content = "Play";
				this.StopProgressSimulation();
			});
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007B84 File Offset: 0x00005D84
		private void TestAndPlayAudio(string audioUrl, Track track, string artistName)
		{
			try
			{
				this.YouTubeStatusText.Text = "Testing audio URL...";
				WebClient webClient = new WebClient();
				webClient.Headers["User-Agent"] = "OpenTracksBETA/1.0";
				webClient.Headers["Range"] = "bytes=0-1024";
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
				{
					Deployment.Current.Dispatcher.BeginInvoke(delegate()
					{
						bool flag = e.Error != null;
						if (flag)
						{
							Debug.WriteLine("URL test failed, but trying playback anyway: " + e.Error.Message);
							this.PlayYouTubeAudio(audioUrl, track, artistName);
						}
						else
						{
							Debug.WriteLine("URL test successful, starting playback");
							this.PlayYouTubeAudio(audioUrl, track, artistName);
						}
					});
				};
				webClient.DownloadStringAsync(new Uri(audioUrl));
			}
			catch (Exception ex)
			{
				Debug.WriteLine("URL test error: " + ex.Message);
				this.PlayYouTubeAudio(audioUrl, track, artistName);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007C64 File Offset: 0x00005E64
		[return: Dynamic]
		private dynamic FindFirstVideo(dynamic json)
		{
			try
			{
				if (SpotifyPage.<>o__34.<>p__7 == null)
				{
					SpotifyPage.<>o__34.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, 83, typeof(SpotifyPage), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> target = SpotifyPage.<>o__34.<>p__7.Target;
				CallSite <>p__ = SpotifyPage.<>o__34.<>p__7;
				if (SpotifyPage.<>o__34.<>p__1 == null)
				{
					SpotifyPage.<>o__34.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, 35, typeof(SpotifyPage), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, object, object> target2 = SpotifyPage.<>o__34.<>p__1.Target;
				CallSite <>p__2 = SpotifyPage.<>o__34.<>p__1;
				if (SpotifyPage.<>o__34.<>p__0 == null)
				{
					SpotifyPage.<>o__34.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "contents", typeof(SpotifyPage), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object obj = target2.Invoke(<>p__2, SpotifyPage.<>o__34.<>p__0.Target.Invoke(SpotifyPage.<>o__34.<>p__0, json), null);
				if (SpotifyPage.<>o__34.<>p__6 == null)
				{
					SpotifyPage.<>o__34.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, 84, typeof(SpotifyPage), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object obj3;
				if (!SpotifyPage.<>o__34.<>p__6.Target.Invoke(SpotifyPage.<>o__34.<>p__6, obj))
				{
					if (SpotifyPage.<>o__34.<>p__5 == null)
					{
						SpotifyPage.<>o__34.<>p__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, 2, typeof(SpotifyPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object, object> target3 = SpotifyPage.<>o__34.<>p__5.Target;
					CallSite <>p__3 = SpotifyPage.<>o__34.<>p__5;
					object obj2 = obj;
					if (SpotifyPage.<>o__34.<>p__4 == null)
					{
						SpotifyPage.<>o__34.<>p__4 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, 35, typeof(SpotifyPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					Func<CallSite, object, object, object> target4 = SpotifyPage.<>o__34.<>p__4.Target;
					CallSite <>p__4 = SpotifyPage.<>o__34.<>p__4;
					if (SpotifyPage.<>o__34.<>p__3 == null)
					{
						SpotifyPage.<>o__34.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "twoColumnSearchResultsRenderer", typeof(SpotifyPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target5 = SpotifyPage.<>o__34.<>p__3.Target;
					CallSite <>p__5 = SpotifyPage.<>o__34.<>p__3;
					if (SpotifyPage.<>o__34.<>p__2 == null)
					{
						SpotifyPage.<>o__34.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "contents", typeof(SpotifyPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					obj3 = target3.Invoke(<>p__3, obj2, target4.Invoke(<>p__4, target5.Invoke(<>p__5, SpotifyPage.<>o__34.<>p__2.Target.Invoke(SpotifyPage.<>o__34.<>p__2, json)), null));
				}
				else
				{
					obj3 = obj;
				}
				bool flag = target.Invoke(<>p__, obj3);
				if (flag)
				{
					if (SpotifyPage.<>o__34.<>p__10 == null)
					{
						SpotifyPage.<>o__34.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "primaryContents", typeof(SpotifyPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target6 = SpotifyPage.<>o__34.<>p__10.Target;
					CallSite <>p__6 = SpotifyPage.<>o__34.<>p__10;
					if (SpotifyPage.<>o__34.<>p__9 == null)
					{
						SpotifyPage.<>o__34.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "twoColumnSearchResultsRenderer", typeof(SpotifyPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target7 = SpotifyPage.<>o__34.<>p__9.Target;
					CallSite <>p__7 = SpotifyPage.<>o__34.<>p__9;
					if (SpotifyPage.<>o__34.<>p__8 == null)
					{
						SpotifyPage.<>o__34.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "contents", typeof(SpotifyPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj4 = target6.Invoke(<>p__6, target7.Invoke(<>p__7, SpotifyPage.<>o__34.<>p__8.Target.Invoke(SpotifyPage.<>o__34.<>p__8, json)));
					if (SpotifyPage.<>o__34.<>p__16 == null)
					{
						SpotifyPage.<>o__34.<>p__16 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, 83, typeof(SpotifyPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target8 = SpotifyPage.<>o__34.<>p__16.Target;
					CallSite <>p__8 = SpotifyPage.<>o__34.<>p__16;
					if (SpotifyPage.<>o__34.<>p__11 == null)
					{
						SpotifyPage.<>o__34.<>p__11 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, 35, typeof(SpotifyPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					obj = SpotifyPage.<>o__34.<>p__11.Target.Invoke(SpotifyPage.<>o__34.<>p__11, obj4, null);
					if (SpotifyPage.<>o__34.<>p__15 == null)
					{
						SpotifyPage.<>o__34.<>p__15 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, 84, typeof(SpotifyPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj6;
					if (!SpotifyPage.<>o__34.<>p__15.Target.Invoke(SpotifyPage.<>o__34.<>p__15, obj))
					{
						if (SpotifyPage.<>o__34.<>p__14 == null)
						{
							SpotifyPage.<>o__34.<>p__14 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, 2, typeof(SpotifyPage), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object, object> target9 = SpotifyPage.<>o__34.<>p__14.Target;
						CallSite <>p__9 = SpotifyPage.<>o__34.<>p__14;
						object obj5 = obj;
						if (SpotifyPage.<>o__34.<>p__13 == null)
						{
							SpotifyPage.<>o__34.<>p__13 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, 35, typeof(SpotifyPage), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						Func<CallSite, object, object, object> target10 = SpotifyPage.<>o__34.<>p__13.Target;
						CallSite <>p__10 = SpotifyPage.<>o__34.<>p__13;
						if (SpotifyPage.<>o__34.<>p__12 == null)
						{
							SpotifyPage.<>o__34.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "sectionListRenderer", typeof(SpotifyPage), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj6 = target9.Invoke(<>p__9, obj5, target10.Invoke(<>p__10, SpotifyPage.<>o__34.<>p__12.Target.Invoke(SpotifyPage.<>o__34.<>p__12, obj4), null));
					}
					else
					{
						obj6 = obj;
					}
					bool flag2 = target8.Invoke(<>p__8, obj6);
					if (flag2)
					{
						if (SpotifyPage.<>o__34.<>p__18 == null)
						{
							SpotifyPage.<>o__34.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "contents", typeof(SpotifyPage), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target11 = SpotifyPage.<>o__34.<>p__18.Target;
						CallSite <>p__11 = SpotifyPage.<>o__34.<>p__18;
						if (SpotifyPage.<>o__34.<>p__17 == null)
						{
							SpotifyPage.<>o__34.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "sectionListRenderer", typeof(SpotifyPage), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						object obj7 = target11.Invoke(<>p__11, SpotifyPage.<>o__34.<>p__17.Target.Invoke(SpotifyPage.<>o__34.<>p__17, obj4));
						if (SpotifyPage.<>o__34.<>p__24 == null)
						{
							SpotifyPage.<>o__34.<>p__24 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, 83, typeof(SpotifyPage), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target12 = SpotifyPage.<>o__34.<>p__24.Target;
						CallSite <>p__12 = SpotifyPage.<>o__34.<>p__24;
						if (SpotifyPage.<>o__34.<>p__19 == null)
						{
							SpotifyPage.<>o__34.<>p__19 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, 35, typeof(SpotifyPage), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						obj = SpotifyPage.<>o__34.<>p__19.Target.Invoke(SpotifyPage.<>o__34.<>p__19, obj7, null);
						if (SpotifyPage.<>o__34.<>p__23 == null)
						{
							SpotifyPage.<>o__34.<>p__23 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, 84, typeof(SpotifyPage), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						object obj9;
						if (!SpotifyPage.<>o__34.<>p__23.Target.Invoke(SpotifyPage.<>o__34.<>p__23, obj))
						{
							if (SpotifyPage.<>o__34.<>p__22 == null)
							{
								SpotifyPage.<>o__34.<>p__22 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, 2, typeof(SpotifyPage), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Func<CallSite, object, object, object> target13 = SpotifyPage.<>o__34.<>p__22.Target;
							CallSite <>p__13 = SpotifyPage.<>o__34.<>p__22;
							object obj8 = obj;
							if (SpotifyPage.<>o__34.<>p__21 == null)
							{
								SpotifyPage.<>o__34.<>p__21 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, 15, typeof(SpotifyPage), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
								}));
							}
							Func<CallSite, object, int, object> target14 = SpotifyPage.<>o__34.<>p__21.Target;
							CallSite <>p__14 = SpotifyPage.<>o__34.<>p__21;
							if (SpotifyPage.<>o__34.<>p__20 == null)
							{
								SpotifyPage.<>o__34.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(SpotifyPage), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj9 = target13.Invoke(<>p__13, obj8, target14.Invoke(<>p__14, SpotifyPage.<>o__34.<>p__20.Target.Invoke(SpotifyPage.<>o__34.<>p__20, obj7), 0));
						}
						else
						{
							obj9 = obj;
						}
						bool flag3 = target12.Invoke(<>p__12, obj9);
						if (flag3)
						{
							if (SpotifyPage.<>o__34.<>p__37 == null)
							{
								SpotifyPage.<>o__34.<>p__37 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(SpotifyPage)));
							}
							foreach (object obj10 in SpotifyPage.<>o__34.<>p__37.Target.Invoke(SpotifyPage.<>o__34.<>p__37, obj7))
							{
								if (SpotifyPage.<>o__34.<>p__27 == null)
								{
									SpotifyPage.<>o__34.<>p__27 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, 83, typeof(SpotifyPage), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								Func<CallSite, object, bool> target15 = SpotifyPage.<>o__34.<>p__27.Target;
								CallSite <>p__15 = SpotifyPage.<>o__34.<>p__27;
								if (SpotifyPage.<>o__34.<>p__26 == null)
								{
									SpotifyPage.<>o__34.<>p__26 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, 35, typeof(SpotifyPage), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
									}));
								}
								Func<CallSite, object, object, object> target16 = SpotifyPage.<>o__34.<>p__26.Target;
								CallSite <>p__16 = SpotifyPage.<>o__34.<>p__26;
								if (SpotifyPage.<>o__34.<>p__25 == null)
								{
									SpotifyPage.<>o__34.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "itemSectionRenderer", typeof(SpotifyPage), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								bool flag4 = target15.Invoke(<>p__15, target16.Invoke(<>p__16, SpotifyPage.<>o__34.<>p__25.Target.Invoke(SpotifyPage.<>o__34.<>p__25, obj10), null));
								if (flag4)
								{
									if (SpotifyPage.<>o__34.<>p__29 == null)
									{
										SpotifyPage.<>o__34.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "contents", typeof(SpotifyPage), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, object, object> target17 = SpotifyPage.<>o__34.<>p__29.Target;
									CallSite <>p__17 = SpotifyPage.<>o__34.<>p__29;
									if (SpotifyPage.<>o__34.<>p__28 == null)
									{
										SpotifyPage.<>o__34.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "itemSectionRenderer", typeof(SpotifyPage), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									object obj11 = target17.Invoke(<>p__17, SpotifyPage.<>o__34.<>p__28.Target.Invoke(SpotifyPage.<>o__34.<>p__28, obj10));
									if (SpotifyPage.<>o__34.<>p__31 == null)
									{
										SpotifyPage.<>o__34.<>p__31 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, 83, typeof(SpotifyPage), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, object, bool> target18 = SpotifyPage.<>o__34.<>p__31.Target;
									CallSite <>p__18 = SpotifyPage.<>o__34.<>p__31;
									if (SpotifyPage.<>o__34.<>p__30 == null)
									{
										SpotifyPage.<>o__34.<>p__30 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, 35, typeof(SpotifyPage), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
										}));
									}
									bool flag5 = target18.Invoke(<>p__18, SpotifyPage.<>o__34.<>p__30.Target.Invoke(SpotifyPage.<>o__34.<>p__30, obj11, null));
									if (flag5)
									{
										if (SpotifyPage.<>o__34.<>p__36 == null)
										{
											SpotifyPage.<>o__34.<>p__36 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(SpotifyPage)));
										}
										foreach (object obj12 in SpotifyPage.<>o__34.<>p__36.Target.Invoke(SpotifyPage.<>o__34.<>p__36, obj11))
										{
											if (SpotifyPage.<>o__34.<>p__34 == null)
											{
												SpotifyPage.<>o__34.<>p__34 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, 83, typeof(SpotifyPage), new CSharpArgumentInfo[]
												{
													CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
												}));
											}
											Func<CallSite, object, bool> target19 = SpotifyPage.<>o__34.<>p__34.Target;
											CallSite <>p__19 = SpotifyPage.<>o__34.<>p__34;
											if (SpotifyPage.<>o__34.<>p__33 == null)
											{
												SpotifyPage.<>o__34.<>p__33 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, 35, typeof(SpotifyPage), new CSharpArgumentInfo[]
												{
													CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
													CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
												}));
											}
											Func<CallSite, object, object, object> target20 = SpotifyPage.<>o__34.<>p__33.Target;
											CallSite <>p__20 = SpotifyPage.<>o__34.<>p__33;
											if (SpotifyPage.<>o__34.<>p__32 == null)
											{
												SpotifyPage.<>o__34.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "videoRenderer", typeof(SpotifyPage), new CSharpArgumentInfo[]
												{
													CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
												}));
											}
											bool flag6 = target19.Invoke(<>p__19, target20.Invoke(<>p__20, SpotifyPage.<>o__34.<>p__32.Target.Invoke(SpotifyPage.<>o__34.<>p__32, obj12), null));
											if (flag6)
											{
												if (SpotifyPage.<>o__34.<>p__35 == null)
												{
													SpotifyPage.<>o__34.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "videoRenderer", typeof(SpotifyPage), new CSharpArgumentInfo[]
													{
														CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
													}));
												}
												return SpotifyPage.<>o__34.<>p__35.Target.Invoke(SpotifyPage.<>o__34.<>p__35, obj12);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00008934 File Offset: 0x00006B34
		private void LoadCredentials()
		{
			bool flag = IsolatedStorageSettings.ApplicationSettings.Contains("SpotifyClientId");
			if (flag)
			{
				this._spotifyClientId = (IsolatedStorageSettings.ApplicationSettings["SpotifyClientId"] as string);
			}
			bool flag2 = IsolatedStorageSettings.ApplicationSettings.Contains("SpotifyClientSecret");
			if (flag2)
			{
				this._spotifyClientSecret = (IsolatedStorageSettings.ApplicationSettings["SpotifyClientSecret"] as string);
			}
			bool flag3 = IsolatedStorageSettings.ApplicationSettings.Contains("SpotifyRedirectUri");
			if (flag3)
			{
				this._spotifyRedirectUri = (IsolatedStorageSettings.ApplicationSettings["SpotifyRedirectUri"] as string);
			}
			bool flag4 = IsolatedStorageSettings.ApplicationSettings.Contains("YouTubeApiKey");
			if (flag4)
			{
				this._youTubeApiKey = (IsolatedStorageSettings.ApplicationSettings["YouTubeApiKey"] as string);
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000089F8 File Offset: 0x00006BF8
		private void SaveCredentials()
		{
			IsolatedStorageSettings.ApplicationSettings["SpotifyClientId"] = this._spotifyClientId;
			IsolatedStorageSettings.ApplicationSettings["SpotifyClientSecret"] = this._spotifyClientSecret;
			IsolatedStorageSettings.ApplicationSettings["SpotifyRedirectUri"] = this._spotifyRedirectUri;
			IsolatedStorageSettings.ApplicationSettings["YouTubeApiKey"] = this._youTubeApiKey;
			IsolatedStorageSettings.ApplicationSettings["SpotifyCredentialsSet"] = true;
			IsolatedStorageSettings.ApplicationSettings.Save();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00008A80 File Offset: 0x00006C80
		private void ShowCredentialsEntry()
		{
			this.CredentialsPanel.Visibility = 0;
			this.LoginPanel.Visibility = 1;
			this.CodeEntryPanel.Visibility = 1;
			this.MainAppPanel.Visibility = 1;
			bool flag = !string.IsNullOrEmpty(this._spotifyClientId);
			if (flag)
			{
				this.ClientIdTextBox.Text = this._spotifyClientId;
			}
			bool flag2 = !string.IsNullOrEmpty(this._spotifyClientSecret);
			if (flag2)
			{
				this.ClientSecretTextBox.Text = this._spotifyClientSecret;
			}
			bool flag3 = !string.IsNullOrEmpty(this._spotifyRedirectUri);
			if (flag3)
			{
				this.RedirectUriTextBox.Text = this._spotifyRedirectUri;
			}
			bool flag4 = !string.IsNullOrEmpty(this._youTubeApiKey);
			if (flag4)
			{
				this.YouTubeApiKeyTextBox.Text = this._youTubeApiKey;
			}
			this.NowPlayingText.Text = "Enter your Spotify App credentials and YouTube API key";
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00008B64 File Offset: 0x00006D64
		private void ShowLoginOptions()
		{
			this.CredentialsPanel.Visibility = 1;
			this.LoginPanel.Visibility = 0;
			this.CodeEntryPanel.Visibility = 1;
			this.MainAppPanel.Visibility = 1;
			this.NowPlayingText.Text = "Login to Spotify Premium";
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00008BB8 File Offset: 0x00006DB8
		private void ShowMainApp()
		{
			this.CredentialsPanel.Visibility = 1;
			this.LoginPanel.Visibility = 1;
			this.CodeEntryPanel.Visibility = 1;
			this.MainAppPanel.Visibility = 0;
			this.NowPlayingText.Text = "Ready to play music!";
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00008C0C File Offset: 0x00006E0C
		private void SaveCredentialsButton_Click(object sender, RoutedEventArgs e)
		{
			string text = this.ClientIdTextBox.Text.Trim();
			string text2 = this.ClientSecretTextBox.Text.Trim();
			string text3 = this.RedirectUriTextBox.Text.Trim();
			string text4 = this.YouTubeApiKeyTextBox.Text.Trim();
			bool flag = string.IsNullOrEmpty(text);
			if (flag)
			{
				MessageBox.Show("Please enter your Client ID");
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(text2);
				if (flag2)
				{
					MessageBox.Show("Please enter your Client Secret");
				}
				else
				{
					bool flag3 = string.IsNullOrEmpty(text3);
					if (flag3)
					{
						MessageBox.Show("Please enter your Redirect URI");
					}
					else
					{
						bool flag4 = string.IsNullOrEmpty(text4);
						if (flag4)
						{
							MessageBox.Show("Please enter your YouTube API Key");
						}
						else
						{
							this._spotifyClientId = text;
							this._spotifyClientSecret = text2;
							this._spotifyRedirectUri = text3;
							this._youTubeApiKey = text4;
							this._youTubeService.SetApiKey(this._youTubeApiKey);
							this.SaveCredentials();
							this.ShowLoginOptions();
							MessageBox.Show("Credentials saved successfully! You can now login.");
						}
					}
				}
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00008D10 File Offset: 0x00006F10
		private void EditCredentialsButton_Click(object sender, RoutedEventArgs e)
		{
			bool flag = IsolatedStorageSettings.ApplicationSettings.Contains("SpotifyAccessToken");
			if (flag)
			{
				IsolatedStorageSettings.ApplicationSettings.Remove("SpotifyAccessToken");
				IsolatedStorageSettings.ApplicationSettings.Remove("SpotifyRefreshToken");
				IsolatedStorageSettings.ApplicationSettings.Save();
			}
			this.ShowCredentialsEntry();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00008D65 File Offset: 0x00006F65
		private void StartCodeEntry_Click(object sender, RoutedEventArgs e)
		{
			this.LoginPanel.Visibility = 1;
			this.CodeEntryPanel.Visibility = 0;
			this.CodeTextBox.Text = "";
			this.NowPlayingText.Text = "Enter authorization code";
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00008DA4 File Offset: 0x00006FA4
		private void SubmitCode_Click(object sender, RoutedEventArgs e)
		{
			string text = this.CodeTextBox.Text.Trim();
			bool flag = string.IsNullOrEmpty(text);
			if (flag)
			{
				MessageBox.Show("Please enter the authorization code");
			}
			else
			{
				text = text.Trim();
				this.ExchangeCodeForToken(text);
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00008DEC File Offset: 0x00006FEC
		private void ExchangeCodeForToken(string code)
		{
			try
			{
				this.LoadingProgressBar.Visibility = 0;
				this.NowPlayingText.Text = "Getting access token...";
				WebClient webClient = new WebClient();
				string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(this._spotifyClientId + ":" + this._spotifyClientSecret));
				webClient.Headers[24] = "Basic " + text;
				webClient.Headers[12] = "application/x-www-form-urlencoded";
				string text2 = "grant_type=authorization_code&code=" + Uri.EscapeDataString(code) + "&redirect_uri=" + Uri.EscapeDataString(this._spotifyRedirectUri);
				webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(this.Client_CodeExchangeCompleted);
				webClient.UploadStringAsync(new Uri("https://accounts.spotify.com/api/token"), "POST", text2);
			}
			catch (Exception ex)
			{
				this.LoadingProgressBar.Visibility = 1;
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00008EF8 File Offset: 0x000070F8
		private void Client_CodeExchangeCompleted(object sender, UploadStringCompletedEventArgs e)
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				this.LoadingProgressBar.Visibility = 1;
				bool flag = e.Error == null;
				if (flag)
				{
					try
					{
						SpotifyTokenResponse spotifyTokenResponse = JsonConvert.DeserializeObject<SpotifyTokenResponse>(e.Result);
						bool flag2 = !string.IsNullOrEmpty(spotifyTokenResponse.access_token);
						if (flag2)
						{
							this._accessToken = spotifyTokenResponse.access_token;
							this._refreshToken = spotifyTokenResponse.refresh_token;
							IsolatedStorageSettings.ApplicationSettings["SpotifyAccessToken"] = this._accessToken;
							IsolatedStorageSettings.ApplicationSettings["SpotifyRefreshToken"] = this._refreshToken;
							IsolatedStorageSettings.ApplicationSettings.Save();
							this.ShowMainApp();
							MessageBox.Show("Login successful! You can now search and control Spotify playback.");
						}
						else
						{
							MessageBox.Show("No access token received. Please try again.");
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error reading response: " + ex.Message);
					}
				}
				else
				{
					string text = "Authentication failed: ";
					bool flag3 = e.Error is WebException;
					if (flag3)
					{
						WebException ex2 = (WebException)e.Error;
						bool flag4 = ex2.Response is HttpWebResponse;
						if (flag4)
						{
							HttpWebResponse httpWebResponse = (HttpWebResponse)ex2.Response;
							text = text + "HTTP " + httpWebResponse.StatusCode;
							HttpStatusCode statusCode = httpWebResponse.StatusCode;
							if (statusCode != 400)
							{
								if (statusCode != 401)
								{
									text = text + " - " + httpWebResponse.StatusDescription;
								}
								else
								{
									text += " - Invalid client credentials";
								}
							}
							else
							{
								text += " - Invalid or expired code";
							}
						}
						else
						{
							text += ex2.Message;
						}
					}
					else
					{
						text += e.Error.Message;
					}
					MessageBox.Show(text);
				}
			});
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00008F38 File Offset: 0x00007138
		private void HowToGetYouTubeApiKey_Click(object sender, RoutedEventArgs e)
		{
			string text = "HOW TO GET YOUTUBE API KEY:\n\n1. Go to https://console.cloud.google.com\n2. Create a new project or select existing one\n3. Enable YouTube Data API v3\n4. Create credentials (API key)\n5. Copy the API key and enter it here\n\nNote: The API key is free for reasonable usage.";
			MessageBox.Show(text);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00008F54 File Offset: 0x00007154
		private void HowToGetCredentials_Click(object sender, RoutedEventArgs e)
		{
			string text = "HOW TO GET SPOTIFY CREDENTIALS:\n\n1. Go to https://developer.spotify.com/dashboard\n2. Log in with your Spotify account\n3. Click 'Create App'\n4. Fill in app name and description\n5. Set the Redirect URI to a valid URI (can be http://localhost for testing)\n6. After creation, you'll see your Client ID and Client Secret\n7. Enter them here along with your Redirect URI\n\nImportant: Make sure your Redirect URI matches exactly what you set in the Spotify Dashboard.";
			MessageBox.Show(text);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00008F6F File Offset: 0x0000716F
		private void BackToLogin_Click(object sender, RoutedEventArgs e)
		{
			this.ShowLoginOptions();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00008F79 File Offset: 0x00007179
		private void BackToCredentials_Click(object sender, RoutedEventArgs e)
		{
			this.ShowCredentialsEntry();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00008F83 File Offset: 0x00007183
		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			this.SearchTracks();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00008F90 File Offset: 0x00007190
		private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			bool flag = e.Key == 3;
			if (flag)
			{
				this.SearchTracks();
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00008FB4 File Offset: 0x000071B4
		private void SearchTracks()
		{
			string text = this.SearchTextBox.Text.Trim();
			bool flag = string.IsNullOrWhiteSpace(text) || text == "Search for tracks...";
			if (flag)
			{
				MessageBox.Show("Please enter a search term");
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(this._accessToken);
				if (flag2)
				{
					MessageBox.Show("Please login first");
					this.ShowLoginOptions();
				}
				else
				{
					this.LoadingProgressBar.Visibility = 0;
					this.NowPlayingText.Text = "Searching...";
					try
					{
						WebClient webClient = new WebClient();
						webClient.Headers[24] = "Bearer " + this._accessToken;
						string text2 = Uri.EscapeDataString(text);
						string text3 = "https://api.spotify.com/v1/search?q=" + text2 + "&type=track&limit=10";
						webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.Client_SearchCompleted);
						webClient.DownloadStringAsync(new Uri(text3));
					}
					catch (Exception ex)
					{
						this.LoadingProgressBar.Visibility = 1;
						MessageBox.Show("Search error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000090E4 File Offset: 0x000072E4
		private void Client_SearchCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				this.LoadingProgressBar.Visibility = 1;
				bool flag = e.Error == null;
				if (flag)
				{
					try
					{
						SpotifySearchResult spotifySearchResult = JsonConvert.DeserializeObject<SpotifySearchResult>(e.Result);
						bool flag2 = spotifySearchResult.Tracks != null && spotifySearchResult.Tracks.Items != null && spotifySearchResult.Tracks.Items.Count > 0;
						if (flag2)
						{
							this.ResultsListBox.ItemsSource = spotifySearchResult.Tracks.Items;
							this.NowPlayingText.Text = "Found " + spotifySearchResult.Tracks.Items.Count + " tracks - Tap to play!";
						}
						else
						{
							this.NowPlayingText.Text = "No results found";
							this.ResultsListBox.ItemsSource = null;
						}
					}
					catch (Exception ex)
					{
						this.NowPlayingText.Text = "Error reading results";
						MessageBox.Show("Error: " + ex.Message);
					}
				}
				else
				{
					this.NowPlayingText.Text = "Search failed";
					this.HandleApiError(e.Error);
				}
			});
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00009124 File Offset: 0x00007324
		private void ResultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Track track = this.ResultsListBox.SelectedItem as Track;
			bool flag = track != null;
			if (flag)
			{
				this._currentTrack = track;
				this._currentTrackUri = track.Uri;
				string text = "Unknown Artist";
				bool flag2 = track.Artists != null && track.Artists.Count > 0;
				if (flag2)
				{
					text = track.Artists[0].Name;
				}
				this.NowPlayingText.Text = "Selected: " + track.Name + " by " + text;
				this.NowPlayingTitle.Text = track.Name;
				this.NowPlayingArtist.Text = text;
				bool flag3 = track.Album != null && track.Album.Images != null && track.Album.Images.Count > 0;
				if (flag3)
				{
					try
					{
						BitmapImage bitmapImage = new BitmapImage();
						bitmapImage.UriSource = new Uri(track.Album.Images[0].Url);
						this.AlbumImage.Source = bitmapImage;
					}
					catch
					{
					}
				}
				this.YouTubeStatusText.Visibility = 0;
				this.YouTubeStatusText.Text = "YouTube audio available";
				this.PlayTrackViaYouTube(this._currentTrack);
				this.PlayButton.Content = "Play Spotify";
				this.PlayPauseButton.Content = "❚❚";
				this.PlayYouTubeButton.Content = "Play on Phone";
				this._isPlaying = false;
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000092C4 File Offset: 0x000074C4
		private void WireAudioPlayerEvents()
		{
			this._audioPlayer.MediaOpened += delegate(object s, RoutedEventArgs e)
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.YouTubeStatusText.Text = "Audio loaded";
				});
			};
			this._audioPlayer.MediaFailed += delegate(object s, ExceptionRoutedEventArgs e)
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.YouTubeStatusText.Text = "Playback failed";
					this._isPlaying = false;
					this.PlayButton.Content = "Play";
				});
			};
			this._audioPlayer.MediaEnded += delegate(object s, RoutedEventArgs e)
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.YouTubeStatusText.Text = "Playback finished";
					this._isPlaying = false;
					this.PlayButton.Content = "Play";
					this.StopProgressSimulation();
				});
			};
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000931C File Offset: 0x0000751C
		private void PlaySelectedTrack()
		{
			bool flag = string.IsNullOrEmpty(this._accessToken);
			if (flag)
			{
				MessageBox.Show("Please login first");
			}
			else
			{
				bool flag2 = this._currentTrackUri == null;
				if (flag2)
				{
					MessageBox.Show("No track selected");
				}
				else
				{
					this.EnsureDeviceActive(delegate
					{
						this.StartPlaybackOnPhone();
					});
				}
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00009378 File Offset: 0x00007578
		private void TransferPlaybackToPhone()
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.Headers[24] = "Bearer " + this._accessToken;
				webClient.Headers[12] = "application/json";
				string text = "{\"device_ids\": [\"\"], \"play\": false}";
				webClient.UploadStringCompleted += delegate(object sender, UploadStringCompletedEventArgs e)
				{
					bool flag = e.Error == null;
					if (flag)
					{
						this.StartPlaybackOnPhone();
					}
					else
					{
						this.StartPlaybackOnPhone();
					}
				};
				webClient.UploadStringAsync(new Uri("https://api.spotify.com/v1/me/player"), "PUT", text);
			}
			catch (Exception)
			{
				this.StartPlaybackOnPhone();
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00009410 File Offset: 0x00007610
		private void StartPlaybackOnPhone()
		{
			try
			{
				this.NowPlayingText.Text = "Starting playback on Windows Phone...";
				WebClient webClient = new WebClient();
				webClient.Headers[24] = "Bearer " + this._accessToken;
				webClient.Headers[12] = "application/json";
				string text = "{\"uris\": [\"" + this._currentTrackUri + "\"]}";
				webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(this.Client_PlaySelectedCompleted);
				webClient.UploadStringAsync(new Uri("https://api.spotify.com/v1/me/player/play"), "PUT", text);
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				Deployment.Current.Dispatcher.BeginInvoke(delegate()
				{
					this.NowPlayingText.Text = "Play error";
					MessageBox.Show("Play error: " + ex.Message);
				});
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000094F0 File Offset: 0x000076F0
		private void CheckAndPlay()
		{
			try
			{
				this.NowPlayingText.Text = "Checking for active devices...";
				WebClient webClient = new WebClient();
				webClient.Headers[24] = "Bearer " + this._accessToken;
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
				{
					bool flag = e.Error == null;
					if (flag)
					{
						this.StartPlayback();
					}
					else
					{
						Deployment.Current.Dispatcher.BeginInvoke(delegate()
						{
							MessageBox.Show("No active Spotify devices found.\n\nPlease:\n1. Open Spotify on your phone/computer\n2. Ensure you're logged in\n3. Try playing music through Spotify first\n4. Then return to this app");
							this.NowPlayingText.Text = "No active device found";
						});
					}
				};
				webClient.DownloadStringAsync(new Uri("https://api.spotify.com/v1/me/player/devices"));
			}
			catch (Exception ex)
			{
				this.NowPlayingText.Text = "Device check error";
				MessageBox.Show("Device check error: " + ex.Message);
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00009598 File Offset: 0x00007798
		private void EnsureDeviceActive(Action onDeviceReady)
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.Headers[24] = "Bearer " + this._accessToken;
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
				{
					bool flag = e.Error == null;
					if (flag)
					{
						onDeviceReady.Invoke();
					}
					else
					{
						onDeviceReady.Invoke();
					}
				};
				webClient.DownloadStringAsync(new Uri("https://api.spotify.com/v1/me/player"));
			}
			catch (Exception ex)
			{
				MessageBox.Show("Device activation error: " + ex.Message);
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000962C File Offset: 0x0000782C
		private void Client_PlaySelectedCompleted(object sender, UploadStringCompletedEventArgs e)
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				bool flag = e.Error == null;
				if (flag)
				{
					this.PlayButton.Content = "Pause";
					this._isPlaying = true;
					string text = "Unknown Artist";
					bool flag2 = this._currentTrack.Artists != null && this._currentTrack.Artists.Count > 0;
					if (flag2)
					{
						text = this._currentTrack.Artists[0].Name;
					}
					this.NowPlayingText.Text = "Playing: " + this._currentTrack.Name + " by " + text;
					this.StartProgressSimulation();
				}
				else
				{
					this.NowPlayingText.Text = "Playback failed";
					this.HandlePlaybackError(e.Error);
				}
			});
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000966C File Offset: 0x0000786C
		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this._currentTrackUri == null;
			if (flag)
			{
				MessageBox.Show("Please select a track first");
			}
			else
			{
				bool isPlaying = this._isPlaying;
				if (isPlaying)
				{
					this.PauseMusic();
				}
				else
				{
					this.PlayMusic();
				}
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000096B4 File Offset: 0x000078B4
		private void PlayMusic()
		{
			try
			{
				this.NowPlayingText.Text = "Starting playback...";
				WebClient webClient = new WebClient();
				webClient.Headers[24] = "Bearer " + this._accessToken;
				webClient.Headers[12] = "application/json";
				string text = "{\"uris\": [\"" + this._currentTrackUri + "\"]}";
				webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(this.Client_PlayCompleted);
				webClient.UploadStringAsync(new Uri("https://api.spotify.com/v1/me/player/play"), "PUT", text);
			}
			catch (Exception ex)
			{
				this.NowPlayingText.Text = "Play error";
				MessageBox.Show("Play error: " + ex.Message);
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000978C File Offset: 0x0000798C
		private void Client_PlayCompleted(object sender, UploadStringCompletedEventArgs e)
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				bool flag = e.Error == null;
				if (flag)
				{
					this.PlayButton.Content = "Pause";
					this._isPlaying = true;
					this.NowPlayingText.Text = "Now playing: " + this._currentTrack.Name;
					this.StartProgressSimulation();
				}
				else
				{
					this.NowPlayingText.Text = "Playback failed";
					this.HandlePlaybackError(e.Error);
				}
			});
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000097CC File Offset: 0x000079CC
		private void PauseMusic()
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.Headers[24] = "Bearer " + this._accessToken;
				webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(this.Client_PauseCompleted);
				webClient.UploadStringAsync(new Uri("https://api.spotify.com/v1/me/player/pause"), "PUT", "");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Pause error: " + ex.Message);
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000985C File Offset: 0x00007A5C
		private void Client_PauseCompleted(object sender, UploadStringCompletedEventArgs e)
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate()
			{
				bool flag = e.Error == null;
				if (flag)
				{
					this.PlayButton.Content = "Play";
					this._isPlaying = false;
					this.NowPlayingText.Text = "Paused: " + this._currentTrack.Name;
					this.StopProgressSimulation();
				}
				else
				{
					MessageBox.Show("Pause failed: " + e.Error.Message);
				}
			});
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000989C File Offset: 0x00007A9C
		private void StartProgressSimulation()
		{
			this.StopProgressSimulation();
			this._progressTimer = new DispatcherTimer();
			this._progressTimer.Interval = TimeSpan.FromMilliseconds(500.0);
			this._progressTimer.Start();
			this.ProgressBar.Value = 0.0;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000098F8 File Offset: 0x00007AF8
		private void StopProgressSimulation()
		{
			bool flag = this._progressTimer != null;
			if (flag)
			{
				this._progressTimer.Stop();
				this._progressTimer = null;
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00009928 File Offset: 0x00007B28
		private void HandleApiError(Exception error)
		{
			bool flag = error is WebException;
			if (flag)
			{
				WebException ex = (WebException)error;
				bool flag2 = ex.Response is HttpWebResponse;
				if (flag2)
				{
					HttpWebResponse httpWebResponse = (HttpWebResponse)ex.Response;
					bool flag3 = httpWebResponse.StatusCode == 401;
					if (flag3)
					{
						MessageBox.Show("Session expired. Please login again.");
						this.Logout();
					}
					else
					{
						MessageBox.Show("API error: " + httpWebResponse.StatusCode);
					}
				}
				else
				{
					MessageBox.Show("Network error: " + ex.Message);
				}
			}
			else
			{
				MessageBox.Show("Error: " + error.Message);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000099EC File Offset: 0x00007BEC
		private void PlayOnPhone_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this._currentTrackUri == null;
			if (flag)
			{
				MessageBox.Show("Please select a track first");
			}
			else
			{
				this.TransferPlaybackToPhone();
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00009A1C File Offset: 0x00007C1C
		private void HandlePlaybackError(Exception error)
		{
			string text = "Playback failed. ";
			bool flag = error is WebException;
			if (flag)
			{
				WebException ex = (WebException)error;
				bool flag2 = ex.Response is HttpWebResponse;
				if (flag2)
				{
					HttpWebResponse httpWebResponse = (HttpWebResponse)ex.Response;
					bool flag3 = httpWebResponse.StatusCode == 404;
					if (flag3)
					{
						text += "Make sure:\n• You have Spotify Premium\n• Spotify is open on another device";
					}
					else
					{
						bool flag4 = httpWebResponse.StatusCode == 401;
						if (flag4)
						{
							text += "Session expired. Please login again.";
							this.Logout();
						}
						else
						{
							text = text + "Error: " + httpWebResponse.StatusCode;
						}
					}
				}
				else
				{
					text += error.Message;
				}
			}
			else
			{
				text += error.Message;
			}
			MessageBox.Show(text);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00009AFC File Offset: 0x00007CFC
		private void Logout()
		{
			bool flag = IsolatedStorageSettings.ApplicationSettings.Contains("SpotifyAccessToken");
			if (flag)
			{
				IsolatedStorageSettings.ApplicationSettings.Remove("SpotifyAccessToken");
				IsolatedStorageSettings.ApplicationSettings.Remove("SpotifyRefreshToken");
				IsolatedStorageSettings.ApplicationSettings.Save();
			}
			this._accessToken = null;
			this._refreshToken = null;
			this._currentTrackUri = null;
			this._isPlaying = false;
			this.StopProgressSimulation();
			this.ProgressBar.Value = 0.0;
			this.AlbumImage.Source = null;
			this.ResultsListBox.ItemsSource = null;
			this.ShowLoginOptions();
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00009BA3 File Offset: 0x00007DA3
		private void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			this.Logout();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00009BB0 File Offset: 0x00007DB0
		private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			bool flag = this.SearchTextBox.Text == "Search for tracks...";
			if (flag)
			{
				this.SearchTextBox.Text = "";
				this.SearchTextBox.Foreground = new SolidColorBrush(Colors.White);
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00009C00 File Offset: 0x00007E00
		private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			bool flag = string.IsNullOrWhiteSpace(this.SearchTextBox.Text);
			if (flag)
			{
				this.SearchTextBox.Text = "Search for tracks...";
				this.SearchTextBox.Foreground = new SolidColorBrush(Colors.Gray);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00009C4B File Offset: 0x00007E4B
		private void NextButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendPlayerCommand("https://api.spotify.com/v1/me/player/next", "POST");
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00009C5F File Offset: 0x00007E5F
		private void PrevButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendPlayerCommand("https://api.spotify.com/v1/me/player/previous", "POST");
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00009C74 File Offset: 0x00007E74
		private void SendPlayerCommand(string url, string method)
		{
			bool flag = string.IsNullOrEmpty(this._accessToken);
			if (flag)
			{
				MessageBox.Show("Please login first");
			}
			else
			{
				try
				{
					WebClient webClient = new WebClient();
					webClient.Headers[24] = "Bearer " + this._accessToken;
					webClient.UploadStringAsync(new Uri(url), method, "");
				}
				catch (Exception ex)
				{
					MessageBox.Show("Command failed: " + ex.Message);
				}
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00009D08 File Offset: 0x00007F08
		private void InitializePlayerControls()
		{
			this._progressTimer = new DispatcherTimer();
			this._progressTimer.Interval = TimeSpan.FromMilliseconds(500.0);
			this._progressTimer.Tick += new EventHandler(this.ProgressTimer_Tick);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00009D48 File Offset: 0x00007F48
		private void ProgressTimer_Tick(object sender, EventArgs e)
		{
			bool flag = this.AudioPlayer.NaturalDuration.HasTimeSpan && this._isPlaying;
			if (flag)
			{
				double value = this.AudioPlayer.Position.TotalSeconds / this.AudioPlayer.NaturalDuration.TimeSpan.TotalSeconds * 100.0;
				this.ProgressBar.Value = value;
				this.CurrentTimeText.Text = this.FormatTime(this.AudioPlayer.Position);
				this.TotalTimeText.Text = this.FormatTime(this.AudioPlayer.NaturalDuration.TimeSpan);
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00009E08 File Offset: 0x00008008
		private string FormatTime(TimeSpan time)
		{
			return string.Format("{0}:{1:00}", (int)time.TotalMinutes, time.Seconds);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00009E40 File Offset: 0x00008040
		private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
		{
			bool isPlaying = this._isPlaying;
			if (isPlaying)
			{
				this.ShowPauseConfirmation();
			}
			else
			{
				this.StartPlayback();
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00009E6C File Offset: 0x0000806C
		private void OverlayPlayButton_Click(object sender, RoutedEventArgs e)
		{
			this.PlayPauseButton_Click(sender, e);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00009E78 File Offset: 0x00008078
		private void ShowPauseConfirmation()
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("This audio stream will be reset and cannot be resumed. Continue?", "Confirm Pause", 1);
			bool flag = messageBoxResult == 1;
			if (flag)
			{
				this.PausePlayback();
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00009EA8 File Offset: 0x000080A8
		private void StartPlayback()
		{
			bool flag = this._currentTrack == null;
			if (!flag)
			{
				string searchQuery = string.Format("{0} {1}", this._currentTrack.Name, this._currentTrack.Artists[0].Name);
				this.SetPlaybackState(true, false);
				this.YouTubeStatusText.Text = "Searching for audio...";
				this.YouTubeStatusBorder.Background = new SolidColorBrush(Color.FromArgb(32, byte.MaxValue, 109, 0));
				this._youTubeService.GetAudioStreamUrl(searchQuery, delegate(string audioUrl)
				{
					base.Dispatcher.BeginInvoke(delegate()
					{
						try
						{
							this.AudioPlayer.Source = new Uri(audioUrl);
							this.YouTubeStatusText.Text = "Loading audio stream...";
							this._isPlaying = true;
							this.UpdatePlayButton();
						}
						catch (Exception ex)
						{
							this.YouTubeStatusText.Text = "Error: " + ex.Message;
							this.SetPlaybackState(false, false);
						}
					});
				}, delegate(string error)
				{
					base.Dispatcher.BeginInvoke(delegate()
					{
						this.YouTubeStatusText.Text = "Error: " + error;
						this.SetPlaybackState(false, false);
					});
				});
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00009F54 File Offset: 0x00008154
		private void PausePlayback()
		{
			this.AudioPlayer.Stop();
			this._isPlaying = false;
			this._progressTimer.Stop();
			this.UpdatePlayButton();
			this.YouTubeStatusText.Text = "Paused";
			this.YouTubeStatusBorder.Background = new SolidColorBrush(Color.FromArgb(32, 102, 102, 102));
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00009FB8 File Offset: 0x000081B8
		private void SetPlaybackState(bool isLoading, bool isPlaying)
		{
			this.OverlayLoadingSpinner.Visibility = (isLoading ? 0 : 1);
			this.OverlayPlayButton.Visibility = ((!isLoading) ? 0 : 1);
			bool flag = !isLoading;
			if (flag)
			{
				this._isPlaying = isPlaying;
				this.UpdatePlayButton();
				if (isPlaying)
				{
					this._progressTimer.Start();
				}
				else
				{
					this._progressTimer.Stop();
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000A028 File Offset: 0x00008228
		private void UpdatePlayButton()
		{
			this.PlayPauseButton.Content = (this._isPlaying ? "❚❚" : "►");
			this.OverlayPlayButton.Content = (this._isPlaying ? "❚❚" : "►");
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000A078 File Offset: 0x00008278
		private void UpdateNowPlayingInfo()
		{
			bool flag = this._currentTrack != null;
			if (flag)
			{
				this.NowPlayingTitle.Text = this._currentTrack.Name;
				this.NowPlayingArtist.Text = this._currentTrack.Artists[0].Name;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000A0D0 File Offset: 0x000082D0
		private void ShuffleButton_Click(object sender, RoutedEventArgs e)
		{
			this._isShuffled = !this._isShuffled;
			this.ShuffleButton.Background = (this._isShuffled ? new SolidColorBrush(Color.FromArgb(64, 29, 185, 84)) : new SolidColorBrush(Colors.Transparent));
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000A124 File Offset: 0x00008324
		private void RepeatButton_Click(object sender, RoutedEventArgs e)
		{
			this._repeatMode = (this._repeatMode + 1) % 3;
			this.RepeatButton.Content = ((this._repeatMode == 0) ? "\ud83d\udd01" : ((this._repeatMode == 1) ? "\ud83d\udd02" : "❶"));
			Color color = (this._repeatMode == 0) ? Colors.Transparent : ((this._repeatMode == 1) ? Color.FromArgb(64, 29, 185, 84) : Color.FromArgb(64, byte.MaxValue, 109, 0));
			this.RepeatButton.Background = new SolidColorBrush(color);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000A1C0 File Offset: 0x000083C0
		private void PlaybackSpeedButton_Click(object sender, RoutedEventArgs e)
		{
			double[] array = new double[]
			{
				0.5,
				0.75,
				1.0,
				1.25,
				1.5,
				2.0
			};
			this._playbackSpeed = array[(Array.IndexOf<double>(array, this._playbackSpeed) + 1) % array.Length];
			this.PlaybackSpeedButton.Content = string.Format("{0}×", this._playbackSpeed);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000A21B File Offset: 0x0000841B
		private void VolumeButton_Click(object sender, RoutedEventArgs e)
		{
			this.AudioPlayer.Volume = ((this.AudioPlayer.Volume == 1.0) ? 0.5 : 1.0);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000A254 File Offset: 0x00008454
		private void DownloadButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Download feature would be implemented here");
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000A262 File Offset: 0x00008462
		private void AddToPlaylistButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Add to playlist feature would be implemented here");
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000A270 File Offset: 0x00008470
		private void ShareTrackButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Share track feature would be implemented here");
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000A27E File Offset: 0x0000847E
		private void ViewLyricsButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Lyrics view would be implemented here");
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000A28C File Offset: 0x0000848C
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/SpotifyPage.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.NowPlayingText = (TextBlock)base.FindName("NowPlayingText");
				this.CredentialsPanel = (StackPanel)base.FindName("CredentialsPanel");
				this.ClientIdTextBox = (TextBox)base.FindName("ClientIdTextBox");
				this.ClientSecretTextBox = (TextBox)base.FindName("ClientSecretTextBox");
				this.RedirectUriTextBox = (TextBox)base.FindName("RedirectUriTextBox");
				this.YouTubeApiKeyTextBox = (TextBox)base.FindName("YouTubeApiKeyTextBox");
				this.SaveCredentialsButton = (Button)base.FindName("SaveCredentialsButton");
				this.HowToGetCredentialsButton = (Button)base.FindName("HowToGetCredentialsButton");
				this.LoginPanel = (StackPanel)base.FindName("LoginPanel");
				this.HowToGetYouTubeApiKeyButton = (Button)base.FindName("HowToGetYouTubeApiKeyButton");
				this.CodeEntryPanel = (StackPanel)base.FindName("CodeEntryPanel");
				this.CodeTextBox = (TextBox)base.FindName("CodeTextBox");
				this.MainAppPanel = (StackPanel)base.FindName("MainAppPanel");
				this.AlbumImage = (Image)base.FindName("AlbumImage");
				this.OverlayPlayButton = (Button)base.FindName("OverlayPlayButton");
				this.OverlayLoadingSpinner = (ProgressBar)base.FindName("OverlayLoadingSpinner");
				this.NowPlayingTitle = (TextBlock)base.FindName("NowPlayingTitle");
				this.NowPlayingArtist = (TextBlock)base.FindName("NowPlayingArtist");
				this.YouTubeStatusBorder = (Border)base.FindName("YouTubeStatusBorder");
				this.CurrentTimeText = (TextBlock)base.FindName("CurrentTimeText");
				this.ProgressBar = (ProgressBar)base.FindName("ProgressBar");
				this.TotalTimeText = (TextBlock)base.FindName("TotalTimeText");
				this.ShuffleButton = (Button)base.FindName("ShuffleButton");
				this.PlayPauseButton = (Button)base.FindName("PlayPauseButton");
				this.RepeatButton = (Button)base.FindName("RepeatButton");
				this.PlaybackSpeedButton = (Button)base.FindName("PlaybackSpeedButton");
				this.SearchTextBox = (TextBox)base.FindName("SearchTextBox");
				this.SearchButton = (Button)base.FindName("SearchButton");
				this.LoadingProgressBar = (ProgressBar)base.FindName("LoadingProgressBar");
				this.ResultsListBox = (ListBox)base.FindName("ResultsListBox");
				this.YouTubeStatusText = (TextBox)base.FindName("YouTubeStatusText");
				this.HELP = (TextBlock)base.FindName("HELP");
				this.PlayButton = (Button)base.FindName("PlayButton");
				this.PlayYouTubeButton = (Button)base.FindName("PlayYouTubeButton");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x0400009B RID: 155
		private string _spotifyClientId;

		// Token: 0x0400009C RID: 156
		private string _spotifyClientSecret;

		// Token: 0x0400009D RID: 157
		private string _spotifyRedirectUri;

		// Token: 0x0400009E RID: 158
		private string _youTubeApiKey;

		// Token: 0x0400009F RID: 159
		private string _accessToken;

		// Token: 0x040000A0 RID: 160
		private string _refreshToken;

		// Token: 0x040000A1 RID: 161
		private bool _isPlaying = false;

		// Token: 0x040000A2 RID: 162
		private string _currentTrackUri;

		// Token: 0x040000A3 RID: 163
		private Track _currentTrack;

		// Token: 0x040000A4 RID: 164
		private DispatcherTimer _progressTimer;

		// Token: 0x040000A5 RID: 165
		private YouTubeAudioService _youTubeService;

		// Token: 0x040000A6 RID: 166
		private MediaElement _audioPlayer;

		// Token: 0x040000A7 RID: 167
		private bool _isShuffled = false;

		// Token: 0x040000A8 RID: 168
		private int _repeatMode = 0;

		// Token: 0x040000A9 RID: 169
		private double _playbackSpeed = 1.0;

		// Token: 0x040000AA RID: 170
		internal Grid LayoutRoot;

		// Token: 0x040000AB RID: 171
		internal TextBlock NowPlayingText;

		// Token: 0x040000AC RID: 172
		internal StackPanel CredentialsPanel;

		// Token: 0x040000AD RID: 173
		internal TextBox ClientIdTextBox;

		// Token: 0x040000AE RID: 174
		internal TextBox ClientSecretTextBox;

		// Token: 0x040000AF RID: 175
		internal TextBox RedirectUriTextBox;

		// Token: 0x040000B0 RID: 176
		internal TextBox YouTubeApiKeyTextBox;

		// Token: 0x040000B1 RID: 177
		internal Button SaveCredentialsButton;

		// Token: 0x040000B2 RID: 178
		internal Button HowToGetCredentialsButton;

		// Token: 0x040000B3 RID: 179
		internal StackPanel LoginPanel;

		// Token: 0x040000B4 RID: 180
		internal Button HowToGetYouTubeApiKeyButton;

		// Token: 0x040000B5 RID: 181
		internal StackPanel CodeEntryPanel;

		// Token: 0x040000B6 RID: 182
		internal TextBox CodeTextBox;

		// Token: 0x040000B7 RID: 183
		internal StackPanel MainAppPanel;

		// Token: 0x040000B8 RID: 184
		internal Image AlbumImage;

		// Token: 0x040000B9 RID: 185
		internal Button OverlayPlayButton;

		// Token: 0x040000BA RID: 186
		internal ProgressBar OverlayLoadingSpinner;

		// Token: 0x040000BB RID: 187
		internal TextBlock NowPlayingTitle;

		// Token: 0x040000BC RID: 188
		internal TextBlock NowPlayingArtist;

		// Token: 0x040000BD RID: 189
		internal Border YouTubeStatusBorder;

		// Token: 0x040000BE RID: 190
		internal TextBlock CurrentTimeText;

		// Token: 0x040000BF RID: 191
		internal ProgressBar ProgressBar;

		// Token: 0x040000C0 RID: 192
		internal TextBlock TotalTimeText;

		// Token: 0x040000C1 RID: 193
		internal Button ShuffleButton;

		// Token: 0x040000C2 RID: 194
		internal Button PlayPauseButton;

		// Token: 0x040000C3 RID: 195
		internal Button RepeatButton;

		// Token: 0x040000C4 RID: 196
		internal Button PlaybackSpeedButton;

		// Token: 0x040000C5 RID: 197
		internal TextBox SearchTextBox;

		// Token: 0x040000C6 RID: 198
		internal Button SearchButton;

		// Token: 0x040000C7 RID: 199
		internal ProgressBar LoadingProgressBar;

		// Token: 0x040000C8 RID: 200
		internal ListBox ResultsListBox;

		// Token: 0x040000C9 RID: 201
		internal TextBox YouTubeStatusText;

		// Token: 0x040000CA RID: 202
		internal TextBlock HELP;

		// Token: 0x040000CB RID: 203
		internal Button PlayButton;

		// Token: 0x040000CC RID: 204
		internal Button PlayYouTubeButton;

		// Token: 0x040000CD RID: 205
		internal MediaElement AudioPlayer;

		// Token: 0x040000CE RID: 206
		private bool _contentLoaded;
	}
}
