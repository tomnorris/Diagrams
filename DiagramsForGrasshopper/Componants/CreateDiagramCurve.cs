using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper.Componants
{
    public class CreateDiagramCurve : ReportBaseComponent
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramCurve class.
        /// </summary>
        public CreateDiagramCurve()
          : base("CreateDiagramCurve", "DCruve",
              "Description",
              "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Height in Pixels", GH_ParamAccess.item);
            pManager.AddColourParameter("Colour", "LC", "Height in Pixels", GH_ParamAccess.item);
            pManager.AddNumberParameter("Weight", "LW", "Height in Pixels", GH_ParamAccess.item);



        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            pManager.AddGenericParameter("DiagramCurve", "DC", "Diagram", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            double weight = 1;
            Color clr = new Color();
            Curve crv = null;

            DA.GetData(0, ref crv);
            DA.GetData(1, ref clr);
           DA.GetData(2, ref weight);


            if (crv == null)
            {
                AddUsefulMessage(DA, "Curve cannot be Null");
                return;
            }

                                              

            DiagramCurve diagramCurve = DiagramCurve.Create(crv,clr, (float)weight);

            DA.SetData(1, diagramCurve);


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
            get { return new Guid("eeb4bbf4-584b-4d37-8896-5d61b8cdd82c"); }
        }
    }
}