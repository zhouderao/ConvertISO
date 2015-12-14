using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ConvertISO
{
    public abstract class Shape
    {

        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }


        //抽象方法
        public abstract void GDIDraw(Graphics grp, float frameHeight,Brush brush);
        public abstract void GDIDraw(Graphics grp, float frameHeight,float x,float y,Brush brush);
        public abstract void ChangeZeroPos(float x,float y);
    }
}
