using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OpenTracksBETA.Resources;
using OpenTracksBETA.ViewModels;
using Windows.ApplicationModel.Activation;

namespace OpenTracksBETA
{
	// Token: 0x02000005 RID: 5
	public class App : Application
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000259C File Offset: 0x0000079C
		public static MainViewModel ViewModel
		{
			get
			{
				bool flag = App.viewModel == null;
				if (flag)
				{
					App.viewModel = new MainViewModel();
				}
				return App.viewModel;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000025C9 File Offset: 0x000007C9
		// (set) Token: 0x06000015 RID: 21 RVA: 0x000025D0 File Offset: 0x000007D0
		public static PhoneApplicationFrame RootFrame { get; private set; }

		// Token: 0x06000016 RID: 22 RVA: 0x000025D8 File Offset: 0x000007D8
		public App()
		{
			base.UnhandledException += new EventHandler<ApplicationUnhandledExceptionEventArgs>(this.Application_UnhandledException);
			this.InitializeComponent();
			this.InitializePhoneApplication();
			this.InitializeLanguage();
			bool isAttached = Debugger.IsAttached;
			if (isAttached)
			{
				Application.Current.Host.Settings.EnableFrameRateCounter = true;
				PhoneApplicationService.Current.UserIdleDetectionMode = 1;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000264C File Offset: 0x0000084C
		private void ApplySavedCulture()
		{
			bool flag = IsolatedStorageSettings.ApplicationSettings.Contains("AppLanguage");
			if (flag)
			{
				string text = IsolatedStorageSettings.ApplicationSettings["AppLanguage"] as string;
				bool flag2 = !string.IsNullOrEmpty(text);
				if (flag2)
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo(text);
					Thread.CurrentThread.CurrentUICulture = new CultureInfo(text);
				}
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026B5 File Offset: 0x000008B5
		private void Application_ContractActivated(object sender, IActivatedEventArgs e)
		{
			this.ApplySavedCulture();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026C0 File Offset: 0x000008C0
		private void Application_Launching(object sender, LaunchingEventArgs e)
		{
			PhoneApplicationService.Current.ApplicationIdleDetectionMode = 1;
			bool flag = IsolatedStorageSettings.ApplicationSettings.Contains("AppLanguage");
			if (flag)
			{
				string text = IsolatedStorageSettings.ApplicationSettings["AppLanguage"] as string;
				bool flag2 = !string.IsNullOrEmpty(text);
				if (flag2)
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo(text);
					Thread.CurrentThread.CurrentUICulture = new CultureInfo(text);
				}
			}
			this.ApplySavedCulture();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000273C File Offset: 0x0000093C
		private void Application_Activated(object sender, ActivatedEventArgs e)
		{
			PhoneApplicationService.Current.ApplicationIdleDetectionMode = 1;
			bool flag = IsolatedStorageSettings.ApplicationSettings.Contains("AppLanguage");
			if (flag)
			{
				string text = IsolatedStorageSettings.ApplicationSettings["AppLanguage"] as string;
				bool flag2 = !string.IsNullOrEmpty(text);
				if (flag2)
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo(text);
					Thread.CurrentThread.CurrentUICulture = new CultureInfo(text);
				}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027B1 File Offset: 0x000009B1
		private void Application_Deactivated(object sender, DeactivatedEventArgs e)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027B1 File Offset: 0x000009B1
		private void Application_Closing(object sender, ClosingEventArgs e)
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000027B4 File Offset: 0x000009B4
		private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			bool isAttached = Debugger.IsAttached;
			if (isAttached)
			{
				Debugger.Break();
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			bool isAttached = Debugger.IsAttached;
			if (isAttached)
			{
				Debugger.Break();
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027F4 File Offset: 0x000009F4
		private void InitializePhoneApplication()
		{
			bool flag = this.phoneApplicationInitialized;
			if (!flag)
			{
				App.RootFrame = new TransitionFrame();
				App.RootFrame.Navigated += new NavigatedEventHandler(this.CompleteInitializePhoneApplication);
				App.RootFrame.NavigationFailed += new NavigationFailedEventHandler(this.RootFrame_NavigationFailed);
				App.RootFrame.Navigated += new NavigatedEventHandler(this.CheckForResetNavigation);
				PhoneApplicationService.Current.ContractActivated += new EventHandler<IActivatedEventArgs>(this.Application_ContractActivated);
				this.phoneApplicationInitialized = true;
				PhoneApplicationService.Current.ApplicationIdleDetectionMode = 1;
				bool flag2 = IsolatedStorageSettings.ApplicationSettings.Contains("AppLanguage");
				if (flag2)
				{
					string text = IsolatedStorageSettings.ApplicationSettings["AppLanguage"] as string;
					bool flag3 = !string.IsNullOrEmpty(text);
					if (flag3)
					{
						Thread.CurrentThread.CurrentCulture = new CultureInfo(text);
						Thread.CurrentThread.CurrentUICulture = new CultureInfo(text);
					}
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000028E8 File Offset: 0x00000AE8
		private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
		{
			bool flag = base.RootVisual != App.RootFrame;
			if (flag)
			{
				base.RootVisual = App.RootFrame;
			}
			App.RootFrame.Navigated -= new NavigatedEventHandler(this.CompleteInitializePhoneApplication);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002930 File Offset: 0x00000B30
		private void CheckForResetNavigation(object sender, NavigationEventArgs e)
		{
			bool flag = e.NavigationMode == 4;
			if (flag)
			{
				App.RootFrame.Navigated += new NavigatedEventHandler(this.ClearBackStackAfterReset);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002964 File Offset: 0x00000B64
		private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
		{
			App.RootFrame.Navigated -= new NavigatedEventHandler(this.ClearBackStackAfterReset);
			bool flag = e.NavigationMode != null && e.NavigationMode != 3;
			if (!flag)
			{
				while (App.RootFrame.RemoveBackEntry() != null)
				{
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029BC File Offset: 0x00000BBC
		private void InitializeLanguage()
		{
			try
			{
				App.RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);
				FlowDirection flowDirection = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
				App.RootFrame.FlowDirection = flowDirection;
				PhoneApplicationService.Current.ApplicationIdleDetectionMode = 1;
				bool flag = IsolatedStorageSettings.ApplicationSettings.Contains("AppLanguage");
				if (flag)
				{
					string text = IsolatedStorageSettings.ApplicationSettings["AppLanguage"] as string;
					bool flag2 = !string.IsNullOrEmpty(text);
					if (flag2)
					{
						Thread.CurrentThread.CurrentCulture = new CultureInfo(text);
						Thread.CurrentThread.CurrentUICulture = new CultureInfo(text);
					}
				}
			}
			catch
			{
				bool isAttached = Debugger.IsAttached;
				if (isAttached)
				{
					Debugger.Break();
				}
				throw;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002A98 File Offset: 0x00000C98
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/App.xaml", 2));
			}
		}

		// Token: 0x04000010 RID: 16
		private static MainViewModel viewModel = null;

		// Token: 0x04000011 RID: 17
		public static int LoggedInUserId = 0;

		// Token: 0x04000013 RID: 19
		private bool phoneApplicationInitialized = false;

		// Token: 0x04000014 RID: 20
		private bool _contentLoaded;
	}
}
