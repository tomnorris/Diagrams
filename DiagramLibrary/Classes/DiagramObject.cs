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
       public  abstract string DiagramObjectType();

        public DiagramObject() { }

       
       

        public Pen GetPen() {
            return new Pen(m_Colour, m_LineWeight);
        }

        public Brush GetBrush()
        {
            return new SolidBrush(m_Colour);
        }



     

        public virtual void DrawBitmap(Graphics g) { }

        public virtual void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance) { }

    }
}
