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
    public class DiagramLabel : DiagramObject
    {

        private PointF m_LeaderLocation;
        private float m_Offset;
        private DiagramText m_DiagramText;
        private DiagramCurve m_leader;
      

        public DiagramText DiagramText
        {
            get { return m_DiagramText; }
            set { m_DiagramText = value; }
        }




        public static DiagramLabel Create(string text, PointF leaderLocation, float offset, Vector3d direction, Color colour, float lineWeight, float textSize,
          Color maskColour, Color frameColor, float frameLineWeight,
          string fontName, float padding, DiagramCurveEnd crvEnd)
        {


            DiagramLabel diagramLabel = new DiagramLabel();

            var line = new Line(new Point3d(leaderLocation.X, leaderLocation.Y, 0), direction, offset);
            diagramLabel.m_DiagramText = DiagramText.Create(text, new PointF((float)line.To.X,(float)line.To.Y), colour, textSize, Rhino.Geometry.TextJustification.BottomLeft, maskColour, frameColor, frameLineWeight, fontName, new SizeF(-1, -1), padding, Rhino.Geometry.TextJustification.BottomLeft);
            var line2 = new Line(line.To, new Vector3d(1,0,0), diagramLabel.m_DiagramText.GetTotalSize().Width);
            diagramLabel.m_leader = DiagramCurve.Create(new Polyline( new Point3d[] { line.From,line.To,line2.To }).ToNurbsCurve(), colour, lineWeight);

            if (crvEnd != null) {
                diagramLabel.m_leader.AddCurveEnds(crvEnd, null);
            }
            diagramLabel.m_LeaderLocation = leaderLocation;
            diagramLabel.m_Offset = offset;

            return diagramLabel;
        }

       

        public override DiagramObject Duplicate()
        {
            DiagramLabel diagramLabel = new DiagramLabel();
            diagramLabel.m_DiagramText = m_DiagramText.Duplicate() as DiagramText;
            diagramLabel.m_LeaderLocation = m_LeaderLocation;
            diagramLabel.m_Offset = m_Offset;
            diagramLabel.m_leader = m_leader;
            return diagramLabel;
        }

        public override BoundingBox GetBoundingBox()
        {
            BoundingBox bb = new BoundingBox(new Point3d[] { new Point3d(m_LeaderLocation.X, m_LeaderLocation.Y, 0) });
            bb.Union(m_DiagramText.GetBoundingBox());
         
            return bb;
        }

        public override void DrawBitmap(GH_Component component, Graphics g)
        {
            m_leader.DrawBitmap(component, g);
            m_DiagramText.DrawBitmap(component, g);
        }

        public override void DrawRhinoPreview(GH_Component component, DisplayPipeline pipeline, double tolerance, Transform xform, bool colorOverride)
        {
            m_leader.DrawRhinoPreview(component, pipeline, tolerance, xform, colorOverride);
            m_DiagramText.DrawRhinoPreview(component, pipeline, tolerance, xform, colorOverride);
        }


    }

  
}
