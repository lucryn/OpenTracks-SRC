using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA
{
	// Token: 0x02000010 RID: 16
	public class Podcasts : PhoneApplicationPage
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00006358 File Offset: 0x00004558
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00006360 File Offset: 0x00004560
		public ObservableCollection<PodcastItem> PodcastList { get; set; }

		// Token: 0x060000B1 RID: 177 RVA: 0x0000636C File Offset: 0x0000456C
		public Podcasts()
		{
			this.InitializeComponent();
			ObservableCollection<PodcastItem> observableCollection = new ObservableCollection<PodcastItem>();
			observableCollection.Add(new PodcastItem
			{
				Title = "Create a app for windows phone 7 (podcast edition)",
				Url = "https://opsrv.vercel.app/pc5.mp3",
				CoverArt = "https://phonesdata.com/files/models/Nokia-Lumia-800-768.jpg",
				Artist = "Strblitz, ChockingNetDude, Longhorn4029"
			});
			observableCollection.Add(new PodcastItem
			{
				Title = "What's version difference for opentracks?",
				Url = "https://opsrv.vercel.app/pc3.mp3",
				CoverArt = "https://chockingnetdude.vercel.app/wp/ots.png",
				Artist = "ChockingNetDude"
			});
			observableCollection.Add(new PodcastItem
			{
				Title = "iPhone 14 Pro did not get what it deserves",
				Url = "https://opsrv.vercel.app/pc1.mp3",
				CoverArt = "https://gsmfind.com/img/product/featured/24/apple-iphone-14-pro-max.jpg",
				Artist = "ChockingNetDude, Longhorn4029"
			});
			observableCollection.Add(new PodcastItem
			{
				Title = "Fix Login issues on opentracks (easy)",
				Url = "https://opsrv.vercel.app/pc4.mp3",
				CoverArt = "https://chockingnetdude.vercel.app/pages/ots.png",
				Artist = "ChockingNetDude, a"
			});
			observableCollection.Add(new PodcastItem
			{
				Title = "iPhone 11 Pro, the start of 3 eyes camera.",
				Url = "https://opsrv.vercel.app/pc2.mp3",
				CoverArt = "https://open-tracks.vercel.app/Assets/test1.png",
				Artist = "ChockingNetDude"
			});
			observableCollection.Add(new PodcastItem
			{
				Title = "午夜白卡談 - 和平&健康",
				Url = "https://opsrv.vercel.app/narrowpdct.mp3",
				CoverArt = "https://opsrv.vercel.app/bakatalk.png",
				Artist = "午夜白卡談"
			});
			observableCollection.Add(new PodcastItem
			{
				Title = "b1an - Win8 in 2024? worth it?",
				Url = "https://open-tracks.vercel.app/subpage/podcasts/chockingnetdude/tutor1-2.mp3",
				CoverArt = "https://open-tracks.vercel.app/Assets/test.jpg",
				Artist = "b1an"
			});
			observableCollection.Add(new PodcastItem
			{
				Title = "ChockingNetDude - Tutorial",
				Url = "https://opsrv.vercel.app/tutu.mp3",
				Artist = "ChockingNetDude",
				CoverArt = "https://open-tracks.vercel.app/Assets/test1.png"
			});
			this.PodcastList = observableCollection;
			base.DataContext = this;
			this.mediaPlayer.MediaOpened += new RoutedEventHandler(this.mediaPlayer_MediaOpened);
			this.mediaPlayer.MediaEnded += new RoutedEventHandler(this.mediaPlayer_MediaEnded);
			this.mediaPlayer.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(this.mediaPlayer_MediaFailed);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000065C4 File Offset: 0x000047C4
		private void Play_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.mediaPlayer.Source != null;
			if (flag)
			{
				this.mediaPlayer.Play();
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000065F5 File Offset: 0x000047F5
		private void Pause_Click(object sender, RoutedEventArgs e)
		{
			this.mediaPlayer.Pause();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00006604 File Offset: 0x00004804
		private void Rewind_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.mediaPlayer.Position.TotalSeconds > 10.0;
			if (flag)
			{
				this.mediaPlayer.Position = this.mediaPlayer.Position - TimeSpan.FromSeconds(10.0);
			}
			else
			{
				this.mediaPlayer.Position = TimeSpan.Zero;
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00006675 File Offset: 0x00004875
		private void Forward_Click(object sender, RoutedEventArgs e)
		{
			this.mediaPlayer.Position = this.mediaPlayer.Position + TimeSpan.FromSeconds(15.0);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000066A4 File Offset: 0x000048A4
		private void podcastList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PodcastItem podcastItem = this.podcastList.SelectedItem as PodcastItem;
			bool flag = podcastItem != null;
			if (flag)
			{
				this.mediaPlayer.Source = new Uri(podcastItem.Url, 1);
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000066E5 File Offset: 0x000048E5
		private void mediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
		{
			this.mediaPlayer.Play();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000066F4 File Offset: 0x000048F4
		private void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Playback finished.");
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00006702 File Offset: 0x00004902
		private void mediaPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			MessageBox.Show("Playback failed: " + e.ErrorException.Message);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00006720 File Offset: 0x00004920
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/Podcasts.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.podcastList = (ListBox)base.FindName("podcastList");
				this.mediaPlayer = (MediaElement)base.FindName("mediaPlayer");
			}
		}

		// Token: 0x04000082 RID: 130
		internal Grid LayoutRoot;

		// Token: 0x04000083 RID: 131
		internal ListBox podcastList;

		// Token: 0x04000084 RID: 132
		internal MediaElement mediaPlayer;

		// Token: 0x04000085 RID: 133
		private bool _contentLoaded;
	}
}
