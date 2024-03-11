using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{

    public class CurveModifiers : ModifiersBase
    {

        public double LineWeight = Diagram.DefaultLineWeight;
        public Color LineColors = Diagram.DefaultColor;
        private Grasshopper.Kernel.Types.IGH_Goo StartingCurveEndVariable = null;
        private Grasshopper.Kernel.Types.IGH_Goo EndingCurveEndVariable = null;
        public DiagramCurveEnd StartingCurveEnd = null;
        public DiagramCurveEnd EndingCurveEnd = null;

        private int m_ItemCount;
        public override string Name { get { return "+ Curve Modifiers"; } }

        public override int ItemCount { get { return m_ItemCount; } }

        public CurveModifiers(bool addLineWieght, bool addLineColor, bool addStartingCurveEnd, bool addEndingCurveEnd)
        {
            if (addLineWieght)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_Number(), "LineWeight", "LnWeight", "The optional Lineweight of the Diagram Object") as Grasshopper.Kernel.Parameters.Param_Number;
                new_param.PersistentData.Append(new GH_Number(LineWeight));
                m_Params.Add(new_param);
                m_ItemCount++;
            }

            if (addLineColor)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_Colour(), "LineColor", "LnColor", "The optional LineColor of the Diagram Object") as Grasshopper.Kernel.Parameters.Param_Colour;
                new_param.PersistentData.Append(new GH_Colour(LineColors));
                m_Params.Add(new_param);
                m_ItemCount++;
            }


            if (addStartingCurveEnd)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_GenericObject(), "StartingCurveEnd", "StartCrvEnd", "The optional CurveEnd for the Start of the Diagram Object. You can Create a CurveEnd 3 differnt ways: \n 1) (Prefered) Use the CreateDiagramCurevEnd Component. \n " +
                    "2) Use a DiagramCurve or DiagramHatch Component. \n 3) Imput a Grasshopper Curve") as Grasshopper.Kernel.Parameters.Param_GenericObject;
               // new_param.PersistentData.Append(StartingCurveEndVariable.Duplicate());
                m_Params.Add(new_param);
                m_ItemCount++;
            }

            if (addEndingCurveEnd)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_GenericObject(), "EndingCurveEnd", "EndCrvEnd", "The optional CurveEnd for the End of the Diagram Object. You can Create a CurveEnd 3 differnt ways: \n 1) (Prefered) Use the CreateDiagramCurevEnd Component. \n " +
                    "2) Use a DiagramCurve or DiagramHatch Component. \n 3) Imput a Grasshopper Curve") as Grasshopper.Kernel.Parameters.Param_GenericObject;
               // new_param.PersistentData.Append(EndingCurveEndVariable.Duplicate());
                m_Params.Add(new_param);
                m_ItemCount++;
            }


        }


        public override void GetValues(IGH_DataAccess DA, IGH_Component componant)
        {
            if (this.HasBeenAdded)
            {
                StartingCurveEndVariable = null; //Reset these so they are not persistent
                EndingCurveEndVariable = null;
                StartingCurveEnd = null;
                EndingCurveEnd = null;

                if (componant.Params.IndexOfInputParam("LineWeight") > -1)
                {
                    DA.GetData("LineWeight", ref LineWeight);
                }
                if (componant.Params.IndexOfInputParam("LineColor") > -1)
                {
                    DA.GetData("LineColor", ref LineColors);
                }
                if (componant.Params.IndexOfInputParam("StartingCurveEnd") > -1)
                {
                    DA.GetData("StartingCurveEnd", ref StartingCurveEndVariable);
                }
                if (componant.Params.IndexOfInputParam("EndingCurveEnd") > -1)
                {
                    DA.GetData("EndingCurveEnd", ref EndingCurveEndVariable);
                }

                try
                {
                    if (StartingCurveEndVariable != null)
                    {

                        if (StartingCurveEndVariable is GH_Curve)
                        {
                            StartingCurveEndVariable.CastTo(out Curve crv);
                            StartingCurveEnd = new DiagramCurveEnd(DiagramCurve.Create(crv, Diagram.DefaultColor, Diagram.DefaultLineWeight), crv.PointAtNormalizedLength(0.5), Plane.WorldXY.YAxis, false);

                        }
                     
                       

                            StartingCurveEndVariable.CastTo(out Diagram CurveEndStartDiagram);

                            for (int i = 0; i < CurveEndStartDiagram.Objects.Count; i++)
                            {
                                if (CurveEndStartDiagram.Objects[i] is BaseCurveDiagramObject)
                                {
                                    StartingCurveEnd = new DiagramCurveEnd(CurveEndStartDiagram.Objects[i] as BaseCurveDiagramObject, Point3d.Origin, Plane.WorldXY.YAxis, false);
                                    break;
                                }

                                if (CurveEndStartDiagram.Objects[i] is DiagramCurveEnd)
                                {
                                    StartingCurveEnd = CurveEndStartDiagram.Objects[i] as DiagramCurveEnd;
                                    break;
                                }

                            }
                    }

                }
                catch (Exception ex)
                {
                    componant.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,"Failed to unwrap Starting Curve End, see input hint for help: " + ex.Message);
                }

                try
                {

                    if (EndingCurveEndVariable != null)
                    {
                        if (EndingCurveEndVariable is GH_Curve)
                        {
                            EndingCurveEndVariable.CastTo(out Curve crv);
                            EndingCurveEnd = new DiagramCurveEnd(DiagramCurve.Create(crv, Diagram.DefaultColor, Diagram.DefaultLineWeight), crv.PointAtNormalizedLength(0.5), Plane.WorldXY.YAxis, false);

                        }

                      

                            EndingCurveEndVariable.CastTo(out Diagram CurveEndEndDiagram);

                            for (int i = 0; i < CurveEndEndDiagram.Objects.Count; i++)
                            {
                                if (CurveEndEndDiagram.Objects[i] is BaseCurveDiagramObject)
                                {
                                    EndingCurveEnd = new DiagramCurveEnd(CurveEndEndDiagram.Objects[i] as BaseCurveDiagramObject, Point3d.Origin, Plane.WorldXY.YAxis, false);
                                    break;
                                }

                                if (CurveEndEndDiagram.Objects[i] is DiagramCurveEnd)
                                {
                                    EndingCurveEnd = CurveEndEndDiagram.Objects[i] as DiagramCurveEnd;
                                    break;
                                }

                            }
                        

                    }

                }
                catch (Exception ex)
                {

                    componant.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Failed to unwrap Ending Curve End, see input hint for help:: " + ex.Message);
                }



            }
        }
    }

}
