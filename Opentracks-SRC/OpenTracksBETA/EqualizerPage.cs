using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace OpenTracksBETA
{
	// Token: 0x02000008 RID: 8
	public class EqualizerPage : PhoneApplicationPage
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00002CF6 File Offset: 0x00000EF6
		public EqualizerPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000022A0 File Offset: 0x000004A0
		private void Home_Click(object sender, EventArgs e)
		{
			base.NavigationService.Navigate(new Uri("/MainPage.xaml", 2));
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002D08 File Offset: 0x00000F08
		private void Preset_Pop_Click(object sender, RoutedEventArgs e)
		{
			this.BassSlider.Value = 40.0;
			this.MidSlider.Value = 70.0;
			this.TrebleSlider.Value = 60.0;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002D58 File Offset: 0x00000F58
		private void Preset_Bass_Click(object sender, RoutedEventArgs e)
		{
			this.BassSlider.Value = 80.0;
			this.MidSlider.Value = 50.0;
			this.TrebleSlider.Value = 40.0;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002DA8 File Offset: 0x00000FA8
		private void Preset_Treble_Click(object sender, RoutedEventArgs e)
		{
			this.BassSlider.Value = 30.0;
			this.MidSlider.Value = 50.0;
			this.TrebleSlider.Value = 80.0;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002DF5 File Offset: 0x00000FF5
		private void ApplyEQ_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Equalizer settings applied!");
			base.NavigationService.GoBack();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002E10 File Offset: 0x00001010
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Application.LoadComponent(this, new Uri("/OpenTracksBETA;component/EqualizerPage.xaml", 2));
				this.BassSlider = (Slider)base.FindName("BassSlider");
				this.MidSlider = (Slider)base.FindName("MidSlider");
				this.TrebleSlider = (Slider)base.FindName("TrebleSlider");
			}
		}

		// Token: 0x04000021 RID: 33
		internal Slider BassSlider;

		// Token: 0x04000022 RID: 34
		internal Slider MidSlider;

		// Token: 0x04000023 RID: 35
		internal Slider TrebleSlider;

		// Token: 0x04000024 RID: 36
		private bool _contentLoaded;
	}
}
