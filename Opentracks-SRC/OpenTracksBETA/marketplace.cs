using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA
{
	// Token: 0x0200000D RID: 13
	public class marketplace : PhoneApplicationPage
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00005BAF File Offset: 0x00003DAF
		public marketplace()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005BC7 File Offset: 0x00003DC7
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			base.Dispatcher.BeginInvoke(delegate()
			{
				this.SlideInStoryboard.Begin();
			});
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005BEA File Offset: 0x00003DEA
		private void Spotify_Tap(object sender, GestureEventArgs e)
		{
			this.NavigateWithSlideAnimation("/SpotifyPage.xaml");
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005BF9 File Offset: 0x00003DF9
		private void YTM_Tap(object sender, GestureEventArgs e)
		{
			this.NavigateWithSlideAnimation("/YTMusic.xaml");
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005C08 File Offset: 0x00003E08
		private void Zendo_Tap(object sender, GestureEventArgs e)
		{
			this.NavigateWithSlideAnimation("/Zendo.xaml");
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005C17 File Offset: 0x00003E17
		private void AppleMusic_Tap(object sender, GestureEventArgs e)
		{
			this.NavigateWithSlideAnimation("/Podcasts.xaml");
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005C26 File Offset: 0x00003E26
		private void NavigateWithSlideAnimation(string targetPage)
		{
			this._targetPage = targetPage;
			this._isBackNavigation = false;
			this.SlideOutStoryboard.Completed += new EventHandler(this.SlideOutStoryboard_Completed);
			this.SlideOutStoryboard.Begin();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005C5C File Offset: 0x00003E5C
		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			e.Cancel = true;
			this._targetPage = "/MainPage.xaml";
			this._isBackNavigation = true;
			this.SlideOutStoryboard.Completed += new EventHandler(this.SlideOutStoryboard_Completed);
			this.SlideOutStoryboard.Begin();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005CA8 File Offset: 0x00003EA8
		private void SlideOutStoryboard_Completed(object sender, EventArgs e)
		{
			this.SlideOutStoryboard.Completed -= new EventHandler(this.SlideOutStoryboard_Completed);
			bool flag = !string.IsNullOrEmpty(this._targetPage);
			if (flag)
			{
				base.NavigationService.Navigate(new Uri(this._targetPage, 2));
				this._targetPage = null;
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005D04 File Offset: 0x00003F04
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/marketplace.xaml", 2));
				this.SlideInStoryboard = (Storyboard)base.FindName("SlideInStoryboard");
				this.SlideOutStoryboard = (Storyboard)base.FindName("SlideOutStoryboard");
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.ContentPanel = (Grid)base.FindName("ContentPanel");
			}
		}

		// Token: 0x0400006F RID: 111
		private string _targetPage;

		// Token: 0x04000070 RID: 112
		private bool _isBackNavigation = false;

		// Token: 0x04000071 RID: 113
		internal Storyboard SlideInStoryboard;

		// Token: 0x04000072 RID: 114
		internal Storyboard SlideOutStoryboard;

		// Token: 0x04000073 RID: 115
		internal Grid LayoutRoot;

		// Token: 0x04000074 RID: 116
		internal Grid ContentPanel;

		// Token: 0x04000075 RID: 117
		private bool _contentLoaded;
	}
}
