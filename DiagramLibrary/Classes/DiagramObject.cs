using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    abstract public class DiagramObject 
    {
         protected Color m_Colour;
      
         protected float m_LineWeight;

    


        public virtual Color Colour { 
            get { return m_Colour; }
            set { m_Colour = value;  }
        }

        public float LineWeight //For text this is text size?!
        {
            get { return m_LineWeight; }
            set { m_LineWeight = value; }
        }

       

        public DiagramObject() { }

   
       

        public Pen GetPen() {
            return new Pen(m_Colour, m_LineWeight);
        }


        public virtual PointF GetLocation()
        {
            BoundingBox bbox = this.GetBoundingBox();
            return new PointF((float)(bbox.Min.X), (float)(bbox.Min.Y));
        }


        public SizeF GetTotalSize() {
            BoundingBox bbox = this.GetBoundingBox();
            return new SizeF((float)(bbox.Max.X - bbox.Min.X), (float)(bbox.Max.Y - bbox.Min.Y));
        }

        public abstract BoundingBox GetBoundingBox();

        public abstract void DrawBitmap(Grasshopper.Kernel.GH_Component component,Graphics g);

        public abstract void DrawRhinoPreview(Grasshopper.Kernel.GH_Component component,Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, bool colorOverride);

        public abstract DiagramObject Duplicate();

    }
}
