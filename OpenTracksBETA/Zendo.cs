using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;
using MiniJSON;

namespace OpenTracksBETA
{
	// Token: 0x02000020 RID: 32
	public class Zendo : PhoneApplicationPage
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x0000BA84 File Offset: 0x00009C84
		public Zendo()
		{
			this.InitializeComponent();
			BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(this.Instance_PlayStateChanged);
			this.progressTimer.Interval = TimeSpan.FromSeconds(1.0);
			this.progressTimer.Tick += new EventHandler(this.ProgressTimer_Tick);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000BAF4 File Offset: 0x00009CF4
		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			string text = this.SearchBox.Text;
			bool flag = !string.IsNullOrWhiteSpace(text);
			if (flag)
			{
				this.SearchSongsAsync(text);
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000BB28 File Offset: 0x00009D28
		private void SearchSongsAsync(string name)
		{
			string text = "http://api.zendomusic.ru/get-info/search-songs.php?name=" + Uri.EscapeDataString(name) + "&originalurl=true&originalicon=true";
			WebClient webClient = new WebClient();
			webClient.Headers["Accept"] = "application/json";
			webClient.DownloadStringCompleted += delegate(object s, DownloadStringCompletedEventArgs e)
			{
				bool flag = e.Error != null;
				if (!flag)
				{
					try
					{
						List<object> list = Json.Deserialize(e.Result) as List<object>;
						List<Track1> songs = new List<Track1>();
						foreach (object obj in list)
						{
							Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
							songs.Add(new Track1
							{
								Title = (dictionary["songname"] as string),
								Artist = (dictionary["author"] as string),
								Cover = (dictionary["icon"] as string),
								Mp3Url = (dictionary["url"] as string)
							});
						}
						base.Dispatcher.BeginInvoke(delegate()
						{
							this.ResultsList.ItemsSource = songs;
						});
					}
					catch
					{
					}
				}
			};
			webClient.DownloadStringAsync(new Uri(text));
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000BB88 File Offset: 0x00009D88
		private void PlayTrack(Track1 track)
		{
			bool flag = track == null || string.IsNullOrEmpty(track.Mp3Url);
			if (!flag)
			{
				Zendo.currentTrack = track;
				Uri albumArtUri = null;
				bool flag2 = !string.IsNullOrEmpty(track.Cover);
				if (flag2)
				{
					albumArtUri = new Uri(track.Cover, 1);
				}
				AudioTrack audioTrack = new AudioTrack(new Uri(track.Mp3Url, 1), track.Title, track.Artist, null, albumArtUri, "track_" + Guid.NewGuid().ToString(), 31L);
				base.Dispatcher.BeginInvoke(delegate()
				{
					this.MiniCover.Source = ((albumArtUri != null) ? new BitmapImage(albumArtUri) : null);
					this.CurrentTrackName.Text = track.Title;
					this.ProgressSlider.Value = 0.0;
					BackgroundAudioPlayer.Instance.Track = audioTrack;
				});
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000BC84 File Offset: 0x00009E84
		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			bool flag = button == null;
			if (!flag)
			{
				Track1 track = button.DataContext as Track1;
				this.PlayTrack(track);
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000BCB8 File Offset: 0x00009EB8
		private void RandomButton_Click(object sender, RoutedEventArgs e)
		{
			string text = "http://api.zendomusic.ru/get-info/random-song.php?pass=yourpass&token=yourtoken&originalurl=true&originalicon=true";
			WebClient webClient = new WebClient();
			webClient.Headers["Accept"] = "application/json";
			webClient.DownloadStringCompleted += delegate(object s, DownloadStringCompletedEventArgs ev)
			{
				bool flag = ev.Error != null;
				if (!flag)
				{
					try
					{
						List<object> list = Json.Deserialize(ev.Result) as List<object>;
						bool flag2 = list.Count > 0;
						if (flag2)
						{
							Dictionary<string, object> dictionary = list[0] as Dictionary<string, object>;
							Track1 track = new Track1
							{
								Title = (dictionary["songname"] as string),
								Artist = (dictionary["author"] as string),
								Cover = (dictionary["icon"] as string),
								Mp3Url = (dictionary["url"] as string)
							};
							base.Dispatcher.BeginInvoke(delegate()
							{
								this.RandomCover.Source = new BitmapImage(new Uri(track.Cover, 1));
								this.RandomTitle.Text = track.Title;
								this.RandomArtist.Text = track.Artist;
								this.RandomTrackPanel.Visibility = 0;
								this.PlayTrack(track);
								this.MarqueeStoryboard.Begin();
							});
						}
					}
					catch
					{
					}
				}
			};
			webClient.DownloadStringAsync(new Uri(text));
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000BD08 File Offset: 0x00009F08
		private void MiniPause_Click(object sender, RoutedEventArgs e)
		{
			bool flag = BackgroundAudioPlayer.Instance.PlayerState == 3;
			if (flag)
			{
				BackgroundAudioPlayer.Instance.Pause();
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000BD34 File Offset: 0x00009F34
		private void Instance_PlayStateChanged(object sender, EventArgs e)
		{
			switch (BackgroundAudioPlayer.Instance.PlayerState)
			{
			case 1:
			case 2:
			case 7:
				this.progressTimer.Stop();
				this.ProgressSlider.Value = 0.0;
				break;
			case 3:
				this.progressTimer.Start();
				break;
			case 6:
				BackgroundAudioPlayer.Instance.Play();
				break;
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000BDB4 File Offset: 0x00009FB4
		private void ResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Track1 selectedTrack = this.ResultsList.SelectedItem as Track1;
			bool flag = selectedTrack == null || string.IsNullOrEmpty(selectedTrack.Mp3Url);
			if (!flag)
			{
				base.Dispatcher.BeginInvoke(delegate()
				{
					this.MiniCover.Source = new BitmapImage(new Uri(selectedTrack.Cover, 1));
					this.CurrentTrackName.Text = selectedTrack.Title;
					this.ProgressSlider.Value = 0.0;
					this.PlayTrack(selectedTrack);
					this.MarqueeStoryboard.Begin();
					this.ResultsList.SelectedItem = null;
				});
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000BE20 File Offset: 0x0000A020
		private void ProgressTimer_Tick(object sender, EventArgs e)
		{
			BackgroundAudioPlayer instance = BackgroundAudioPlayer.Instance;
			bool flag = instance.Track != null && instance.Position.TotalSeconds > 0.0;
			if (flag)
			{
				double totalSeconds = instance.Track.Duration.TotalSeconds;
				double totalSeconds2 = instance.Position.TotalSeconds;
				bool flag2 = totalSeconds > 0.0 && !double.IsNaN(totalSeconds2) && !double.IsInfinity(totalSeconds2);
				if (flag2)
				{
					double value = totalSeconds2 / totalSeconds * 100.0;
					this.ProgressSlider.Value = value;
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000BECC File Offset: 0x0000A0CC
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/Zendo.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.MarqueeStoryboard = (Storyboard)base.FindName("MarqueeStoryboard");
				this.SearchBox = (TextBox)base.FindName("SearchBox");
				this.ResultsList = (ListBox)base.FindName("ResultsList");
				this.RandomTrackPanel = (StackPanel)base.FindName("RandomTrackPanel");
				this.RandomCover = (Image)base.FindName("RandomCover");
				this.RandomTitle = (TextBlock)base.FindName("RandomTitle");
				this.RandomArtist = (TextBlock)base.FindName("RandomArtist");
				this.MiniControlBarWrapper = (Grid)base.FindName("MiniControlBarWrapper");
				this.MiniCover = (Image)base.FindName("MiniCover");
				this.CurrentTrackName = (TextBlock)base.FindName("CurrentTrackName");
				this.TrackTransform = (TranslateTransform)base.FindName("TrackTransform");
				this.ProgressSlider = (ProgressBar)base.FindName("ProgressSlider");
				this.Player = (MediaElement)base.FindName("Player");
			}
		}

		// Token: 0x04000102 RID: 258
		private DispatcherTimer progressTimer = new DispatcherTimer();

		// Token: 0x04000103 RID: 259
		private static Track1 currentTrack;

		// Token: 0x04000104 RID: 260
		internal Grid LayoutRoot;

		// Token: 0x04000105 RID: 261
		internal Storyboard MarqueeStoryboard;

		// Token: 0x04000106 RID: 262
		internal TextBox SearchBox;

		// Token: 0x04000107 RID: 263
		internal ListBox ResultsList;

		// Token: 0x04000108 RID: 264
		internal StackPanel RandomTrackPanel;

		// Token: 0x04000109 RID: 265
		internal Image RandomCover;

		// Token: 0x0400010A RID: 266
		internal TextBlock RandomTitle;

		// Token: 0x0400010B RID: 267
		internal TextBlock RandomArtist;

		// Token: 0x0400010C RID: 268
		internal Grid MiniControlBarWrapper;

		// Token: 0x0400010D RID: 269
		internal Image MiniCover;

		// Token: 0x0400010E RID: 270
		internal TextBlock CurrentTrackName;

		// Token: 0x0400010F RID: 271
		internal TranslateTransform TrackTransform;

		// Token: 0x04000110 RID: 272
		internal ProgressBar ProgressSlider;

		// Token: 0x04000111 RID: 273
		internal MediaElement Player;

		// Token: 0x04000112 RID: 274
		private bool _contentLoaded;
	}
}
