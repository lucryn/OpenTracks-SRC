using System;

namespace OpenTracksBETA
{
	// Token: 0x02000011 RID: 17
	public class PodcastItem
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00006796 File Offset: 0x00004996
		// (set) Token: 0x060000BC RID: 188 RVA: 0x0000679E File Offset: 0x0000499E
		public string Title { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000067A7 File Offset: 0x000049A7
		// (set) Token: 0x060000BE RID: 190 RVA: 0x000067AF File Offset: 0x000049AF
		public string Artist { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000BF RID: 191 RVA: 0x000067B8 File Offset: 0x000049B8
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x000067C0 File Offset: 0x000049C0
		public string Url { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000067C9 File Offset: 0x000049C9
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x000067D1 File Offset: 0x000049D1
		public string CoverArt { get; set; }
	}
}
