using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramLabel : DiagramComponent
    {
        /// <summary>
        /// Initializes a new instance of the DiagramLabel class.
        /// </summary>
        /// <summary>
        /// Initializes a new instance of the CreateDiagramLabel class.
        /// </summary>
        public CreateDiagramLabel()
          : base("CreateDiagramLabel", "DLabel",
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
            pManager.AddNumberParameter("Offset", "O", "Offset from Label Point", GH_ParamAccess.item, 10);
            pManager.AddVectorParameter("Direction", "D", "Direction for label offset", GH_ParamAccess.item, new Vector3d(1, 1, 0));
            pManager.HideParameter(3);
            pManager.AddNumberParameter("LineWeight", "LW", "Line Weight of the Label", GH_ParamAccess.item, Diagram.DefaultLineWeight);

            pManager.AddNumberParameter("LabelScale", "LS", "Label size", GH_ParamAccess.item, Diagram.DefaultTextScale);

            pManager.AddTextParameter("Font", "F", "Font family name", GH_ParamAccess.item, Diagram.DefaultFontName);

            pManager.AddNumberParameter("Padding", "P", "Text Padding", GH_ParamAccess.item, 0);
            pManager.AddColourParameter("Colour", "Clr", "Colour for text", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddColourParameter("FrameColor", "FClr", "Colour for text", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddColourParameter("BackgroundColor", "BgClr", "BackgroundColour for text", GH_ParamAccess.item, Color.Transparent);
            pManager.AddNumberParameter("FrameLineWeight", "FLW", "Line Weight of the Frame", GH_ParamAccess.item, 0);
            pManager.AddGenericParameter("CurveEndStart", "CES", "Diagram Object which will be the Curve End for the start of the Curve, only Curve and FilledCurves are supported", GH_ParamAccess.item);
                     this.Params.Input[12].Optional = true;

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
            double frameLineWieght = 0;
            double lineWieght = Diagram.DefaultLineWeight;
            double offset = 10;
            Vector3d direction = new Vector3d(1, 1, 0);

            Grasshopper.Kernel.Types.IGH_Goo CurveStartObj = null;

            DA.GetData(0, ref text);
            DA.GetData(1, ref pt);
            DA.GetData(2, ref offset);
            DA.GetData(3, ref direction);
            DA.GetData(4, ref lineWieght);
                        DA.GetData(5, ref labelScale);
            DA.GetData(6, ref font);
            DA.GetData(7, ref padding);
            DA.GetData(8, ref clr);
            DA.GetData(9, ref frameClr);
            DA.GetData(10, ref maskClr);
            DA.GetData(11, ref frameLineWieght);
            DA.GetData(12, ref CurveStartObj);


            if (text == "")
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Text cannot be Empty");
                return null;
            }





            PointF location = Diagram.ConvertPoint(pt);
          
            DiagramCurveEnd crvEnd = null;


            try
            {
                CurveStartObj.CastTo(out Diagram CurveEndStartDiagram);

                for (int i = 0; i < CurveEndStartDiagram.Objects.Count; i++)
                {
                    if (CurveEndStartDiagram.Objects[i] is BaseCurveDiagramObject)
                    {

                        crvEnd = new DiagramCurveEnd(CurveEndStartDiagram.Objects[i] as BaseCurveDiagramObject, new Point3d(0, 0, 0), Plane.WorldXY.YAxis, false);

                   
                        break;
                    }

                    if (CurveEndStartDiagram.Objects[i] is DiagramCurveEnd)
                    {
                        crvEnd = CurveEndStartDiagram.Objects[i] as DiagramCurveEnd;
                        break;
                    }

                }

            }
            catch (Exception)
            {


            }

            DiagramLabel diagramLabel = DiagramLabel.Create(text, location, (float)offset, direction, clr, (float)lineWieght, (float)labelScale, maskClr, frameClr, (float)frameLineWieght, font, (float)padding, crvEnd);


            SizeF size = diagramLabel.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, location);
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
                return DiagramsForGrasshopper.Properties.Resources.LabelIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5bfac095-5802-43ce-a5c2-8b0e9afef8c1"); }
        }
    }
}