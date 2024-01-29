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

    


        public Color Colour { 
            get { return m_Colour; }
        }

        public float LineWeight
        {
            get { return m_LineWeight; }
        }

        public  abstract string DiagramObjectType();

        public DiagramObject() { }

       
       

        public Pen GetPen() {
            return new Pen(m_Colour, m_LineWeight);
        }


        public PointF GetLocation()
        {
            BoundingBox bbox = this.GetBoundingBox();
            return new PointF((float)(bbox.Min.X), (float)(bbox.Min.Y));
        }


        public SizeF GetTotalSize() {
            BoundingBox bbox = this.GetBoundingBox();
            return new SizeF((float)(bbox.Max.X - bbox.Min.X), (float)(bbox.Max.Y - bbox.Min.Y));
        }

        public virtual BoundingBox GetBoundingBox() {throw new NotImplementedException();}
        
        public virtual void DrawBitmap(Graphics g) { throw new NotImplementedException(); }

        public virtual void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance,Transform xform,bool colorOverride) { throw new NotImplementedException(); }

    }
}
