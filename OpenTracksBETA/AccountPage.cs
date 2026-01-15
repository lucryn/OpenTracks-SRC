using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;

namespace OpenTracksBETA
{
	// Token: 0x02000004 RID: 4
	public class AccountPage : PhoneApplicationPage
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000022BA File Offset: 0x000004BA
		public AccountPage()
		{
			this.InitializeComponent();
			base.Loaded += delegate(object s, RoutedEventArgs e)
			{
				this.SlideInStoryboard.Begin();
			};
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022E5 File Offset: 0x000004E5
		private void AccountPage_Loaded(object sender, RoutedEventArgs e)
		{
			this.SlideInStoryboard.Begin();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022F4 File Offset: 0x000004F4
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

		// Token: 0x0600000D RID: 13 RVA: 0x00002339 File Offset: 0x00000539
		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			e.Cancel = true;
			this.SlideOutAndNavigate();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000234C File Offset: 0x0000054C
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			this.SlideInStoryboard.Begin();
			string text = "";
			bool flag = base.NavigationContext.QueryString.ContainsKey("email");
			if (flag)
			{
				text = base.NavigationContext.QueryString["email"];
			}
			string text2 = "abc123";
			string deviceName = DeviceStatus.DeviceName;
			string text3 = Environment.OSVersion.ToString();
			string deviceManufacturer = DeviceStatus.DeviceManufacturer;
			this.EmailText.Text = "Email: " + text;
			this.UserIdText.Text = "User ID: " + text2;
			this.DeviceNameText.Text = "Device Name: " + deviceName;
			this.OSVersionText.Text = "OS Version: " + text3;
			this.ManufacturerText.Text = "Manufacturer: " + deviceManufacturer;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002438 File Offset: 0x00000638
		private void Logout_Click(object sender, RoutedEventArgs e)
		{
			IsolatedStorageSettings.ApplicationSettings.Remove("email");
			IsolatedStorageSettings.ApplicationSettings.Save();
			MessageBox.Show("✔ Logged out successfully! we will now restart your app.");
			base.NavigationService.Navigate(new Uri("/LoginPage.xaml", 2));
			Application.Current.Terminate();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002490 File Offset: 0x00000690
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/AccountPage.xaml", 2));
				this.SlideInStoryboard = (Storyboard)base.FindName("SlideInStoryboard");
				this.SlideOutStoryboard = (Storyboard)base.FindName("SlideOutStoryboard");
				this.MainContent = (Grid)base.FindName("MainContent");
				this.SlideTransform = (TranslateTransform)base.FindName("SlideTransform");
				this.EmailText = (TextBlock)base.FindName("EmailText");
				this.UserIdText = (TextBlock)base.FindName("UserIdText");
				this.DeviceNameText = (TextBlock)base.FindName("DeviceNameText");
				this.OSVersionText = (TextBlock)base.FindName("OSVersionText");
				this.ManufacturerText = (TextBlock)base.FindName("ManufacturerText");
			}
		}

		// Token: 0x04000005 RID: 5
		private bool isNavigatingBack = false;

		// Token: 0x04000006 RID: 6
		internal Storyboard SlideInStoryboard;

		// Token: 0x04000007 RID: 7
		internal Storyboard SlideOutStoryboard;

		// Token: 0x04000008 RID: 8
		internal Grid MainContent;

		// Token: 0x04000009 RID: 9
		internal TranslateTransform SlideTransform;

		// Token: 0x0400000A RID: 10
		internal TextBlock EmailText;

		// Token: 0x0400000B RID: 11
		internal TextBlock UserIdText;

		// Token: 0x0400000C RID: 12
		internal TextBlock DeviceNameText;

		// Token: 0x0400000D RID: 13
		internal TextBlock OSVersionText;

		// Token: 0x0400000E RID: 14
		internal TextBlock ManufacturerText;

		// Token: 0x0400000F RID: 15
		private bool _contentLoaded;
	}
}
