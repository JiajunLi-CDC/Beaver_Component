﻿using Beaver_ML.JointsOptimize;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace Beaver_ML.Component.Joints
{
    public class JointsRotation_Component : GH_Component
    { 
        public JointsRotation_Component()
          : base("JointsRotation", "JointsRotation",
              "Rotate the points on the sphere joints",
              "Beaver", "Subcategory")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points on Joints to rotate", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "Pl", "Intial base plane which defines the {x}, {y}, and {z} world axes", 0, Plane.WorldXY);          
            pManager.AddNumberParameter("Degrees X", "D¹", "The angle in degrees to rotate around axis X", 0, 0.0);
            pManager.AddNumberParameter("Degrees Y", "D²", "The angle in degrees to rotate around axis Y", 0, 0.0);
            pManager.AddNumberParameter("Degrees Z", "D³", "The angle in degrees to rotate around axis Z", 0, 0.0);
            pManager[0].Optional = true;
        }

      
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Rotated", "R", "Resulting rotated Points on Joints", GH_ParamAccess.list);
            pManager.AddTransformParameter("Transform", "X", "Transformation data", 0);
            //pManager.HideParameter(1);
            //pManager.HideParameter(2);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> iPoints = new List<Point3d>();
            Plane iorigin_plane = default(Plane);
            double rx = 0;
            double ry = 0;
            double rz = 0;

            DA.GetDataList<Point3d>(0, iPoints);
            DA.GetData<Plane>(1, ref iorigin_plane);  //物件的起始平面，默认为坐标原点xyz平面
            DA.GetData(2,ref rx);
            DA.GetData(3, ref ry);
            DA.GetData(4, ref rz);

            //...........................................
            JointsRotation rotationmanager = new JointsRotation(iPoints, iorigin_plane);
            List<Point3d> newPoints = new List<Point3d>();
            rotationmanager.CauculateNewPoints(rx, ry, rz,ref newPoints);   //旋转并且输出新的点
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("8f327a66-8435-4556-b416-819c747e9d19"); }
        }
    }
}