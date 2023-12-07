using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    public class DiagramCurveGoo : GH_Goo<DiagramCurve>
    {

        public DiagramCurveGoo()
        {
            this.Value = new DiagramCurve();
        }


        public DiagramCurveGoo(DiagramCurve diagram)
        {
            if (diagram == null)
                diagram = new DiagramCurve();
            this.Value = diagram;
        }


        public override IGH_Goo Duplicate()
        {
            return DuplicateDiagramCurve();
        }

        public DiagramCurveGoo DuplicateDiagramCurve()
        {
            return new DiagramCurveGoo(Value == null ? new DiagramCurve() : Value.Duplicate());
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
                return "Null DiagramCurve";
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("DiagramCurve"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a Diagram Curve"); }
        }









    }
}
