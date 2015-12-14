using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertISO
{
    public static class ArcMethod
    {
        public static float GetAngle(float sinValue,float cosValue)
        {
            if (cosValue == 0)
            {
                if (sinValue > 0)
                    return 90;
                else
                    return 270;
            }
            else if (cosValue > 0)
            {
                float angle = (float)(Math.Atan(sinValue / cosValue) * 180 / Math.PI);
                if (angle < 0)
                    angle += 360;
                return angle;
            }
            else
            {
                float angle = (float)(Math.Atan(sinValue / cosValue) * 180 / Math.PI) + 180;
                return angle;
            }
        }

    }
}
