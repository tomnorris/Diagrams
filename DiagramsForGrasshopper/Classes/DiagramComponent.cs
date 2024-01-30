using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;


namespace DiagramsForGrasshopper
{
    public abstract class DiagramComponent : ReportBaseComponent

    {


        protected  Transform p_Xform = Transform.ZeroTransformation;
      protected  DiagramLibrary.Diagram p_Diagram = null;

        public DiagramComponent(string a, string b, string c, string d, string e)
           : base(a, b, c, d, e) // pass through the GH_Comp
        {
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            pManager.AddGenericParameter("Diagram Objects", "DObjs", "The Diagram Objects Created by this componant", GH_ParamAccess.list);

        }

       

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            p_Diagram = null;
            p_Xform = Transform.ZeroTransformation;


            p_Diagram = DiagramSolveInstance(DA);



            DA.SetData(1, p_Diagram);
        }

        public virtual DiagramLibrary.Diagram DiagramSolveInstance(IGH_DataAccess DA) { return null; }

        

        public override BoundingBox ClippingBox
        {

            get
            {
              
                if (p_Diagram != null) { return p_Diagram.GetGeometryBoundingBox(); } else { return BoundingBox.Empty; }
            }
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
          
            if (p_Diagram == null) { return; }

            if (!this.Locked)
            {

                p_Diagram.DrawRhinoPreview(args.Display, GH_Component.DocumentTolerance(), p_Xform, this.m_attributes.Selected);



            }
        }


    }

}
