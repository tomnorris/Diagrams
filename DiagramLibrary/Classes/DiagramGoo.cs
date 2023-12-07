using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    public class DiagramGoo : GH_Goo<Diagram>
    {

        public DiagramGoo()
        {
            this.Value = new Diagram();
        }


        public DiagramGoo(Diagram diagram)
        {
            if (diagram == null)
                diagram = new Diagram();
            this.Value = diagram;
        }


        public override IGH_Goo Duplicate()
        {
            return DuplicateDiagram();
        }

        public DiagramGoo DuplicateDiagram()
        {
            return new DiagramGoo(Value == null ? new Diagram() : Value.Duplicate());
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
                return "Null Diagram";
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("Diagram"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a Diagram"); }
        }









    }
}
