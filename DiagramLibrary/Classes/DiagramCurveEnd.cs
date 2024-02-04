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
   public class DiagramCurveEnd :NoDrawDiagramObject
    {



        BaseCurveDiagramObject m_Object = null;

        private Point3d m_BasePoint = Point3d.Unset;
        private Vector3d m_BaseDirection = Vector3d.Unset;
        private bool m_Flipped = false;

        public DiagramCurveEnd DuplicateCurveEnd() {

            return Duplicate() as DiagramCurveEnd;
        }

        public override DiagramObject Duplicate()
        {
            return new DiagramCurveEnd(m_Object.Duplicate() as BaseCurveDiagramObject, m_BasePoint, m_BaseDirection, m_Flipped);
        }

        public DiagramCurveEnd(BaseCurveDiagramObject diagramObject, Point3d location, Vector3d rotation, bool flipped)

        {
            m_Object = diagramObject.Duplicate() as BaseCurveDiagramObject;

            m_BasePoint = location;
            m_BaseDirection = rotation;
            m_Flipped = flipped;

        }

        public void Flip() {
            m_Flipped = !m_Flipped;
        }

    

        public void DrawRhinoPreview( Grasshopper.Kernel.GH_Component component,Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform transform, bool colorOverride,Point3d location, Vector3d rotation)

        {

            Vector3d flipCorrectedDirection = m_BaseDirection;
            if (m_Flipped) {
                flipCorrectedDirection.Reverse();
            }

            var positionedObject = m_Object.SetLocationAndDirectionForDrawing(m_BasePoint, flipCorrectedDirection, location, rotation);
            if (positionedObject != null)
            {
                positionedObject.DrawRhinoPreview( component,pipeline,  tolerance,  transform,  colorOverride);
            }
        }


        public void DrawBitmap(Grasshopper.Kernel.GH_Component component,Graphics g, Point3d location, Vector3d rotation) {
            var positionedObject = m_Object.SetLocationAndDirectionForDrawing(m_BasePoint, m_BaseDirection,location, rotation);
            if (positionedObject != null) {
                positionedObject.DrawBitmap( component,g);
            }
        }



        public static DiagramCurveEnd DefaultDimentionCurveEnd(double scale,Color color, float lineWieght)
        {
           
                List<Curve> crvs = new List<Curve>();
                crvs.Add(new Line(Point3d.Origin, Vector3d.YAxis, 10* scale).ToNurbsCurve());
              crvs.Add(new Line(Point3d.Origin, Vector3d.XAxis, 5* scale).ToNurbsCurve());
                crvs.Add(new Line(Point3d.Origin, Vector3d.XAxis, -5* scale).ToNurbsCurve());
                crvs.Add(new Line(Point3d.Origin, new Vector3d(1,1,0), 5* scale).ToNurbsCurve());
                crvs.Add(new Line(Point3d.Origin, new Vector3d(1, 1, 0), -5* scale).ToNurbsCurve());

                DiagramCurveCollection crvCollection = DiagramCurveCollection.Create(crvs, color, lineWieght);
                DiagramCurveEnd curveEnd = new DiagramCurveEnd(crvCollection, Point3d.Origin, Vector3d.YAxis, false);
                return curveEnd;
          
        }



    }



}
