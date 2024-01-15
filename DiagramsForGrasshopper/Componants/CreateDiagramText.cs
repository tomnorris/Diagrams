using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper.Componants
{
    public class CreateDiagramText : ReportBaseComponent
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramText class.
        /// </summary>
        public CreateDiagramText()
          : base("CreateDiagramText", "DText",
              "Create Text for a Diagram",
               "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "T", "Text as a string", GH_ParamAccess.item);
            pManager.AddPointParameter("Location", "L", "Location for text", GH_ParamAccess.item,new Point3d(0,0,0));
         
            pManager.AddNumberParameter("Scale", "S", "Text size", GH_ParamAccess.item,1);
                    
            pManager.AddNumberParameter("Max Width", "W", "Maximum Width, Set to less than 0 to ignore ", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Max Height", "H", "Maximum Height, Set to less than 0 to ignore", GH_ParamAccess.item, -1);
             pManager.AddColourParameter("Colour", "C", "Colour for text", GH_ParamAccess.item, Color.Black);
         
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
            double weight = 1;
            Color clr = Color.Empty;
            Color frameClr = Color.Empty;
            Color maskClr = Color.Empty;
            string text = "";
            Point3d pt = new Point3d(0, 0, 0);
            string font = "Arial";
         
            double padding = 3;
            double width = -1;
            double height = -1;
           


            DA.GetData(0, ref text);
            DA.GetData(1, ref pt);
            DA.GetData(2, ref weight);
             DA.GetData(3, ref width);
            DA.GetData(4, ref height);
             DA.GetData(5, ref clr);
          

            if (text == "")
            {
                AddUsefulMessage(DA, "Text cannot be Empty");
                return;
            }

            if (weight == double.NaN)
            {
                AddUsefulMessage(DA, "Either set a valid [Width, Height and Location] or [Rectangle], width cannot be NaN");
                return;
            }
            if (weight < 0)
            {
                AddUsefulMessage(DA, "Either set a valid [Width, Height and Location] or [Rectangle], width cannot be negative");
                return;
            }

            TextJustification anchor = TextJustification.BottomLeft;

          

            TextJustification jusitification = TextJustification.BottomLeft;

           


            bool showMask = false;
           


            DiagramText diagramCurve = DiagramText.Create(text,new PointF((float)pt.X,(float)pt.Y), clr, (float)weight,anchor,maskClr, frameClr,1f,showMask,font,new SizeF((float)width,(float)height),(float)padding, jusitification);

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
            get { return new Guid("1c44a5e7-7fdd-41e0-8d7d-4b4c27b77a12"); }
        }
    }
}