using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ConvertISO
{
    public class AntiArc : Shape
    {
        private PointF arcCenter;

        private float startAng;

        private float endAng;

        private float radius;

        public AntiArc(PointF startPoint, PointF endPoint, PointF relPos)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;

            this.arcCenter.Y = this.StartPoint.Y + relPos.Y;
            this.arcCenter.X = this.StartPoint.X + relPos.X;

            this.radius = (float)(Math.Sqrt(relPos.X * relPos.X + relPos.Y * relPos.Y));

            this.startAng = ArcMethod.GetAngle(-relPos.Y / this.radius, -relPos.X / this.radius);

            if (startPoint.X == endPoint.X && startPoint.Y == endPoint.Y)
                this.endAng = this.startAng;
            else
                this.endAng = ArcMethod.GetAngle((this.EndPoint.Y - this.StartPoint.Y - relPos.Y) / this.radius,
                (this.EndPoint.X - this.StartPoint.X - relPos.X) / this.radius);
        }

        public override void GDIDraw(Graphics grp, float frameHeight, float x, float y,Brush brush)
        {
            Pen pen = new Pen(Color.Green, 1.0f);

            RectangleF rect = new RectangleF();

            rect.X = this.arcCenter.X * 10 - this.radius * 10 + x;
            rect.Y = frameHeight - (this.arcCenter.Y * 10 + this.radius * 10) - y;

            rect.Width = 20 * this.radius;
            rect.Height = 20 * this.radius;

            float ang = this.endAng - this.startAng;

            if (ang <= 0)
                ang += 360;

            grp.DrawArc(pen, rect, 360 - this.endAng, ang);
            grp.FillEllipse(brush, this.StartPoint.X * 10 + x - 4, frameHeight - this.StartPoint.Y * 10 - y - 4, 8, 8);
        }

        public override void GDIDraw(Graphics grp, float frameHeight, Brush brush)
        {
            this.GDIDraw(grp, frameHeight, 0, 0,brush);
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
