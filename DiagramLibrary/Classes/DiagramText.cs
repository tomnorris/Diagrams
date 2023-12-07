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



        public static DiagramText Create(string Text, PointF Location, Color Colour, float LineWeight)
        {
            DiagramText diagramText = new DiagramText();
            diagramText.m_Colour = Colour;
            diagramText.m_LineWeight = LineWeight;
            diagramText.m_Text = Text;
            diagramText.m_Location = Location;
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


            g.DrawString(m_Text, font, this.GetBrush(), CompensatedPoint);


            /* PointF pt = new PointF((float)m_Text.Plane.OriginX, -(float)m_Text.Plane.OriginY - (float)m_LineWeight);
             g.ScaleTransform(1, -1);
             g.DrawString(m_Text.PlainText, font, this.GetBrush(), pt);
             g.ResetTransform();
             */


        }



        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance)
        {
            //this needs updateing
         

            /*var newText = m_Text.Duplicate() as TextEntity;
            newText.Plane = Plane.WorldXY;

            newText.Transform(Transform.Translation(new Vector3d(m_Text.Plane.Origin / m_LineWeight)));
            pipeline.DrawText(newText, m_Colour, m_LineWeight);
            */


        }

    }
}
