using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;
using Rhino.Geometry;

namespace Beaver_ML.JointsOptimize
{
    /*
    *用于计算节点的优化
    */
    class SANDGurobiJoints
    {
        //添加变量，除了第一个节点，剩余节点的旋转角度
        public static GRBVar[,] GetGurobiRotationVariables(GRBModel Model, Dictionary<int, List<Point3d>> holes)
        {
            GRBVar[,] status = new GRBVar[holes.Count-1, 3];  //除了第一个节点，剩余的节点的xyz三个方向的旋转度需要计算
            for (int i = 0; i < holes.Count - 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {                  
                     status[i, j] = Model.AddVar(0, 360, 0, GRB.INTEGER,  i + "rotation" + j );
                }
            }
            return status;
        }

        //添加变量，每个点的最近点判断
        public static GRBVar[,] GetGurobiIsCloseVariables(GRBModel Model, List<Point3d> allholes)
        {
          
            GRBVar[,] status = new GRBVar[allholes.Count ,allholes.Count];  //判断每个节点，其它节点相对于它是否是最近的点
            for (int i = 0; i < allholes.Count; i++)
            {
                for (int j = 0; j < allholes.Count; j++)
                {
                    status[i,j] = Model.AddVar(0, 1, 0, GRB.BINARY, i + "closed" + j);
                }
            }

            return status;
        }

        //添加约束：每个点只有一个最近点（在一个半径为1的球体上，球心在坐标原点，如果一个点的坐标为（a,b,c）,那么当x，y，z三个方向都分别旋转n1°，n2°，n3°时，新的点的坐标是多少？？这个问题还未解决）
        public static void OnlyOneIsClosed(GRBModel model, GRBVar[,] IsClosed, List<Point3d> allholes)
        {
            for (int i = 0; i < allholes.Count; i++)
            {
                for (int j = 0; j < allholes.Count; j++)
                {
                    
                    if (i == j)
                    {

                    }
                    else
                    {
                        Point3d A = allholes[i];
                        Point3d B = allholes[j];   //原始坐标



                        GRBLinExpr grblinExpr = new GRBLinExpr();
                        grblinExpr.AddTerm(1.0, IsClosed[i,j]);  //每个库存组的相同长度，相同结构的杆件数量
                        model.AddGenConstrIndicator(IsClosed[i, j], 1, grblinExpr, '>', 0, "LargerThanOther" + i + j );   //当某个结构是最大时
                    }
                  
                    
                }          
            }
        }
    }
}
