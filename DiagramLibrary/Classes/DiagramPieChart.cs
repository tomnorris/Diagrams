using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino;
using Rhino.Display;
using Rhino.Geometry;

namespace DiagramLibrary
{
    class DiagramPieChart : DiagramObject
    {
        public override void DrawBitmap(GH_Component component, Graphics g)
        {
            throw new NotImplementedException();
        }

        public override void DrawRhinoPreview(GH_Component component, DisplayPipeline pipeline, double tolerance, Transform xform, bool colorOverride, RhinoDoc doc, bool Bake)
        {
            throw new NotImplementedException();
        }

        public override DiagramObject Duplicate()
        {
            throw new NotImplementedException();
        }

        public override BoundingBox GetBoundingBox()
        {
            throw new NotImplementedException();
        }
    }
}
