using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Newtonsoft.Json.Linq;

namespace OpenTracksBETA
{
	// Token: 0x0200000B RID: 11
	public class LoginPage : PhoneApplicationPage
	{
		// Token: 0x06000043 RID: 67 RVA: 0x0000326C File Offset: 0x0000146C
		public LoginPage()
		{
			this.InitializeComponent();
			base.Loaded += new RoutedEventHandler(this.LoginPage_Loaded);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003290 File Offset: 0x00001490
		private void LoginPage_Loaded(object sender, RoutedEventArgs e)
		{
			IsolatedStorageSettings applicationSettings = IsolatedStorageSettings.ApplicationSettings;
			bool flag = applicationSettings != null && applicationSettings.Contains("email");
			if (flag)
			{
				string text = applicationSettings["email"] as string;
				bool flag2 = !string.IsNullOrEmpty(text);
				if (flag2)
				{
					string text2 = string.Format("/AccountPage.xaml?email={0}", text);
					base.NavigationService.Navigate(new Uri(text2, 2));
					base.NavigationService.RemoveBackEntry();
				}
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003308 File Offset: 0x00001508
		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string email = this.EmailBox.Text;
			string password = this.PasswordBox.Password;
			JObject jobject = new JObject
			{
				{
					"email",
					email
				},
				{
					"password",
					password
				}
			};
			WebClient webClient = new WebClient();
			webClient.Headers["Content-Type"] = "application/json";
			webClient.UploadStringCompleted += delegate(object s, UploadStringCompletedEventArgs args)
			{
				bool flag = args.Error == null;
				if (flag)
				{
					IsolatedStorageSettings.ApplicationSettings["email"] = email;
					IsolatedStorageSettings.ApplicationSettings.Save();
					this.NavigationService.Navigate(new Uri("/AccountPage.xaml?email=" + email, 2));
					MessageBox.Show("✔ Login successful! We will now restart your app to unlock features for you.");
					Application.Current.Terminate();
				}
				else
				{
					MessageBox.Show("❌ Login failed. Maybe try re-login again? or check your password?");
				}
			};
			webClient.UploadStringAsync(new Uri("https://otsacc.vercel.app/api/login"), "POST", jobject.ToString());
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000033BC File Offset: 0x000015BC
		private void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			string email = this.RegisterEmailBox.Text;
			string password = this.RegisterPasswordBox.Password;
			bool flag = string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password);
			if (flag)
			{
				MessageBox.Show("Please fill in all fields.");
			}
			else
			{
				JObject jobject = new JObject
				{
					{
						"email",
						email
					},
					{
						"password",
						password
					}
				};
				WebClient webClient = new WebClient();
				webClient.Headers["Content-Type"] = "application/json";
				webClient.UploadStringCompleted += delegate(object s, UploadStringCompletedEventArgs args)
				{
					bool flag2 = args.Error == null;
					if (flag2)
					{
						IsolatedStorageSettings.ApplicationSettings["email"] = email;
						IsolatedStorageSettings.ApplicationSettings.Save();
						MessageBox.Show("✔ Registration successful! Please check your email for confirmation. and then you can login to your opentracks account");
					}
					else
					{
						MessageBox.Show("❌ Registration failed: " + args.Error.Message);
					}
				};
				webClient.UploadStringAsync(new Uri("https://otsacc.vercel.app/api/register"), "POST", jobject.ToString());
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003494 File Offset: 0x00001694
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/LoginPage.xaml", 2));
				this.LayoutRoot30 = (Grid)base.FindName("LayoutRoot30");
				this.EmailBox = (TextBox)base.FindName("EmailBox");
				this.PasswordBox = (PasswordBox)base.FindName("PasswordBox");
				this.RegisterEmailBox = (TextBox)base.FindName("RegisterEmailBox");
				this.RegisterPasswordBox = (PasswordBox)base.FindName("RegisterPasswordBox");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x04000029 RID: 41
		internal Grid LayoutRoot30;

		// Token: 0x0400002A RID: 42
		internal TextBox EmailBox;

		// Token: 0x0400002B RID: 43
		internal PasswordBox PasswordBox;

		// Token: 0x0400002C RID: 44
		internal TextBox RegisterEmailBox;

		// Token: 0x0400002D RID: 45
		internal PasswordBox RegisterPasswordBox;

		// Token: 0x0400002E RID: 46
		internal MediaElement AudioPlayer;

		// Token: 0x0400002F RID: 47
		private bool _contentLoaded;
	}
}
