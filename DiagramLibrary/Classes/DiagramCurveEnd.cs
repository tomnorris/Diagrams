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
   public class DiagramCurveEnd :DiagramObject
    {



        BaseCurveDiagramObject m_Object = null;

        private Point3d m_BasePoint = Point3d.Unset;
        private Vector3d m_BaseDirection = Vector3d.Unset;
        private bool m_Flipped = false;

        public DiagramCurveEnd Dupliacte() {

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

    

        public void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform transform, bool colorOverride,Point3d location, Vector3d rotation)

        {

            Vector3d flipCorrectedDirection = m_BaseDirection;
            if (m_Flipped) {
                flipCorrectedDirection.Reverse();
            }

            var positionedObject = m_Object.SetLocationAndDirectionForDrawing(m_BasePoint, flipCorrectedDirection, location, rotation);
            if (positionedObject != null)
            {
                positionedObject.DrawRhinoPreview(pipeline,  tolerance,  transform,  colorOverride);
            }
        }


        public void DrawBitmap(Graphics g, Point3d location, Vector3d rotation) {
            var positionedObject = m_Object.SetLocationAndDirectionForDrawing(m_BasePoint, m_BaseDirection,location, rotation);
            if (positionedObject != null) {
                positionedObject.DrawBitmap(g);
            }
        }





        //These are required for DiagramObject so we can store this information as a diagram, however CurveEnds are only valid as when applied to a curve and all the heaviy lift is done by the curve, CurveEnds should never draw themselves or any of the below methods

        public override void DrawBitmap(Graphics g)
        {
            return; //skip
        }

        public override void DrawRhinoPreview(DisplayPipeline pipeline, double tolerance, Transform xform, bool colorOverride)
        {
            return; // skip
        }

        public override BoundingBox GetBoundingBox()
        {
            return BoundingBox.Empty;
        }

        public override PointF GetLocation()
        {
            return PointF.Empty;
        }




    }



}
