using System;
using System.Collections.Generic;

namespace OpenTracksBETA
{
	// Token: 0x02000019 RID: 25
	public class Track
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000A959 File Offset: 0x00008B59
		// (set) Token: 0x06000152 RID: 338 RVA: 0x0000A961 File Offset: 0x00008B61
		public string Id { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000A96A File Offset: 0x00008B6A
		// (set) Token: 0x06000154 RID: 340 RVA: 0x0000A972 File Offset: 0x00008B72
		public string Name { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000A97B File Offset: 0x00008B7B
		// (set) Token: 0x06000156 RID: 342 RVA: 0x0000A983 File Offset: 0x00008B83
		public string Uri { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000A98C File Offset: 0x00008B8C
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000A994 File Offset: 0x00008B94
		public List<Artist> Artists { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000A99D File Offset: 0x00008B9D
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000A9A5 File Offset: 0x00008BA5
		public Album Album { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000A9AE File Offset: 0x00008BAE
		// (set) Token: 0x0600015C RID: 348 RVA: 0x0000A9B6 File Offset: 0x00008BB6
		public int DurationMs { get; set; }
	}
}
