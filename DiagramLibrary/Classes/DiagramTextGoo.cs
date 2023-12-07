using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    public class DiagramTextGoo : GH_Goo<DiagramText>
    {

        public DiagramTextGoo()
        {
            this.Value = new DiagramText();
        }


        public DiagramTextGoo(DiagramText diagram)
        {
            if (diagram == null)
                diagram = new DiagramText();
            this.Value = diagram;
        }


        public override IGH_Goo Duplicate()
        {
            return DuplicateDiagramText();
        }

        public DiagramTextGoo DuplicateDiagramText()
        {
            return new DiagramTextGoo(Value == null ? new DiagramText() : Value.Duplicate());
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
                return "Null DiagramText";
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("DiagramText"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a Diagram Text"); }
        }









    }
}
