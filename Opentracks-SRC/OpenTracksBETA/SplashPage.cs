using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA
{
	// Token: 0x02000014 RID: 20
	public class SplashPage : PhoneApplicationPage
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00006BF9 File Offset: 0x00004DF9
		public SplashPage()
		{
			this.InitializeComponent();
			base.Loaded += new RoutedEventHandler(this.SplashPage_Loaded);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006C1D File Offset: 0x00004E1D
		private void SplashPage_Loaded(object sender, RoutedEventArgs e)
		{
			this.SplashInStoryboard.Completed += delegate(object s, EventArgs args)
			{
				DispatcherTimer timer = new DispatcherTimer();
				timer.Interval = TimeSpan.FromSeconds(0.2);
				timer.Tick += delegate(object ts, EventArgs te)
				{
					timer.Stop();
					this.SplashOutStoryboard.Completed += delegate(object fs, EventArgs fe)
					{
						bool flag = IsolatedStorageSettings.ApplicationSettings.Contains("email");
						bool flag2 = flag;
						if (flag2)
						{
							base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
						}
						else
						{
							base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
						}
					};
					this.SplashOutStoryboard.Begin();
				};
				timer.Start();
			};
			this.SplashInStoryboard.Begin();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006C44 File Offset: 0x00004E44
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/SplashPage.xaml", 2));
				this.SplashInStoryboard = (Storyboard)base.FindName("SplashInStoryboard");
				this.SplashOutStoryboard = (Storyboard)base.FindName("SplashOutStoryboard");
				this.SplashImage = (Image)base.FindName("SplashImage");
				this.SplashTransform = (CompositeTransform)base.FindName("SplashTransform");
			}
		}

		// Token: 0x04000096 RID: 150
		internal Storyboard SplashInStoryboard;

		// Token: 0x04000097 RID: 151
		internal Storyboard SplashOutStoryboard;

		// Token: 0x04000098 RID: 152
		internal Image SplashImage;

		// Token: 0x04000099 RID: 153
		internal CompositeTransform SplashTransform;

		// Token: 0x0400009A RID: 154
		private bool _contentLoaded;
	}
}
