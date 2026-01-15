using System;
using System.ComponentModel;
using System.Diagnostics;

namespace OpenTracksBETA.ViewModels
{
	// Token: 0x02000023 RID: 35
	public class ItemViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000C32C File Offset: 0x0000A52C
		// (set) Token: 0x060001BB RID: 443 RVA: 0x0000C344 File Offset: 0x0000A544
		public string LineOne
		{
			get
			{
				return this._lineOne;
			}
			set
			{
				bool flag = value != this._lineOne;
				if (flag)
				{
					this._lineOne = value;
					this.NotifyPropertyChanged("LineOne");
				}
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000C378 File Offset: 0x0000A578
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000C390 File Offset: 0x0000A590
		public string LineTwo
		{
			get
			{
				return this._lineTwo;
			}
			set
			{
				bool flag = value != this._lineTwo;
				if (flag)
				{
					this._lineTwo = value;
					this.NotifyPropertyChanged("LineTwo");
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000C3C4 File Offset: 0x0000A5C4
		// (set) Token: 0x060001BF RID: 447 RVA: 0x0000C3DC File Offset: 0x0000A5DC
		public string LineThree
		{
			get
			{
				return this._lineThree;
			}
			set
			{
				bool flag = value != this._lineThree;
				if (flag)
				{
					this._lineThree = value;
					this.NotifyPropertyChanged("LineThree");
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060001C0 RID: 448 RVA: 0x0000C410 File Offset: 0x0000A610
		// (remove) Token: 0x060001C1 RID: 449 RVA: 0x0000C448 File Offset: 0x0000A648
		[DebuggerBrowsable(0)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060001C2 RID: 450 RVA: 0x0000C480 File Offset: 0x0000A680
		private void NotifyPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0400011A RID: 282
		private string _lineOne;

		// Token: 0x0400011B RID: 283
		private string _lineTwo;

		// Token: 0x0400011C RID: 284
		private string _lineThree;
	}
}
