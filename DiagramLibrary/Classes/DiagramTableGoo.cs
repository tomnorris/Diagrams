using Grasshopper.Kernel.Types;


namespace DiagramLibrary
{
    public class DiagramTableGoo : GH_Goo<DiagramTable>
    {

        public DiagramTableGoo()
        {
            this.Value = new DiagramTable();
        }


        public DiagramTableGoo(DiagramTable diagram)
        {
            if (diagram == null)
                diagram = new DiagramTable();
            this.Value = diagram;
        }


        public override IGH_Goo Duplicate()
        {
            return DuplicateDiagramTable();
        }

        public DiagramTableGoo DuplicateDiagramTable()
        {
            return new DiagramTableGoo(Value == null ? new DiagramTable() : Value.Duplicate());
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
                return "Null DiagramTable";
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("DiagramTable"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a Diagram Table"); }
        }









    }
}
