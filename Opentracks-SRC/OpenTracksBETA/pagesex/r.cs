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
	// Token: 0x0200002A RID: 42
	public class r : PhoneApplicationPage
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0001002D File Offset: 0x0000E22D
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00010035 File Offset: 0x0000E235
		public MainViewModel ViewModel { get; private set; }

		// Token: 0x06000256 RID: 598 RVA: 0x00010040 File Offset: 0x0000E240
		public r()
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

		// Token: 0x06000257 RID: 599 RVA: 0x000100EC File Offset: 0x0000E2EC
		private void RTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TrackItem trackItem = this.RTracks.SelectedItem as TrackItem;
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

		// Token: 0x06000258 RID: 600 RVA: 0x000022A0 File Offset: 0x000004A0
		private void Home_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00010184 File Offset: 0x0000E384
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

		// Token: 0x0600025A RID: 602 RVA: 0x00010240 File Offset: 0x0000E440
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

		// Token: 0x0600025B RID: 603 RVA: 0x000102A0 File Offset: 0x0000E4A0
		private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			bool flag = this.AudioPlayer.NaturalDuration.HasTimeSpan && Math.Abs(this.AudioPlayer.Position.TotalSeconds - e.NewValue) > 1.0;
			if (flag)
			{
				this.AudioPlayer.Position = TimeSpan.FromSeconds(e.NewValue);
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00010310 File Offset: 0x0000E510
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

		// Token: 0x0600025D RID: 605 RVA: 0x00010384 File Offset: 0x0000E584
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/pagesex/r.xaml", 2));
				this.LayoutRoot10 = (Grid)base.FindName("LayoutRoot10");
				this.MarqueeStoryboard = (Storyboard)base.FindName("MarqueeStoryboard");
				this.RTracks = (ListBox)base.FindName("RTracks");
				this.CurrentTrackName = (TextBlock)base.FindName("CurrentTrackName");
				this.TrackTransform = (CompositeTransform)base.FindName("TrackTransform");
				this.PlayPauseButton = (Button)base.FindName("PlayPauseButton");
				this.StartTimeLabel = (TextBlock)base.FindName("StartTimeLabel");
				this.ProgressSlider = (Slider)base.FindName("ProgressSlider");
				this.EndTimeLabel = (TextBlock)base.FindName("EndTimeLabel");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x04000160 RID: 352
		private DispatcherTimer progressTimer;

		// Token: 0x04000161 RID: 353
		private bool isPlaying = false;

		// Token: 0x04000162 RID: 354
		private TrackItem currentTrack;

		// Token: 0x04000163 RID: 355
		internal Grid LayoutRoot10;

		// Token: 0x04000164 RID: 356
		internal Storyboard MarqueeStoryboard;

		// Token: 0x04000165 RID: 357
		internal ListBox RTracks;

		// Token: 0x04000166 RID: 358
		internal TextBlock CurrentTrackName;

		// Token: 0x04000167 RID: 359
		internal CompositeTransform TrackTransform;

		// Token: 0x04000168 RID: 360
		internal Button PlayPauseButton;

		// Token: 0x04000169 RID: 361
		internal TextBlock StartTimeLabel;

		// Token: 0x0400016A RID: 362
		internal Slider ProgressSlider;

		// Token: 0x0400016B RID: 363
		internal TextBlock EndTimeLabel;

		// Token: 0x0400016C RID: 364
		internal MediaElement AudioPlayer;

		// Token: 0x0400016D RID: 365
		private bool _contentLoaded;
	}
}
