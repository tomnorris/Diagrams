using Grasshopper.Kernel;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    public class DiagramPointLabel : DiagramObject
    {

        private PointF m_PointLocation;
        private DiagramText m_DiagramText;
        private DiagramFilledCurve m_Mask;

        public DiagramText DiagramText
        {
            get { return m_DiagramText; }
            set { m_DiagramText = value; }
        }

        public DiagramFilledCurve Mask
        {
            get { return m_Mask; }
            set { m_Mask = value; }
        }


        public static DiagramPointLabel Create(string text, PointF leaderLocation, Color colour, float textSize,
          Color maskColour, Color frameColor, float frameLineWeight,
          string fontName, float padding)
        {
            DiagramPointLabel diagramLabel = new DiagramPointLabel();
            diagramLabel.m_DiagramText = DiagramText.Create(text, leaderLocation, colour, textSize / Math.Max(text.Length / 1.667f, 1f), Rhino.Geometry.TextJustification.MiddleCenter, Color.Transparent, Color.Transparent, 0, fontName, new SizeF(textSize * 4, textSize * 4), padding, Rhino.Geometry.TextJustification.MiddleCenter);
            diagramLabel.m_PointLocation = leaderLocation;
            diagramLabel. m_Mask = DiagramFilledCurve.Create(new Circle(new Plane(new Point3d(leaderLocation.X, leaderLocation.Y, 0),Plane.WorldXY.ZAxis),textSize).ToNurbsCurve(), maskColour, frameColor, frameLineWeight);

            return diagramLabel;
        }

       

        public override DiagramObject Duplicate()
        {
            DiagramPointLabel diagramLabel = new DiagramPointLabel();
            diagramLabel.m_DiagramText = m_DiagramText.Duplicate() as DiagramText;
            diagramLabel.m_PointLocation = m_PointLocation;
            diagramLabel.m_Mask = m_Mask;
            return diagramLabel;
        }

        public override BoundingBox GetBoundingBox()
        {
            BoundingBox bb = new BoundingBox(new Point3d[] { Diagram.ConvertPoint(m_PointLocation) });
            bb.Union(m_DiagramText.GetBoundingBox());
            bb.Union(m_Mask.GetBoundingBox());
            return bb;
        }

        public override void DrawBitmap(GH_Component component, Graphics g)
        {
            m_Mask.DrawBitmap(component, g);
            m_DiagramText.DrawBitmap(component, g);
        }

        public override void DrawRhinoPreview(GH_Component component, DisplayPipeline pipeline, double tolerance, Transform xform, bool colorOverride, Rhino.RhinoDoc doc, bool Bake)
        {
            m_Mask.DrawRhinoPreview(component, pipeline,  tolerance,  xform,  colorOverride, doc,  Bake);
            m_DiagramText.DrawRhinoPreview(component, pipeline, tolerance, xform, colorOverride, doc,  Bake);
        }


    }
}
