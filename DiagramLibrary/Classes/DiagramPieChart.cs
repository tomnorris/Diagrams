using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino;
using Rhino.Display;
using Rhino.Geometry;

using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;


namespace DiagramLibrary
{
   public class DiagramPieChart : DiagramObject
    {
        List<double> m_Data = new List<double>();
        List<string> m_DataLabels = new List<string>();
        List<Color> m_DataFillColors = new List<Color>();
        List<Color> m_DataLineColors = new List<Color>();

        float m_OuterRadius;
        float m_InnerRadius;
        float m_LabelRadius;
        float m_TextSize;
        Color m_MaskColor;
        Color m_FrameColor;
        float m_FrameLineWieght;
        string m_FontName;
        float m_Padding;
        DiagramCurveEnd m_CurveEnd;
        PointF m_Location;






        public static DiagramPieChart Create(List<double> Data, List<string> DataLabels, List<Color> DataFillColors, List<Color> DataLineColors, PointF Location,
        float OuterRadius, float InnerRadius, float LabelRadius,float LineWieght, float TextSize, Color textColor, Color maskColor, Color frameColor, float frameLineWieght,
        string fontName, float padding, DiagramCurveEnd CurveEnd)
        {
            DiagramPieChart chart = new DiagramPieChart();
            chart.m_Data = Data;

            List<Color> backupFillColours = DiagramColour.GetColors(chart.m_Data.Count);

            for (int i = 0; i < chart. m_Data.Count; i++)
            {

                if (DataLabels.Count > i) {
                    chart.m_DataLabels.Add(DataLabels[i]);
                        } else {
                    chart.m_DataLabels.Add(string.Empty);
                }
                if (DataFillColors.Count > i)
                {
                    chart.m_DataFillColors.Add(DataFillColors[i]);
                }
                else
                {
                    chart.m_DataFillColors.Add(backupFillColours[i]);
                    
                }




                if (DataLineColors.Count > i)
                {
                    chart.m_DataLineColors.Add(DataLineColors[i]);
                }
                else
                {
                    if (i == 0)
                    {
                        chart.m_DataLineColors.AddRange(DiagramColour.GetColors(1,Diagram.DefaultColor));
                    }
                    else {
                        chart.m_DataLineColors.AddRange(DiagramColour.GetColors(1, chart.m_DataLineColors[i - 1]));
                    }
                   
                }

            }
           
            chart.m_Location = Location;
          

            chart.m_OuterRadius = OuterRadius;
            chart.m_InnerRadius = InnerRadius;
            chart.m_LabelRadius = LabelRadius;
            chart.m_TextSize = TextSize;
            chart.m_Colour = textColor;
            chart.m_MaskColor = maskColor;
            chart.m_FrameColor = frameColor;
            chart.m_FrameLineWieght = frameLineWieght;
            chart.m_FontName = fontName;
            chart.m_Padding = padding;
            chart.m_CurveEnd = CurveEnd;
            chart.m_LineWeight = LineWieght;


            return chart;

        }


        public static DiagramPieChart Create(List<double> Data, List<string> DataLabels)
        {
             
            return Create( Data, DataLabels, DiagramColour.GetColors(Data.Count),  DiagramColour.GetColors(Data.Count, Diagram.DefaultColor), Diagram.ConvertPoint(Point3d.Origin), 100,
                60, 130, Diagram.DefaultLineWeight, Diagram.DefaultTextScale,Diagram.DefaultColor, Color.Transparent, Diagram.DefaultColor,  -1,
         Diagram.DefaultFontName, Diagram.DefaultPadding,  null);

        }



        public static  DiagramPieChart Create(List<double> Data, List<string> DataLabels, List<Color> DataFillColors, PointF Location,
      float OuterRadius, float InnerRadius, float LabelRadius )
        {
           
            return Create(Data, DataLabels, DataFillColors, DiagramColour.GetColors(Data.Count, Diagram.DefaultColor), Location, OuterRadius,
                InnerRadius, LabelRadius, Diagram.DefaultLineWeight, Diagram.DefaultTextScale, Diagram.DefaultColor, Color.Transparent, Diagram.DefaultColor, -1,
         Diagram.DefaultFontName, Diagram.DefaultPadding, null);

        }


        public override DiagramObject Duplicate()
        {
            DiagramPieChart chart = new DiagramPieChart();
            chart.m_Data = m_Data;
            chart.m_DataLabels = m_DataLabels;
            chart.m_DataFillColors = m_DataFillColors;
            chart.m_Location = m_Location;
            chart.m_DataLineColors = m_DataLineColors;
            chart.m_LineWeight = m_LineWeight;
            chart.m_OuterRadius = m_OuterRadius;
            chart.m_InnerRadius = m_InnerRadius;
            chart.m_LabelRadius = m_LabelRadius;
            chart.m_TextSize = m_TextSize;
            chart.m_MaskColor = m_MaskColor;
            chart.m_FrameColor = m_FrameColor;
            chart.m_FrameLineWieght = m_FrameLineWieght;
            chart.m_FontName = m_FontName;
            chart.m_Padding = m_Padding;
            chart.m_CurveEnd = m_CurveEnd;



            return chart;
        }



        private List<double> CalulateSegmentParameters()
        {
            List<double> parameters = new List<double>();
            parameters.Add(0);

            double total = 0;
            for (int i = 0; i < m_Data.Count; i++)
            {
                total += m_Data[i];
            }

            double startingparam = 0;
            for (int i = 0; i < m_Data.Count; i++)
            {
                double paramAmount = ( m_Data[i] / total) *(2 * Math.PI);
                parameters.Add(paramAmount + startingparam);
                startingparam += paramAmount;
            }

            return parameters;
        }

        private List<DiagramObject> GenerateObjects()
        {

            List<DiagramObject> diagramObjects = new List<DiagramObject>();

            Point3d rhinoPt = Diagram.ConvertPoint(m_Location);
            Circle mainCircle = new Circle(rhinoPt, m_OuterRadius);
            Circle innerCircle = new Circle(rhinoPt, m_InnerRadius);
            Circle labelCircle = new Circle(rhinoPt, m_LabelRadius);

            List<double> parameters = CalulateSegmentParameters();

            for (int i = 0; i < m_Data.Count; i++)
            {
                List<Curve> curves = new List<Curve>();



                Arc outerArc = new Arc(mainCircle, new Interval(parameters[i], parameters[i + 1]));
                Arc labelArc = new Arc(labelCircle, new Interval(parameters[i], parameters[i + 1]));


                curves.Add(outerArc.ToNurbsCurve());
                Point3d innerPointStart = rhinoPt;
                Point3d innerPointEnd = rhinoPt;
                if (m_InnerRadius > 0)
                {
                    Arc innerArc = new Arc(innerCircle, new Interval(parameters[i], parameters[i + 1]));


                    curves.Add(innerArc.ToNurbsCurve());
                    innerPointStart = innerArc.StartPoint;
                    innerPointEnd = innerArc.EndPoint;

                }

                Line radius1 = new Line(innerPointStart, outerArc.StartPoint);
                Line radius2 = new Line(innerPointEnd, outerArc.EndPoint);

                curves.Add(radius1.ToNurbsCurve());
                curves.Add(radius2.ToNurbsCurve());

                Curve[] joinedCurves = Curve.JoinCurves(curves);
                foreach (var joinedCurve in joinedCurves)
                {
                    diagramObjects.Add(DiagramFilledCurve.Create(joinedCurve, m_DataFillColors[i], m_DataLineColors[i], m_LineWeight));
                }

               if( m_DataLabels[i] == string.Empty) { continue;  }
            
                Point3d labelPointOnOuterArc = outerArc.ToNurbsCurve().PointAtNormalizedLength(0.5);
                Point3d labelPointOnLabelArc = labelArc.ToNurbsCurve().PointAtNormalizedLength(0.5);
                Line labelDummyLine = new Line(labelPointOnOuterArc, labelPointOnLabelArc);


                diagramObjects.Add(DiagramLabel.Create(m_DataLabels[i], Diagram.ConvertPoint(labelDummyLine.From), (float)labelDummyLine.Length, labelDummyLine.Direction,m_Colour, m_LineWeight, m_TextSize, m_MaskColor, m_FrameColor, m_FrameLineWieght, m_FontName, m_Padding, m_CurveEnd));

            }
            return diagramObjects;
        }




        public override void DrawBitmap(GH_Component component, Graphics g)
        {
            List<DiagramObject> objs = this.GenerateObjects();
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].DrawBitmap(component, g);
            }
        }

        public override void DrawRhinoPreview(GH_Component component, DisplayPipeline pipeline, double tolerance, Transform xform, bool colorOverride, RhinoDoc doc, bool Bake)
        {


            List<DiagramObject> objs = this.GenerateObjects();
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].DrawRhinoPreview(component, pipeline, tolerance, xform, colorOverride, doc, Bake);
            }



        }


        public override BoundingBox GetBoundingBox()
        {
            BoundingBox bbox = BoundingBox.Empty;
            List<DiagramObject> objs = this.GenerateObjects();
            for (int i = 0; i < objs.Count; i++)
            {
                bbox.Union(objs[i].GetBoundingBox());
            }
            return bbox;
        }
    }
}
