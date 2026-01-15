using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using MiniJSON;

namespace OpenTracksBETA
{
	// Token: 0x0200001F RID: 31
	public class YTMusic : PhoneApplicationPage
	{
		// Token: 0x06000183 RID: 387 RVA: 0x0000AF0A File Offset: 0x0000910A
		public YTMusic()
		{
			this.InitializeComponent();
			this.InitializeMediaPlayer();
			this.InitializeProgressTimer();
			this.UpdateStatus("Ready to play music");
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000AF3C File Offset: 0x0000913C
		private void InitializeMediaPlayer()
		{
			this.HiddenVideoPlayer.MediaOpened += new RoutedEventHandler(this.HiddenVideoPlayer_MediaOpened);
			this.HiddenVideoPlayer.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(this.HiddenVideoPlayer_MediaFailed);
			this.HiddenVideoPlayer.MediaEnded += new RoutedEventHandler(this.HiddenVideoPlayer_MediaEnded);
			this.HiddenVideoPlayer.CurrentStateChanged += new RoutedEventHandler(this.HiddenVideoPlayer_CurrentStateChanged);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000AFAA File Offset: 0x000091AA
		private void InitializeProgressTimer()
		{
			this.progressTimer = new DispatcherTimer();
			this.progressTimer.Interval = TimeSpan.FromMilliseconds(1000.0);
			this.progressTimer.Tick += new EventHandler(this.ProgressTimer_Tick);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000AFEA File Offset: 0x000091EA
		private void ProgressTimer_Tick(object sender, EventArgs e)
		{
			this.UpdateMiniPlayerStatus();
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000AFF4 File Offset: 0x000091F4
		private void UpdateMiniPlayerStatus()
		{
			bool flag = this.HiddenVideoPlayer.NaturalDuration.HasTimeSpan && this.HiddenVideoPlayer.CurrentState == 3;
			if (flag)
			{
				try
				{
					TimeSpan position = this.HiddenVideoPlayer.Position;
					TimeSpan timeSpan = this.HiddenVideoPlayer.NaturalDuration.TimeSpan;
					bool flag2 = timeSpan.TotalSeconds > 0.0;
					if (flag2)
					{
						string text = this.FormatTimeSpan(position);
						string text2 = this.FormatTimeSpan(timeSpan);
						bool flag3 = (int)position.TotalSeconds % 30 == 0;
						if (flag3)
						{
							this.UpdateStatus("Playing: {currentTime} / {totalTime}");
						}
					}
				}
				catch (Exception ex)
				{
				}
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000B0B8 File Offset: 0x000092B8
		private string FormatTimeSpan(TimeSpan timeSpan)
		{
			return string.Format("{0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000B0EC File Offset: 0x000092EC
		private void LoadHomeVideos()
		{
			this.ShowLoading(true);
			this.UpdateStatus("Loading NCS music...");
			this.FetchVideosFromApi("http://yt.legacyprojects.ru/get_search_videos.php?query=ncs");
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000B110 File Offset: 0x00009310
		private void FetchVideosFromApi(string apiUrl)
		{
			this.UpdateStatus("Fetching music...");
			WebClient webClient = new WebClient();
			webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
			{
				bool flag = e.Error != null;
				if (flag)
				{
					base.Dispatcher.BeginInvoke(delegate()
					{
						this.ShowLoading(false);
						this.UpdateStatus("Error: " + e.Error.Message);
					});
				}
				else
				{
					try
					{
						List<YTMusic.YouTubeVideo> videos = this.ParseVideosFromJson(e.Result);
						base.Dispatcher.BeginInvoke(delegate()
						{
							this.VideosListBox.ItemsSource = videos;
							this.ShowLoading(false);
							bool flag2 = videos.Count > 0;
							if (flag2)
							{
								this.UpdateStatus("Found " + videos.Count + " tracks");
							}
							else
							{
								this.UpdateStatus("No music found");
							}
						});
					}
					catch (Exception ex)
					{
						base.Dispatcher.BeginInvoke(delegate()
						{
							this.ShowLoading(false);
							this.UpdateStatus("Error parsing results");
						});
					}
				}
			};
			webClient.DownloadStringAsync(new Uri(apiUrl));
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000B150 File Offset: 0x00009350
		private List<YTMusic.YouTubeVideo> ParseVideosFromJson(string json)
		{
			List<YTMusic.YouTubeVideo> list = new List<YTMusic.YouTubeVideo>();
			try
			{
				List<object> list2 = Json.Deserialize(json) as List<object>;
				bool flag = list2 == null;
				if (flag)
				{
					return list;
				}
				foreach (object obj in list2)
				{
					Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
					bool flag2 = dictionary == null;
					if (!flag2)
					{
						YTMusic.YouTubeVideo youTubeVideo = new YTMusic.YouTubeVideo();
						youTubeVideo.VideoId = this.GetStringValue(dictionary, "video_id");
						youTubeVideo.Title = this.GetStringValue(dictionary, "title");
						youTubeVideo.Channel = this.GetStringValue(dictionary, "author");
						youTubeVideo.ThumbnailUrl = this.GetStringValue(dictionary, "thumbnail");
						youTubeVideo.Duration = "Unknown";
						bool flag3 = !string.IsNullOrEmpty(youTubeVideo.VideoId) && !string.IsNullOrEmpty(youTubeVideo.Title);
						if (flag3)
						{
							list.Add(youTubeVideo);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Parse error: " + ex.Message);
			}
			return list;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000B2C4 File Offset: 0x000094C4
		private void PlayVideo(YTMusic.YouTubeVideo video)
		{
			try
			{
				bool flag = this.HiddenVideoPlayer.CurrentState == 3;
				if (flag)
				{
					this.HiddenVideoPlayer.Stop();
				}
				this.currentVideo = video;
				string text = string.Format("http://yt.legacyprojects.ru/direct_url?video_id={0}", video.VideoId);
				this.UpdateStatus("Loading: " + video.Title);
				this.HiddenVideoPlayer.Source = new Uri(text, 1);
				this.MiniPlayerTitle.Text = video.Title;
				this.MiniPlayerChannel.Text = video.Channel;
				this.MiniPlayerBar.Visibility = 0;
				this.MiniPlayPauseButton.Content = "❚❚";
				this.isPlaying = true;
				this.progressTimer.Start();
			}
			catch (Exception ex)
			{
				this.UpdateStatus("Error: " + ex.Message);
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000B3BC File Offset: 0x000095BC
		private void HiddenVideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(delegate()
			{
				this.UpdateStatus("Now playing: " + this.currentVideo.Title);
			});
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000B3D7 File Offset: 0x000095D7
		private void HiddenVideoPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(delegate()
			{
				this.UpdateStatus("Playback failed");
				this.progressTimer.Stop();
				this.isPlaying = false;
				this.MiniPlayPauseButton.Content = "▶";
			});
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000B3F2 File Offset: 0x000095F2
		private void HiddenVideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(delegate()
			{
				this.UpdateStatus("Track finished");
				this.progressTimer.Stop();
				this.isPlaying = false;
				this.MiniPlayPauseButton.Content = "▶";
			});
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000B40D File Offset: 0x0000960D
		private void HiddenVideoPlayer_CurrentStateChanged(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(delegate()
			{
				bool flag = this.HiddenVideoPlayer.CurrentState == 3;
				if (flag)
				{
					this.isPlaying = true;
					this.MiniPlayPauseButton.Content = "❚❚";
				}
				else
				{
					bool flag2 = this.HiddenVideoPlayer.CurrentState == 4 || this.HiddenVideoPlayer.CurrentState == 5;
					if (flag2)
					{
						this.isPlaying = false;
						this.MiniPlayPauseButton.Content = "▶";
					}
				}
			});
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000B428 File Offset: 0x00009628
		private string GetStringValue(Dictionary<string, object> dict, string key)
		{
			bool flag = dict.ContainsKey(key) && dict[key] != null;
			string result;
			if (flag)
			{
				result = dict[key].ToString();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000B465 File Offset: 0x00009665
		private void ShowLoading(bool show)
		{
			this.LoadingPanel.Visibility = (show ? 0 : 1);
			this.VideosListBox.Visibility = (show ? 1 : 0);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000B490 File Offset: 0x00009690
		private void UpdateStatus(string status)
		{
			base.Dispatcher.BeginInvoke(delegate()
			{
				this.StatusText.Text = status;
				Debug.WriteLine("YTMusic: " + status);
			});
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000B4CC File Offset: 0x000096CC
		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string text = this.SearchTextBox.Text.Trim();
			this.SearchHintText.Visibility = (string.IsNullOrEmpty(text) ? 0 : 1);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000B504 File Offset: 0x00009704
		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			string text = this.SearchTextBox.Text.Trim();
			bool flag = !string.IsNullOrEmpty(text);
			if (flag)
			{
				this.PerformSearch(text);
			}
			else
			{
				this.LoadHomeVideos();
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000B548 File Offset: 0x00009748
		private void PerformSearch(string query)
		{
			this.ShowLoading(true);
			this.UpdateStatus("Searching: " + query);
			string apiUrl = "http://yt.legacyprojects.ru/get_search_videos.php?query=" + Uri.EscapeDataString(query);
			this.FetchVideosFromApi(apiUrl);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000B58C File Offset: 0x0000978C
		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			bool flag = button != null && button.Tag is YTMusic.YouTubeVideo;
			if (flag)
			{
				YTMusic.YouTubeVideo video = button.Tag as YTMusic.YouTubeVideo;
				this.PlayVideo(video);
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000B5D0 File Offset: 0x000097D0
		private void VideosListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.VideosListBox.SelectedItem is YTMusic.YouTubeVideo;
			if (flag)
			{
				YTMusic.YouTubeVideo video = this.VideosListBox.SelectedItem as YTMusic.YouTubeVideo;
				this.PlayVideo(video);
				this.VideosListBox.SelectedIndex = -1;
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000B620 File Offset: 0x00009820
		private void MiniPlayPauseButton_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.HiddenVideoPlayer != null && this.currentVideo != null;
			if (flag)
			{
				bool flag2 = this.isPlaying;
				if (flag2)
				{
					this.HiddenVideoPlayer.Pause();
					this.MiniPlayPauseButton.Content = "▶";
					this.isPlaying = false;
					this.UpdateStatus("Paused: " + this.currentVideo.Title);
				}
				else
				{
					this.HiddenVideoPlayer.Play();
					this.MiniPlayPauseButton.Content = "❚❚";
					this.isPlaying = true;
					this.UpdateStatus("Playing: " + this.currentVideo.Title);
				}
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000B6DC File Offset: 0x000098DC
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			bool flag = this.progressTimer != null;
			if (flag)
			{
				this.progressTimer.Stop();
			}
			base.OnNavigatedFrom(e);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000B710 File Offset: 0x00009910
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/YTMusic.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.TitlePanel = (StackPanel)base.FindName("TitlePanel");
				this.ApplicationTitle = (TextBlock)base.FindName("ApplicationTitle");
				this.PageTitle = (TextBlock)base.FindName("PageTitle");
				this.ContentPanel = (Grid)base.FindName("ContentPanel");
				this.SearchTextBox = (TextBox)base.FindName("SearchTextBox");
				this.SearchHintText = (TextBlock)base.FindName("SearchHintText");
				this.SearchButton = (Button)base.FindName("SearchButton");
				this.StatusText = (TextBlock)base.FindName("StatusText");
				this.LoadingPanel = (StackPanel)base.FindName("LoadingPanel");
				this.VideosListBox = (ListBox)base.FindName("VideosListBox");
				this.MiniPlayerBar = (Border)base.FindName("MiniPlayerBar");
				this.MiniPlayerTitle = (TextBlock)base.FindName("MiniPlayerTitle");
				this.MiniPlayerChannel = (TextBlock)base.FindName("MiniPlayerChannel");
				this.MiniPlayPauseButton = (Button)base.FindName("MiniPlayPauseButton");
				this.HiddenVideoPlayer = (MediaElement)base.FindName("HiddenVideoPlayer");
			}
		}

		// Token: 0x040000EB RID: 235
		private const string SEARCH_API_URL = "http://yt.legacyprojects.ru/get_search_videos.php?query=";

		// Token: 0x040000EC RID: 236
		private const string VIDEO_URL = "http://yt.legacyprojects.ru/direct_url?video_id={0}";

		// Token: 0x040000ED RID: 237
		private const string HOME_API_URL = "http://yt.legacyprojects.ru/get_search_videos.php?query=ncs";

		// Token: 0x040000EE RID: 238
		private YTMusic.YouTubeVideo currentVideo;

		// Token: 0x040000EF RID: 239
		private bool isPlaying = false;

		// Token: 0x040000F0 RID: 240
		private DispatcherTimer progressTimer;

		// Token: 0x040000F1 RID: 241
		internal Grid LayoutRoot;

		// Token: 0x040000F2 RID: 242
		internal StackPanel TitlePanel;

		// Token: 0x040000F3 RID: 243
		internal TextBlock ApplicationTitle;

		// Token: 0x040000F4 RID: 244
		internal TextBlock PageTitle;

		// Token: 0x040000F5 RID: 245
		internal Grid ContentPanel;

		// Token: 0x040000F6 RID: 246
		internal TextBox SearchTextBox;

		// Token: 0x040000F7 RID: 247
		internal TextBlock SearchHintText;

		// Token: 0x040000F8 RID: 248
		internal Button SearchButton;

		// Token: 0x040000F9 RID: 249
		internal TextBlock StatusText;

		// Token: 0x040000FA RID: 250
		internal StackPanel LoadingPanel;

		// Token: 0x040000FB RID: 251
		internal ListBox VideosListBox;

		// Token: 0x040000FC RID: 252
		internal Border MiniPlayerBar;

		// Token: 0x040000FD RID: 253
		internal TextBlock MiniPlayerTitle;

		// Token: 0x040000FE RID: 254
		internal TextBlock MiniPlayerChannel;

		// Token: 0x040000FF RID: 255
		internal Button MiniPlayPauseButton;

		// Token: 0x04000100 RID: 256
		internal MediaElement HiddenVideoPlayer;

		// Token: 0x04000101 RID: 257
		private bool _contentLoaded;

		// Token: 0x02000066 RID: 102
		public class YouTubeVideo
		{
			// Token: 0x17000088 RID: 136
			// (get) Token: 0x060002EB RID: 747 RVA: 0x00012FCC File Offset: 0x000111CC
			// (set) Token: 0x060002EC RID: 748 RVA: 0x00012FD4 File Offset: 0x000111D4
			public string VideoId { get; set; }

			// Token: 0x17000089 RID: 137
			// (get) Token: 0x060002ED RID: 749 RVA: 0x00012FDD File Offset: 0x000111DD
			// (set) Token: 0x060002EE RID: 750 RVA: 0x00012FE5 File Offset: 0x000111E5
			public string Title { get; set; }

			// Token: 0x1700008A RID: 138
			// (get) Token: 0x060002EF RID: 751 RVA: 0x00012FEE File Offset: 0x000111EE
			// (set) Token: 0x060002F0 RID: 752 RVA: 0x00012FF6 File Offset: 0x000111F6
			public string Channel { get; set; }

			// Token: 0x1700008B RID: 139
			// (get) Token: 0x060002F1 RID: 753 RVA: 0x00012FFF File Offset: 0x000111FF
			// (set) Token: 0x060002F2 RID: 754 RVA: 0x00013007 File Offset: 0x00011207
			public string Duration { get; set; }

			// Token: 0x1700008C RID: 140
			// (get) Token: 0x060002F3 RID: 755 RVA: 0x00013010 File Offset: 0x00011210
			// (set) Token: 0x060002F4 RID: 756 RVA: 0x00013018 File Offset: 0x00011218
			public string ThumbnailUrl { get; set; }

			// Token: 0x1700008D RID: 141
			// (get) Token: 0x060002F5 RID: 757 RVA: 0x00013021 File Offset: 0x00011221
			// (set) Token: 0x060002F6 RID: 758 RVA: 0x00013029 File Offset: 0x00011229
			public string VideoUrl { get; set; }

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x060002F7 RID: 759 RVA: 0x00013034 File Offset: 0x00011234
			public BitmapImage Thumbnail
			{
				get
				{
					bool flag = !string.IsNullOrEmpty(this.ThumbnailUrl);
					if (flag)
					{
						try
						{
							return new BitmapImage(new Uri(this.ThumbnailUrl, 1));
						}
						catch
						{
							return new BitmapImage(new Uri("/Assets/placeholder.png", 2));
						}
					}
					return new BitmapImage(new Uri("/Assets/placeholder.png", 2));
				}
			}
		}
	}
}
