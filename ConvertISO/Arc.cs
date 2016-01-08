using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ConvertISO
{
    public class Arc : Shape
    {
        private PointF arcCenter;

        private float startAng;

        private float endAng;

        private float radius;

        public Arc(PointF startPoint, PointF endPoint,PointF relPos)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;

            this.arcCenter.X = this.StartPoint.X + relPos.X;
            this.arcCenter.Y = this.StartPoint.Y + relPos.Y;

            this.radius = (float)(Math.Sqrt(relPos.X * relPos.X + relPos.Y * relPos.Y));

            this.startAng = ArcMethod.GetAngle(-relPos.Y / this.radius, -relPos.X / this.radius);

            if (startPoint.X == endPoint.X && startPoint.Y == endPoint.Y)
                this.endAng = this.startAng;
            else
                this.endAng = ArcMethod.GetAngle((this.EndPoint.Y - this.arcCenter.Y) / this.radius,
                (this.EndPoint.X - this.arcCenter.X) / this.radius);

        }

        public Arc(PointF center,float staAngle,float endAngle,float radius)
        {
            PointF sPnt = new PointF();
            PointF ePnt = new PointF();

            sPnt.X = (float)(center.X + radius * Math.Cos(staAngle * Math.PI / 180));
            sPnt.Y = (float)(center.Y + radius * Math.Sin(staAngle * Math.PI / 180));
            ePnt.X = (float)(center.X + radius * Math.Cos(endAngle * Math.PI / 180));
            ePnt.Y = (float)(center.Y + radius * Math.Sin(endAngle * Math.PI / 180));

            this.StartPoint = sPnt;
            this.EndPoint = ePnt;

            this.radius = radius;
            this.startAng = staAngle;
            this.endAng = endAngle;
            this.arcCenter = center;

        }

        public override void GDIDraw(Graphics grp, float frameHeight, float x, float y, Brush brush)
        {
            Pen pen = new Pen(Color.Green, 1.0f);

            RectangleF rect = new RectangleF();
            rect.X = this.arcCenter.X - this.radius + x;
            rect.Y = frameHeight - (this.arcCenter.Y + this.radius) - y;
            rect.Width = 2 * this.radius;
            rect.Height = 2 * this.radius;

            float ang = this.startAng - this.endAng;
            if (ang <= 0)
                ang += 360;
            grp.DrawArc(pen, rect, 360 - this.startAng, ang);
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
            point.X = this.arcCenter.X + x;
            point.Y = this.arcCenter.Y + y;
            this.arcCenter = point;
        }

    }
}
