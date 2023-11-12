using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beaver_ML.JointsOptimize.Info;
using Gurobi;
using Rhino.Geometry;

namespace Beaver_ML.JointsOptimize
{
    class JointGurobiOptimizer
    {
        public Joints joints { get; private set; }  //需要优化的节点

        List<Point3d> allhole = new List<Point3d>();  //所有孔洞点的List

        public JointsOptimizeResult result = new JointsOptimizeResult();  //记录结果

        public OptimOptions Options { get; private set; }  //优化选项

        // 实例化对象
        public JointGurobiOptimizer(OptimOptions Options ,Joints joints )
        {
            bool flag = Options == null;
            if (flag)
            {
                this.Options = new OptimOptions();    //使用默认的options选项
            }
            else
            {
                this.Options = Options;     //使用传递进来的options选项
            }

            this.allhole = new List<Point3d>();  //所有孔洞点的List
            for (int i = 0; i < joints.holes.Count; i++)
            {
                List<Point3d> eachstructurehole ;
                joints.holes.TryGetValue(i, out eachstructurehole);
                for (int j = 0; j < eachstructurehole.Count; j++)
                {
                    allhole.Add(eachstructurehole[j]);
                }
            }
        }

        // 优化计算（这里算上了所有的荷载）
        public void Solve()
        {
            Dictionary<int, List<Point3d>> holes = this.joints.holes;
            bool flag = holes.Count < 2;
            if (flag)
            {
                throw new ArgumentException("LoadCases with the provided names are not existing in the structure. Check the names or use 'all' to compute all LoadCases.");
            }
            else
            {
                this.SolveGurobiJJ(holes,allhole);
            } 
        }

        public void SolveGurobiJJ(Dictionary<int, List<Point3d>> holes, List<Point3d> allhole)
        {
            //创建Gurobi环境和模型
            GRBEnv grbenv = new GRBEnv();
            GRBModel grbmodel = new GRBModel(grbenv);

            grbmodel.Parameters.TimeLimit = (double)this.Options.MaxTime;  //获取选项中的计算最大时间
            foreach (Tuple<string, string> tuple in this.Options.GurobiParameters)   //设置gurobi每个变量的参数值，一般为默认
            {
                grbmodel.Set(tuple.Item1, tuple.Item2);
            }

            //-------------------------------------------------------------------

        }
    }
}
