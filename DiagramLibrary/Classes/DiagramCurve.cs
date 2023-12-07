using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace DiagramLibrary
{
    public class DiagramCurve:DiagramObject
    {
        private Curve m_Curve;
        public override string DiagramObjectType() { return "DiagramCurve"; }




        public static DiagramCurve Create(Curve crv, Color Colour, float LineWeight)
        {
            DiagramCurve diagramCurve = new DiagramCurve();
            diagramCurve.m_Colour = Colour;
            diagramCurve.m_LineWeight = LineWeight;
            diagramCurve.m_Curve = crv;

         
            return diagramCurve;
        }

        public DiagramCurve Duplicate()
        {
            DiagramCurve diagramCurve = new DiagramCurve();
            diagramCurve.m_Colour = m_Colour;
            diagramCurve.m_LineWeight = m_LineWeight;
            diagramCurve.m_Curve = m_Curve;
            
            return diagramCurve;
        }


        public override void DrawBitmap(Graphics g)
        {
           
            PointF[] pts = GetPoints();
                g.DrawLines(this.GetPen(), pts);
         
            
        }

        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform transform, bool colorOverride)
        {
            Color clr = Diagram.SelectedColor;
            if (colorOverride == false)
            {
                clr = m_Colour;
            }

            int thickness = (int)this.m_LineWeight;
            if (thickness <= 0)
            {
                thickness = 1;
            }

            if (transform != Transform.ZeroTransformation) { 
            Curve transformedCurve = m_Curve.DuplicateCurve();
            transformedCurve.Transform(transform);

            pipeline.DrawCurve(transformedCurve, clr, thickness);
        } else {
                pipeline.DrawCurve(m_Curve, clr, thickness);
            }


        }


        public PointF[] GetPoints()
        {
            PolylineCurve polyc = m_Curve.ToPolyline(0.01, 0.01, 1, 1000);
            

            PointF[] pts = new PointF[polyc.PointCount];
            for (int i = 0; i < polyc.PointCount; i++)
            {
                pts[i] = new PointF((float)polyc.Point(i).X, (float)polyc.Point(i).Y);
            }

            return pts;

            
        }

       public  Curve GetCurve() {
            return m_Curve;
        }



    }
}
