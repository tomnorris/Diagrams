using Grasshopper.Kernel;
using System;
using System.Collections.Generic;


namespace DiagramsForGrasshopper
{
    public abstract class ReportBaseComponent : GH_Component
    {


        public ReportBaseComponent(string a, string b, string c, string d, string e)
           : base(a, b, c, d, e) // pass through the GH_Comp
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Report", "R", "A report from this componant", GH_ParamAccess.list);

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.ClearRuntimeMessages();
            DiagramCompnentSolveInstance(DA);

            DA.SetData(0, this.RuntimeMessages(GH_RuntimeMessageLevel.Blank | GH_RuntimeMessageLevel.Error | GH_RuntimeMessageLevel.Remark | GH_RuntimeMessageLevel.Warning));
        }

        public virtual  void DiagramCompnentSolveInstance(IGH_DataAccess DA) { }

        
    }
}
