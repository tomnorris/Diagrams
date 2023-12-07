using System;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

namespace DiagramsForGrasshopper
{
    public class DiagramComponentAttibutes : GH_ComponentAttributes
    {



        // private double Scale = 1;
        private float CapsuleHeight = 20;


        public DiagramComponentAttibutes(GH_Component owner) : base(owner) { }

        protected override void Layout()
        {

            Pivot = GH_Convert.ToPoint(Pivot);


            float width = 125;
            float height = CapsuleHeight;


            CanvasDiagram owner = this.Owner as CanvasDiagram;

            if (owner.Diagram != null)
            {

                Size size = owner.Diagram.GetBoundingSize(owner.Scale);
                if (size.Width > 10)
                {

                    width = size.Width + 60;

                }
                owner.Update = true;
            }


            m_innerBounds = new RectangleF(Pivot.X, Pivot.Y, width, height);
            LayoutInputParams(Owner, m_innerBounds);
            LayoutOutputParams(Owner, m_innerBounds);
            Bounds = LayoutBounds(Owner, m_innerBounds);



        }

        protected override void Render(Grasshopper.GUI.Canvas.GH_Canvas canvas, Graphics graphics, Grasshopper.GUI.Canvas.GH_CanvasChannel channel)
        {
            if (channel != Grasshopper.GUI.Canvas.GH_CanvasChannel.Objects)
            {
                base.Render(canvas, graphics, channel);
                return;
            }

            //RenderComponentCapsule(canvas, graphics, true, false, false, true, true, true);
            RenderComponentCapsule(canvas, graphics, true, false, false, true, true, true);


            CanvasDiagram canvasDiagram = this.Owner as CanvasDiagram;
            if (canvasDiagram.Diagram != null)
            {
              
                Size size = canvasDiagram.Diagram.GetBoundingSize(canvasDiagram.Scale);

                graphics.DrawLine(Pens.DarkGray, m_innerBounds.Location, new Point((int)m_innerBounds.X, (int)(m_innerBounds.Y + CapsuleHeight)));
                graphics.DrawLine(Pens.DarkGray, new Point((int)(m_innerBounds.X+m_innerBounds.Width), (int)(m_innerBounds.Y + CapsuleHeight)), 
                    new Point((int)(m_innerBounds.X + m_innerBounds.Width), (int)(m_innerBounds.Y + CapsuleHeight)));

                string text = canvasDiagram.Diagram.Title;
                graphics.DrawString(text, new Font(canvas.Font.FontFamily, 8), Brushes.Black, new Point((int)(m_innerBounds.X + 3), (int)(m_innerBounds.Y + 3)));


                Rectangle rec = new Rectangle((int)this.Bounds.X+4, (int)(this.Bounds.Y + CapsuleHeight)+4, (int)this.Bounds.Width-8, (int)(this.Bounds.Width / size.Width * size.Height) -8);
                graphics.FillRectangle(Brushes.White, rec);
                graphics.DrawRectangle(Pens.Black, rec);

                if (Grasshopper.GUI.Canvas.GH_Canvas.ZoomFadeLow > 0 )
                {

                    if (canvasDiagram.Update || canvasDiagram.Bitmap == null)
                    {
                        canvasDiagram.Bitmap = canvasDiagram.Diagram.GetBitmap(canvasDiagram.Scale);
                        canvasDiagram.Update = false;
                    }
                    
                    graphics.DrawImage(canvasDiagram.Bitmap, rec.X+1, rec.Y+1, rec.Width-2, rec.Height-2);
                    
                }

            }
        }
        /* protected override void Layout()
         {

             Pivot = GH_Convert.ToPoint(Pivot);


             float width = 125;
             float height = 100;


             CanvasDiagram owner = this.Owner as CanvasDiagram;

             if (owner.Diagram != null)
             {
                 Scale = owner.Scale;
                 Size size = owner.Diagram.GetBoundingSize(Scale);
                 if (size.Height > 10 && size.Width > 10)
                 {
                     height = size.Height + HalfMargin + HalfMargin;
                     width = size.Width + HalfMargin + HalfMargin;
                 }
             }


             m_innerBounds = new RectangleF(Pivot.X, Pivot.Y, width, height);
             LayoutInputParams(Owner, m_innerBounds);
             LayoutOutputParams(Owner, m_innerBounds);
             Bounds = LayoutBounds(Owner, m_innerBounds);



         }


         protected override void Render(Grasshopper.GUI.Canvas.GH_Canvas canvas, Graphics graphics, Grasshopper.GUI.Canvas.GH_CanvasChannel channel)
         {
             if (channel != Grasshopper.GUI.Canvas.GH_CanvasChannel.Objects)
             {
                 base.Render(canvas, graphics, channel);
                 return;
             }

             //RenderComponentCapsule(canvas, graphics, true, false, false, true, true, true);
             RenderComponentCapsule(canvas, graphics, true, false, false, true, true, true);


             Rectangle rec = GH_Convert.ToRectangle(m_innerBounds);
             graphics.FillRectangle(Brushes.White, rec);

             if (Grasshopper.GUI.Canvas.GH_Canvas.ZoomFadeLow > 0)
             {
                 Rectangle reci = rec;
                 reci.Inflate(-5, -5);

                 CanvasDiagram owner = this.Owner as CanvasDiagram;
                 if (owner.Diagram != null)
                 {
                     if (owner.Update || owner.Bitmap == null)
                     {
                         owner.Bitmap = owner.Diagram.GetBitmap(Scale);
                         owner.Update = false;
                         }
                     graphics.DrawImage(owner.Bitmap, m_innerBounds.X + HalfMargin, m_innerBounds.Y + HalfMargin, m_innerBounds.Width - HalfMargin - HalfMargin, m_innerBounds.Height - HalfMargin - HalfMargin);

                 }
             }
             else
             {
                 Brush blendfill = new SolidBrush(Color.FromArgb(255 - Grasshopper.GUI.Canvas.GH_Canvas.ZoomFadeLow, Color.White));
                 graphics.FillRectangle(blendfill, rec);
                 blendfill.Dispose();

             }
             if (Grasshopper.GUI.Canvas.GH_Canvas.ZoomFadeLow < 255)
             {
                 Brush blendfill = new SolidBrush(Color.FromArgb(255 - Grasshopper.GUI.Canvas.GH_Canvas.ZoomFadeLow, Color.White));
                 graphics.FillRectangle(blendfill, rec);
                 blendfill.Dispose();
             }
             graphics.DrawRectangle(Pens.Black, rec);
         }*/
    }
}
