using Grasshopper.Kernel;
using System;
using System.Collections.Generic;


namespace DiagramsForGrasshopper
{
    public abstract class ReportBaseComponent : GH_Component
    {
       public ReportBaseComponent(string a, string b, string c, string d, string e)
          : base(a,b,c,d,e) // pass through the GH_Comp
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Report", "R", "A report from this componant", GH_ParamAccess.list);
        
        }




        public void AddUsefulMessage(IGH_DataAccess DA, string message, GH_RuntimeMessageLevel level = GH_RuntimeMessageLevel.Error)
        {
            try
            {

                DA.SetDataList(0, new List<string> { message });
            }
            catch (Exception ex)
            {
                // not all component have the report 
            }

            this.AddRuntimeMessage(level, message);
        }
    }
}
