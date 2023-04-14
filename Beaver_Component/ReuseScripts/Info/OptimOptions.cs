using System;
using System.Collections.Generic;
//using Beaver3D.LCA;

namespace Beaver.ReuseScripts.Info
{
	// Token: 0x0200000E RID: 14
	public class OptimOptions
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00004656 File Offset: 0x00002856
		// (set) Token: 0x06000083 RID: 131 RVA: 0x0000465E File Offset: 0x0000285E
		//public ILCA LCA { get; private set; } = new GHGFrontiers();

		// Token: 0x0400003A RID: 58
		public int MaxTime = int.MaxValue;

		// Token: 0x0400003B RID: 59
		public bool LogToConsole = true;

		// Token: 0x0400003C RID: 60
		public string LogFormName = "MyLogFormName";

		// Token: 0x0400003D RID: 61
		public bool Selfweight = true;

		// Token: 0x0400003E RID: 62
		public bool Compatibility = true;

		// Token: 0x0400003F RID: 63
		public double MaxMass = double.MaxValue;

		// Token: 0x04000041 RID: 65
		public bool SOS_Assignment = false;

		// Token: 0x04000042 RID: 66
		public bool SOS_Continuous = false;

		// Token: 0x04000043 RID: 67
		//public LPOptimizer LPOptimizer = LPOptimizer.Gurobi;

		//// Token: 0x04000044 RID: 68
		//public MILPOptimizer MILPOptimizer = MILPOptimizer.Gurobi;

		//// Token: 0x04000045 RID: 69
		//public NLPOptimizer NLPOptimizer = NLPOptimizer.IPOPT;

		//// Token: 0x04000046 RID: 70
		//public MILPFormulation MILPFormulation = MILPFormulation.RasmussenStolpe;

		// Token: 0x04000047 RID: 71
		public List<Tuple<string, string>> GurobiParameters = new List<Tuple<string, string>>();

		// Token: 0x04000048 RID: 72
		public bool CuttingStock = false;
	}
}
