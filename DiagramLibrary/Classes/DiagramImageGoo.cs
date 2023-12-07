using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    public class DiagramImageGoo : GH_Goo<DiagramImage>
    {

        public DiagramImageGoo()
        {
            this.Value = new DiagramImage();
        }


        public DiagramImageGoo(DiagramImage diagram)
        {
            if (diagram == null)
                diagram = new DiagramImage();
            this.Value = diagram;
        }


        public override IGH_Goo Duplicate()
        {
            return DuplicateDiagramImage();
        }

        public DiagramImageGoo DuplicateDiagramImage()
        {
            return new DiagramImageGoo(Value == null ? new DiagramImage() : Value.Duplicate());
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
                return "Null DiagramImage";
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("DiagramImage"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a Diagram Image"); }
        }









    }
}
