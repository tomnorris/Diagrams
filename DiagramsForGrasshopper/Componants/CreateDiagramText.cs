using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper.Componants
{
    public class CreateDiagramText : DiagramComponent
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
         //   this.Params.Input[0].ObjectChanged += CreateDiagramText_ObjectChanged;

            pManager.AddPointParameter("Location", "L", "Location for text", GH_ParamAccess.item, Point3d.Origin);
            pManager.HideParameter(1);
            pManager.AddNumberParameter("TextScale", "TS", "Text size", GH_ParamAccess.item, Diagram.DefaultTextScale);
                    
            pManager.AddNumberParameter("Max Width", "W", "Maximum Width, Set to less than 0 to ignore ", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Max Height", "H", "Maximum Height, Set to less than 0 to ignore", GH_ParamAccess.item, -1);
             pManager.AddColourParameter("Colour", "Clr", "Colour for text", GH_ParamAccess.item, Diagram.DefaultColor);
         
        }

      
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            double textScale = Diagram.DefaultTextScale;
            Color clr = Diagram.DefaultColor;
            Color frameClr = Diagram.DefaultColor;
            Color maskClr = Color.Transparent;
            string text = "";
            Point3d pt = new Point3d(0, 0, 0);
            string font = "Arial";
         
            double padding = 3;
            double width = -1;
            double height = -1;
           


            DA.GetData(0, ref text);
            DA.GetData(1, ref pt);
            DA.GetData(2, ref textScale);
             DA.GetData(3, ref width);
            DA.GetData(4, ref height);
             DA.GetData(5, ref clr);
          

            if (text == "")
            {
                AddUsefulMessage(DA, "Text cannot be Empty");
                return null;
            }

         

            TextJustification anchor = TextJustification.BottomLeft;

          

            TextJustification jusitification = TextJustification.BottomLeft;

           


          

            PointF location = new PointF((float)pt.X, (float)pt.Y);

            DiagramText diagramText = DiagramText.Create(text, location, clr, (float)textScale,anchor,maskClr, frameClr,-1f,font,new SizeF((float)width,(float)height),(float)padding, jusitification);
                                  
            SizeF size = diagramText.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent,0,Color.Transparent, diagramText.GetAnchorCompensatedPoint(size));
            diagram.AddDiagramObject(diagramText);
            return diagram;

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