using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA
{
	// Token: 0x02000006 RID: 6
	public class BrowsePage : PhoneApplicationPage
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002ADA File Offset: 0x00000CDA
		public BrowsePage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002AF4 File Offset: 0x00000CF4
		private void SlideOutAndNavigate()
		{
			bool flag = this.isNavigatingBack;
			if (!flag)
			{
				this.isNavigatingBack = true;
				this.SlideOutStoryboard.Completed += delegate(object s, EventArgs e)
				{
					base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
				};
				this.SlideOutStoryboard.Begin();
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002B39 File Offset: 0x00000D39
		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			e.Cancel = true;
			this.SlideOutAndNavigate();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B4B File Offset: 0x00000D4B
		private void a1_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/pagesex/chillin.xaml", 2));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002B65 File Offset: 0x00000D65
		private void b1_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/pagesex/hype.xaml", 2));
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002B7F File Offset: 0x00000D7F
		private void c1_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/pagesex/New.xaml", 2));
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B99 File Offset: 0x00000D99
		private void d1_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/pagesex/r.xaml", 2));
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000022A0 File Offset: 0x000004A0
		private void Home_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002BB4 File Offset: 0x00000DB4
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/BrowsePage.xaml", 2));
				this.SlideOutStoryboard = (Storyboard)base.FindName("SlideOutStoryboard");
				this.LayoutRoot5 = (Grid)base.FindName("LayoutRoot5");
				this.SlideTransform = (TranslateTransform)base.FindName("SlideTransform");
				this.aButton = (Button)base.FindName("aButton");
				this.bButton = (Button)base.FindName("bButton");
				this.cButton = (Button)base.FindName("cButton");
				this.dButton = (Button)base.FindName("dButton");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x04000015 RID: 21
		private bool isNavigatingBack = false;

		// Token: 0x04000016 RID: 22
		internal Storyboard SlideOutStoryboard;

		// Token: 0x04000017 RID: 23
		internal Grid LayoutRoot5;

		// Token: 0x04000018 RID: 24
		internal TranslateTransform SlideTransform;

		// Token: 0x04000019 RID: 25
		internal Button aButton;

		// Token: 0x0400001A RID: 26
		internal Button bButton;

		// Token: 0x0400001B RID: 27
		internal Button cButton;

		// Token: 0x0400001C RID: 28
		internal Button dButton;

		// Token: 0x0400001D RID: 29
		internal MediaElement AudioPlayer;

		// Token: 0x0400001E RID: 30
		private bool _contentLoaded;
	}
}
