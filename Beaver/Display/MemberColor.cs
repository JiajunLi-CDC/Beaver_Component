using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Beaver3D.Model;
using Beaver3D.Optimization;
using Beaver3D.Reuse;

namespace Beaver.Display
{
	// Token: 0x0200002D RID: 45
	public static class MemberColor
	{
		// Token: 0x06000147 RID: 327 RVA: 0x0000A878 File Offset: 0x00008A78
		public static Color GetMemberColor(Structure Structure, Assignment Assignment, int i, ColorStyle ColorStyle)
		{
			Color result = default(Color);
			Tuple<ElementGroup, int>[] assignmentIndices = Assignment.GetAssignmentIndices();
			bool flag = assignmentIndices.Length > 1;
			bool flag2 = ColorStyle == ColorStyle.ForceRedBlue;
			if (flag2)
			{
				IMember1D member1D = Structure.Members[i] as IMember1D;
				bool flag3 = member1D != null;
				if (flag3)
				{
					bool flag4 = (from x in member1D.Nx.Values
					select x.Min()).Min() < 0.0;
					if (flag4)
					{
						result = ColorPalette.CCompression;
					}
					else
					{
						result = ColorPalette.CTension;
					}
				}
			}
			else
			{
				bool flag5 = ColorStyle == ColorStyle.ReuseNew;
				if (flag5)
				{
					bool flag6 = assignmentIndices[i].Item1.Type == ElementType.Reuse;
					if (flag6)
					{
						result = ColorPalette.CReuseUsed;
					}
					else
					{
						bool flag7 = assignmentIndices[i].Item1.Type == ElementType.New;
						if (flag7)
						{
							result = ColorPalette.CNewUsed;
						}
					}
				}
				else
				{
					bool flag8 = ColorStyle == ColorStyle.Composite && assignmentIndices.Length <= 1;
					if (flag8)
					{
						bool flag9 = assignmentIndices[i].Item1.Type == ElementType.Reuse;
						if (flag9)
						{
							result = ColorPalette.CReuseUsed;
						}
						else
						{
							bool flag10 = assignmentIndices[i].Item1.Type == ElementType.New;
							if (flag10)
							{
								result = ColorPalette.CNewUsed;
							}
						}
					}
					else
					{
						bool flag11 = ColorStyle == ColorStyle.Composite && assignmentIndices.Length > 1 && assignmentIndices[i].Item1.Type == ElementType.Reuse;
						if (flag11)
						{
							result = ColorPalette.BrightPastel[i % ColorPalette.BrightPastel.Count];
						}
						else
						{
							bool flag12 = ColorStyle == ColorStyle.Composite && assignmentIndices.Length > 1 && assignmentIndices[i].Item1.Type == ElementType.New;
							if (flag12)
							{
								result = ColorPalette.CNewUsed;
							}
							else
							{
								bool flag13 = ColorStyle == ColorStyle.Utilization;
								if (flag13)
								{
									result = Color.YellowGreen;
								}
								else
								{
									result = Color.FromArgb(255, 255, 255);
								}
							}
						}
					}
				}
			}
			return result;
		}
	}
}
