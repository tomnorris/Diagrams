using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;




namespace DiagramsForGrasshopper
{
    public class CreateDiagramPieChart : DiagramComponent
    {
        /// <summary>
        /// Initializes a new instance of the CreatePieChart class.
        /// </summary>
        public CreateDiagramPieChart()
          : base("CreateDiagramPieChart", "CreateDPie","Description",
                "Display", "Diagram")
        {
        }
        
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Data", "D", "A list of data to repsent in a pie chart", GH_ParamAccess.list);
            pManager.AddTextParameter("DataLabels", "DA", "An optional list of Text labels for each of the data items", GH_ParamAccess.list,new List<string> { string.Empty});
            var clr1 = DiagramColour.GetColors(1);
            var clr2 = DiagramColour.GetColors(1);
            pManager.AddColourParameter("DataColour", "DC", "An optional list of Colours for each of the data items", GH_ParamAccess.list, clr1);
            pManager.AddColourParameter("DataLineColour", "DLC", "An optional list of Colours for each of the data items", GH_ParamAccess.list, clr2);
            pManager.AddPointParameter("Location", "L", "The point on the target for the label", GH_ParamAccess.item, Point3d.Origin);
            pManager.HideParameter(1);
            pManager.AddNumberParameter("OuterRadius", "OR", "Offset from Label Point", GH_ParamAccess.item, 100);
            pManager.AddNumberParameter("InnerRadius", "IR", "Offset from Label Point", GH_ParamAccess.item, 30);
           pManager.AddNumberParameter("LabelRadius", "LR", "Offset from Label Point", GH_ParamAccess.item, 130);


            pManager.AddNumberParameter("LineWeight", "LW", "Line Weight of the Label", GH_ParamAccess.item, Diagram.DefaultLineWeight);
            pManager.AddNumberParameter("LabelScale", "LS", "Label size", GH_ParamAccess.item, Diagram.DefaultTextScale);
                        pManager.AddTextParameter("Font", "F", "Font family name", GH_ParamAccess.item, Diagram.DefaultFontName);
                        pManager.AddNumberParameter("Padding", "P", "Text Padding", GH_ParamAccess.item, 0);
            pManager.AddColourParameter("TextColour", "TClr", "Colour for text", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddColourParameter("FrameColor", "FClr", "Colour for text", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddColourParameter("BackgroundColor", "BgClr", "BackgroundColour for text", GH_ParamAccess.item, Color.Transparent);
            pManager.AddNumberParameter("FrameLineWeight", "FLW", "Line Weight of the Frame", GH_ParamAccess.item, 0);
            pManager.AddGenericParameter("CurveEndStart", "CES", "Diagram Object which will be the Curve End for the start of the Curve, only Curve and FilledCurves are supported", GH_ParamAccess.item);
            this.Params.Input[16].Optional = true;

        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            //TODO write this function so it works!

          
            Color clr = Diagram.DefaultColor;
            Color frameClr = Diagram.DefaultColor;
            Color maskClr = Color.Transparent;
            string text = "";
            Point3d pt = Point3d.Origin;
            string font = Diagram.DefaultFontName;

            double padding = 0;
            double frameLineWieght = 0;
            double lineWieght = Diagram.DefaultLineWeight;
            double textScale = Diagram.DefaultTextScale;
            Vector3d direction = new Vector3d(1, 1, 0);

            List<double> data = new List<double>();
            List<string> dataLabels = new List<string>();
            List<Color> dataFillColors = new List<Color>();
            List<Color> dataLineColors = new List<Color>();

            double outerRadius = 100;
            double innerRadius = 30;
            double labelRadius = 130;

            Grasshopper.Kernel.Types.IGH_Goo CurveStartObj = null;

            DA.GetDataList(0, data);
            DA.GetDataList(1,  dataLabels);
            DA.GetDataList(2,  dataFillColors);
            DA.GetDataList(3, dataLineColors);
            DA.GetData(4, ref pt);
            DA.GetData(5, ref outerRadius);
            DA.GetData(6, ref innerRadius);
            DA.GetData(7, ref labelRadius);


            DA.GetData(8, ref lineWieght);
            DA.GetData(9, ref textScale);

            DA.GetData(10, ref font);
            DA.GetData(11, ref padding);
            DA.GetData(12, ref clr);
            DA.GetData(13, ref frameClr);
            DA.GetData(14, ref maskClr);
            DA.GetData(15, ref frameLineWieght);
            DA.GetData(16, ref CurveStartObj);


     

            if (data.Count == 0)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "data cannot be Empty");
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

            DiagramPieChart diagramLabel = DiagramPieChart.Create(data, dataLabels, dataFillColors, dataLineColors, location,
        (float) outerRadius, (float)innerRadius, (float)labelRadius, (float)lineWieght, (float)textScale,clr, maskClr, frameClr, (float)frameLineWieght,
         font, (float)padding, crvEnd);






            SizeF size = diagramLabel.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, diagramLabel.GetBoundingBoxLocation());
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
                return DiagramsForGrasshopper.Properties.Resources.PieIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("01b20595-71ef-48b8-a343-0c62b8bb46b4"); }
        }

        public override GH_Exposure Exposure

        {

            get { return GH_Exposure.secondary; }

        }
    }
}