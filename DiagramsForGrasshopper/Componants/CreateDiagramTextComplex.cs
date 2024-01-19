using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper.Componants
{
    public class CreateDiagramTextComplex : ReportBaseComponent
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramText class.
        /// </summary>
        public CreateDiagramTextComplex()
          : base("CreateDiagramTextComplex", "DTextComplex",
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
            pManager.AddPointParameter("Location", "L", "Location for text", GH_ParamAccess.item,Point3d.Unset);
            pManager.HideParameter(1);
            pManager.AddIntegerParameter("Anchor", "A",
                 "Text Anchor 0: Bottom Left, 1: Bottom Center, 2: Bottom Right \n 3: Middle Left, 4: Middle Center, 5: Middle Right \n 6: Top Left, 7: Top Center, 8: Top Right",
                 GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Scale", "S", "Text size", GH_ParamAccess.item,1);
            pManager.AddTextParameter("Font", "F", "Font family name", GH_ParamAccess.item, "Arial");
                      
            pManager.AddNumberParameter("Max Width", "W", "Maximum Width, Set to less than 0 to ignore ", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Max Height", "H", "Maximum Height, Set to less than 0 to ignore", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Padding", "P", "Text Padding", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("Jusitification", "J", "Text justification. Horizontals(Left, Center, Right) only take effect if Width is set, Verticals (Top, Middle, Bottom) only take effect if Height it set. 0: Bottom Left, 1: Bottom Center, 2: Bottom Right \n 3: Middle Left, 4: Middle Center, 5: Middle Right \n 6: Top Left, 7: Top Center, 8: Top Right", GH_ParamAccess.item, 0);
            pManager.AddColourParameter("Colour", "C", "Colour for text", GH_ParamAccess.item, Color.Black);
            pManager.AddColourParameter("FrameColor", "FC", "Colour for text", GH_ParamAccess.item, Color.Empty);
            pManager.AddColourParameter("MaskColor", "MC", "Colour for text", GH_ParamAccess.item, Color.Empty);
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
            int anchorInt = 0;
            double padding = 0;
            double width = -1;
            double height = -1;
            int jusitificationInt = 0;


            DA.GetData(0, ref text);
            DA.GetData(1, ref pt);
            DA.GetData(2, ref anchorInt);


            DA.GetData(3, ref weight);
            DA.GetData(4, ref font);


            DA.GetData(5, ref width);
            DA.GetData(6, ref height);
            DA.GetData(7, ref padding);

            DA.GetData(8, ref jusitificationInt);
            DA.GetData(9, ref clr);
            DA.GetData(10, ref frameClr);
            DA.GetData(11, ref maskClr);

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

            switch (anchorInt)
            {
                case 0:
                    anchor = TextJustification.BottomLeft;
                    break;
                case 1:
                    anchor = TextJustification.BottomCenter;
                    break;
                case 2:
                    anchor = TextJustification.BottomRight;
                    break;
                case 3:
                    anchor = TextJustification.MiddleLeft;
                    break;
                case 4:
                    anchor = TextJustification.MiddleCenter;
                    break;
                case 5:
                    anchor = TextJustification.MiddleRight;
                    break;
                case 6:
                    anchor = TextJustification.TopLeft;
                    break;
                case 7:
                    anchor = TextJustification.TopCenter;
                    break;
                case 8:
                    anchor = TextJustification.TopRight;
                    break;
                default:
                    // Use default values
                    anchor = TextJustification.BottomLeft;
                    break;
            }

            TextJustification jusitification = TextJustification.BottomLeft;

            switch (jusitificationInt)
            {
                case 0:
                    jusitification = TextJustification.BottomLeft;
                    break;
                case 1:
                    jusitification = TextJustification.BottomCenter;
                    break;
                case 2:
                    jusitification = TextJustification.BottomRight;
                    break;
                case 3:
                    jusitification = TextJustification.MiddleLeft;
                    break;
                case 4:
                    jusitification = TextJustification.MiddleCenter;
                    break;
                case 5:
                    jusitification = TextJustification.MiddleRight;
                    break;
                case 6:
                    jusitification = TextJustification.TopLeft;
                    break;
                case 7:
                    jusitification = TextJustification.TopCenter;
                    break;
                case 8:
                    jusitification = TextJustification.TopRight;
                    break;
                default:
                    // Use default values
                    jusitification = TextJustification.BottomLeft;
                    break;
            }


            bool showMask = false;
           

            if (maskClr != Color.Empty) {
                showMask = true;
            }

            if (frameClr != Color.Empty) {
                showMask = true;
            }

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
            get { return new Guid("5d84478c-324e-4368-83e6-8b75f2abc1e9"); }
        }
    }
}