using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using DiagramLibrary;

namespace DiagramsForGrasshopper
{
    public class RhinoDiagram : ReportBaseComponent
{
       

        public Diagram Diagram = null;
        private Transform Xform = Transform.ZeroTransformation;
        /// <summary>
        /// Initializes a new instance of the RhinoDiagram class.
        /// </summary>
        public RhinoDiagram()
          : base("RhinoDiagram", "RhinoDiagram",
              "Description",
                "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
           
            pManager.AddGenericParameter("Diagram", "D", "Image or Diagram to display inside this componant", GH_ParamAccess.item);
            pManager.AddTransformParameter("Transform", "Xfrom", "A transformation for the diagram", GH_ParamAccess.item);
            this.Params.Input[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            pManager.AddGeometryParameter("G", "G", "Geo", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
           
            Diagram diagram = null;
            Transform xfrom = Transform.ZeroTransformation;



            if (!DA.GetData(0, ref diagram))
            {
                return;
            }

            DA.GetData(1, ref xfrom);



            if (diagram == null)
            {
                AddUsefulMessage(DA, "Diagram cannot be null");
                return;
            }
            Xform = xfrom;
            Diagram = diagram;


           // DA.SetDataList(1, diagram.GetRhinoDiagram(GH_Component.DocumentTolerance()));
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1754a08f-2742-4038-807c-8d93f7bfe4a0"); }
        }

       

        public override BoundingBox ClippingBox
        { 

            get { if (Diagram != null) { return Diagram.GetGeometryBoundingBox(); } else { return BoundingBox.Empty;  } }
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            if (Diagram == null) { return; }

            if (!this.Locked)
            {
                if (this.m_attributes.Selected) {
                    Diagram.DrawRhinoPreview(args.Display, GH_Component.DocumentTolerance(), Xform, true);
                } else {
                    Diagram.DrawRhinoPreview(args.Display, GH_Component.DocumentTolerance(), Xform, false);
                }

               
            }
        }
        
    }
}