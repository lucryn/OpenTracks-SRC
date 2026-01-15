using System;
using OpenTracksBETA.Resources;

namespace OpenTracksBETA
{
	// Token: 0x0200000A RID: 10
	public class LocalizedStrings
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003240 File Offset: 0x00001440
		public AppResources LocalizedResources
		{
			get
			{
				return LocalizedStrings._localizedResources;
			}
		}

		// Token: 0x04000028 RID: 40
		private static AppResources _localizedResources = new AppResources();
	}
}
