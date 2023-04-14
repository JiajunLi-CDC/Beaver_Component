using System;
using System.Windows.Forms;

namespace Beaver.ReuseScripts.Info
{
	// Token: 0x02000012 RID: 18
	public class MyListBox : ListBox
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00004CCC File Offset: 0x00002ECC
		public MyListBox(int Width, int Height)
		{
			base.HorizontalScrollbar = true;
			base.Width = Width;
			base.Height = Height;
			this.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			base.ResizeRedraw = true;
			base.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
		}

		// Token: 0x04000056 RID: 86
		public bool ESCKeyPressed = false;
	}
}
