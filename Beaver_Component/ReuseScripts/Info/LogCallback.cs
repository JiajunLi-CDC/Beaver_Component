using System;
using System.Drawing;
using System.Windows.Forms;
using Gurobi;
namespace Beaver.ReuseScripts.Info
{
    // Token: 0x02000011 RID: 17
    public class LogCallback : LightCallback
    {
        // Token: 0x1700002B RID: 43
        // (get) Token: 0x06000089 RID: 137 RVA: 0x000048FC File Offset: 0x00002AFC
        // (set) Token: 0x0600008A RID: 138 RVA: 0x00004904 File Offset: 0x00002B04
        public MyLog OutputForm
        {
            get; private set;
        }

        // Token: 0x1700002C RID: 44
        // (get) Token: 0x0600008B RID: 139 RVA: 0x0000490D File Offset: 0x00002B0D
        // (set) Token: 0x0600008C RID: 140 RVA: 0x00004915 File Offset: 0x00002B15
        public FormStartPosition Pos
        {
            get; private set;
        }

        // Token: 0x1700002D RID: 45
        // (get) Token: 0x0600008D RID: 141 RVA: 0x0000491E File Offset: 0x00002B1E
        // (set) Token: 0x0600008E RID: 142 RVA: 0x00004926 File Offset: 0x00002B26
        public Point Location
        {
            get; private set;
        }

        // Token: 0x1700002E RID: 46
        // (get) Token: 0x0600008F RID: 143 RVA: 0x0000492F File Offset: 0x00002B2F
        // (set) Token: 0x06000090 RID: 144 RVA: 0x00004937 File Offset: 0x00002B37
        public Label feedbackLabel
        {
            get; private set;
        }

        // Token: 0x1700002F RID: 47
        // (get) Token: 0x06000091 RID: 145 RVA: 0x00004940 File Offset: 0x00002B40
        // (set) Token: 0x06000092 RID: 146 RVA: 0x00004948 File Offset: 0x00002B48
        public MyListBox LB
        {
            get; private set;
        }

        // Token: 0x06000093 RID: 147 RVA: 0x00004954 File Offset: 0x00002B54
        public LogCallback(FormStartPosition StartPos = FormStartPosition.Manual, Point Location = default(Point), string LogFormName = "MyLogFormName")
        {
            this.OutputForm = new MyLog(StartPos, Location, LogFormName);
            this.LB = new MyListBox(this.OutputForm.Width - 20, this.OutputForm.Height - 40);
            this.OutputForm.Controls.Add(this.LB);
            this.OutputForm.Dock = DockStyle.Left;
            this.OutputForm.Show();
            this.OutputForm.Refresh();
            this.OutputForm.BringToFront();
        }

        // Token: 0x06000094 RID: 148 RVA: 0x000049E8 File Offset: 0x00002BE8
        protected override void Callback()
        {
            bool flag = LightCallback.IsKeyPushedDown(Keys.F1);
            if (flag)
            {
                base.Abort();
            }
            try
            {
                bool flag2 = this.where == 6;
                if (flag2)
                {
                    this.LB.Items.Add(base.GetStringInfo(6001));
                    bool flag3 = this.LB.Items.Count > 1000;
                    if (flag3)
                    {
                        this.LB.Items.RemoveAt(0);
                    }
                    this.LB.SelectedIndex = this.LB.Items.Count - 1;
                    this.LB.ClearSelected();
                }
                else
                {
                    bool flag4 = this.where == 5;
                    if (flag4)
                    {
                        double doubleInfo = base.GetDoubleInfo(6002);
                        bool flag5 = doubleInfo < 10.0 && doubleInfo - this.lasttime > 0.1;
                        if (flag5)
                        {
                            this.LowerBounds.Add(new Tuple<double, double>(doubleInfo, base.GetDoubleInfo(5004)));
                            this.UpperBounds.Add(new Tuple<double, double>(doubleInfo, base.GetDoubleInfo(5003)));
                            this.lasttime = doubleInfo;
                        }
                        else
                        {
                            bool flag6 = doubleInfo >= 10.0 && doubleInfo - this.lasttime > 3.0;
                            if (flag6)
                            {
                                this.LowerBounds.Add(new Tuple<double, double>(doubleInfo, base.GetDoubleInfo(5004)));
                                this.UpperBounds.Add(new Tuple<double, double>(doubleInfo, base.GetDoubleInfo(5003)));
                                this.lasttime = doubleInfo;
                            }
                        }
                    }
                    else
                    {
                        bool flag7 = this.where == 4;
                        if (flag7)
                        {
                            double doubleInfo2 = base.GetDoubleInfo(6002);
                            this.LowerBounds.Add(new Tuple<double, double>(doubleInfo2, base.GetDoubleInfo(4004)));
                            this.UpperBounds.Add(new Tuple<double, double>(doubleInfo2, base.GetDoubleInfo(4003)));
                            this.lasttime = doubleInfo2;
                        }
                    }
                }
            }
            catch (GRBException ex)
            {
                this.LB.Items.Add("Error code: " + ex.ErrorCode.ToString());
                this.LB.Items.Add(ex.Message);
                this.LB.Items.Add(ex.StackTrace);
            }
            catch (Exception ex2)
            {
                this.LB.Items.Add("Error during callback");
                this.LB.Items.Add(ex2.StackTrace);
            }
        }
    }
}
