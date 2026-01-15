using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA
{
	// Token: 0x02000007 RID: 7
	public class credits : PhoneApplicationPage
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002C9B File Offset: 0x00000E9B
		public credits()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002CAC File Offset: 0x00000EAC
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/credits.xaml", 2));
				this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			}
		}

		// Token: 0x0400001F RID: 31
		internal Grid LayoutRoot;

		// Token: 0x04000020 RID: 32
		private bool _contentLoaded;
	}
}
