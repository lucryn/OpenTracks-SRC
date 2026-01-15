using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA
{
	// Token: 0x02000009 RID: 9
	public class HelpBotPage : PhoneApplicationPage
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00002E86 File Offset: 0x00001086
		public HelpBotPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002E97 File Offset: 0x00001097
		private void HelpBotPage_Loaded(object sender, RoutedEventArgs e)
		{
			this.AddBotBubble("\ud83d\udc4b Hello! I'm your help assistant. How can I help you today?");
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002EA8 File Offset: 0x000010A8
		private void SendButton_Click(object sender, RoutedEventArgs e)
		{
			string text = this.UserInputBox.Text.Trim();
			bool flag = string.IsNullOrWhiteSpace(text);
			if (!flag)
			{
				this.AddUserBubble(text);
				string botResponse = this.GetBotResponse(text.ToLower());
				this.AddBotBubble(botResponse);
				this.UserInputBox.Text = string.Empty;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002F04 File Offset: 0x00001104
		private string GetBotResponse(string message)
		{
			bool flag = message.Contains("login");
			string result;
			if (flag)
			{
				result = "\ud83d\udd10 To sign in, visit the Login page and enter your credentials.";
			}
			else
			{
				bool flag2 = message.Contains("upload");
				if (flag2)
				{
					result = "\ud83d\udce4 Upload your music from the Upload tab — just look for the cloud icon.";
				}
				else
				{
					bool flag3 = message.Contains("favorites") || message.Contains("favourites");
					if (flag3)
					{
						result = "⭐ Your favorites list appears once you're signed in.";
					}
					else
					{
						bool flag4 = message.Contains("help") || message.Contains("support");
						if (flag4)
						{
							result = "\ud83d\udccc Available prompts: help, favourites, upload, login.";
						}
						else
						{
							bool flag5 = message.Contains("hi") || message.Contains("hello");
							if (flag5)
							{
								result = "Hey there! I am your assistant helper. How can I assist you today?";
							}
							else
							{
								result = "If you need more help, please contact @chockingnetdude on Telegram. Thanks!";
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002FC8 File Offset: 0x000011C8
		private void AddUserBubble(string message)
		{
			Border border = new Border
			{
				Background = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 0, 122, 204)),
				CornerRadius = new CornerRadius(20.0),
				Padding = new Thickness(14.0),
				Margin = new Thickness(40.0, 8.0, 0.0, 8.0),
				MaxWidth = 360.0,
				HorizontalAlignment = 2
			};
			TextBlock child = new TextBlock
			{
				Text = message,
				FontSize = 22.0,
				TextWrapping = 2,
				Foreground = new SolidColorBrush(Colors.White),
				FontFamily = new FontFamily("Segoe WP")
			};
			border.Child = child;
			this.ChatPanel.Children.Add(border);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000030D0 File Offset: 0x000012D0
		private void AddBotBubble(string message)
		{
			Border border = new Border
			{
				Background = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 235, 235, 235)),
				CornerRadius = new CornerRadius(20.0),
				Padding = new Thickness(14.0),
				Margin = new Thickness(0.0, 8.0, 40.0, 8.0),
				MaxWidth = 360.0,
				HorizontalAlignment = 0
			};
			TextBlock child = new TextBlock
			{
				Text = message,
				FontSize = 22.0,
				TextWrapping = 2,
				Foreground = new SolidColorBrush(Colors.Black),
				FontFamily = new FontFamily("Segoe WP")
			};
			border.Child = child;
			this.ChatPanel.Children.Add(border);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000031E0 File Offset: 0x000013E0
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/HelpBotPage.xaml", 2));
				this.ChatPanel = (StackPanel)base.FindName("ChatPanel");
				this.UserInputBox = (TextBox)base.FindName("UserInputBox");
			}
		}

		// Token: 0x04000025 RID: 37
		internal StackPanel ChatPanel;

		// Token: 0x04000026 RID: 38
		internal TextBox UserInputBox;

		// Token: 0x04000027 RID: 39
		private bool _contentLoaded;
	}
}
