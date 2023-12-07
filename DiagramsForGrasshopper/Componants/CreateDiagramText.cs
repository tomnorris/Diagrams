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
            pManager.AddColourParameter("Colour", "C", "Colour for text", GH_ParamAccess.item, Color.Black);
            pManager.AddNumberParameter("Weight", "LW", "Text size", GH_ParamAccess.item,1);
            pManager.AddTextParameter("Font", "F", "Font family name", GH_ParamAccess.item, "Arial");
            pManager.AddIntegerParameter("Justification", "J", 
                "Text justification 0: Bottom Left, 1: Bottom Center, 2: Bottom Right \n 3: Middle Left, 4: Middle Center, 5: Middle Right \n 6: Top Left, 7: Top Center, 8: Top Right", 
                GH_ParamAccess.item, 0);

            

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
            Color clr = new Color();
            string text = "";
            Point3d pt = Point3d.Unset;
            string font = "Arial";
            int justificationInt = 0;


            DA.GetData(0, ref text);
            DA.GetData(1, ref pt);
           
            DA.GetData(2, ref clr);
            DA.GetData(3, ref weight);

            DA.GetData(4, ref font);
            DA.GetData(5, ref justificationInt);





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

            TextJustification justification = TextJustification.BottomLeft;
        
            switch (justificationInt)
            {
                case 0:
                    justification = TextJustification.BottomLeft;
                    break;
                case 1:
                    justification = TextJustification.BottomCenter;
                    break;
                case 2:
                    justification = TextJustification.BottomRight;
                    break;
                case 3:
                    justification = TextJustification.MiddleLeft;
                    break;
                case 4:
                    justification = TextJustification.MiddleCenter;
                    break;
                case 5:
                    justification = TextJustification.MiddleRight;
                    break;
                case 6:
                    justification = TextJustification.TopLeft;
                    break;
                case 7:
                    justification = TextJustification.TopCenter;
                    break;
                case 8:
                    justification = TextJustification.TopRight;
                    break;
                default:
                    // Use default values
                    justification = TextJustification.BottomLeft;
                    break;
            }

            


            

            DiagramText diagramCurve = DiagramText.Create(text,new PointF((float)pt.X,(float)pt.Y), clr, (float)weight,justification,Color.Blue,true,Color.LightCyan,1f,true,"Arial",true,new SizeF(100F,100F),3f);

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