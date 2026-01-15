using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA.Crash
{
	// Token: 0x02000022 RID: 34
	public class mediafail : PhoneApplicationPage
	{
		// Token: 0x060001B8 RID: 440 RVA: 0x0000C2C0 File Offset: 0x0000A4C0
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/mediafail.xaml", 2));
				this.LayoutRoot20 = (Grid)base.FindName("LayoutRoot20");
				this.AudioPlayer = (MediaElement)base.FindName("AudioPlayer");
			}
		}

		// Token: 0x04000117 RID: 279
		internal Grid LayoutRoot20;

		// Token: 0x04000118 RID: 280
		internal MediaElement AudioPlayer;

		// Token: 0x04000119 RID: 281
		private bool _contentLoaded;
	}
}
