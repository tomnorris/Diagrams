using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramCurveEnd : DiagramComponent
    {
        /// <summary>
        /// Initializes a new instance of the DiagramCurveEnd class.
        /// </summary>
        public CreateDiagramCurveEnd()
          : base("CreateDiagramCurveEnd", "DCrvEnd",
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
            pManager.AddNumberParameter("Weight", "LW", "Line Weigh of the Curve", GH_ParamAccess.item, Diagram.DefaultLineWeight);
            pManager.AddPointParameter("Point", "P", "Pivot point of the Curve End, this is the point which will be put on the target Curve's end point, Default is the Origin", GH_ParamAccess.item, Point3d.Origin);
            pManager.AddVectorParameter("Direction", "D", "This is the direction of the Curve End which will be aligned with the tangent of the Target Curve's end point, Default is the Y - Axis", GH_ParamAccess.item,Vector3d.YAxis);
            pManager.AddBooleanParameter("Flipped", "F", "Flip the direction of the Curve End", GH_ParamAccess.item, true);

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
            Point3d point = Point3d.Origin;
           Vector3d direction = Vector3d.YAxis;
            bool flipped = true;


            DA.GetData(0, ref crv);
            DA.GetData(1, ref clr);
            DA.GetData(2, ref weight);
            DA.GetData(3, ref point);
            DA.GetData(4, ref direction);


            DA.GetData(5, ref flipped);
       

            if (crv == null)
            {
                 this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error,"Curve cannot be Null");
                return null;
            }



            DiagramCurve diagramCurve = DiagramCurve.Create(crv, clr, (float)weight);

            DiagramCurveEnd diagramCurveEnd = new DiagramCurveEnd(diagramCurve, point, direction, flipped);

           


            SizeF size = diagramCurve.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, diagramCurve.GetLocation());
            diagram.AddDiagramObject(diagramCurveEnd);


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
            get { return new Guid("ef7532ad-fac2-44c6-9695-d14adb58dd30"); }
        }
    }
}