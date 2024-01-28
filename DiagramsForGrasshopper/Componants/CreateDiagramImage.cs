using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper.Componants
{
    public class CreateDiagramImage : ReportBaseComponent
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramImage class.
        /// </summary>
        public CreateDiagramImage()
          : base("CreateDiagramImage", "Nickname",
              "Description",
              "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Image", "C", "Height in Pixels", GH_ParamAccess.item);
            pManager.AddPointParameter("Location", "L", "Height in Pixels", GH_ParamAccess.item);
            pManager.AddNumberParameter("Width", "W", "Width in Pixels", GH_ParamAccess.item,-1);
            pManager.AddNumberParameter("Height", "H", "Height in Pixels", GH_ParamAccess.item, -1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            pManager.AddGenericParameter("DiagramText", "DT", "Diagram", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "";
            Point3d pt = Point3d.Unset;
              double width = -1;
            double height = -1;

            DA.GetData(0, ref path);
            DA.GetData(1, ref pt);
            DA.GetData(2, ref width);
            DA.GetData(2, ref height);




            Image btm = Bitmap.FromFile(path);

            if (width < 0) {
                width = btm.Width;
            }

            if (height < 0) {
                height = btm.Height;
            }


            DiagramImage diagramCurve = DiagramImage.Create(path, new PointF((float)pt.X, (float)pt.Y), new SizeF((float)width, (float)height));

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
            get { return new Guid("5b4628dd-e32b-461b-aeb4-8da00c6746a8"); }
        }
    }
}