using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
   public abstract class  DiagramCachedClass : DiagramObject
    {
        protected List<DiagramObject> m_ObjectCache = new List<DiagramObject>();

        public override Color Colour
        {
            get { return m_Colour; }
            set { m_Colour = value; UpdateCache(); }
        }

        public override float LineWeight
        {
            get { return m_LineWeight; }
            set { m_LineWeight = value; UpdateCache(); }
        }

        public virtual void UpdateCache()
        {
            this.m_ObjectCache = GenerateObjects();
        }

        public abstract List<DiagramObject> GenerateObjects();


        public override void DrawBitmap(GH_Component component, Graphics g)
        {

            for (int i = 0; i < this.m_ObjectCache.Count; i++)
            {
                this.m_ObjectCache[i].DrawBitmap(component, g);
            }
        }

        public override void DrawRhinoPreview(GH_Component component, Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, bool colorOverride, RhinoDoc doc, bool Bake)
        {
            
            for (int i = 0; i < this.m_ObjectCache.Count; i++)
            {
                this.m_ObjectCache[i].DrawRhinoPreview(component, pipeline, tolerance, xform, colorOverride, doc, Bake);
            }
                       
        }


        public override BoundingBox GetBoundingBox()
        {
            BoundingBox bbox = BoundingBox.Empty;

            for (int i = 0; i < m_ObjectCache.Count; i++)
            {
                bbox.Union(m_ObjectCache[i].GetBoundingBox());
            }
            return bbox;
        }

    }
}
