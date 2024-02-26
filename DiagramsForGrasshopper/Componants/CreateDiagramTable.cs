using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using DiagramLibrary;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramTable : DiagramComponent
    { 
        /// <summary>
        /// Initializes a new instance of the CreateDiagramTable class.
        /// </summary>
    public CreateDiagramTable()
          : base("CreateDiagramTable", "DTable",
                "Description",
              "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        pManager.AddTextParameter("Data", "D", "Data as a string DataTree each branch is a row", GH_ParamAccess.tree);
         //   this.Params.Input[0].ObjectChanged += CreateDiagramTable_ObjectChanged;
            pManager.AddPointParameter("Location", "L", "Location for text", GH_ParamAccess.item, new Point3d(0, 0, 0));
        pManager.HideParameter(1);
        pManager.AddNumberParameter("TextScale", "TS", "Text size", GH_ParamAccess.item, Diagram.DefaultTextScale);
        pManager.AddNumberParameter("LineWidth", "LW", "Frames Line widths", GH_ParamAccess.item, Diagram.DefaultLineWeight);
        pManager.AddNumberParameter("CellWidths", "CW", "List of Widths for each column, first value will be the default width", GH_ParamAccess.list,100);
        pManager.AddNumberParameter("CellHeight", "CH", "List of Heights for each Row, first value will be the default height", GH_ParamAccess.list,30);
        pManager.AddColourParameter("LineColour", "LClr", "Colour for text and Lines", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddNumberParameter("Padding", "P", "Text Padding", GH_ParamAccess.item, 3);
            pManager.AddIntegerParameter("Jusitification", "J", "Text justification. Horizontals(Left, Center, Right) only take effect if Width is set, Verticals (Top, Middle, Bottom) only take effect if Height it set. 0: Bottom Left, 1: Bottom Center, 2: Bottom Right \n 3: Middle Left, 4: Middle Center, 5: Middle Right \n 6: Top Left, 7: Top Center, 8: Top Right", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("Font", "F", "Font family name", GH_ParamAccess.item, Diagram.DefaultFontName);
            pManager.AddColourParameter("Background Colour", "BClr", "Back Colour", GH_ParamAccess.item, Color.Transparent);
        }

     

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            pManager.AddGenericParameter("CSV", "CSV", "CSV format of the table data", GH_ParamAccess.item);
        }




        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            double lineWeight = Diagram.DefaultLineWeight;
            double textScale = Diagram.DefaultTextScale;
            Color clr = Diagram.DefaultColor;
            Color bgClr = Color.Transparent;

            Point3d pt = new Point3d(0, 0, 0);
            string font = Diagram.DefaultFontName;

            double padding = 3;
          
            int jusitificationInt = 0;
            List<double> width = new List<double>();
            List<double> height = new List<double>();
          

            GH_Structure<GH_String> data;
            if (!DA.GetDataTree(0, out data))
            {
                 this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "data cannot be Empty");
                return null;
            }
         
            DA.GetData(1, ref pt);
            DA.GetData(2, ref textScale);
            DA.GetData(3, ref lineWeight);
            DA.GetDataList(4,  width);
            DA.GetDataList(5, height);
            DA.GetData(6, ref clr);
            DA.GetData(7, ref padding);
                        DA.GetData(8, ref jusitificationInt);
            DA.GetData(9, ref font);
            DA.GetData(10, ref bgClr);



            if (data == null)
            {
              
                return null;
            }

            if (width.Count == 0)
            {
                 this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Cell Widths need at least on value");
                return null;
            }
            if (height.Count == 0)
            {
                 this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Cell Heights need at least on value");
                return null;
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

            PointF location = Diagram.ConvertPoint(pt);

            DiagramTable diagramTable = DiagramTable.Create(data,width,height, (float )textScale, location, clr, (float)lineWeight,font,(float)padding,jusitification);
            SizeF size = diagramTable.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, bgClr,0,Color.Transparent,location);
            diagram.AddDiagramObject( diagramTable );


           

            DA.SetData(2, diagramTable.ToCSV());
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
            get { return new Guid("b0ba788e-7715-436d-8d7c-ef12e1f1fce3"); }
        }

        public override GH_Exposure Exposure

        {

            get { return GH_Exposure.secondary; }

        }
    }
}