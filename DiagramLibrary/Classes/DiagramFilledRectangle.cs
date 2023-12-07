using Rhino.Geometry;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramFilledRectangle : DiagramFilledCurve
    {
        

        public static DiagramFilledRectangle Create(Rectangle3d outerRectangle, Color colour, bool drawLine, Color lineColour, float lineWeight)
        {
            DiagramFilledRectangle rectangle = new DiagramFilledRectangle();
            rectangle.m_Colour = colour;
            rectangle.m_LineWeight = lineWeight;
            rectangle.m_DrawLine = drawLine;
            rectangle.m_LineColor = lineColour;

            
                rectangle.m_OuterCurves.Add(DiagramCurve.Create(outerRectangle.ToNurbsCurve(), lineColour, lineWeight));
           

            return rectangle;
        }

        public void UpdateRectangle(Rectangle3d newRectangle)
        {
            if (m_OuterCurves.Count > 0)
            {
                // Update the first (and only) outer curve with the new rectangle
                m_OuterCurves[0] =  DiagramCurve.Create(newRectangle.ToNurbsCurve(), m_LineColor, m_LineWeight);
            }
            else
            {
                // If there are no outer curves, add the new rectangle as the first outer curve
                m_OuterCurves.Add(DiagramCurve.Create(newRectangle.ToNurbsCurve(), m_LineColor, m_LineWeight));
            }
        }

        public void UpdateRectangle(PointF origin, SizeF size)
        {
            Rectangle3d newRectangle = new Rectangle3d(new Plane(Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis), new Point3d(origin.X, origin.Y, 0), new Point3d(origin.X + size.Width, origin.Y + size.Height, 0));
            UpdateRectangle(newRectangle);
        }

        public new DiagramFilledRectangle Duplicate()
        {
            DiagramFilledRectangle duplicatedRectangle = new DiagramFilledRectangle();


            duplicatedRectangle.m_OuterCurves = m_OuterCurves;
            duplicatedRectangle.m_InnerCurves = m_InnerCurves;

           
            duplicatedRectangle.m_Colour = m_Colour;
            duplicatedRectangle.m_LineWeight = m_LineWeight;
            duplicatedRectangle.m_DrawLine = m_DrawLine;
            duplicatedRectangle.m_LineColor = m_LineColor;

            return duplicatedRectangle;
        }
    }
}
