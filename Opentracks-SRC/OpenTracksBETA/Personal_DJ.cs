using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
using OpenTracksBETA.ViewModels;

namespace OpenTracksBETA
{
	// Token: 0x0200000F RID: 15
	public class Personal_DJ : PhoneApplicationPage
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00005E10 File Offset: 0x00004010
		public Personal_DJ()
		{
			this.InitializeComponent();
			base.DataContext = App.ViewModel;
			base.Dispatcher.BeginInvoke(delegate()
			{
				this.PlayRandomTrack();
			});
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005E64 File Offset: 0x00004064
		private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
		{
			this.LayoutRoot.Visibility = 0;
			DoubleAnimation doubleAnimation = new DoubleAnimation
			{
				From = new double?((double)150),
				To = new double?(0.0),
				Duration = TimeSpan.FromSeconds(0.6),
				EasingFunction = new CubicEase
				{
					EasingMode = 0
				}
			};
			DoubleAnimation doubleAnimation2 = new DoubleAnimation
			{
				From = new double?(0.0),
				To = new double?((double)1),
				Duration = TimeSpan.FromSeconds(0.6),
				BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(100.0)),
				EasingFunction = new CubicEase
				{
					EasingMode = 0
				}
			};
			Storyboard.SetTarget(doubleAnimation, this.LayoutRootTransform);
			Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Y", new object[0]));
			Storyboard.SetTarget(doubleAnimation2, this.LayoutRoot);
			Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath("Opacity", new object[0]));
			Storyboard storyboard = new Storyboard();
			storyboard.Children.Add(doubleAnimation);
			storyboard.Children.Add(doubleAnimation2);
			storyboard.Begin();
			base.Dispatcher.BeginInvoke(delegate()
			{
				this.PlayRandomTrack();
			});
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005FD4 File Offset: 0x000041D4
		private void AnimateSlideOut(Action onComplete = null)
		{
			DoubleAnimation doubleAnimation = new DoubleAnimation
			{
				From = new double?(0.0),
				To = new double?((double)800),
				Duration = TimeSpan.FromSeconds(0.7),
				EasingFunction = new ExponentialEase
				{
					EasingMode = 1
				}
			};
			DoubleAnimation doubleAnimation2 = new DoubleAnimation
			{
				From = new double?((double)1),
				To = new double?(0.0),
				Duration = TimeSpan.FromSeconds(0.6),
				BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(100.0)),
				EasingFunction = new ExponentialEase
				{
					EasingMode = 1
				}
			};
			Storyboard.SetTarget(doubleAnimation, this.LayoutRootTransform);
			Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Y", new object[0]));
			Storyboard.SetTarget(doubleAnimation2, this.LayoutRoot);
			Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath("Opacity", new object[0]));
			Storyboard storyboard = new Storyboard();
			storyboard.Children.Add(doubleAnimation);
			storyboard.Children.Add(doubleAnimation2);
			bool flag = onComplete != null;
			if (flag)
			{
				storyboard.Completed += delegate(object s, EventArgs e)
				{
					onComplete.Invoke();
				};
			}
			storyboard.Begin();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000614E File Offset: 0x0000434E
		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			e.Cancel = true;
			this.AnimateSlideOut(delegate
			{
				base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
			});
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000616C File Offset: 0x0000436C
		private void PlayRandomTrack()
		{
			ObservableCollection<TrackItem> allTracks = App.ViewModel.AllTracks;
			bool flag = allTracks == null || allTracks.Count == 0;
			if (!flag)
			{
				int num = this.random.Next(allTracks.Count);
				this.currentTrackIndex = num;
				TrackItem trackItem = allTracks[this.currentTrackIndex];
				this.AudioPlayer.Source = new Uri(trackItem.AudioPath, 1);
				this.AudioPlayer.MediaOpened += delegate(object s, RoutedEventArgs e)
				{
					this.AudioPlayer.Play();
				};
				this.PlayPauseButton.Content = "❚❚";
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00006204 File Offset: 0x00004404
		private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.AudioPlayer.CurrentState == 3;
			if (flag)
			{
				this.AudioPlayer.Pause();
				this.PlayPauseButton.Content = "▶";
			}
			else
			{
				bool flag2 = this.AudioPlayer.Source != null;
				if (flag2)
				{
					this.AudioPlayer.Play();
					this.PlayPauseButton.Content = "❚❚";
				}
				else
				{
					this.PlayRandomTrack();
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00006284 File Offset: 0x00004484
		private void ForwardButton_Click(object sender, RoutedEventArgs e)
		{
			this.PlayRandomTrack();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000628E File Offset: 0x0000448E
		private void BackwardButton_Click(object sender, RoutedEventArgs e)
		{
			this.AudioPlayer.Position = TimeSpan.Zero;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000022A0 File Offset: 0x000004A0
		private void Home_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000062A4 File Offset: 0x000044A4
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/Personal_DJ.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.LayoutRootTransform = (TranslateTransform)base.FindName("LayoutRootTransform");
				this.AlbumCover = (Image)base.FindName("AlbumCover");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
				this.PlayPauseButton = (Button)base.FindName("PlayPauseButton");
			}
		}

		// Token: 0x04000079 RID: 121
		private int currentTrackIndex = -1;

		// Token: 0x0400007A RID: 122
		private Random random = new Random();

		// Token: 0x0400007B RID: 123
		internal Grid LayoutRoot;

		// Token: 0x0400007C RID: 124
		internal TranslateTransform LayoutRootTransform;

		// Token: 0x0400007D RID: 125
		internal Image AlbumCover;

		// Token: 0x0400007E RID: 126
		internal MediaElement AudioPlayer;

		// Token: 0x0400007F RID: 127
		internal Button PlayPauseButton;

		// Token: 0x04000080 RID: 128
		private bool _contentLoaded;
	}
}
