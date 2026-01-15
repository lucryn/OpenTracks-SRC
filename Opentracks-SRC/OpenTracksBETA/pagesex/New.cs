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
	// Token: 0x02000029 RID: 41
	public class New : PhoneApplicationPage
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000FBB5 File Offset: 0x0000DDB5
		// (set) Token: 0x06000249 RID: 585 RVA: 0x0000FBBD File Offset: 0x0000DDBD
		public MainViewModel ViewModel { get; private set; }

		// Token: 0x0600024A RID: 586 RVA: 0x0000FBC8 File Offset: 0x0000DDC8
		public New()
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

		// Token: 0x0600024B RID: 587 RVA: 0x0000FC74 File Offset: 0x0000DE74
		private void NATracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TrackItem trackItem = this.NATracks.SelectedItem as TrackItem;
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

		// Token: 0x0600024C RID: 588 RVA: 0x000022A0 File Offset: 0x000004A0
		private void Home_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000FD0C File Offset: 0x0000DF0C
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

		// Token: 0x0600024E RID: 590 RVA: 0x0000FDC8 File Offset: 0x0000DFC8
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

		// Token: 0x0600024F RID: 591 RVA: 0x0000FE28 File Offset: 0x0000E028
		private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			bool flag = this.AudioPlayer.NaturalDuration.HasTimeSpan && Math.Abs(this.AudioPlayer.Position.TotalSeconds - e.NewValue) > 1.0;
			if (flag)
			{
				this.AudioPlayer.Position = TimeSpan.FromSeconds(e.NewValue);
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000FE98 File Offset: 0x0000E098
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

		// Token: 0x06000251 RID: 593 RVA: 0x0000FF0C File Offset: 0x0000E10C
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/pagesex/New.xaml", 2));
				this.LayoutRoot10 = (Grid)base.FindName("LayoutRoot10");
				this.MarqueeStoryboard = (Storyboard)base.FindName("MarqueeStoryboard");
				this.NATracks = (ListBox)base.FindName("NATracks");
				this.CurrentTrackName = (TextBlock)base.FindName("CurrentTrackName");
				this.TrackTransform = (CompositeTransform)base.FindName("TrackTransform");
				this.PlayPauseButton = (Button)base.FindName("PlayPauseButton");
				this.StartTimeLabel = (TextBlock)base.FindName("StartTimeLabel");
				this.ProgressSlider = (Slider)base.FindName("ProgressSlider");
				this.EndTimeLabel = (TextBlock)base.FindName("EndTimeLabel");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x04000151 RID: 337
		private DispatcherTimer progressTimer;

		// Token: 0x04000152 RID: 338
		private bool isPlaying = false;

		// Token: 0x04000153 RID: 339
		private TrackItem currentTrack;

		// Token: 0x04000154 RID: 340
		internal Grid LayoutRoot10;

		// Token: 0x04000155 RID: 341
		internal Storyboard MarqueeStoryboard;

		// Token: 0x04000156 RID: 342
		internal ListBox NATracks;

		// Token: 0x04000157 RID: 343
		internal TextBlock CurrentTrackName;

		// Token: 0x04000158 RID: 344
		internal CompositeTransform TrackTransform;

		// Token: 0x04000159 RID: 345
		internal Button PlayPauseButton;

		// Token: 0x0400015A RID: 346
		internal TextBlock StartTimeLabel;

		// Token: 0x0400015B RID: 347
		internal Slider ProgressSlider;

		// Token: 0x0400015C RID: 348
		internal TextBlock EndTimeLabel;

		// Token: 0x0400015D RID: 349
		internal MediaElement AudioPlayer;

		// Token: 0x0400015E RID: 350
		private bool _contentLoaded;
	}
}
