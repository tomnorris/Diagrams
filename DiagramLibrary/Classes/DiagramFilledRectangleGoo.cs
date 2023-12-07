using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    public class DiagramFilledRectangleGoo : GH_Goo<DiagramFilledRectangle>
    {
        public DiagramFilledRectangleGoo()
        {
            this.Value = new DiagramFilledRectangle();
        }

        public DiagramFilledRectangleGoo(DiagramFilledRectangle rectangle)
        {
            if (rectangle == null)
                rectangle = new DiagramFilledRectangle();
            this.Value = rectangle;
        }

        public override IGH_Goo Duplicate()
        {
            return DuplicateDiagramFilledRectangle();
        }

        public DiagramFilledRectangleGoo DuplicateDiagramFilledRectangle()
        {
            return new DiagramFilledRectangleGoo(Value == null ? new DiagramFilledRectangle() : Value.Duplicate());
        }

        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return true;
            }
        }

        public override string ToString()
        {
            if (Value == null)
                return "Null DiagramFilledRectangle";
            else
                return Value.ToString();
        }

        public override string TypeName
        {
            get { return ("DiagramFilledRectangle"); }
        }

        public override string TypeDescription
        {
            get { return ("Defines a Diagram Filled Rectangle"); }
        }
    }
}
