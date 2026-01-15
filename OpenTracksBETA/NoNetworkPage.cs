using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA
{
	// Token: 0x0200000E RID: 14
	public class NoNetworkPage : PhoneApplicationPage
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00005D9F File Offset: 0x00003F9F
		public NoNetworkPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005DB0 File Offset: 0x00003FB0
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/NoNetworkPage.xaml", 2));
				this.LayoutRoot1 = (Grid)base.FindName("LayoutRoot1");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x04000076 RID: 118
		internal Grid LayoutRoot1;

		// Token: 0x04000077 RID: 119
		internal MediaElement AudioPlayer;

		// Token: 0x04000078 RID: 120
		private bool _contentLoaded;
	}
}
