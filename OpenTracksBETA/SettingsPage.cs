using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA
{
	// Token: 0x02000013 RID: 19
	public class SettingsPage : PhoneApplicationPage
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x000067FC File Offset: 0x000049FC
		public SettingsPage()
		{
			this.InitializeComponent();
			base.Loaded += new RoutedEventHandler(this.SettingsPage_Loaded);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006830 File Offset: 0x00004A30
		private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
		{
			List<LanguageOption> list = new List<LanguageOption>();
			list.Add(new LanguageOption
			{
				Name = "English",
				CultureCode = "en-US"
			});
			list.Add(new LanguageOption
			{
				Name = "中文 (香港)",
				CultureCode = "zh-HK"
			});
			list.Add(new LanguageOption
			{
				Name = "Spanish",
				CultureCode = "es"
			});
			list.Add(new LanguageOption
			{
				Name = "Ukranian",
				CultureCode = "uk-UA"
			});
			list.Add(new LanguageOption
			{
				Name = "Hungarian",
				CultureCode = "hu-HU"
			});
			list.Add(new LanguageOption
			{
				Name = "Poland",
				CultureCode = "pl-PL"
			});
			list.Add(new LanguageOption
			{
				Name = "German",
				CultureCode = "de"
			});
			list.Add(new LanguageOption
			{
				Name = "Russian (Incompleted)",
				CultureCode = "ru-RU"
			});
			List<LanguageOption> list2 = list;
			this.LanguageListBox.ItemsSource = list2;
			bool flag = IsolatedStorageSettings.ApplicationSettings.Contains("AppLanguage");
			if (flag)
			{
				string savedCulture = IsolatedStorageSettings.ApplicationSettings["AppLanguage"] as string;
				LanguageOption languageOption = Enumerable.FirstOrDefault<LanguageOption>(list2, (LanguageOption l) => l.CultureCode == savedCulture);
				bool flag2 = languageOption != null;
				if (flag2)
				{
					this.LanguageListBox.SelectedItem = languageOption;
				}
			}
			this.isInitializing = false;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000069D9 File Offset: 0x00004BD9
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000069E4 File Offset: 0x00004BE4
		private void LanguageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.isInitializing;
			if (!flag)
			{
				LanguageOption languageOption = this.LanguageListBox.SelectedItem as LanguageOption;
				bool flag2 = languageOption != null && !string.IsNullOrEmpty(languageOption.CultureCode);
				if (flag2)
				{
					IsolatedStorageSettings.ApplicationSettings["AppLanguage"] = languageOption.CultureCode;
					IsolatedStorageSettings.ApplicationSettings.Save();
					Thread.CurrentThread.CurrentCulture = new CultureInfo(languageOption.CultureCode);
					Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageOption.CultureCode);
					MessageBox.Show("Language set to: " + languageOption.Name);
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006A90 File Offset: 0x00004C90
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

		// Token: 0x060000CE RID: 206 RVA: 0x00006AD5 File Offset: 0x00004CD5
		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			e.Cancel = true;
			this.SlideOutAndNavigate();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006AE7 File Offset: 0x00004CE7
		private void Home_Click(object sender, EventArgs e)
		{
			this.SlideOutAndNavigate();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006AF1 File Offset: 0x00004CF1
		private void EQ_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/EqualizerPage.xaml", 2));
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006B0B File Offset: 0x00004D0B
		private void ClearCacheButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Cache cleared successfully!");
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006B19 File Offset: 0x00004D19
		private void HTTPButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Restart your App to continue... Note: if you want to go back to https mode for safe use, please restart your app once you've finish everything on http mode");
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006B28 File Offset: 0x00004D28
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/SettingsPage.xaml", 2));
				this.SlideOutStoryboard = (Storyboard)base.FindName("SlideOutStoryboard");
				this.LayoutRoot2 = (Grid)base.FindName("LayoutRoot2");
				this.SlideTransform = (TranslateTransform)base.FindName("SlideTransform");
				this.LanguageListBox = (ListBox)base.FindName("LanguageListBox");
				this.LayoutRoot41 = (Grid)base.FindName("LayoutRoot41");
				this.UpdateStatus = (TextBlock)base.FindName("UpdateStatus");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x0400008C RID: 140
		private bool isNavigatingBack = false;

		// Token: 0x0400008D RID: 141
		private bool isInitializing = true;

		// Token: 0x0400008E RID: 142
		internal Storyboard SlideOutStoryboard;

		// Token: 0x0400008F RID: 143
		internal Grid LayoutRoot2;

		// Token: 0x04000090 RID: 144
		internal TranslateTransform SlideTransform;

		// Token: 0x04000091 RID: 145
		internal ListBox LanguageListBox;

		// Token: 0x04000092 RID: 146
		internal Grid LayoutRoot41;

		// Token: 0x04000093 RID: 147
		internal TextBlock UpdateStatus;

		// Token: 0x04000094 RID: 148
		internal MediaElement AudioPlayer;

		// Token: 0x04000095 RID: 149
		private bool _contentLoaded;
	}
}
