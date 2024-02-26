using Rhino.Display;
using Rhino.Geometry;

using System.Drawing;


namespace DiagramLibrary
{
    public abstract class NoDrawDiagramObject : DiagramObject
    {

        public abstract override  DiagramObject Duplicate();

        //These are required for DiagramObject so we can store this information as a diagram eg CurveEnds which are only valid as when applied to a curve and all the heaviy lift is done by the curve, CurveEnds should never draw themselves or any of the below methods

        public override void DrawBitmap(Grasshopper.Kernel.GH_Component component, Graphics g)
        {
            return; //skip
        }

        public override void DrawRhinoPreview( Grasshopper.Kernel.GH_Component component,DisplayPipeline pipeline, double tolerance, Transform xform, bool colorOverride, Rhino.RhinoDoc doc, bool Bake)
        {
            return; // skip
        }

        public override BoundingBox GetBoundingBox()
        {
            return BoundingBox.Empty;
        }

        public override PointF GetBoundingBoxLocation()
        {
            return PointF.Empty;
        }




    }
}
