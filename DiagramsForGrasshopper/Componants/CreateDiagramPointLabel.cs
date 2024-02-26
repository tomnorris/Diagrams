using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramPointLabel : DiagramComponent
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramLabel class.
        /// </summary>
        public CreateDiagramPointLabel()
          : base("CreateDiagramPointLabel", "DPointLabel",
              "Description",
            "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "T", "Text as a string", GH_ParamAccess.item);
                      pManager.AddPointParameter("LabelPoint", "L", "The point on the target for the label", GH_ParamAccess.item, Point3d.Origin);
            pManager.HideParameter(1);
                  pManager.AddNumberParameter("LabelScale", "LS", "Label size", GH_ParamAccess.item, Diagram.DefaultTextScale);
            pManager.AddTextParameter("Font", "F", "Font family name", GH_ParamAccess.item, Diagram.DefaultFontName);
                   
            pManager.AddNumberParameter("Padding", "P", "Text Padding", GH_ParamAccess.item, 0);
            pManager.AddColourParameter("Colour", "Clr", "Colour for text", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddColourParameter("FrameColor", "FClr", "Colour for text", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddColourParameter("BackgroundColor", "BgClr", "BackgroundColour for text", GH_ParamAccess.item, Color.Transparent);
            pManager.AddNumberParameter("FrameLineWeight", "FLW", "Line Weight of the Frame", GH_ParamAccess.item, Diagram.DefaultLineWeight);
        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            double labelScale = Diagram.DefaultTextScale;
            Color clr = Diagram.DefaultColor;
            Color frameClr = Diagram.DefaultColor;
            Color maskClr = Color.Transparent;
            string text = "";
            Point3d pt = new Point3d(0, 0, 0);
            string font = Diagram.DefaultFontName;
          
            double padding = 0;
            double frameLineWieght = Diagram.DefaultLineWeight;


            DA.GetData(0, ref text);
            DA.GetData(1, ref pt);
            DA.GetData(2, ref labelScale);
            DA.GetData(3, ref font);
            DA.GetData(4, ref padding);
            DA.GetData(5, ref clr);
            DA.GetData(6, ref frameClr);
            DA.GetData(7, ref maskClr);
            DA.GetData(8, ref frameLineWieght);

            if (text == "")
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Text cannot be Empty");
                return null;
            }





            PointF location = Diagram.ConvertPoint(pt);

            DiagramPointLabel diagramLabel = DiagramPointLabel.Create(text, location, clr, (float)labelScale, maskClr, frameClr, (float)frameLineWieght, font,  (float)padding);

            SizeF size = diagramLabel.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, diagramLabel.DiagramText.GetAnchorCompensatedPoint(size));
            diagram.AddDiagramObject(diagramLabel);
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
            get { return new Guid("bae04aa7-f9d5-4a55-8871-aa5954130fa2"); }
        }
    }
}