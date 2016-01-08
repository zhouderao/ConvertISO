using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertISO
{
    public partial class FormMain : Form
    {
        private List<Shape> shapes = new List<Shape>();
        public FormMain()
        {
            InitializeComponent();
        }


        private void btnOpn_Click(object sender, EventArgs e)
        {
            int n = 12;
            string path = @"D:";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "3b代码文件|*.3b";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //相关信息重置
                shapes.Clear();
                this.lstBxB.Items.Clear();
                this.lstBxG.Items.Clear();


                PointF staPnt = new PointF();
                PointF endPnt = new PointF();
                PointF relPnt = new PointF();
                path = ofd.FileName;
                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                string strLine = null;
                string outStr = null;
                #region 字符串转换
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (strLine.Contains("Start Point"))
                    {
                        this.lstBxB.Items.Add(strLine);

                        strLine = strLine.Substring(strLine.IndexOf('=') + 1, strLine.IndexOf(';') - strLine.IndexOf('=') - 1);

                        string[] strArr = strLine.Split(',');

                        staPnt.X = (float)(Convert.ToDouble(strArr[0]));
                        staPnt.Y = (float)(Convert.ToDouble(strArr[1]));

                        outStr = "N10 T84 T86 G90 G92X";
                        outStr += String.Format("{0:N3}", staPnt.X) + "Y" + String.Format("{0:N3}", staPnt.Y) + ";";
                        this.lstBxG.Items.Add(outStr);
                    }
                    else if (strLine.Count(c => c == 'B') == 3)
                    {
                        this.lstBxB.Items.Add(strLine);
                        if (strLine.Contains("L"))
                        {
                            endPnt = this.getPoint(strLine);
                            outStr = "N" + n.ToString() + " G01 X";
                            outStr += String.Format("{0:N3}", endPnt.X) + " Y" + String.Format("{0:N3}",endPnt.Y) + " ;";

                            Line line = new Line(staPnt, endPnt);
                            shapes.Add(line);

                            n += 2;
                            staPnt = endPnt;

                            this.lstBxG.Items.Add(outStr);
                        }
                        else if (strLine.Contains("SR"))
                        {
                            endPnt = this.getPoint(strLine);

                            int quad = (int)Convert.ToDouble(strLine.Substring(strLine.IndexOf('R') + 1, 1));
                            relPnt = this.getRelPoint(strLine.Substring(strLine.IndexOf('B') + 1, strLine.IndexOf('G')),quad);

                            outStr = "N" + n.ToString() + " G02 X";
                            outStr += String.Format("{0:N3}", endPnt.X) + " Y" + String.Format("{0:N3}", endPnt.Y) +" I"+
                                String.Format("{0:N3}", relPnt.X) + " J" + String.Format("{0:N3}", relPnt.Y) + " ;";

                            Arc arc = new Arc(staPnt,endPnt,relPnt);

                            shapes.Add(arc);

                            n += 2;
                            staPnt = endPnt;

                            this.lstBxG.Items.Add(outStr);
                        }
                        else if (strLine.Contains("NR"))
                        {
                            endPnt = this.getPoint(strLine);
                            int quad = (int)Convert.ToDouble(strLine.Substring(strLine.IndexOf('R') + 1, 1));
                            relPnt = this.getRelPoint(strLine.Substring(strLine.IndexOf('B') + 1, strLine.IndexOf('G')),quad);

                            outStr = "N" + n.ToString() + " G03 X";
                            outStr += String.Format("{0:N3}", endPnt.X) + " Y" + String.Format("{0:N3}", endPnt.Y) + " I" +
                                String.Format("{0:N3}", relPnt.X) + " J" + String.Format("{0:N3}", relPnt.Y) + " ;";


                            AntiArc arc = new AntiArc(staPnt, endPnt, relPnt);
                            shapes.Add(arc);

                            n += 2;
                            staPnt = endPnt;

                            this.lstBxG.Items.Add(outStr);
                        }

                    }

                }
                #endregion

                Graphics grp = this.pnlDraw.CreateGraphics();

                grp.Clear(this.pnlDraw.BackColor);

                int tempI = 0;
                foreach (Shape shape in shapes)
                {
                    shape.GDIDraw(grp, this.pnlDraw.Height, 400, 400, GetBrush(tempI));
                    tempI++;
                }
                outStr = "N" + n.ToString() + " T85 T87 M02;";
                this.lstBxG.Items.Add(outStr);
                sr.Close();
            }
            ofd.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.lstBxG.Items.Count == 0)
            {
                MessageBox.Show("不存在路径，请载入", "提示");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "标准G代码文件|*.iso";
            
            string path = string.Empty;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName;
                string strName = "(" + sfd.FileName.Substring(path.LastIndexOf("\\") + 1) + " " + System.DateTime.Now.ToString() + ")";
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine(strName);
                foreach (String str in this.lstBxG.Items)
                {
                    sw.WriteLine(str);
                }

                sw.Close();
                fs.Close();
            }
            sfd.Dispose();
        }

        private PointF getPoint(string str)
        {
            str = str.Substring(str.IndexOf(';') + 1);
            string[] arr = str.Split(',');

            float x = (float)Convert.ToDouble((arr[0]));
            float y = (float)Convert.ToDouble((arr[1]));

            return new PointF(x, y);
        }

        private PointF getRelPoint(string str,int n)
        {
            string[] arr = str.Split('B');
            float x = (float)(Convert.ToDouble((arr[0])) / 1000);
            float y = (float)(Convert.ToDouble((arr[1])) / 1000);

            switch (n)
            {
                case 1:
                    x *= -1;
                    y *= -1;
                    break;
                case 2:
                    y *= -1;
                    break;
                case 4:
                    x *= -1;
                    break;
                default:
                    break;
            }

            return new PointF(x, y);
        }


        private Brush GetBrush(int tempNum)
        {
            Brush brush = Brushes.White;
            switch (tempNum % 6)
            {
                case 0:
                    brush = Brushes.Red;
                    break;
                case 1:
                    brush = Brushes.Yellow;
                    break;
                case 2:
                    brush = Brushes.Green;
                    break;
                case 3:
                    brush = Brushes.Aqua;
                    break;
                case 4:
                    brush = Brushes.Blue;
                    break;
                case 5:
                    brush = Brushes.Magenta;
                    break;
            }
            return brush;
        }
    }
}
