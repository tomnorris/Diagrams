using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    public class DiagramFilledCurveGoo : GH_Goo<DiagramFilledCurve>
    {

        public DiagramFilledCurveGoo()
        {
            this.Value = new DiagramFilledCurve();
        }


        public DiagramFilledCurveGoo(DiagramFilledCurve diagram)
        {
            if (diagram == null)
                diagram = new DiagramFilledCurve();
            this.Value = diagram;
        }


        public override IGH_Goo Duplicate()
        {
            return DuplicateDiagramFilledCurve();
        }

        public DiagramFilledCurveGoo DuplicateDiagramFilledCurve()
        {
            return new DiagramFilledCurveGoo(Value == null ? new DiagramFilledCurve() : Value.Duplicate());
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
                return "Null DiagramFilledCurve";
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("DiagramFilledCurve"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a Diagram Filled Curve"); }
        }









    }
}
