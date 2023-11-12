using System;
using System.Collections.Generic;
//using Beaver3D.LCA;

namespace Beaver_ML.JointsOptimize.Info
{
	// Token: 0x0200000E RID: 14
	public class OptimOptions
	{

		// Token: 0x0400003A RID: 58
		public int MaxTime = int.MaxValue;

		// Token: 0x0400003B RID: 59
		public bool LogToConsole = true;

		// Token: 0x0400003C RID: 60
		public string LogFormName = "GurobiOptimize";

		// Token: 0x0400003F RID: 63
		public double MaxMass = double.MaxValue;

		// Token: 0x04000041 RID: 65
		public bool SOS_Assignment = false;

		// Token: 0x04000042 RID: 66
		public bool SOS_Continuous = false;

		// Token: 0x04000047 RID: 71
		public List<Tuple<string, string>> GurobiParameters = new List<Tuple<string, string>>();

		// Token: 0x04000048 RID: 72
		public bool CuttingStock = false;
	}
}
