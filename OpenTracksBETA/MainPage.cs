using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using OpenTracksBETA.ViewModels;

namespace OpenTracksBETA
{
	// Token: 0x0200000C RID: 12
	public class MainPage : PhoneApplicationPage
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000048 RID: 72 RVA: 0x0000354F File Offset: 0x0000174F
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00003557 File Offset: 0x00001757
		public MainViewModel ViewModel { get; private set; }

		// Token: 0x0600004A RID: 74 RVA: 0x00003560 File Offset: 0x00001760
		public MainPage()
		{
			this.InitializeComponent();
			PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(this.Application_Deactivated);
			PhoneApplicationService.Current.Activated += new EventHandler<ActivatedEventArgs>(this.Application_Activated);
			this.CheckLastReminder();
			this.StartReminderTimer();
			string text = Environment.OSVersion.ToString();
			bool flag = text == "Windows.Mobile";
			if (flag)
			{
				MessageBox.Show("Warning: Windows Mobile 10 is no longer supported by OpenTracks. Please use Windows Phone 8.1 or OpenTracks Lite.");
				Application.Current.Terminate();
			}
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1.0);
			timer.Tick += delegate(object s, EventArgs e)
			{
				timer.Stop();
				bool flag4 = IsolatedStorageSettings.ApplicationSettings.Contains("email");
				if (flag4)
				{
					this.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
				}
				else
				{
					string text2 = "Welcome to OpenTracks! Would you like to sign in or continue without an account? Features like favourites and explore hub require an account. This message will appear every time you start the app if you don't sign in.";
					string text3 = "Welcome to OpenTracks!";
					MessageBoxResult messageBoxResult = MessageBox.Show(text2, text3, 1);
					bool flag5 = messageBoxResult == 1;
					if (flag5)
					{
						this.NavigationService.Navigate(new Uri("/LoginPage.xaml", 2));
					}
					else
					{
						this.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
					}
				}
			};
			timer.Start();
			bool flag2 = IsolatedStorageSettings.ApplicationSettings.Contains("email");
			this.SignedInContent1.Visibility = (flag2 ? 0 : 1);
			this.GuestMessage1.Visibility = (flag2 ? 1 : 0);
			this.SignedInContent2.Visibility = (flag2 ? 0 : 1);
			this.GuestMessage2.Visibility = (flag2 ? 1 : 0);
			this.SignedInContent3.Visibility = (flag2 ? 0 : 1);
			this.og.Visibility = (flag2 ? 0 : 1);
			this.GuestMessage3.Visibility = (flag2 ? 1 : 0);
			this.SetRandomBackground();
			this.ViewModel = new MainViewModel();
			base.DataContext = this.ViewModel;
			this.AudioPlayer.MediaOpened += new RoutedEventHandler(this.AudioPlayer_MediaOpened);
			this.AudioPlayer.MediaEnded += new RoutedEventHandler(this.AudioPlayer_MediaEnded);
			this.AudioPlayer.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(this.AudioPlayer_MediaFailed);
			this.progressTimer = new DispatcherTimer();
			this.progressTimer.Interval = TimeSpan.FromSeconds(0.1);
			this.progressTimer.Tick += new EventHandler(this.ProgressTimer_Tick);
			this.AudioPlayer.MediaFailed += delegate(object s, ExceptionRoutedEventArgs e)
			{
				string text2 = "Media failed: ";
				Exception errorException = e.ErrorException;
				Debug.WriteLine(text2 + ((errorException != null) ? errorException.Message : null));
				base.Dispatcher.BeginInvoke(delegate()
				{
					base.NavigationService.Navigate(new Uri("/Crash/mediafail.xaml", 2));
				});
			};
			this.AudioPlayer.MediaOpened += delegate(object s, RoutedEventArgs e)
			{
				Debug.WriteLine("Media opened, starting playback");
				this.AudioPlayer.Play();
			};
			this.AudioPlayer.MediaEnded += delegate(object s, RoutedEventArgs e)
			{
				Debug.WriteLine("Media ended, restarting");
				this.AudioPlayer.Position = TimeSpan.Zero;
				this.AudioPlayer.Play();
			};
			ShellTile shellTile = Enumerable.FirstOrDefault<ShellTile>(ShellTile.ActiveTiles);
			bool flag3 = shellTile != null;
			if (flag3)
			{
				try
				{
					StandardTileData standardTileData = new StandardTileData
					{
						Title = "OpenTracks",
						BackTitle = "\ud83d\udce3 Notice",
						BackContent = "Initial D will be the best and ever racing and drifting anime in history..AE86..",
						Count = new int?(1)
					};
					shellTile.Update(standardTileData);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Tile update failed: " + ex.Message);
				}
			}
			base.Loaded += new RoutedEventHandler(this.MainPage_Loaded);
			this.LoadFavourites();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000038CC File Offset: 0x00001ACC
		private void NavigateToDJ_Tap(object sender, GestureEventArgs e)
		{
			bool flag = this.IsUserLoggedIn();
			if (flag)
			{
				base.NavigationService.Navigate(new Uri("/Personal_DJ.xaml", 2));
			}
			else
			{
				this.ShowLoginPrompt();
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003908 File Offset: 0x00001B08
		private void NavigateToSettings_Tap(object sender, GestureEventArgs e)
		{
			bool flag = this.IsUserLoggedIn();
			if (flag)
			{
				base.NavigationService.Navigate(new Uri("/marketplace.xaml", 2));
			}
			else
			{
				this.ShowLoginPrompt();
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003944 File Offset: 0x00001B44
		private void TextBlock_Tap(object sender, GestureEventArgs e)
		{
			bool flag = this.IsUserLoggedIn();
			if (flag)
			{
				base.NavigationService.Navigate(new Uri("/AccountPage.xaml", 2));
			}
			else
			{
				this.ShowLoginPrompt();
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003980 File Offset: 0x00001B80
		private void TextBlock1_Tap(object sender, GestureEventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/About.xaml", 2));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000399A File Offset: 0x00001B9A
		private void TextBlock2_Tap(object sender, GestureEventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/SettingsPage.xaml", 2));
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000039B4 File Offset: 0x00001BB4
		private void NavigateToTracks_Tap(object sender, GestureEventArgs e)
		{
			bool flag = this.IsUserLoggedIn();
			if (flag)
			{
				base.NavigationService.Navigate(new Uri("/BrowsePage.xaml", 2));
			}
			else
			{
				this.ShowLoginPrompt();
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000039F0 File Offset: 0x00001BF0
		private void SaveFavourites()
		{
			IsolatedStorageSettings applicationSettings = IsolatedStorageSettings.ApplicationSettings;
			List<string> list = Enumerable.ToList<string>(Enumerable.Select<TrackItem, string>(this.ViewModel.Favourites, (TrackItem t) => t.AudioPath));
			applicationSettings["Favourites"] = list;
			applicationSettings.Save();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003A50 File Offset: 0x00001C50
		private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
		{
			Border border = sender as Border;
			bool flag = border != null;
			if (flag)
			{
				border.Background = new SolidColorBrush(Color.FromArgb(102, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003A94 File Offset: 0x00001C94
		private void MenuItem_MouseLeave(object sender, MouseEventArgs e)
		{
			Border border = sender as Border;
			bool flag = border != null;
			if (flag)
			{
				border.Background = new SolidColorBrush(Color.FromArgb(34, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003AD8 File Offset: 0x00001CD8
		private void LoadFavourites()
		{
			IsolatedStorageSettings applicationSettings = IsolatedStorageSettings.ApplicationSettings;
			bool flag = applicationSettings.Contains("Favourites");
			if (flag)
			{
				List<string> list = applicationSettings["Favourites"] as List<string>;
				bool flag2 = list != null;
				if (flag2)
				{
					this.ViewModel.Favourites.Clear();
					foreach (string text in list)
					{
						this.ViewModel.Favourites.Add(new TrackItem
						{
							Title = Path.GetFileNameWithoutExtension(text),
							AudioPath = text
						});
					}
				}
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003B9C File Offset: 0x00001D9C
		private void MinimizeNowPlaying_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("MinimizeNowPlaying_Click: Minimizing full player");
			try
			{
				Debug.WriteLine(string.Format("NowPlayingPanel.Visibility before: {0}", this.NowPlayingPanel.Visibility));
				DoubleAnimation doubleAnimation = new DoubleAnimation
				{
					From = new double?(this.NowPlayingTransform.Y),
					To = new double?(800.0),
					Duration = TimeSpan.FromMilliseconds(300.0),
					EasingFunction = new QuadraticEase
					{
						EasingMode = 2
					}
				};
				Storyboard.SetTarget(doubleAnimation, this.NowPlayingTransform);
				Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(TranslateTransform.YProperty));
				Storyboard storyboard = new Storyboard();
				storyboard.Children.Add(doubleAnimation);
				storyboard.Completed += delegate(object s, EventArgs args)
				{
					Debug.WriteLine("Minimize animation completed, hiding panel");
					this.NowPlayingPanel.Visibility = 1;
					Debug.WriteLine(string.Format("NowPlayingPanel.Visibility after: {0}", this.NowPlayingPanel.Visibility));
				};
				storyboard.Begin();
				Debug.WriteLine("Minimize animation started");
			}
			catch (Exception ex)
			{
				Debug.WriteLine(string.Format("MinimizeNowPlaying_Click ERROR: {0}", ex.Message));
				Debug.WriteLine(string.Format("Stack trace: {0}", ex.StackTrace));
				this.NowPlayingPanel.Visibility = 1;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003CE0 File Offset: 0x00001EE0
		private void MiniControlBarWrapper_Tap(object sender, GestureEventArgs e)
		{
			this.NowPlayingPanel.Visibility = 0;
			DoubleAnimation doubleAnimation = new DoubleAnimation
			{
				From = new double?(800.0),
				To = new double?(0.0),
				Duration = TimeSpan.FromMilliseconds(300.0),
				EasingFunction = new QuadraticEase
				{
					EasingMode = 2
				}
			};
			Storyboard.SetTarget(doubleAnimation, this.NowPlayingTransform);
			Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(TranslateTransform.YProperty));
			Storyboard storyboard = new Storyboard();
			storyboard.Children.Add(doubleAnimation);
			storyboard.Begin();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003D94 File Offset: 0x00001F94
		private Color GetDominantColor(BitmapImage image)
		{
			Color result;
			try
			{
				WriteableBitmap writeableBitmap = new WriteableBitmap(image);
				int num = writeableBitmap.PixelWidth * writeableBitmap.PixelHeight;
				int[] pixels = writeableBitmap.Pixels;
				long num2 = 0L;
				long num3 = 0L;
				long num4 = 0L;
				for (int i = 0; i < num; i += 1000)
				{
					int num5 = pixels[i];
					num2 += (long)(num5 >> 16 & 255);
					num3 += (long)(num5 >> 8 & 255);
					num4 += (long)(num5 & 255);
				}
				int num6 = num / 1000;
				result = Color.FromArgb(byte.MaxValue, (byte)(num2 / (long)num6), (byte)(num3 / (long)num6), (byte)(num4 / (long)num6));
			}
			catch
			{
				result = Colors.Black;
			}
			return result;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003E6C File Offset: 0x0000206C
		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			e.Cancel = true;
			MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", 1);
			bool flag = messageBoxResult == 1;
			if (flag)
			{
				Application.Current.Terminate();
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003EA8 File Offset: 0x000020A8
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			this.LayoutRoot.Opacity = 0.0;
			Storyboard storyboard = new Storyboard();
			DoubleAnimation doubleAnimation = new DoubleAnimation
			{
				From = new double?(0.0),
				To = new double?(1.0),
				Duration = TimeSpan.FromSeconds(0.4)
			};
			Storyboard.SetTarget(doubleAnimation, this.LayoutRoot);
			Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity", new object[0]));
			storyboard.Children.Add(doubleAnimation);
			storyboard.Begin();
			while (base.NavigationService.CanGoBack)
			{
				base.NavigationService.RemoveBackEntry();
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003F74 File Offset: 0x00002174
		private void NavigateWithSlideOut(Uri targetUri)
		{
			bool flag = !this._isNavigating;
			if (flag)
			{
				this._isNavigating = true;
				this._pendingNavigationUri = targetUri;
				this.SlideOutStoryboard.Completed += new EventHandler(this.SlideOutStoryboard_Completed);
				this.SlideOutStoryboard.Begin();
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003FC4 File Offset: 0x000021C4
		private void SlideOutStoryboard_Completed(object sender, EventArgs e)
		{
			this.SlideOutStoryboard.Completed -= new EventHandler(this.SlideOutStoryboard_Completed);
			this._isNavigating = false;
			bool flag = this._pendingNavigationUri != null;
			if (flag)
			{
				base.Dispatcher.BeginInvoke(delegate()
				{
					base.NavigationService.Navigate(this._pendingNavigationUri);
					this._pendingNavigationUri = null;
				});
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000401C File Offset: 0x0000221C
		private bool IsUserLoggedIn()
		{
			return IsolatedStorageSettings.ApplicationSettings.Contains("email");
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004040 File Offset: 0x00002240
		private void ShowLoginPrompt()
		{
			string text = "This feature requires an OpenTracks account. Would you like to sign in now?";
			string text2 = "Sign In Required";
			MessageBoxResult messageBoxResult = MessageBox.Show(text, text2, 1);
			bool flag = messageBoxResult == 1;
			if (flag)
			{
				base.NavigationService.Navigate(new Uri("/LoginPage.xaml", 2));
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004084 File Offset: 0x00002284
		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string text = this.SearchBox.Text.Trim();
			this.ViewModel.FilterTracks(text);
			bool flag = !string.IsNullOrWhiteSpace(text);
			if (flag)
			{
				this.SearchResults.Visibility = 0;
				this.SignedInContent3.Visibility = 1;
				this.SearchResults.ItemsSource = this.ViewModel.FilteredTracks;
			}
			else
			{
				this.SearchResults.Visibility = 1;
				this.SignedInContent3.Visibility = 0;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004110 File Offset: 0x00002310
		private void TracksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			bool flag = listBox != null;
			if (flag)
			{
				TrackItem trackItem = listBox.SelectedItem as TrackItem;
				bool flag2 = trackItem != null && !string.IsNullOrEmpty(trackItem.AudioPath);
				if (flag2)
				{
					this.PlayTrack(trackItem, "Search Results");
					this.SearchBox.Text = "";
					this.SearchResults.Visibility = 1;
					this.SignedInContent3.Visibility = 0;
				}
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004190 File Offset: 0x00002390
		private void OpenTracksM_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.OpenTracksM.SelectedItem != this.TracksItem;
			if (flag)
			{
				this.SearchBox.Text = "";
				this.SearchResults.Visibility = 1;
				this.SignedInContent3.Visibility = 0;
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000041E8 File Offset: 0x000023E8
		private void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			bool flag = !NetworkInterface.GetIsNetworkAvailable();
			if (flag)
			{
				base.Dispatcher.BeginInvoke(delegate()
				{
					base.NavigationService.Navigate(new Uri("/NoNetworkPage.xaml", 2));
				});
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000421C File Offset: 0x0000241C
		private void StartReminderTimer()
		{
			this.reminderTimer = new DispatcherTimer();
			this.reminderTimer.Interval = TimeSpan.FromMinutes(15.0);
			this.reminderTimer.Tick += delegate(object s, EventArgs e)
			{
				this.ShowRandomReminderToast();
				this.SaveReminderTimestamp();
			};
			this.reminderTimer.Start();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004274 File Offset: 0x00002474
		private void ShowRandomReminderToast()
		{
			bool flag = this.reminderMessages != null && this.reminderMessages.Length != 0;
			if (flag)
			{
				string content = this.reminderMessages[this.rand.Next(this.reminderMessages.Length)];
				ShellToast shellToast = new ShellToast
				{
					Title = "\ud83d\udd14 Message from Reimu",
					Content = content
				};
				shellToast.Show();
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000042DC File Offset: 0x000024DC
		private void SaveReminderTimestamp()
		{
			IsolatedStorageSettings applicationSettings = IsolatedStorageSettings.ApplicationSettings;
			applicationSettings["LastReminder"] = DateTime.Now;
			applicationSettings.Save();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004310 File Offset: 0x00002510
		private void CheckLastReminder()
		{
			IsolatedStorageSettings applicationSettings = IsolatedStorageSettings.ApplicationSettings;
			bool flag = applicationSettings.Contains("LastReminder");
			if (flag)
			{
				DateTime dateTime = (DateTime)applicationSettings["LastReminder"];
				bool flag2 = (DateTime.Now - dateTime).TotalMinutes >= 15.0;
				if (flag2)
				{
					this.ShowRandomReminderToast();
					this.SaveReminderTimestamp();
				}
			}
			else
			{
				this.ShowRandomReminderToast();
				this.SaveReminderTimestamp();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004390 File Offset: 0x00002590
		private void Track_Hold(object sender, GestureEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			bool flag = frameworkElement != null;
			if (flag)
			{
				TrackItem heldTrack = frameworkElement.DataContext as TrackItem;
				bool flag2 = heldTrack != null && !string.IsNullOrWhiteSpace(heldTrack.AudioPath);
				if (flag2)
				{
					bool flag3 = !Enumerable.Any<TrackItem>(this.ViewModel.Favourites, (TrackItem item) => item.AudioPath == heldTrack.AudioPath);
					if (flag3)
					{
						TrackItem trackItem = new TrackItem
						{
							Title = heldTrack.Title,
							AudioPath = heldTrack.AudioPath
						};
						this.ViewModel.Favourites.Add(trackItem);
						this.SaveFavourites();
					}
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000445C File Offset: 0x0000265C
		private void RemoveFromFavourites(TrackItem track)
		{
			bool flag = track != null && !string.IsNullOrWhiteSpace(track.AudioPath);
			if (flag)
			{
				TrackItem trackItem = Enumerable.FirstOrDefault<TrackItem>(this.ViewModel.Favourites, (TrackItem t) => t.AudioPath == track.AudioPath);
				bool flag2 = trackItem != null;
				if (flag2)
				{
					this.ViewModel.Favourites.Remove(trackItem);
					this.SaveFavourites();
					MessageBox.Show("Track saved to favourites!");
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000044E8 File Offset: 0x000026E8
		private void ForwardButton_Click(object sender, RoutedEventArgs e)
		{
			ObservableCollection<TrackItem> allTracks = this.ViewModel.AllTracks;
			bool flag = allTracks != null && allTracks.Count != 0 && this.currentTrack != null;
			if (flag)
			{
				int num = allTracks.IndexOf(this.currentTrack);
				bool flag2 = num >= 0 && num < allTracks.Count - 1;
				if (flag2)
				{
					this.currentTrack = allTracks[num + 1];
					this.PlayTrack(this.currentTrack, "All Tracks");
				}
				else
				{
					MessageBox.Show("You're at the last track.");
				}
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004574 File Offset: 0x00002774
		private void BackwardButton_Click(object sender, RoutedEventArgs e)
		{
			ObservableCollection<TrackItem> allTracks = this.ViewModel.AllTracks;
			bool flag = allTracks != null && allTracks.Count != 0 && this.currentTrack != null;
			if (flag)
			{
				int num = allTracks.IndexOf(this.currentTrack);
				bool flag2 = num > 0;
				if (flag2)
				{
					this.currentTrack = allTracks[num - 1];
					this.PlayTrack(this.currentTrack, "All Tracks");
				}
				else
				{
					MessageBox.Show("You're at the first track.");
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000045F4 File Offset: 0x000027F4
		private void FavouriteTrack_Hold(object sender, GestureEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			bool flag = frameworkElement != null;
			if (flag)
			{
				TrackItem trackItem = frameworkElement.DataContext as TrackItem;
				bool flag2 = trackItem != null && !string.IsNullOrWhiteSpace(trackItem.AudioPath);
				if (flag2)
				{
					MessageBoxResult messageBoxResult = MessageBox.Show(string.Format("Remove \"{0}\" from favourites?", trackItem.Title), "Confirm", 1);
					bool flag3 = messageBoxResult == 1;
					if (flag3)
					{
						this.RemoveFromFavourites(trackItem);
					}
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000466C File Offset: 0x0000286C
		private void SetRandomBackground()
		{
			string[] array = new string[]
			{
				"/Assets/bg1.png"
			};
			Random random = new Random();
			string text = array[random.Next(array.Length)];
			ImageBrush background = new ImageBrush
			{
				ImageSource = new BitmapImage(new Uri(text, 2)),
				Stretch = 3
			};
			this.OpenTracksM.Background = background;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000046CC File Offset: 0x000028CC
		private void StartAgent()
		{
			ScheduledActionService.Remove("OpenTracksAgent");
			PeriodicTask periodicTask = new PeriodicTask("OpenTracksAgent")
			{
				Description = "Plays music or sends notifications in background"
			};
			ScheduledActionService.Add(periodicTask);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003980 File Offset: 0x00001B80
		private void About_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/About.xaml", 2));
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004703 File Offset: 0x00002903
		private void dj_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/Personal_DJ.xaml", 2));
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000471D File Offset: 0x0000291D
		private void upload_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Please kindly visit via PC or other platform to: https://otsacc.vercel.app/upload.html for uploading musics.");
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000472B File Offset: 0x0000292B
		private void credits_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/credits.xaml", 2));
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004745 File Offset: 0x00002945
		private void help_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/HelpBotPage.xaml", 2));
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000475F File Offset: 0x0000295F
		private void frm_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/ForumPage.xaml", 2));
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004779 File Offset: 0x00002979
		private void profile_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/LoginPage.xaml", 2));
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004794 File Offset: 0x00002994
		private void NewTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TrackItem trackItem = this.NewTracks.SelectedItem as TrackItem;
			bool flag = trackItem != null;
			if (flag)
			{
				this.PlayTrack(trackItem, "NewTracks");
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000047CC File Offset: 0x000029CC
		private void Tracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TrackItem trackItem = this.Tracks.SelectedItem as TrackItem;
			bool flag = trackItem != null;
			if (flag)
			{
				this.PlayTrack(trackItem, "TOP 10 HIT TRACKS 2025/08");
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004804 File Offset: 0x00002A04
		private void AllTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TrackItem trackItem = this.AllTracks.SelectedItem as TrackItem;
			bool flag = trackItem != null;
			if (flag)
			{
				this.PlayTrack(trackItem, "All Tracks");
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000483C File Offset: 0x00002A3C
		private void FavouritesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TrackItem trackItem = (TrackItem)((ListBox)sender).SelectedItem;
			bool flag = trackItem != null;
			if (flag)
			{
				this.PlayTrack(trackItem, "Liked Songs");
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004874 File Offset: 0x00002A74
		public void PlayTrack(TrackItem track, string sourceName = "Unknown Source")
		{
			bool flag = track == null;
			if (flag)
			{
				Debug.WriteLine("PlayTrack called with null track");
			}
			else
			{
				try
				{
					Debug.WriteLine("PlayTrack: Starting playback for " + track.Title);
					Debug.WriteLine("Audio path: " + track.AudioPath);
					bool flag2 = track.AudioPath.Contains("youtube.com") || track.AudioPath.Contains("youtu.be");
					if (flag2)
					{
						this.PlayFallbackAudio(track);
					}
					else
					{
						this.AudioPlayer.Stop();
						this.AudioPlayer.Source = new Uri(track.AudioPath, 0);
						this.UpdatePlaybackUI(track, sourceName);
						Debug.WriteLine("PlayTrack: Source set, waiting for MediaOpened");
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("PlayTrack error: " + ex.Message);
					MessageBox.Show("Playback error: " + ex.Message);
				}
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000497C File Offset: 0x00002B7C
		private void PlayFallbackAudio(TrackItem track)
		{
			try
			{
				MessageBox.Show("Playing audio for: " + track.Title);
				this.AudioPlayer.Stop();
				this.AudioPlayer.Source = new Uri("/Assets/SampleMusic.mp3", 2);
				this.UpdatePlaybackUI(track, "YouTube");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Fallback playback error: " + ex.Message);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004A00 File Offset: 0x00002C00
		private void UpdatePlaybackUI(TrackItem track, string sourceName)
		{
			try
			{
				this.currentTrack = track;
				this.isPlaying = true;
				this.CurrentTrackName.Text = track.Title;
				this.NowPlayingTitle.Text = track.Title;
				this.NowPlayingArtist.Text = ((!string.IsNullOrEmpty(track.Artist)) ? track.Artist : "Unknown Artist");
				this.NowPlayingSource.Text = "Playing From " + sourceName;
				this.FullPlayPauseButton.Content = "❚❚";
				this.MiniPlayPauseButton.Content = "❚❚";
				this.PlayPauseText.Text = "❚❚";
				base.Dispatcher.BeginInvoke(delegate()
				{
					this.StartTrackNameScroll();
				});
				this.NowPlayingPanel.Visibility = 0;
				this.NowPlayingTransform.Y = 0.0;
				this.VolumeSlider.Value = 100.0;
				this.AudioPlayer.Volume = 1.0;
				this.progressTimer.Start();
				Debug.WriteLine("UI updated for track: " + track.Title);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("UpdatePlaybackUI error: " + ex.Message);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004B74 File Offset: 0x00002D74
		private void ProgressTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				bool flag = this.AudioPlayer.CurrentState == 3 && this.AudioPlayer.NaturalDuration.HasTimeSpan && !this.isDragging;
				if (flag)
				{
					TimeSpan position = this.AudioPlayer.Position;
					TimeSpan timeSpan = this.AudioPlayer.NaturalDuration.TimeSpan;
					bool flag2 = timeSpan.TotalSeconds > 0.0;
					if (flag2)
					{
						this.ProgressSlider.Maximum = timeSpan.TotalSeconds;
						this.ProgressSlider.Value = Math.Min(position.TotalSeconds, timeSpan.TotalSeconds);
						this.StartTimeLabel.Text = position.ToString("mm\\:ss");
						bool flag3 = timeSpan > position;
						if (flag3)
						{
							TimeSpan timeSpan2 = timeSpan - position;
							this.EndTimeLabel.Text = timeSpan2.ToString("mm\\:ss");
						}
						this.FullScreenProgressSlider.Maximum = timeSpan.TotalSeconds;
						this.FullScreenProgressSlider.Value = Math.Min(position.TotalSeconds, timeSpan.TotalSeconds);
						this.StartTimeLabel1.Text = position.ToString("mm\\:ss");
						bool flag4 = timeSpan > position;
						if (flag4)
						{
							TimeSpan timeSpan3 = timeSpan - position;
							this.EndTimeLabel1.Text = timeSpan3.ToString("mm\\:ss");
						}
						double num = position.TotalSeconds / timeSpan.TotalSeconds;
						this.ProgressFill.Width = num * 440.0;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("ProgressTimer error: " + ex.Message);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004D54 File Offset: 0x00002F54
		private void FullScreenProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			bool flag = !this.isDragging && this.AudioPlayer.NaturalDuration.HasTimeSpan;
			if (flag)
			{
				try
				{
					bool flag2 = Math.Abs(this.AudioPlayer.Position.TotalSeconds - e.NewValue) > 0.5;
					if (flag2)
					{
						this.AudioPlayer.Position = TimeSpan.FromSeconds(e.NewValue);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004DE8 File Offset: 0x00002FE8
		private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			try
			{
				this.AudioPlayer.Volume = e.NewValue / 100.0;
				this.VolumeLabel.Text = string.Format("{0:0}%", e.NewValue);
				this.VolumeFill.Width = e.NewValue / 100.0 * 440.0;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("VolumeSlider_ValueChanged error: " + ex.Message);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004E88 File Offset: 0x00003088
		private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.currentTrack != null;
			if (flag)
			{
				bool flag2 = this.isPlaying && this.AudioPlayer.CurrentState == 3;
				if (flag2)
				{
					this.AudioPlayer.Pause();
					this.FullPlayPauseButton.Content = "▶";
					this.MiniPlayPauseButton.Content = "▶";
					this.PlayPauseText.Text = "▶";
					this.isPlaying = false;
					this.progressTimer.Stop();
					bool flag3 = this.MarqueeStoryboard != null;
					if (flag3)
					{
						this.MarqueeStoryboard.Stop();
					}
				}
				else
				{
					bool flag4 = this.AudioPlayer.Source == null;
					if (flag4)
					{
						this.PlayTrack(this.currentTrack, "Resuming Playback");
					}
					else
					{
						this.AudioPlayer.Play();
						this.FullPlayPauseButton.Content = "❚❚";
						this.MiniPlayPauseButton.Content = "❚❚";
						this.PlayPauseText.Text = "❚❚";
						this.isPlaying = true;
						this.progressTimer.Start();
						this.StartTrackNameScroll();
					}
				}
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004FC4 File Offset: 0x000031C4
		private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			bool flag = !this.isDragging && this.AudioPlayer.NaturalDuration.HasTimeSpan;
			if (flag)
			{
				try
				{
					bool flag2 = Math.Abs(this.AudioPlayer.Position.TotalSeconds - e.NewValue) > 0.5;
					if (flag2)
					{
						this.AudioPlayer.Position = TimeSpan.FromSeconds(e.NewValue);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005058 File Offset: 0x00003258
		private void AudioPlayer_MediaOpened(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("AudioPlayer_MediaOpened: Media is ready");
			bool hasTimeSpan = this.AudioPlayer.NaturalDuration.HasTimeSpan;
			if (hasTimeSpan)
			{
				TimeSpan timeSpan = this.AudioPlayer.NaturalDuration.TimeSpan;
				this.EndTimeLabel1.Text = timeSpan.ToString("mm\\:ss");
				this.ProgressSlider.Maximum = timeSpan.TotalSeconds;
				this.progressTimer.Start();
				Debug.WriteLine("Media duration: " + timeSpan.ToString());
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000050F2 File Offset: 0x000032F2
		private void AudioPlayer_MediaEnded(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("AudioPlayer_MediaEnded: Playback completed");
			this.progressTimer.Stop();
			this.isPlaying = false;
			this.FullPlayPauseButton.Content = "▶︎";
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005124 File Offset: 0x00003324
		private void AudioPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			string text = "AudioPlayer_MediaFailed: ";
			Exception errorException = e.ErrorException;
			Debug.WriteLine(text + ((errorException != null) ? errorException.Message : null));
			string text2 = "Playback failed: ";
			Exception errorException2 = e.ErrorException;
			MessageBox.Show(text2 + ((errorException2 != null) ? errorException2.Message : null));
			this.isPlaying = false;
			this.FullPlayPauseButton.Content = "▶︎";
			this.progressTimer.Stop();
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000519A File Offset: 0x0000339A
		private void GoToLogin_Click(object sender, RoutedEventArgs e)
		{
			this.NavigateWithSlideOut(new Uri("/LoginPage.xaml", 0));
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000051B0 File Offset: 0x000033B0
		private void StartTrackNameScroll()
		{
			double actualWidth = this.CurrentTrackName.ActualWidth;
			double num = 372.0;
			bool flag = this.TrackTransform == null;
			if (!flag)
			{
				bool flag2 = actualWidth > num;
				if (flag2)
				{
					bool flag3 = this.MarqueeStoryboard.GetCurrentState() == 0;
					if (flag3)
					{
						this.MarqueeStoryboard.Stop();
					}
					this.TrackTransform.TranslateX = num;
					DoubleAnimation doubleAnimation = this.MarqueeStoryboard.Children[0] as DoubleAnimation;
					bool flag4 = doubleAnimation != null;
					if (flag4)
					{
						doubleAnimation.From = new double?(num);
						doubleAnimation.To = new double?(-actualWidth);
					}
					this.MarqueeStoryboard.Begin();
				}
				else
				{
					this.MarqueeStoryboard.Stop();
					this.TrackTransform.TranslateX = 0.0;
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005295 File Offset: 0x00003495
		private void Application_Deactivated(object sender, DeactivatedEventArgs e)
		{
			this.SavePlaybackState();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000529F File Offset: 0x0000349F
		private void Application_Activated(object sender, ActivatedEventArgs e)
		{
			this.RestorePlaybackState();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000052AC File Offset: 0x000034AC
		private TrackItem FindTrackByPath(string audioPath)
		{
			TrackItem trackItem = Enumerable.FirstOrDefault<TrackItem>(this.ViewModel.AllTracks, (TrackItem t) => t.AudioPath == audioPath);
			bool flag = trackItem != null;
			TrackItem result;
			if (flag)
			{
				result = trackItem;
			}
			else
			{
				trackItem = Enumerable.FirstOrDefault<TrackItem>(this.ViewModel.Tracks, (TrackItem t) => t.AudioPath == audioPath);
				bool flag2 = trackItem != null;
				if (flag2)
				{
					result = trackItem;
				}
				else
				{
					trackItem = Enumerable.FirstOrDefault<TrackItem>(this.ViewModel.NewTracks, (TrackItem t) => t.AudioPath == audioPath);
					bool flag3 = trackItem != null;
					if (flag3)
					{
						result = trackItem;
					}
					else
					{
						trackItem = Enumerable.FirstOrDefault<TrackItem>(this.ViewModel.Favourites, (TrackItem t) => t.AudioPath == audioPath);
						result = trackItem;
					}
				}
			}
			return result;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005368 File Offset: 0x00003568
		private void SavePlaybackState()
		{
			try
			{
				IsolatedStorageSettings applicationSettings = IsolatedStorageSettings.ApplicationSettings;
				bool flag = this.currentTrack != null && this.isPlaying;
				if (flag)
				{
					applicationSettings["LastTrackPath"] = this.currentTrack.AudioPath;
					applicationSettings["LastPosition"] = this.AudioPlayer.Position;
					applicationSettings["IsPlaying"] = this.isPlaying;
					Debug.WriteLine("Playback state saved: " + this.currentTrack.Title);
				}
				else
				{
					applicationSettings.Remove("LastTrackPath");
					applicationSettings.Remove("LastPosition");
					applicationSettings.Remove("IsPlaying");
				}
				applicationSettings.Save();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error saving playback state: " + ex.Message);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005458 File Offset: 0x00003658
		private void RestorePlaybackState()
		{
			try
			{
				IsolatedStorageSettings applicationSettings = IsolatedStorageSettings.ApplicationSettings;
				bool flag = applicationSettings.Contains("LastTrackPath") && applicationSettings.Contains("IsPlaying") && (bool)applicationSettings["IsPlaying"];
				if (flag)
				{
					string audioPath = applicationSettings["LastTrackPath"] as string;
					TimeSpan position = applicationSettings.Contains("LastPosition") ? ((TimeSpan)applicationSettings["LastPosition"]) : TimeSpan.Zero;
					TrackItem trackItem = this.FindTrackByPath(audioPath);
					bool flag2 = trackItem != null;
					if (flag2)
					{
						this.currentTrack = trackItem;
						this.CurrentTrackName.Text = trackItem.Title;
						this.NowPlayingTitle.Text = trackItem.Title;
						this.NowPlayingArtist.Text = trackItem.Artist;
						this.NowPlayingSource.Text = "Resumed Playback";
						bool flag3 = (bool)applicationSettings["IsPlaying"];
						if (flag3)
						{
							this.AudioPlayer.Source = new Uri(trackItem.AudioPath, 0);
							this.AudioPlayer.Position = position;
							Debug.WriteLine("Playback restored: " + trackItem.Title);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error restoring playback state: " + ex.Message);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000055D8 File Offset: 0x000037D8
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/MainPage.xaml", 2));
				this.SlideOutStoryboard = (Storyboard)base.FindName("SlideOutStoryboard");
				this.MarqueeStoryboard = (Storyboard)base.FindName("MarqueeStoryboard");
				this.SlideUpStoryboard = (Storyboard)base.FindName("SlideUpStoryboard");
				this.SlideDownStoryboard = (Storyboard)base.FindName("SlideDownStoryboard");
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
				this.SlideTransform = (TranslateTransform)base.FindName("SlideTransform");
				this.OpenTracksM = (Panorama)base.FindName("OpenTracksM");
				this.HomeItem = (PanoramaItem)base.FindName("HomeItem");
				this.LoginStatusPanel = (Border)base.FindName("LoginStatusPanel");
				this.LoggedInUserText = (TextBlock)base.FindName("LoggedInUserText");
				this.NewTracksItem = (PanoramaItem)base.FindName("NewTracksItem");
				this.NewTracksGrid = (Grid)base.FindName("NewTracksGrid");
				this.SignedInContent1 = (Grid)base.FindName("SignedInContent1");
				this.NewTracks = (ListBox)base.FindName("NewTracks");
				this.GuestMessage1 = (StackPanel)base.FindName("GuestMessage1");
				this.TopHitsItem = (PanoramaItem)base.FindName("TopHitsItem");
				this.TracksGrid = (Grid)base.FindName("TracksGrid");
				this.SignedInContent2 = (Grid)base.FindName("SignedInContent2");
				this.Tracks = (ListBox)base.FindName("Tracks");
				this.GuestMessage2 = (StackPanel)base.FindName("GuestMessage2");
				this.TracksItem = (PanoramaItem)base.FindName("TracksItem");
				this.AllTracksGrid = (Grid)base.FindName("AllTracksGrid");
				this.og = (StackPanel)base.FindName("og");
				this.SearchBox = (TextBox)base.FindName("SearchBox");
				this.SearchResults = (ListBox)base.FindName("SearchResults");
				this.SignedInContent3 = (Grid)base.FindName("SignedInContent3");
				this.AllTracks = (ListBox)base.FindName("AllTracks");
				this.GuestMessage3 = (StackPanel)base.FindName("GuestMessage3");
				this.CurrentTrackName = (TextBlock)base.FindName("CurrentTrackName");
				this.TrackTransform = (CompositeTransform)base.FindName("TrackTransform");
				this.MiniPlayPauseButton = (Button)base.FindName("MiniPlayPauseButton");
				this.StartTimeLabel = (TextBlock)base.FindName("StartTimeLabel");
				this.ProgressSlider = (Slider)base.FindName("ProgressSlider");
				this.EndTimeLabel = (TextBlock)base.FindName("EndTimeLabel");
				this.NowPlayingPanel = (Grid)base.FindName("NowPlayingPanel");
				this.NowPlayingTransform = (TranslateTransform)base.FindName("NowPlayingTransform");
				this.CloseNowPlayingButton = (Button)base.FindName("CloseNowPlayingButton");
				this.NowPlayingTitle = (TextBlock)base.FindName("NowPlayingTitle");
				this.NowPlayingArtist = (TextBlock)base.FindName("NowPlayingArtist");
				this.NowPlayingSource = (TextBlock)base.FindName("NowPlayingSource");
				this.StartTimeLabel1 = (TextBlock)base.FindName("StartTimeLabel1");
				this.EndTimeLabel1 = (TextBlock)base.FindName("EndTimeLabel1");
				this.ProgressFill = (Rectangle)base.FindName("ProgressFill");
				this.FullScreenProgressSlider = (Slider)base.FindName("FullScreenProgressSlider");
				this.PrevButton = (Button)base.FindName("PrevButton");
				this.FullPlayPauseButton = (Button)base.FindName("FullPlayPauseButton");
				this.PlayPauseText = (TextBlock)base.FindName("PlayPauseText");
				this.NextButton = (Button)base.FindName("NextButton");
				this.VolumeLabel = (TextBlock)base.FindName("VolumeLabel");
				this.VolumeFill = (Rectangle)base.FindName("VolumeFill");
				this.VolumeSlider = (Slider)base.FindName("VolumeSlider");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x04000031 RID: 49
		private bool isPlaying = false;

		// Token: 0x04000032 RID: 50
		private TrackItem currentTrack;

		// Token: 0x04000033 RID: 51
		private DispatcherTimer progressTimer;

		// Token: 0x04000034 RID: 52
		private bool _isNavigating = false;

		// Token: 0x04000035 RID: 53
		private string[] reminderMessages = new string[]
		{
			"I love eurobeat, do you?",
			"Time to check updates, right..?",
			"I can feel you & me by beats!",
			"Time to play music? awesome!",
			"I thought you've forgotten me.",
			"Welcome back! and time to chill.",
			"Peekaboo! How are you? :)"
		};

		// Token: 0x04000036 RID: 54
		private bool isDragging = false;

		// Token: 0x04000037 RID: 55
		private Uri _pendingNavigationUri;

		// Token: 0x04000038 RID: 56
		private DispatcherTimer reminderTimer;

		// Token: 0x04000039 RID: 57
		private Random rand = new Random();

		// Token: 0x0400003A RID: 58
		internal Storyboard SlideOutStoryboard;

		// Token: 0x0400003B RID: 59
		internal Storyboard MarqueeStoryboard;

		// Token: 0x0400003C RID: 60
		internal Storyboard SlideUpStoryboard;

		// Token: 0x0400003D RID: 61
		internal Storyboard SlideDownStoryboard;

		// Token: 0x0400003E RID: 62
		internal Grid LayoutRoot;

		// Token: 0x0400003F RID: 63
		internal TranslateTransform SlideTransform;

		// Token: 0x04000040 RID: 64
		internal Panorama OpenTracksM;

		// Token: 0x04000041 RID: 65
		internal PanoramaItem HomeItem;

		// Token: 0x04000042 RID: 66
		internal Border LoginStatusPanel;

		// Token: 0x04000043 RID: 67
		internal TextBlock LoggedInUserText;

		// Token: 0x04000044 RID: 68
		internal PanoramaItem NewTracksItem;

		// Token: 0x04000045 RID: 69
		internal Grid NewTracksGrid;

		// Token: 0x04000046 RID: 70
		internal Grid SignedInContent1;

		// Token: 0x04000047 RID: 71
		internal ListBox NewTracks;

		// Token: 0x04000048 RID: 72
		internal StackPanel GuestMessage1;

		// Token: 0x04000049 RID: 73
		internal PanoramaItem TopHitsItem;

		// Token: 0x0400004A RID: 74
		internal Grid TracksGrid;

		// Token: 0x0400004B RID: 75
		internal Grid SignedInContent2;

		// Token: 0x0400004C RID: 76
		internal ListBox Tracks;

		// Token: 0x0400004D RID: 77
		internal StackPanel GuestMessage2;

		// Token: 0x0400004E RID: 78
		internal PanoramaItem TracksItem;

		// Token: 0x0400004F RID: 79
		internal Grid AllTracksGrid;

		// Token: 0x04000050 RID: 80
		internal StackPanel og;

		// Token: 0x04000051 RID: 81
		internal TextBox SearchBox;

		// Token: 0x04000052 RID: 82
		internal ListBox SearchResults;

		// Token: 0x04000053 RID: 83
		internal Grid SignedInContent3;

		// Token: 0x04000054 RID: 84
		internal ListBox AllTracks;

		// Token: 0x04000055 RID: 85
		internal StackPanel GuestMessage3;

		// Token: 0x04000056 RID: 86
		internal TextBlock CurrentTrackName;

		// Token: 0x04000057 RID: 87
		internal CompositeTransform TrackTransform;

		// Token: 0x04000058 RID: 88
		internal Button MiniPlayPauseButton;

		// Token: 0x04000059 RID: 89
		internal TextBlock StartTimeLabel;

		// Token: 0x0400005A RID: 90
		internal Slider ProgressSlider;

		// Token: 0x0400005B RID: 91
		internal TextBlock EndTimeLabel;

		// Token: 0x0400005C RID: 92
		internal Grid NowPlayingPanel;

		// Token: 0x0400005D RID: 93
		internal TranslateTransform NowPlayingTransform;

		// Token: 0x0400005E RID: 94
		internal Button CloseNowPlayingButton;

		// Token: 0x0400005F RID: 95
		internal TextBlock NowPlayingTitle;

		// Token: 0x04000060 RID: 96
		internal TextBlock NowPlayingArtist;

		// Token: 0x04000061 RID: 97
		internal TextBlock NowPlayingSource;

		// Token: 0x04000062 RID: 98
		internal TextBlock StartTimeLabel1;

		// Token: 0x04000063 RID: 99
		internal TextBlock EndTimeLabel1;

		// Token: 0x04000064 RID: 100
		internal Rectangle ProgressFill;

		// Token: 0x04000065 RID: 101
		internal Slider FullScreenProgressSlider;

		// Token: 0x04000066 RID: 102
		internal Button PrevButton;

		// Token: 0x04000067 RID: 103
		internal Button FullPlayPauseButton;

		// Token: 0x04000068 RID: 104
		internal TextBlock PlayPauseText;

		// Token: 0x04000069 RID: 105
		internal Button NextButton;

		// Token: 0x0400006A RID: 106
		internal TextBlock VolumeLabel;

		// Token: 0x0400006B RID: 107
		internal Rectangle VolumeFill;

		// Token: 0x0400006C RID: 108
		internal Slider VolumeSlider;

		// Token: 0x0400006D RID: 109
		internal MediaElement AudioPlayer;

		// Token: 0x0400006E RID: 110
		private bool _contentLoaded;
	}
}
