using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper.Componants
{
    public class CreateDiagramCurve : DiagramComponent
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramCurve class.
        /// </summary>
        public CreateDiagramCurve()
          : base("CreateDiagramCurve", "DCruve",
              "Description",
              "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Crv", "The Curve", GH_ParamAccess.item);
        //    this.Params.Input[0].ObjectChanged += CreateDiagramCurve_ObjectChanged; 
            pManager.AddColourParameter("Colour", "Clr", "Colour of the Curve", GH_ParamAccess.item, Diagram.DefaultColor);
            pManager.AddNumberParameter("Weight", "LW", "Line Weigh of the Curve", GH_ParamAccess.item,Diagram.DefaultLineWeight);
            pManager.AddGenericParameter("CurveEndStart", "CES", "Diagram Object which will be the Curve End for the start of the Curve, only Curve and FilledCurves are supported", GH_ParamAccess.item);
            pManager.AddGenericParameter("CurveEndEnd", "CEE", "Diagram Object which will be the Curve End for the End of the Curve, only Curve and FilledCurves are supported", GH_ParamAccess.item);
            this.Params.Input[3].Optional = true;
            this.Params.Input[4].Optional = true;

        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {

            double weight = Diagram.DefaultLineWeight;
            Color clr = Diagram.DefaultColor;
            Curve crv = null;

            Grasshopper.Kernel.Types.IGH_Goo CurveStartObj = null;
            Grasshopper.Kernel.Types.IGH_Goo CurveEndObj = null;


            DA.GetData(0, ref crv);
            DA.GetData(1, ref clr);
           DA.GetData(2, ref weight);
            DA.GetData(3, ref CurveStartObj);
            DA.GetData(4, ref CurveEndObj);

            if (crv == null)
            {
                AddUsefulMessage(DA, "Curve cannot be Null");
                return null;
            }

           



            DiagramCurve diagramCurve = DiagramCurve.Create(crv,clr, (float)weight);


         
            try
            {
                CurveStartObj.CastTo(out Diagram CurveEndStartDiagram);

                for (int i = 0; i < CurveEndStartDiagram.Objects.Count; i++)
                {
                    if (CurveEndStartDiagram.Objects[i] is BaseCurveDiagramObject)
                    {

                      
                        diagramCurve.AddCurveEnds(CurveEndStartDiagram.Objects[i] as BaseCurveDiagramObject, new Point3d(0, 0, 0), Plane.WorldXY.YAxis, null, Point3d.Unset, Vector3d.Unset);
                        break;
                    }

                    if (CurveEndStartDiagram.Objects[i] is DiagramCurveEnd)
                    {
                        diagramCurve.AddCurveEnds(CurveEndStartDiagram.Objects[i] as DiagramCurveEnd, null);
                                               break;
                    }

                }

                CurveEndObj.CastTo(out Diagram CurveEndEndDiagram);

                for (int i = 0; i < CurveEndEndDiagram.Objects.Count; i++)
                {
                    if (CurveEndEndDiagram.Objects[i] is BaseCurveDiagramObject)
                    {

                        diagramCurve.AddCurveEnds(null, Point3d.Unset, Vector3d.Unset, CurveEndEndDiagram.Objects[i] as BaseCurveDiagramObject, new Point3d(0, 0, 0), Plane.WorldXY.YAxis);
                        break;
                    }

                    if (CurveEndStartDiagram.Objects[i] is DiagramCurveEnd)
                    {
                        diagramCurve.AddCurveEnds(null,CurveEndStartDiagram.Objects[i] as DiagramCurveEnd);
                        break;
                    }

                }




            }
            catch (Exception)
            {


            }




            SizeF size = diagramCurve.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent,0,Color.Transparent, diagramCurve.GetLocation());
            diagram.AddDiagramObject(diagramCurve);


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
            get { return new Guid("eeb4bbf4-584b-4d37-8896-5d61b8cdd82c"); }
        }
    }
}