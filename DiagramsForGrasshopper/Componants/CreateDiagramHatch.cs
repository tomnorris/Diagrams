using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper.Componants
{
    public class CreateDiagramHatch : ReportBaseComponent
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramHatch class.
        /// </summary>
        public CreateDiagramHatch()
          : base("CreateDiagramSolidHatch", "DSHatch",
              "Description",
              "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "Height in Pixels", GH_ParamAccess.item);
            pManager.AddColourParameter("FillColour", "FC", "Height in Pixels", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddBooleanParameter("DrawLines", "D", "Height in Pixels", GH_ParamAccess.item,true);
            pManager.AddColourParameter("LineColour", "LC", "Height in Pixels", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddNumberParameter("Weight", "LW", "Height in Pixels", GH_ParamAccess.item, Diagram.DefaultLineWieght);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            pManager.AddGenericParameter("DiagramHatch", "DH", "Diagram", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double weight = 1;
            Color clr = new Color();
            Color lnClr = new Color();
            Brep brep = null;
            bool drawLines = true;

            DA.GetData(0, ref brep);
            DA.GetData(1, ref clr);
            DA.GetData(2, ref drawLines);
            DA.GetData(3, ref lnClr);
            DA.GetData(4, ref weight);


            if (brep == null) {
                AddUsefulMessage(DA, "Brep cannot be Null");
                return;
            }


            List<DiagramFilledCurve> diagramCurves = DiagramFilledCurve.CreateFromBrep(brep, clr, drawLines, lnClr, (float)weight);

            DA.SetDataList(1, diagramCurves);
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
            get { return new Guid("0d59cdbf-27fd-47c6-b22a-e3f17bea5072"); }
        }
    }
}