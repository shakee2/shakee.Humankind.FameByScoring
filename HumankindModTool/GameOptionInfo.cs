using System;
using System.Collections.Generic;
using Amplitude.Mercury.UI;

namespace HumankindModTool
{
	// Token: 0x02000004 RID: 4
	public class GameOptionInfo
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000284C File Offset: 0x00000A4C
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002854 File Offset: 0x00000A54
		public string Key { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000285D File Offset: 0x00000A5D
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002865 File Offset: 0x00000A65
		public string GroupKey { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000286E File Offset: 0x00000A6E
		// (set) Token: 0x0600000C RID: 12 RVA: 0x00002876 File Offset: 0x00000A76
		public string Title { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000287F File Offset: 0x00000A7F
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002887 File Offset: 0x00000A87
		public string Description { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002890 File Offset: 0x00000A90
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002898 File Offset: 0x00000A98
		public string DefaultValue { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000028A1 File Offset: 0x00000AA1
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000028A9 File Offset: 0x00000AA9
		public UIControlType ControlType { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000028B2 File Offset: 0x00000AB2
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000028BA File Offset: 0x00000ABA
		public List<GameOptionStateInfo> 
		
		States { get; set; } = new List<GameOptionStateInfo>();
	}
}
