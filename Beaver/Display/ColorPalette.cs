using System;
using System.Collections.Generic;
using System.Drawing;

namespace Beaver.Display
{
	// Token: 0x02000029 RID: 41
	public static class ColorPalette
	{
		// Token: 0x06000137 RID: 311 RVA: 0x00009B98 File Offset: 0x00007D98
		public static Color NextColor(int i)
		{
			return ColorPalette.BrightPastel[i % ColorPalette.BrightPastel.Count];
		}

		// Token: 0x04000014 RID: 20
		public static Color CBlank = Color.LightGray;

		// Token: 0x04000015 RID: 21
		public static Color CReuse = Color.LightGray;

		// Token: 0x04000016 RID: 22
		public static Color CNew = Color.DeepSkyBlue;

		// Token: 0x04000017 RID: 23
		public static Color CReuseUsed = Color.Black;

		// Token: 0x04000018 RID: 24
		public static Color CNewUsed = Color.DeepSkyBlue;

		// Token: 0x04000019 RID: 25
		public static Color CTension = Color.Red;

		// Token: 0x0400001A RID: 26
		public static Color CCompression = Color.Blue;

		// Token: 0x0400001B RID: 27
		public static List<Color> BrightPastel = new List<Color>
		{
			ColorTranslator.FromHtml("#418CF0"),
			ColorTranslator.FromHtml("#FCB441"),
			ColorTranslator.FromHtml("#E0400A"),
			ColorTranslator.FromHtml("#056492"),
			ColorTranslator.FromHtml("#1A3B69"),
			ColorTranslator.FromHtml("#FFE382"),
			ColorTranslator.FromHtml("#129CDD"),
			ColorTranslator.FromHtml("#CA6B4B"),
			ColorTranslator.FromHtml("#005CDB"),
			ColorTranslator.FromHtml("#F3D288"),
			ColorTranslator.FromHtml("#506381"),
			ColorTranslator.FromHtml("#F1B9A8"),
			ColorTranslator.FromHtml("#E0830A")
		};
	}
}
