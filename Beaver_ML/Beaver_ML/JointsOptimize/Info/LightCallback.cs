using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Gurobi;


namespace Beaver.ReuseScripts.Info
{
    public class LightCallback : GRBCallback
    {
        // Token: 0x06000085 RID: 133
        [DllImport("user32.dll")]
        private static extern ushort GetAsyncKeyState(int vKey);

        // Token: 0x06000086 RID: 134 RVA: 0x00004700 File Offset: 0x00002900
        public static bool IsKeyPushedDown(Keys vKey)
        {
            return (LightCallback.GetAsyncKeyState((int)vKey) & 32768) > 0;
        }

        // Token: 0x06000088 RID: 136 RVA: 0x00004750 File Offset: 0x00002950
        protected override void Callback()
        {
            bool flag = LightCallback.IsKeyPushedDown(Keys.F1);
            if (flag)
            {
                base.Abort();
            }
            try
            {
                bool flag2 = this.where == 5;
                if (flag2)
                {
                    double doubleInfo = base.GetDoubleInfo(6002);
                    bool flag3 = doubleInfo < 10.0 && doubleInfo - this.lasttime > 0.1;
                    if (flag3)
                    {
                        this.LowerBounds.Add(new Tuple<double, double>(doubleInfo, base.GetDoubleInfo(5004)));
                        this.UpperBounds.Add(new Tuple<double, double>(doubleInfo, base.GetDoubleInfo(5003)));
                        this.lasttime = doubleInfo;
                    }
                    else
                    {
                        bool flag4 = doubleInfo >= 10.0 && doubleInfo - this.lasttime > 3.0;
                        if (flag4)
                        {
                            this.LowerBounds.Add(new Tuple<double, double>(doubleInfo, base.GetDoubleInfo(5004)));
                            this.UpperBounds.Add(new Tuple<double, double>(doubleInfo, base.GetDoubleInfo(5003)));
                            this.lasttime = doubleInfo;
                        }
                    }
                }
                else
                {
                    bool flag5 = this.where == 4;
                    if (flag5)
                    {
                        double doubleInfo2 = base.GetDoubleInfo(6002);
                        this.LowerBounds.Add(new Tuple<double, double>(doubleInfo2, base.GetDoubleInfo(4004)));
                        this.UpperBounds.Add(new Tuple<double, double>(doubleInfo2, base.GetDoubleInfo(4003)));
                        this.lasttime = doubleInfo2;
                    }
                }
            }
            catch (GRBException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        // Token: 0x0400004E RID: 78
        public List<Tuple<double, double>> LowerBounds = new List<Tuple<double, double>>();

        // Token: 0x0400004F RID: 79
        public List<Tuple<double, double>> UpperBounds = new List<Tuple<double, double>>();

        // Token: 0x04000050 RID: 80
        public double lasttime = 0.0;
    }
}
