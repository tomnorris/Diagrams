using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;


namespace DiagramLibrary
{
    public class DiagramText : DiagramObject
    {
        private string m_Text;
        private PointF m_Location;
        private TextJustification m_Anchor;
        private DiagramFilledRectangle m_Mask = null;
        private bool m_MaskEnabled = false;
        private string m_FontName = "Arial";
        private bool m_wrapped = false;
        private SizeF m_WrapSize;
        private float m_Padding = 3f;


        public override string DiagramObjectType() { return "DiagramText"; }



        public static DiagramText Create(string Text, PointF Location, Color Colour, float LineWeight,
            TextJustification anchor, Color maskColour,bool drawFrame, Color frameColor,float frameLineWeight, 
            bool maskEnabled, string fontName,bool wrapped, SizeF wrapSize, float padding)
        {
            DiagramText diagramText = new DiagramText();
            diagramText.m_Colour = Colour;
            diagramText.m_LineWeight = LineWeight;
            diagramText.m_Text = Text;
            diagramText.m_Location = Location;
            diagramText.m_Anchor = anchor;
            diagramText.m_Mask = DiagramFilledRectangle.Create(new Rectangle3d(Plane.WorldXY,1,1),maskColour,drawFrame,frameColor,frameLineWeight);
            diagramText.m_MaskEnabled = maskEnabled;
            diagramText.m_FontName = fontName;
            diagramText.m_wrapped = wrapped;
            diagramText.m_WrapSize = wrapSize;
            diagramText.m_Padding = padding;
            return diagramText;
        }

        public DiagramText Duplicate()
        {
            DiagramText diagramText = new DiagramText();
            diagramText.m_Location = m_Location;
            diagramText.m_Colour = m_Colour;
            diagramText.m_LineWeight = m_LineWeight;
            diagramText.m_Text = m_Text;
            diagramText.m_Anchor = m_Anchor;
            diagramText.m_Mask = m_Mask;
            diagramText.m_MaskEnabled = m_MaskEnabled;
            diagramText.m_FontName = m_FontName;
            diagramText.m_wrapped = m_wrapped;
            diagramText.m_WrapSize = m_WrapSize;
            diagramText.m_Padding = m_Padding;

            return diagramText;
        }

        public void EnableMask(Color color,bool DrawLine,Color LineColor, float LineWeight)
        {
            m_Mask = DiagramFilledRectangle.Create(new Rectangle3d(Plane.WorldXY, 10, 10), color, DrawLine, LineColor, LineWeight);
            m_MaskEnabled = true;
        }

        private PointF GetCompensatedPoint(SizeF size) {
            switch (m_Anchor)
            {
                case TextJustification.None:
                    return m_Location;                    
                case TextJustification.Left:
                    return m_Location;
                case TextJustification.Center:
                    return new PointF(m_Location.X - (size.Width / 2),m_Location.Y);
                case TextJustification.Right:
                    return new PointF(m_Location.X - size.Width, m_Location.Y);
                case TextJustification.Bottom:
                    return m_Location;
                case TextJustification.Middle:
                    return new PointF(m_Location.X , m_Location.Y - (size.Height / 2));
                case TextJustification.Top:
                    return new PointF(m_Location.X, m_Location.Y - size.Height);
                case TextJustification.BottomLeft:
                    return m_Location;
                case TextJustification.BottomCenter:
                    return new PointF(m_Location.X - (size.Width / 2), m_Location.Y);
                case TextJustification.BottomRight:
                    return new PointF(m_Location.X - size.Width, m_Location.Y);
                case TextJustification.MiddleLeft:
                    return new PointF(m_Location.X, m_Location.Y - (size.Height / 2));
                case TextJustification.MiddleCenter:
                    return new PointF(m_Location.X - (size.Width / 2), m_Location.Y - (size.Height / 2));
                case TextJustification.MiddleRight:
                    return new PointF(m_Location.X- size.Width, m_Location.Y - (size.Height / 2));
                case TextJustification.TopLeft:
                    return new PointF(m_Location.X, m_Location.Y - size.Height);
                case TextJustification.TopCenter:
                    return new PointF(m_Location.X - (size.Width / 2), m_Location.Y - size.Height);
                case TextJustification.TopRight:
                    return new PointF(m_Location.X - size.Width, m_Location.Y - size.Height);
                default:
                    return m_Location;
            }
        }

        public override void DrawBitmap(Graphics g)
        {

            var font = new System.Drawing.Font(m_FontName, (float)m_LineWeight);
            SizeF AllowedTextSize = SizeF.Empty;
            if (m_wrapped)
            {
                AllowedTextSize = g.MeasureString(m_Text, font, m_WrapSize, StringFormat.GenericDefault, out int charactersFitted, out int linesFilled);
            }
            else
            {
                AllowedTextSize = g.MeasureString(m_Text, font);


            }

            PointF CompensatedPoint = GetCompensatedPoint(AllowedTextSize);

            if (m_MaskEnabled && m_Mask != null)
            {
                m_Mask.UpdateRectangle(CompensatedPoint, AllowedTextSize);
                m_Mask.DrawBitmap(g);
            }


          

             PointF pt = new PointF(CompensatedPoint.X, -CompensatedPoint.Y - (float)m_LineWeight);
             g.ScaleTransform(1, -1);
             g.DrawString(m_Text, font, this.GetBrush(), pt);
             g.ResetTransform();
           


        }



        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform,bool colorOverride)
        {

            
            SizeF AllowedTextSize = SizeF.Empty;
            if (m_wrapped)
            {


                AllowedTextSize = m_WrapSize;
            }
            else
            {
                AllowedTextSize = pipeline.Measure2dText(m_Text, new Point2d(0, 0), false, 0, (int)m_LineWeight, m_FontName).Size;


            }



            PointF CompensatedPoint = GetCompensatedPoint(AllowedTextSize);

            if (m_MaskEnabled && m_Mask != null)
            {

                m_Mask.UpdateRectangle(CompensatedPoint, AllowedTextSize);
                m_Mask.DrawRhinoPreview(pipeline, tolerance, xform, colorOverride);
            }


            TextEntity txt = new TextEntity();
            txt.PlainText = m_Text;
            txt.Font = new Rhino.DocObjects.Font(m_FontName, Rhino.DocObjects.Font.FontWeight.Normal, Rhino.DocObjects.Font.FontStyle.Upright,false,false);
            txt.Plane = Plane.WorldXY;
            txt.Plane.Transform(Transform.Translation(new Vector3d(m_Location.X,m_Location.Y,0)));
            txt.Justification = m_Anchor;
            txt.TextHeight = m_LineWeight;
            txt.FormatWidth = m_WrapSize.Width;
            

            if (m_wrapped)
            {
                txt.WrapText();
            }
            


            pipeline.DrawText(txt, m_Colour, m_LineWeight);
            


        }

    }
}
