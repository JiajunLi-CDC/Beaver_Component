using Beaver.ReuseScripts.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;
using System.Windows.Forms;
using System.Drawing;

namespace Beaver.ReuseScripts.Optimization
{
    class BilinearTest
    {
        //public Objective Objective { get; private set; } = Objective.MinStructureMass;
        public OptimOptions Options
        {
            get; private set;
        }

        public BilinearTest()
        {
            //this.Objective = Objective;
            this.Options = new OptimOptions();
            //bool flag = Options == null;
            //if (flag)
            //{
            //    this.Options = new OptimOptions();
            //}
            //else
            //{
            //    this.Options = Options;
            //}
        }

        public void Solve(ref List<string> all_results)
        {
            try
            {
                GRBEnv env = new GRBEnv("bilinear.log");
                GRBModel model = new GRBModel(env);

                // Create variables

                GRBVar x = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.CONTINUOUS, "x");
                GRBVar y = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.CONTINUOUS, "y");
                GRBVar z = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.CONTINUOUS, "z");

                // Set objective

                GRBLinExpr obj = x;
                model.SetObjective(obj, GRB.MAXIMIZE);

                // Add linear constraint: x + y + z <= 10

                model.AddConstr(x + y + z <= 10, "c0");

                // Add bilinear inequality: x * y <= 2

                model.AddQConstr(x * y <= 2, "bilinear0");

                // Add bilinear equality: x * z + y * z == 1

                model.AddQConstr(x * z + y * z == 1, "bilinear1");

                // Optimize model
                FormStartPosition startPos;
                Point location;
                this.CloseOpenFormsAndGetPos(out startPos, out location);
                LightCallback lightCallback = null;
                LogCallback logCallback = null;
                bool logToConsole = this.Options.LogToConsole;

                if (logToConsole)
                {
                    logCallback = new LogCallback(startPos, location, this.Options.LogFormName);
                    model.SetCallback(logCallback);
                }
                else
                {
                    lightCallback = new LightCallback();
                    model.SetCallback(lightCallback);
                }

                try
                {
                    model.Optimize();
                }
                catch (GRBException e)
                {
                    Console.WriteLine("Failed (as expected) " + e.ErrorCode + ". " + e.Message);
                }

                model.Set(GRB.IntParam.NonConvex, 2);
                model.Optimize();

                all_results = new List<string>();
                string result;
                result = x.VarName + " = " + x.X + "\n"
                    + y.VarName + " = " + y.X + "\n"
                    + z.VarName + " = " + z.X + "\n"
                    + "Obj: " + model.ObjVal + " " + obj.Value;
                all_results.Add(result);

                //再次计算
                x.Set(GRB.CharAttr.VType, GRB.INTEGER);
                model.Optimize();

                string result2;
                result2 = x.VarName + " = " + x.X + "\n"
                    + y.VarName + " = " + y.X + "\n"
                    + z.VarName + " = " + z.X + "\n"
                    + "Obj: " + model.ObjVal + " " + obj.Value;
                all_results.Add(result2);

                // Dispose of model and env

                model.Dispose();
                env.Dispose();


            }
            catch (GRBException e)
            {
                string result;
                result = "Error code: " + e.ErrorCode + ". " + e.Message;
                all_results.Add(result);
            }

        }

        private void CloseOpenFormsAndGetPos(out FormStartPosition Pos, out Point Location)
        {
            Pos = FormStartPosition.Manual;
            Location = default(Point);
            List<Form> list = new List<Form>();
            foreach (object obj in Application.OpenForms)
            {
                Form form = (Form)obj;
                string name1 = form.Name;
                string name2 = this.Options.LogFormName;
                bool flag = name1 == name2;
                if (flag)
                {
                    list.Add(form);
                }
            }
            foreach (Form form2 in list)
            {
                Pos = form2.StartPosition;
                Location = form2.Location;
                form2.Close();
                form2.Dispose();
            }
        }



    }
}
