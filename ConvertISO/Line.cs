using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ConvertISO
{
    public class Line : Shape
    {
        //构造函数
        public Line(PointF startPoint, PointF endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }


        public override void GDIDraw(Graphics grp, float frameHeight, float x, float y,Brush brush)
        {
            Pen pen = new Pen(Color.Green, 1.0f);

            PointF point1 = new PointF(0, 0);
            PointF point2 = new PointF(0, 0);
            point1.X = this.StartPoint.X + x;
            point1.Y = frameHeight - this.StartPoint.Y - y;
            point2.X = this.EndPoint.X + x;
            point2.Y = frameHeight - this.EndPoint.Y - y;
            grp.DrawLine(pen, point1, point2);

            grp.FillEllipse(brush, this.StartPoint.X + x - 4, frameHeight - this.StartPoint.Y - y - 4, 8, 8);
        }

        public override void GDIDraw(Graphics grp, float frameHeight, Brush brush)
        {
            this.GDIDraw(grp, frameHeight, 0, 0, brush);
        }

        public override void ChangeZeroPos(float x, float y)
        {
            PointF point = new PointF();
            point.X = this.StartPoint.X + x;
            point.Y = this.StartPoint.Y + y;
            this.StartPoint = point;
            point.X = this.EndPoint.X + x;
            point.Y = this.EndPoint.Y + y;
            this.EndPoint = point;
        }


    }
}
