using System;
using System.Collections.Generic;

namespace OpenTracksBETA
{
	// Token: 0x0200001B RID: 27
	public class Album
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000A9E1 File Offset: 0x00008BE1
		// (set) Token: 0x06000164 RID: 356 RVA: 0x0000A9E9 File Offset: 0x00008BE9
		public string Id { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000A9F2 File Offset: 0x00008BF2
		// (set) Token: 0x06000166 RID: 358 RVA: 0x0000A9FA File Offset: 0x00008BFA
		public string Name { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000AA03 File Offset: 0x00008C03
		// (set) Token: 0x06000168 RID: 360 RVA: 0x0000AA0B File Offset: 0x00008C0B
		public List<Image> Images { get; set; }
	}
}
