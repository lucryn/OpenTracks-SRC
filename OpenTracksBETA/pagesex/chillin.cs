using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using OpenTracksBETA.ViewModels;

namespace OpenTracksBETA.pagesex
{
	// Token: 0x02000027 RID: 39
	public class chillin : PhoneApplicationPage
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000F2AE File Offset: 0x0000D4AE
		// (set) Token: 0x06000231 RID: 561 RVA: 0x0000F2B6 File Offset: 0x0000D4B6
		public MainViewModel ViewModel { get; private set; }

		// Token: 0x06000232 RID: 562 RVA: 0x0000F2C0 File Offset: 0x0000D4C0
		public chillin()
		{
			this.InitializeComponent();
			this.ViewModel = new MainViewModel();
			base.DataContext = this.ViewModel;
			this.AudioPlayer.MediaFailed += delegate(object s, ExceptionRoutedEventArgs e)
			{
				base.NavigationService.Navigate(new Uri("/Crash/mediafail.xaml", 2));
			};
			this.AudioPlayer.MediaOpened += delegate(object s, RoutedEventArgs e)
			{
				this.AudioPlayer.Play();
			};
			this.progressTimer = new DispatcherTimer();
			this.progressTimer.Interval = TimeSpan.FromSeconds(1.0);
			this.progressTimer.Tick += new EventHandler(this.ProgressTimer_Tick);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000022A0 File Offset: 0x000004A0
		private void Home_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000F36C File Offset: 0x0000D56C
		private void ChillTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TrackItem trackItem = this.ChillTracks.SelectedItem as TrackItem;
			bool flag = trackItem != null;
			if (flag)
			{
				this.currentTrack = trackItem;
				this.AudioPlayer.Source = new Uri(trackItem.AudioPath);
				this.AudioPlayer.MediaOpened += new RoutedEventHandler(this.AudioPlayer_MediaOpened);
				this.AudioPlayer.Play();
				this.isPlaying = true;
				this.PlayPauseButton.Content = "❚❚";
				this.CurrentTrackName.Text = trackItem.Title;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000F404 File Offset: 0x0000D604
		private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.currentTrack == null;
			if (!flag)
			{
				bool flag2 = this.isPlaying && this.AudioPlayer.CurrentState == 3;
				if (flag2)
				{
					this.AudioPlayer.Pause();
					this.PlayPauseButton.Content = "▶";
					this.isPlaying = false;
					this.MarqueeStoryboard.Stop();
					this.progressTimer.Stop();
				}
				else
				{
					this.AudioPlayer.Play();
					this.PlayPauseButton.Content = "❚❚";
					this.isPlaying = true;
					this.MarqueeStoryboard.Begin();
					this.progressTimer.Start();
				}
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000F4C0 File Offset: 0x0000D6C0
		private void ProgressTimer_Tick(object sender, EventArgs e)
		{
			bool hasTimeSpan = this.AudioPlayer.NaturalDuration.HasTimeSpan;
			if (hasTimeSpan)
			{
				TimeSpan position = this.AudioPlayer.Position;
				this.ProgressSlider.Value = position.TotalSeconds;
				this.StartTimeLabel.Text = position.ToString("mm\\:ss");
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000F520 File Offset: 0x0000D720
		private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			bool flag = this.AudioPlayer.NaturalDuration.HasTimeSpan && Math.Abs(this.AudioPlayer.Position.TotalSeconds - e.NewValue) > 1.0;
			if (flag)
			{
				this.AudioPlayer.Position = TimeSpan.FromSeconds(e.NewValue);
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000F590 File Offset: 0x0000D790
		private void AudioPlayer_MediaOpened(object sender, RoutedEventArgs e)
		{
			bool hasTimeSpan = this.AudioPlayer.NaturalDuration.HasTimeSpan;
			if (hasTimeSpan)
			{
				TimeSpan timeSpan = this.AudioPlayer.NaturalDuration.TimeSpan;
				this.EndTimeLabel.Text = timeSpan.ToString("mm\\:ss");
				this.ProgressSlider.Maximum = timeSpan.TotalSeconds;
				this.progressTimer.Start();
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000F604 File Offset: 0x0000D804
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/pagesex/chillin.xaml", 2));
				this.LayoutRoot10 = (Grid)base.FindName("LayoutRoot10");
				this.MarqueeStoryboard = (Storyboard)base.FindName("MarqueeStoryboard");
				this.ChillTracks = (ListBox)base.FindName("ChillTracks");
				this.CurrentTrackName = (TextBlock)base.FindName("CurrentTrackName");
				this.TrackTransform = (CompositeTransform)base.FindName("TrackTransform");
				this.PlayPauseButton = (Button)base.FindName("PlayPauseButton");
				this.StartTimeLabel = (TextBlock)base.FindName("StartTimeLabel");
				this.ProgressSlider = (Slider)base.FindName("ProgressSlider");
				this.EndTimeLabel = (TextBlock)base.FindName("EndTimeLabel");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x04000133 RID: 307
		private DispatcherTimer progressTimer;

		// Token: 0x04000134 RID: 308
		private bool isPlaying = false;

		// Token: 0x04000135 RID: 309
		private TrackItem currentTrack;

		// Token: 0x04000136 RID: 310
		internal Grid LayoutRoot10;

		// Token: 0x04000137 RID: 311
		internal Storyboard MarqueeStoryboard;

		// Token: 0x04000138 RID: 312
		internal ListBox ChillTracks;

		// Token: 0x04000139 RID: 313
		internal TextBlock CurrentTrackName;

		// Token: 0x0400013A RID: 314
		internal CompositeTransform TrackTransform;

		// Token: 0x0400013B RID: 315
		internal Button PlayPauseButton;

		// Token: 0x0400013C RID: 316
		internal TextBlock StartTimeLabel;

		// Token: 0x0400013D RID: 317
		internal Slider ProgressSlider;

		// Token: 0x0400013E RID: 318
		internal TextBlock EndTimeLabel;

		// Token: 0x0400013F RID: 319
		internal MediaElement AudioPlayer;

		// Token: 0x04000140 RID: 320
		private bool _contentLoaded;
	}
}
