using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
   public  class DiagramFilledCurve : DiagramObject
    {
       protected bool m_DrawLine;
        protected Color m_LineColor;
       
        public override string DiagramObjectType() { return "DiagramFilledCurve"; }

        protected List<DiagramCurve> m_InnerCurves =  new List<DiagramCurve>();
        protected List<DiagramCurve> m_OuterCurves = new List<DiagramCurve>();


        public static DiagramFilledCurve Create(Curve[] OuterCurves, Curve[] InnerCurves, Color Colour, bool drawLine, Color LineColour, float LineWeight)
        {

            DiagramFilledCurve diagramFilledCurve = new DiagramFilledCurve();
            diagramFilledCurve.m_Colour = Colour;
            diagramFilledCurve.m_LineWeight = LineWeight;
            diagramFilledCurve.m_DrawLine = drawLine;
            diagramFilledCurve. m_LineColor = LineColour;

            for (int i = 0; i < OuterCurves.Length; i++)
            {
                diagramFilledCurve. m_OuterCurves.Add(DiagramCurve.Create(OuterCurves[i], LineColour, LineWeight));
            }
            if (InnerCurves != null)
            {
                for (int i = 0; i < InnerCurves.Length; i++)
                {
                    diagramFilledCurve.m_InnerCurves.Add(DiagramCurve.Create(InnerCurves[i], LineColour, LineWeight));
                }
            }


            return diagramFilledCurve;
        }

        public static List<DiagramFilledCurve> CreateFromBrep(Brep brep, Color Colour, bool drawLine, Color LineColour, float LineWeight)
        {
            List<DiagramFilledCurve> hatches = new List<DiagramFilledCurve>();

            for (int i = 0; i < brep.Faces.Count; i++)
            {

                Curve[] crvsInner = brep.Faces[i].DuplicateFace(false).DuplicateNakedEdgeCurves(false, true);
                Curve[] crvsOuter = brep.Faces[i].DuplicateFace(false).DuplicateNakedEdgeCurves(true, false);


                DiagramFilledCurve dHatch = DiagramFilledCurve.Create(crvsOuter, crvsInner, Colour, drawLine, LineColour, LineWeight);
                hatches.Add(dHatch);


            }

           

            return hatches;
        }
      
        public DiagramFilledCurve Duplicate()
        {
            DiagramFilledCurve diagramFilledCurve = new DiagramFilledCurve();
            diagramFilledCurve.m_Colour = m_Colour;
            diagramFilledCurve.m_LineWeight = m_LineWeight;
            diagramFilledCurve.m_DrawLine = m_DrawLine;
            diagramFilledCurve.m_LineColor = m_LineColor;
            diagramFilledCurve.m_OuterCurves = m_OuterCurves;
            diagramFilledCurve.m_InnerCurves = m_InnerCurves;
            
            return diagramFilledCurve;
        }





        public override void DrawBitmap(Graphics g)

        {

            

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
           

           List<Point3d> points3d = new List<Point3d>();

            foreach (DiagramCurve crv in m_OuterCurves)
            {
               
                PointF[] pts = crv.GetPoints();
              
                path.AddLines(pts);
            }

            foreach (DiagramCurve crv in m_InnerCurves)
            {
                PointF[] pts = crv.GetPoints();

                path.AddLines(pts);
            }


        
            g.FillPath(this.GetBrush(),path);

            if (m_DrawLine) {
                g.DrawPath(this.GetPen(), path);

            }


        }


        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance)
        {
            List<DiagramCurve> dcrvs = new List<DiagramCurve>();
            dcrvs.AddRange(m_OuterCurves);
            dcrvs.AddRange(m_InnerCurves);
            Curve[] crvs = dcrvs.Select(x => (Curve)x.GetCurve()).ToArray();
            Brep[] breps = Brep.CreatePlanarBreps(crvs, tolerance);

            foreach (var item in breps)
            {
                pipeline.DrawBrepShaded(item, new Rhino.Display.DisplayMaterial(m_Colour, 1.0 - (m_Colour.A / 255)));
            }



            if (m_DrawLine) {
                foreach (var item in m_OuterCurves)
                {
                    item.DrawRhinoPreview(pipeline, tolerance);
                }
                foreach (var item in m_InnerCurves)
                {
                    item.DrawRhinoPreview(pipeline, tolerance);
                }

            }


        }

       


        public new Pen GetPen()
        {
            return new Pen(m_LineColor, m_LineWeight);
        }

    }
}
