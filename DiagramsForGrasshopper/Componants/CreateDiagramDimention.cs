using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramDimention : DiagramComponent
    {
        /// <summary>
        /// Initializes a new instance of the DiagramDimention class.
        /// </summary>
        public CreateDiagramDimention()
          : base("DiagramDimention", "DDimention",
              "Description",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point1", "P1", "Point1", GH_ParamAccess.item);
            pManager.AddPointParameter("Point2", "P1", "Point1", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset", "O", "Offset from Object", GH_ParamAccess.item, 10);
            pManager.AddColourParameter("Colour", "Clr", "Colour of the Curve", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddNumberParameter("Weight", "LW", "Line Weigh of the Curve", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("TextScale", "TS", "Size of text", GH_ParamAccess.item, Diagram.DefaultTextScale);
            pManager.AddTextParameter("TextOverride", "TO", "Text to override of the dimention", GH_ParamAccess.item, string.Empty);
            pManager.AddTextParameter("Suffix", "S", "Suffix of the dimention", GH_ParamAccess.item, string.Empty);
            pManager.AddColourParameter("BackgroundColour", "BgClr", "Colour of the Text background", GH_ParamAccess.item, Color.Transparent);
                    pManager.AddTextParameter("Fontname", "F", "Text Font Name", GH_ParamAccess.item, Diagram.DefaultFontName);
            pManager.AddNumberParameter("Padding", "P", "Padding of text", GH_ParamAccess.item, 3);
            pManager.AddIntegerParameter("Round", "R", "The number of decimals the text will round to", GH_ParamAccess.item, 2);
            pManager.AddGenericParameter("CurveEndStart", "CES", "Diagram Object which will be the Curve End for the start of the Curve, only Curve and FilledCurves are supported", GH_ParamAccess.item);
            pManager.AddGenericParameter("CurveEndEnd", "CEE", "Diagram Object which will be the Curve End for the End of the Curve, only Curve and FilledCurves are supported", GH_ParamAccess.item);
            this.Params.Input[12].Optional = true;
            this.Params.Input[13].Optional = true;



        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {

            double weight = 0;
            double offset = 0;
            Color clr = Diagram.DefaultColor;
            Color maskColour = Color.Transparent;
            Point3d p1 = Point3d.Unset;
            Point3d p2 = Point3d.Unset;
            string suffix = string.Empty;
            string overrideText = string.Empty;
            double textSize = Diagram.DefaultTextScale;
            string fontname = Diagram.DefaultFontName;
            double padding = 3;
            int round = 2;

            Grasshopper.Kernel.Types.IGH_Goo CurveStartObj = null;
            Grasshopper.Kernel.Types.IGH_Goo CurveEndObj = null;


            DA.GetData(0, ref p1);
            DA.GetData(1, ref p2);
            DA.GetData(2, ref offset);
            DA.GetData(3, ref clr);
            DA.GetData(4, ref weight);
            DA.GetData(5, ref textSize);
            DA.GetData(6, ref overrideText);
            DA.GetData(7, ref suffix);
            DA.GetData(8, ref maskColour);

            DA.GetData(9, ref fontname);
            DA.GetData(10, ref padding);
            DA.GetData(11, ref round);
        
            DA.GetData(12, ref CurveStartObj);
            DA.GetData(13, ref CurveEndObj);

            if (p1 == Point3d.Unset)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Point1 cannot be Null");
                return null;
            }

            if (p2 == Point3d.Unset)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Point2 cannot be Null");
                return null;
            }







            DiagramDimention diagramDimention = DiagramDimention.Create(p1, p2, clr, (float)weight, suffix, overrideText, maskColour, (float)textSize, fontname, (float)padding, round, DiagramCurveEnd.DefaultDimentionCurveEnd(1,clr,(float)weight), (float)offset);



            try
            {
                CurveStartObj.CastTo(out Diagram CurveEndStartDiagram);

                for (int i = 0; i < CurveEndStartDiagram.Objects.Count; i++)
                {
                    if (CurveEndStartDiagram.Objects[i] is BaseCurveDiagramObject)
                    {


                        diagramDimention.AddCurveEnds(CurveEndStartDiagram.Objects[i] as BaseCurveDiagramObject, new Point3d(0, 0, 0), Plane.WorldXY.YAxis, null, Point3d.Unset, Vector3d.Unset);
                        break;
                    }

                    if (CurveEndStartDiagram.Objects[i] is DiagramCurveEnd)
                    {
                        diagramDimention.AddCurveEnds(CurveEndStartDiagram.Objects[i] as DiagramCurveEnd, null);
                        break;
                    }

                }

            }
            catch (Exception)
            {


            }


            try
            {

                CurveEndObj.CastTo(out Diagram CurveEndEndDiagram);

                for (int i = 0; i < CurveEndEndDiagram.Objects.Count; i++)
                {
                    if (CurveEndEndDiagram.Objects[i] is BaseCurveDiagramObject)
                    {

                        diagramDimention.AddCurveEnds(null, Point3d.Unset, Vector3d.Unset, CurveEndEndDiagram.Objects[i] as BaseCurveDiagramObject, new Point3d(0, 0, 0), Plane.WorldXY.YAxis);
                        break;
                    }

                    if (CurveEndEndDiagram.Objects[i] is DiagramCurveEnd)
                    {
                        diagramDimention.AddCurveEnds(null, CurveEndEndDiagram.Objects[i] as DiagramCurveEnd);
                        break;
                    }

                }
        }
            catch (Exception)
            {


            }








    SizeF size = diagramDimention.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, diagramDimention.GetBoundingBoxLocation());
            diagram.AddDiagramObject(diagramDimention);


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
                return DiagramsForGrasshopper.Properties.Resources.DimIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("890c976e-2a3e-4a9e-9cb6-99624e768edf"); }
        }
    }
}