using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA
{
	// Token: 0x02000003 RID: 3
	public class About : PhoneApplicationPage
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002090 File Offset: 0x00000290
		public About()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020A8 File Offset: 0x000002A8
		private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
		{
			this.LayoutRoot.Visibility = 0;
			DoubleAnimation doubleAnimation = new DoubleAnimation
			{
				From = new double?(0.0),
				To = new double?((double)1),
				Duration = TimeSpan.FromSeconds(0.6),
				EasingFunction = new SineEase
				{
					EasingMode = 0
				}
			};
			Storyboard.SetTarget(doubleAnimation, this.LayoutRoot);
			Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity", new object[0]));
			Storyboard storyboard = new Storyboard();
			storyboard.Children.Add(doubleAnimation);
			storyboard.Begin();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002158 File Offset: 0x00000358
		private void FadeOutAndGoHome()
		{
			bool flag = this.isNavigatingBack;
			if (!flag)
			{
				this.isNavigatingBack = true;
				DoubleAnimation doubleAnimation = new DoubleAnimation
				{
					From = new double?((double)1),
					To = new double?(0.0),
					Duration = TimeSpan.FromSeconds(0.4),
					EasingFunction = new SineEase
					{
						EasingMode = 1
					}
				};
				Storyboard.SetTarget(doubleAnimation, this.LayoutRoot);
				Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity", new object[0]));
				Storyboard storyboard = new Storyboard();
				storyboard.Children.Add(doubleAnimation);
				storyboard.Completed += delegate(object s, EventArgs args)
				{
					base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
				};
				storyboard.Begin();
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002224 File Offset: 0x00000424
		private void Home_Click(object sender, EventArgs e)
		{
			this.FadeOutAndGoHome();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000222E File Offset: 0x0000042E
		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			e.Cancel = true;
			this.FadeOutAndGoHome();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002240 File Offset: 0x00000440
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/About.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.ContentPanel = (Grid)base.FindName("ContentPanel");
			}
		}

		// Token: 0x04000001 RID: 1
		private bool isNavigatingBack = false;

		// Token: 0x04000002 RID: 2
		internal Grid LayoutRoot;

		// Token: 0x04000003 RID: 3
		internal Grid ContentPanel;

		// Token: 0x04000004 RID: 4
		private bool _contentLoaded;
	}
}
