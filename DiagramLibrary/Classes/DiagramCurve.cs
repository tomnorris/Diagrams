using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace DiagramLibrary
{
    public class DiagramCurve : BaseCurveDiagramObject
    {
        protected Curve m_Curve;
        protected DiagramCurveEnd m_StartCurveEnd = null;
        protected DiagramCurveEnd m_EndCurveEnd = null;



        public Curve Curve
        {
            get { return m_Curve; }
            set { m_Curve = value; }
        }

        public static DiagramCurve Create(Curve crv, Color Colour, float LineWeight)
        {
            DiagramCurve diagramCurve = new DiagramCurve();
            diagramCurve.m_Colour = Colour;
            diagramCurve.m_LineWeight = LineWeight;
            diagramCurve.m_Curve = crv;


            return diagramCurve;
        }

        public DiagramCurve DuplicateDiagramCurve()
        {
            return Duplicate() as DiagramCurve;
        }


        public override DiagramObject Duplicate()
        {
            DiagramCurve diagramCurve = new DiagramCurve();
            diagramCurve.m_Colour = m_Colour;
            diagramCurve.m_LineWeight = m_LineWeight;
            diagramCurve.m_Curve = m_Curve.DuplicateCurve();
            if (m_StartCurveEnd != null)
            {
                diagramCurve.m_StartCurveEnd = m_StartCurveEnd.DuplicateCurveEnd();

            }

            if (m_EndCurveEnd != null)
            {
                diagramCurve.m_EndCurveEnd = m_EndCurveEnd.DuplicateCurveEnd();
            }


            return diagramCurve;
        }

        public override BoundingBox GetBoundingBox()
        {
            return this.m_Curve.GetBoundingBox(true);
        }

        public void AddCurveEnds(BaseCurveDiagramObject start, Point3d setPointStart, Vector3d setDirectionStart, BaseCurveDiagramObject end, Point3d setPointEnd, Vector3d setDirectionEnd)

        {

            if (start != null)
            {
                m_StartCurveEnd = new DiagramCurveEnd(start, setPointStart, setDirectionStart, true);

            }

            if (end != null)
            {
                m_EndCurveEnd = new DiagramCurveEnd(end, setPointEnd, setDirectionEnd, false);

            }


        }

        public void AddCurveEnds(DiagramCurveEnd start, DiagramCurveEnd end)

        {

            if (start != null)
            {
                m_StartCurveEnd = start;

            }

            if (end != null)
            {
                m_EndCurveEnd = end;

            }


        }




        public override BaseCurveDiagramObject SetLocationAndDirectionForDrawing(Point3d basePoint, Vector3d baseDirection, Point3d location, Vector3d rotation)
        {
            if (baseDirection == Vector3d.Unset)
            {
                return null;
            }



            DiagramCurve clone = Duplicate() as DiagramCurve;

            clone.m_Curve.Translate(new Vector3d(location.X - basePoint.X, location.Y - basePoint.Y, 0));
            double angle = Vector3d.VectorAngle(baseDirection, rotation, Plane.WorldXY);

            clone.m_Curve.Rotate(angle, Plane.WorldXY.Normal, location);

            return clone;
        }



        public override void DrawBitmap(Grasshopper.Kernel.GH_Component component, Graphics g)
        {

            if (m_StartCurveEnd != null)

            {
                m_StartCurveEnd.DrawBitmap(component, g, m_Curve.PointAtStart, m_Curve.TangentAtStart);
            }



            if (m_EndCurveEnd != null)
            {
                m_EndCurveEnd.DrawBitmap(component, g, m_Curve.PointAtEnd, m_Curve.TangentAtEnd);

            }





            PointF[] pts = GetPoints();
            if (pts != null)
            {
                g.DrawLines(this.GetPen(), pts);
            }



        }

        public override void DrawRhinoPreview(Grasshopper.Kernel.GH_Component component, Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform transform, bool colorOverride, Rhino.RhinoDoc doc, bool Bake)
        {
            Color clr = Diagram.SelectedColor;
            if (colorOverride == false)
            {
                clr = m_Colour;
            }


            if (m_StartCurveEnd != null)

            {
                m_StartCurveEnd.DrawRhinoPreview(component, pipeline, tolerance, transform, colorOverride, m_Curve.PointAtStart, m_Curve.TangentAtStart,  doc,  Bake);
            }


            if (m_EndCurveEnd != null)
            {
                m_EndCurveEnd.DrawRhinoPreview(component, pipeline, tolerance, transform, colorOverride, m_Curve.PointAtEnd, m_Curve.TangentAtEnd,  doc,  Bake);




            }
            int thickness = (int)this.m_LineWeight;
            if (thickness <= 0)
            {
                thickness = 1;
            }

            Curve drawCurve = m_Curve;
            if (transform != Transform.ZeroTransformation)
            {
                drawCurve = m_Curve.DuplicateCurve();
                drawCurve.Transform(transform);

               
            }



            if (Bake)
            {
                var attr = new Rhino.DocObjects.ObjectAttributes();
                attr.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;
                attr.ObjectColor = clr;
                attr.PlotWeightSource = Rhino.DocObjects.ObjectPlotWeightSource.PlotWeightFromObject;
                attr.PlotWeight = thickness;
                
                doc.Objects.AddCurve(drawCurve, attr);
            }
            else
            {
                pipeline.DrawCurve(m_Curve, clr, thickness);
            }


            }


        public PointF[] GetPoints()
        {
            PolylineCurve polyc = m_Curve.ToPolyline(0.01, 0.01, 1, 1000);

            if (polyc == null)
            {
                return null;
            }

            PointF[] pts = new PointF[polyc.PointCount];
            for (int i = 0; i < polyc.PointCount; i++)
            {
                pts[i] = Diagram.ConvertPoint(polyc.Point(i));
            }

            return pts;


        }

        public Curve GetCurve()
        {
            return m_Curve;
        }



    }
}
