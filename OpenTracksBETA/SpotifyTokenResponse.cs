using System;

namespace OpenTracksBETA
{
	// Token: 0x02000016 RID: 22
	public class SpotifyTokenResponse
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000A8E2 File Offset: 0x00008AE2
		// (set) Token: 0x06000141 RID: 321 RVA: 0x0000A8EA File Offset: 0x00008AEA
		public string access_token { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000A8F3 File Offset: 0x00008AF3
		// (set) Token: 0x06000143 RID: 323 RVA: 0x0000A8FB File Offset: 0x00008AFB
		public string token_type { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000A904 File Offset: 0x00008B04
		// (set) Token: 0x06000145 RID: 325 RVA: 0x0000A90C File Offset: 0x00008B0C
		public int expires_in { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000A915 File Offset: 0x00008B15
		// (set) Token: 0x06000147 RID: 327 RVA: 0x0000A91D File Offset: 0x00008B1D
		public string refresh_token { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000A926 File Offset: 0x00008B26
		// (set) Token: 0x06000149 RID: 329 RVA: 0x0000A92E File Offset: 0x00008B2E
		public string scope { get; set; }
	}
}
