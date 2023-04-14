using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Optimization;
using Beaver.Properties;

namespace Beaver.Optimization.Options
{
	// Token: 0x0200000B RID: 11
	public class DSCOOptions_Component : GH_Component
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00003E7F File Offset: 0x0000207F
		public DSCOOptions_Component() : base("DSCOOptions", "DSCOOptions", "Options for Discrete Stock Constrained Optimization", "Beaver", "06 Optimization")
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003EA4 File Offset: 0x000020A4
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddIntegerParameter("Optimizer", "Opt", "Optimizer (0 = Gurobi (Default), 1 = ...", 0, 0);
			pManager.AddIntegerParameter("Max Time", "MaxTime", "Time limit for the optimization in seconds. default = No Limit", 0, int.MaxValue);
			pManager.AddBooleanParameter("Logging", "Log", "Log Optimizer in Console Windows. default = true", 0, true);
			pManager.AddTextParameter("LogForm Name", "LogName", "Name of the Windows Form used for logging", 0, "MILP Optimization Log");
			pManager.AddBooleanParameter("Compatibility", "Compat", "Consider geometric compatibility", 0, true);
			pManager.AddBooleanParameter("SelfWeight", "SelfW", "Consider self-weight lumped at member end nodes. default = false", 0, false);
			pManager.AddBooleanParameter("CuttingStock", "CuttingStock", "Consider partition of stock elements into multiple members (cutting stock)", 0, false);
			pManager.AddBooleanParameter("TopologyFixed", "TopologyFixed", "Consider whether the structure topology is fixed", 0, false);
			pManager.AddTextParameter("Parameters", "Param", "List of Optimizer parameters in the form or 'Parameter name, value'", GH_ParamAccess.list);
			pManager[0].Optional = true;
			pManager[1].Optional = true;
			pManager[2].Optional = true;
			pManager[3].Optional = true;
			pManager[4].Optional = true;
			pManager[5].Optional = true;
			pManager[6].Optional = true;
			pManager[7].Optional = true;
			pManager[8].Optional = true;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003FE9 File Offset: 0x000021E9
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("CTT Optimization Options", "DSCOOpt", "CTT Optimization Options", 0);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004004 File Offset: 0x00002204
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			char[] separator = new char[]
			{
				' ',
				',',
				'.',
				':',
				';'
			};
			int milpoptimizer = 0;
			int maxValue = int.MaxValue;
			bool logToConsole = true;
			string logFormName = "MILP Optimization Log";
			int milpformulation = 0;
			bool compatibility = true;
			bool selfweight = false;
			bool cuttingStock = false;
			bool topologyFixed = false;
			List<string> list = new List<string>();   //表单中的gurobi优化器参数列表或“参数名称，值”


			DA.GetData<int>(0, ref milpoptimizer);
			DA.GetData<int>(1, ref maxValue);
			DA.GetData<bool>(2, ref logToConsole);
			DA.GetData<string>(3, ref logFormName);
			DA.GetData<bool>(4, ref compatibility);
			DA.GetData<bool>(5, ref selfweight);
			DA.GetData<bool>(6, ref cuttingStock);
			DA.GetData<bool>(7, ref topologyFixed);
			DA.GetDataList<string>(8, list);

			OptimOptions optimOptions = new OptimOptions();
			optimOptions.MILPOptimizer = (MILPOptimizer)milpoptimizer;  //使用gurobi（milpoptimizer默认=0）
			optimOptions.MaxTime = maxValue;
			optimOptions.LogToConsole = logToConsole;
			optimOptions.LogFormName = logFormName;    //弹窗的名称
			optimOptions.MILPFormulation = (MILPFormulation)milpformulation;    //选择计算公式，默认Bruetting的计算公式（milpformulation=0）
			optimOptions.Selfweight = selfweight;
			optimOptions.Compatibility = compatibility;
			optimOptions.CuttingStock = cuttingStock;
            optimOptions.TopologyFixed = topologyFixed;

            foreach (string text in list)
			{
				string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);    
				string item = array[0];
				string item2 = array[1];
				optimOptions.GurobiParameters.Add(new Tuple<string, string>(item, item2));
			}
			DA.SetData(0, optimOptions);
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600006F RID: 111 RVA: 0x0000415C File Offset: 0x0000235C
		protected override Bitmap Icon
		{
			get
			{
				return Resources.DSCO_Option_BeaverIcon;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00004163 File Offset: 0x00002363
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("DE07C224-ED04-496A-9E18-B25B1EB07FC1");
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00004170 File Offset: 0x00002370
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.quinary;
			}
		}
	}
}
