using Grasshopper.Kernel.Types.Transforms;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beaver_ML.JointsOptimize
{

    //用于球形节点的旋转
    public class JointsRotation
    {
        public double x_degree = 0;   //各个轴的旋转角度

        public double y_degree = 0;

        public double z_degree = 0;

        public static ITransform transform = null;  //旋转后的迁移信息

        public Plane originPlan = new Plane(Plane.WorldXY); //迁移前的基准平面（默认为XYZ平面）

        public List<Point3d> originJoint_Points = new List<Point3d>();   //旋转前节点的表面点

        //public List<Point3d> rotateJoint_Points = new List<Point3d>();   //旋转后节点的表面点

        public Point3d point_Center = new Point3d();   //球形节点中心
        
        public JointsRotation(List<Point3d> originJoint_Points,Plane originPlan)
        {          
            this.originPlan = originPlan;
            this.originJoint_Points = originJoint_Points;
        }

        public void CauculateNewPoints(double x_degree, double y_degree, double z_degree,ref List<Point3d> newpoints)
        {
            this.x_degree = x_degree;
            this.y_degree = y_degree;
            this.z_degree = z_degree;

            List<Point3d> rotateJoint_Points = new List<Point3d>();
            Transform transform = CauculateTransform().ToMatrix();  //计算迁移信息

            foreach (Point3d p in originJoint_Points)
            {              
                Point3d newp = new Point3d(p.X, p.Y, p.Z);//复制并且迁移 
                newp.Transform(transform);    

                rotateJoint_Points.Add(newp);
            }

            newpoints =  rotateJoint_Points;
        }
        public ITransform CauculateTransform()
        {
            Plane plane2;
            plane2 = new Plane(originPlan);   //复制一个原始平面

            Vector3d vector3d = originPlan.XAxis;
            Vector3d vector3d2 = originPlan.YAxis;
            Vector3d vector3d3 = originPlan.ZAxis;

            plane2.Transform(Transform.Rotation(x_degree * 0.017453292519943295, vector3d, originPlan.Origin));   //绕x旋转
            plane2.Transform(Transform.Rotation(y_degree * 0.017453292519943295, vector3d2, originPlan.Origin));   //绕y旋转
            plane2.Transform(Transform.Rotation(z_degree * 0.017453292519943295, vector3d3, originPlan.Origin));   //绕z旋转

            ITransform transform = new Orientation(originPlan, plane2);    //获取迁移的信息

            return transform;
        }


    }
}
