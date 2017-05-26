using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Assets
{
   /// <summary>
   /// Static math routines for projectsnstuff
   /// </summary>
   public static class MathRoutines
   {

      // Don't ask if it works
      static public bool ClockWise(float _a, float _b)
      {
         float a = _a + (float)Math.PI;
         float b = _b + (float)Math.PI;

         if (a < (float)Math.PI / 2 && b > (3 / 2) * (float)Math.PI)
         {
            return b > a;
         }
         else if (a > (3 / 2) * (float)Math.PI && b < (float)Math.PI / 2)
         {
            return b > a;
         }
         return a > b;
      }

      static public float AngleDifference(float _a, float _b)
      {
         float a = _a + (float)Math.PI;
         float b = _b + (float)Math.PI;

         float diff = Math.Max(a, b) - Math.Min(a, b);
         if (diff > (float)Math.PI)
            diff = (float)Math.PI * 2 - diff;

         return diff;
      }

      /// <summary>
      /// Converts rotation angle to vector components.
      /// Angle 0 point upwards (0,-1) and angle pi/2 points right (1,0).
      /// </summary>
      /// <param name="rotation"></param>
      /// <returns></returns>
      static public Vector2 getComponentsByRotation(float rotation)
      {
         float Xc;
         float Yc;

         double convRot = (double)rotation;
         Xc = (float)Math.Sin(convRot);
         Yc = -(float)Math.Cos(convRot);
         return new Vector2(Xc, Yc);

      }

      /// <summary>
      /// Calculates the angle between two objects
      /// </summary>
      /// <param name="diff">The difference vector</param>
      /// <returns>angle between 0 and 2PI</returns>
      static public float getRotationByComponents(Vector2 diff)
      {
         return (float)Math.Atan2(-diff.X, diff.Y) + (float)Math.PI;
      }

      /// <summary>
      /// Calculates the distance between two points
      /// </summary>
      /// <param name="pos1"></param>
      /// <param name="pos2"></param>
      /// <returns></returns>
      static public double getDistance(Vector2 pos1, Vector2 pos2)
      {
         return Vector2.Distance(pos1, pos2);
      }

      /// <summary>
      /// Pyörittää pistettä point pisteen center suhteen annetun kulman verran.
      /// </summary>
      /// <param name="point"></param>
      /// <param name="center"></param>
      /// <param name="Rotation"></param>
      /// <returns></returns>
      static public Vector2 projectWithRotation(Vector2 point, Vector2 center, float Rotation)
      {
         return center + getComponentsByRotation(getRotationByComponents(-center + point) + Rotation) * (float)getDistance(center, point);

         /* edellä tehty kaikki samassa, vältytään uusien olioiden luonnilta, jos se nyt mitään auttaa
         float angle_between = getRotationByComponents( - center + point);
         Vector2 movedPoint;
         movedPoint = getComponentsByRotation(angle_between + Rotation);
         movedPoint = center + movedPoint * (float)getDistance(center, point);
         return movedPoint;
          */
      }


      /// <summary>
      /// Tutkii sisältääkö pyöritetty suorakulmio annetun pisteen.
      /// Ei oikeasti pyöritä suorakulmiota, vaan pyörittää pistettä vastakkaiseen suuntaan.
      /// </summary>
      /// <param name="rect"></param>
      /// <param name="rRotation"></param>
      /// <param name="point"></param>
      /// <returns></returns>
      static public bool rectangleContains(Rectangle rect, float rRotation, Vector2 point)
      {
         Vector2 center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
         Vector2 movedPoint = projectWithRotation(point, center, -rRotation);
         if (rect.Contains(new Point((int)movedPoint.X, (int)movedPoint.Y))) return true;
         else return false;
      }


      /// <summary>
      /// Detects rectangle collision
      /// </summary>
      /// <param name="rect1"></param>
      /// <param name="r1Rotation"></param>
      /// <param name="rect2"></param>
      /// <param name="r2Rotation"></param>
      /// <returns></returns>
      static public bool RectanglesCollide(Rectangle rect1, float r1Rotation, Rectangle rect2, float r2Rotation)
      {
         float rotation;
         Rectangle rect;
         Rectangle recta;

         Vector2 corner;
         Vector2 center;

         int loop = 0;
         int outerloop = 0;
         while (outerloop < 2)
         {
            if (outerloop == 0)
            {
               rotation = r1Rotation;
               rect = rect2;
               recta = rect1;
            }
            else
            {
               rotation = r2Rotation;
               rect = rect1;
               recta = rect2;
            }
            while (loop < 4)
            {
               if (loop == 0)
               {
                  corner = new Vector2(recta.X, recta.Y);
                  center = new Vector2(recta.X + recta.Width / 2, recta.Y + recta.Height / 2);
               }
               else if (loop == 1)
               {
                  corner = new Vector2(recta.X + recta.Width, recta.Y);
                  center = new Vector2(recta.X + recta.Width / 2, recta.Y + recta.Height / 2);
               }
               else if (loop == 2)
               {
                  corner = new Vector2(recta.X + recta.Width, recta.Y + recta.Height);
                  center = new Vector2(recta.X + recta.Width / 2, recta.Y + recta.Height / 2);
               }
               else
               {
                  corner = new Vector2(recta.X, recta.Y + recta.Height);
                  center = new Vector2(recta.X + recta.Width / 2, recta.Y + recta.Height / 2);
               }

               float a_centerToCorner = getRotationByComponents(new Vector2(center.X - corner.X, center.Y - corner.Y));

               Vector2 kulmaPiste = projectWithRotation(corner, center, r1Rotation - a_centerToCorner);

               if (rectangleContains(rect, rotation, kulmaPiste)) return true;
               loop++;
            }
            outerloop++;
         }

         return false;
      }

      /// <summary>
      /// Clamppaa annetun integerin annetulle välille
      /// </summary>
      /// <param name="val">arvo</param>
      /// <param name="min">minimi</param>
      /// <param name="max">maksimi</param>
      /// <returns></returns>
      public static int rajoitaInt(int val, int min, int max)
      {
         if (val < min) return min;
         else if (val > max) return max;
         return val;
      }

      /// <summary>
      /// Checks if a point is inside a specified circle.
      /// </summary>
      /// <param name="center"></param>
      /// <param name="r"></param>
      /// <param name="piste"></param>
      /// <returns></returns>
      static public bool circleContainsPoint(Vector2 center, float r, Vector2 piste)
      {
         return Vector2.DistanceSquared(center, piste) < r * r;
      }

      /// <summary>
      /// Calculate intersection between two lines. If the lines don't intersect,
      /// this will return Vector2.Zero
      /// </summary>
      /// <param name="L1Start">First line's start point</param>
      /// <param name="L1End">First line's end point</param>
      /// <param name="L2Start">The other line's start point</param>
      /// <param name="L2End">The other line's end point</param>
      /// <returns></returns>
      static public Vector2 lineIntersection(Vector2 L1Start, Vector2 L1End, Vector2 L2Start, Vector2 L2End)
      {
         //Varastettu Internetistä ja portattu C++->C#
         //Lähde http://local.wasp.uwa.edu.au/~pbourke/geometry/lineline2d/example.cpp
         float denom = ((L2End.Y - L2Start.Y) * (L1End.X - L1Start.X)) -
                   ((L2End.X - L2Start.X) * (L1End.Y - L1Start.Y));

         float nume_a = ((L2End.X - L2Start.X) * (L1Start.Y - L2Start.Y)) -
                        ((L2End.Y - L2Start.Y) * (L1Start.X - L2Start.X));

         float nume_b = ((L1End.X - L1Start.X) * (L1Start.Y - L2Start.Y)) -
                        ((L1End.Y - L1Start.Y) * (L1Start.X - L2Start.X));

         if (denom == 0.0f)
         {
            if (nume_a == 0.0f && nume_b == 0.0f)
            {
               // Samansuuntaiset
               return Vector2.Zero;
            }
            // Rinnakkaiset
            return Vector2.Zero;
         }

         float ua = nume_a / denom;
         float ub = nume_b / denom;

         if (ua >= 0.0f && ua <= 1.0f && ub >= 0.0f && ub <= 1.0f)
         {
            // Get the intersection point.
            float interx = L1Start.X + ua * (L1End.X - L1Start.X);
            float intery = L1Start.Y + ua * (L1End.Y - L1Start.Y);

            return new Vector2(interx, intery);
         }
         return Vector2.Zero;
      }

      /// <summary>
      /// Checks if sphere hits rectangle in 3d-space and returns boolean value
      /// </summary>
      /// <param name="rCorners">Points that define rectangle </param>
      /// <param name="sCenter">Center point of sphere</param>
      /// <param name="sRadius">Radius of the sphere</param>
      static public bool sphereHitsRectangle(Vector3[] rCorners, Vector3 sCenter, float sRadius)
      {
         // TODO: tämä toimimaan, palauttaa true jos pallo osuu suorakaiteeseen, muuten false

         // rCorners on neljän alkion array pisteistä, jotka määrittelevät suorakulmion
         // sCenter on pallon keskipiste ja sRadius sen säde
         // note: kolmedee, koordinaatit siis mallia (x, y, z)
         BoundingBox bb = BoundingBox.CreateFromPoints(rCorners);
         BoundingSphere bs = new BoundingSphere(sCenter, sRadius);
         return bs.Intersects(bb);
      }

      /// <summary>
      /// Tutkii onko vektorien määrät pisteet myötäpäivään vai vastapäivään.
      /// Toisin sanoen on c janan ab oikealla vai vasemmalla puolella.
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <param name="c"></param>
      /// <returns></returns>
      public static int clockwise(Vector2 a, Vector2 b, Vector2 c)
      {
         float w = a.X * b.Y - a.Y * b.X + b.X * c.Y - b.Y * c.X + c.X * a.Y - c.Y * a.X;
         if (w > 0) return 1;
         else if (w < 0) return -1;
         else return 0;
      }

      public static double angleBetween(Vector2 a, Vector2 b)
      {
         return Math.Acos(Vector2.Dot(a, b) / (a.Length() * b.Length()));
      }

      /// <summary>
      /// Returns given rotation in a value from 0 to 2*Pi
      /// </summary>
      /// <param name="rot">Fucked-up angle</param>
      public static float simplifyRotation(float rot)
      {
         rot = rot % (2f * (float)(Math.PI));
         if (rot < 0) rot = (2f * (float)Math.PI) - (rot * -1);
         return rot;
      }

      /// <summary>
      /// Rotate vector2
      /// </summary>
      /// <param name="point"></param>
      /// <param name="radians"></param>
      /// <returns></returns>
      public static Vector2 RotateVector2(Vector2 point, float radians)
      {
         float cosRadians = (float)Math.Cos(radians);
         float sinRadians = (float)Math.Sin(radians);

         return new Vector2(
             point.X * cosRadians - point.Y * sinRadians,
             point.X * sinRadians + point.Y * cosRadians);
      }

      /// <summary>
      /// Tarkistaa onko r2 kokonaan r2 sisällä
      /// </summary>
      /// <param name="r1"></param>
      /// <param name="r2"></param>
      /// <returns></returns>
      public static bool TotallyContains(Rectangle r1, Rectangle r2)
      {
         Point p1 = new Point(r2.X, r2.Y);
         Point p2 = new Point(r2.X + r2.Width, r2.Y);
         Point p3 = new Point(r2.X, r2.Y + r2.Height);
         Point p4 = new Point(r2.X + r2.Width, r2.Y + r2.Height);

         if (r1.Contains(p1) && r1.Contains(p2) && r1.Contains(p3) && r1.Contains(p4))
            return true;
         else
            return false;
      }

      // If a rectangle collides with a vector
      public static bool RectCollision(Vector2 position, Vector2 oldPosition, Rectangle r, float angle = 0)
      {
         Vector2 topL = new Vector2(r.Left, r.Top);
         Vector2 topR = new Vector2(r.Right, r.Top);

         Vector2 downL = new Vector2(r.Left, r.Bottom);
         Vector2 downR = new Vector2(r.Right, r.Bottom);

         if (angle != 0)
         {
            Vector2 center = new Vector2(r.Center.X, r.Center.Y);

            topL = MathRoutines.projectWithRotation(new Vector2(r.Left, r.Top), center, angle);
            topR = MathRoutines.projectWithRotation(new Vector2(r.Right, r.Top), center, angle);

            downL = MathRoutines.projectWithRotation(new Vector2(r.Left, r.Bottom), center, angle);
            downR = MathRoutines.projectWithRotation(new Vector2(r.Right, r.Bottom), center, angle);
         }

         if (r.Contains(new Point((int)position.X, (int)position.Y)) && r.Contains(new Point((int)oldPosition.X, (int)oldPosition.Y)))
            return true;
         if (MathRoutines.lineIntersection(oldPosition, position, topL, topR) != Vector2.Zero)
            return true;
         if (MathRoutines.lineIntersection(oldPosition, position, downL, downR) != Vector2.Zero)
            return true;
         if (MathRoutines.lineIntersection(oldPosition, position, topL, downL) != Vector2.Zero)
            return true;
         if (MathRoutines.lineIntersection(oldPosition, position, topR, downR) != Vector2.Zero)
            return true;

         return false;
      }

      public static float DegreeToRadian(float angle)
      {
         return (float)Math.PI * angle / 180.0f;
      }

      public static float RadianToDegree(float angle)
      {
         return angle * (180.0f / (float)Math.PI);
      }
   }
}
