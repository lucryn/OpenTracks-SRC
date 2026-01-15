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
	// Token: 0x02000028 RID: 40
	public class hype : PhoneApplicationPage
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000F73E File Offset: 0x0000D93E
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000F746 File Offset: 0x0000D946
		public MainViewModel ViewModel { get; private set; }

		// Token: 0x0600023E RID: 574 RVA: 0x0000F750 File Offset: 0x0000D950
		public hype()
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

		// Token: 0x0600023F RID: 575 RVA: 0x0000F7FC File Offset: 0x0000D9FC
		private void HypeTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TrackItem trackItem = this.HypeTracks.SelectedItem as TrackItem;
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

		// Token: 0x06000240 RID: 576 RVA: 0x000022A0 File Offset: 0x000004A0
		private void Home_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000F894 File Offset: 0x0000DA94
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

		// Token: 0x06000242 RID: 578 RVA: 0x0000F950 File Offset: 0x0000DB50
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

		// Token: 0x06000243 RID: 579 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
		private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			bool flag = this.AudioPlayer.NaturalDuration.HasTimeSpan && Math.Abs(this.AudioPlayer.Position.TotalSeconds - e.NewValue) > 1.0;
			if (flag)
			{
				this.AudioPlayer.Position = TimeSpan.FromSeconds(e.NewValue);
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000FA20 File Offset: 0x0000DC20
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

		// Token: 0x06000245 RID: 581 RVA: 0x0000FA94 File Offset: 0x0000DC94
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/pagesex/hype.xaml", 2));
				this.LayoutRoot10 = (Grid)base.FindName("LayoutRoot10");
				this.MarqueeStoryboard = (Storyboard)base.FindName("MarqueeStoryboard");
				this.HypeTracks = (ListBox)base.FindName("HypeTracks");
				this.CurrentTrackName = (TextBlock)base.FindName("CurrentTrackName");
				this.TrackTransform = (CompositeTransform)base.FindName("TrackTransform");
				this.PlayPauseButton = (Button)base.FindName("PlayPauseButton");
				this.StartTimeLabel = (TextBlock)base.FindName("StartTimeLabel");
				this.ProgressSlider = (Slider)base.FindName("ProgressSlider");
				this.EndTimeLabel = (TextBlock)base.FindName("EndTimeLabel");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x04000142 RID: 322
		private DispatcherTimer progressTimer;

		// Token: 0x04000143 RID: 323
		private bool isPlaying = false;

		// Token: 0x04000144 RID: 324
		private TrackItem currentTrack;

		// Token: 0x04000145 RID: 325
		internal Grid LayoutRoot10;

		// Token: 0x04000146 RID: 326
		internal Storyboard MarqueeStoryboard;

		// Token: 0x04000147 RID: 327
		internal ListBox HypeTracks;

		// Token: 0x04000148 RID: 328
		internal TextBlock CurrentTrackName;

		// Token: 0x04000149 RID: 329
		internal CompositeTransform TrackTransform;

		// Token: 0x0400014A RID: 330
		internal Button PlayPauseButton;

		// Token: 0x0400014B RID: 331
		internal TextBlock StartTimeLabel;

		// Token: 0x0400014C RID: 332
		internal Slider ProgressSlider;

		// Token: 0x0400014D RID: 333
		internal TextBlock EndTimeLabel;

		// Token: 0x0400014E RID: 334
		internal MediaElement AudioPlayer;

		// Token: 0x0400014F RID: 335
		private bool _contentLoaded;
	}
}
