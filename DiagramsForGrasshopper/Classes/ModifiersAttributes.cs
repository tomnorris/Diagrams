using System;
using System.Drawing;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System.Windows.Forms;

namespace DiagramsForGrasshopper
{
    public class ModifiersAttributes : GH_ComponentAttributes
    {

        private const int BUTTON_HEIGHT = 30;
        private const int BUTTON_PADDING = 1;
        private const int INPUT_VERT_SPACING = 20;
        private const float CORNER_RADIUS = 6;
     

        private float m_CapsuleHeight = 20;


        public ModifiersAttributes(GH_Component owner) : base(owner)
        {
        }


        protected override void Layout()
        {

            Pivot = GH_Convert.ToPoint(Pivot);


           


            DiagramComponentWithModifiers owner = this.Owner as DiagramComponentWithModifiers;


            m_CapsuleHeight = 0;
            for (int i = 0; i < owner.Params.Input.Count; i++)
            {
                m_CapsuleHeight += INPUT_VERT_SPACING;
            }

            if (m_CapsuleHeight < INPUT_VERT_SPACING * 2) {
                m_CapsuleHeight = INPUT_VERT_SPACING * 2; //Account for two outputs
            }
       
           

            m_innerBounds = new RectangleF(Pivot.X, Pivot.Y, 40, m_CapsuleHeight);
            LayoutInputParams(Owner, m_innerBounds);
            LayoutOutputParams(Owner, m_innerBounds);
            int modifiersNotAddedCount = 0;

            for (int i = 0; i < owner.Modifiers.Count; i++)
            {
                if (owner.Modifiers[i].HasBeenAdded == false)
                {
                    modifiersNotAddedCount++;
                }
            }


            var outerBounds = new RectangleF(Pivot.X, Pivot.Y, 40, ((BUTTON_HEIGHT+ BUTTON_PADDING) * modifiersNotAddedCount) + m_CapsuleHeight);
            Bounds = LayoutBounds(Owner, outerBounds);



        }
        

        protected override void Render(Grasshopper.GUI.Canvas.GH_Canvas canvas, Graphics graphics, Grasshopper.GUI.Canvas.GH_CanvasChannel channel)
        {
            if (channel != Grasshopper.GUI.Canvas.GH_CanvasChannel.Objects)
            {
                base.Render(canvas, graphics, channel);
                return;
            }
          
            RenderComponentCapsule(canvas, graphics, true, false, true, true, true, true);

            DiagramComponentWithModifiers owner = this.Owner as DiagramComponentWithModifiers;

            var leftEdgeOfLines = m_innerBounds.X - Bounds.X + m_innerBounds.Width +1 ;

            RectangleF gradRec = new RectangleF(Bounds.Location.X-1, Bounds.Location.Y, leftEdgeOfLines, Bounds.Height);
            var clr = Grasshopper.GUI.Canvas.GH_CapsuleRenderEngine.GetImpliedStyle(Grasshopper.GUI.Canvas.GH_Palette.Normal, this.Selected, this.Owner.Locked,this.Owner.Hidden);
            if (owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Error) {
                clr = Grasshopper.GUI.Canvas.GH_CapsuleRenderEngine.GetImpliedStyle(Grasshopper.GUI.Canvas.GH_Palette.Error, this.Selected, this.Owner.Locked, this.Owner.Hidden);
            }
            if (owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Warning)
            {
                clr = Grasshopper.GUI.Canvas.GH_CapsuleRenderEngine.GetImpliedStyle(Grasshopper.GUI.Canvas.GH_Palette.Warning, this.Selected, this.Owner.Locked, this.Owner.Hidden);
            }
            Pen ghostPen = new Pen(Color.FromArgb(60, Color.White), 3.0f);
            Pen pen = new Pen(clr.Edge, 1.0f);
            Pen gradientPen = new Pen(new System.Drawing.Drawing2D.LinearGradientBrush(gradRec, clr.Edge, clr.Fill, System.Drawing.Drawing2D.LinearGradientMode.Horizontal), 1.0f);
            Pen gapPen = new Pen(new System.Drawing.Drawing2D.LinearGradientBrush(gradRec, Color.FromArgb(255, 220, 217, 210), clr.Fill, System.Drawing.Drawing2D.LinearGradientMode.Horizontal), 1.0f);
            Brush gapBrush = new SolidBrush( Color.FromArgb(255, 220, 217, 210));
            Pen ghostGradientPen = new Pen(new System.Drawing.Drawing2D.LinearGradientBrush(gradRec, Color.FromArgb(60, Color.White), clr.Fill, System.Drawing.Drawing2D.LinearGradientMode.Horizontal), 3.0f);  
           
            int spacing = 0;
           
            for (int j = 0; j < owner.Seperators.Count - 1; j++)
            {

                /* spacing += owner.Seperators[j];
                 graphics.DrawLine(ghostGradientPen, new PointF(Bounds.Location.X,  Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3), new PointF(m_innerBounds.Location.X+ m_innerBounds.Width-1, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3));
                 graphics.DrawLine(ghostGradientPen, new PointF(Bounds.Location.X,  Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 2), new PointF(m_innerBounds.Location.X + m_innerBounds.Width-1, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 2));


                 graphics.DrawArc(ghostPen, new RectangleF(new PointF(Bounds.Location.X,  Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 2 + 3), new SizeF(CORNER_RADIUS, CORNER_RADIUS)), 180, 90);
                 graphics.DrawArc(ghostPen, new RectangleF(new PointF(Bounds.Location.X,  Bounds.Location.Y + (INPUT_VERT_SPACING * spacing)  - 3), new SizeF(CORNER_RADIUS, CORNER_RADIUS)), 90, 90);


                 graphics.DrawLine(gradientPen, new PointF(Bounds.Location.X,  Bounds.Location.Y+(INPUT_VERT_SPACING * spacing) + 3), new PointF(m_innerBounds.Location.X + m_innerBounds.Width-1, Bounds.Location.Y +(INPUT_VERT_SPACING * spacing) + 3));
                 graphics.DrawLine(gradientPen, new PointF(Bounds.Location.X,  Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 2), new PointF(m_innerBounds.Location.X + m_innerBounds.Width-1, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 2));

                 graphics.DrawArc(pen, new RectangleF(new PointF(Bounds.Location.X ,  Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 2 + 3), new SizeF(CORNER_RADIUS, CORNER_RADIUS)),180, 90);
                 graphics.DrawArc(pen, new RectangleF(new PointF(Bounds.Location.X ,  Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) -3), new SizeF(CORNER_RADIUS, CORNER_RADIUS)), 90, 90);

                 graphics.DrawLine(gapPen, new PointF(Bounds.Location.X - 1,  Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 1), new PointF(m_innerBounds.Location.X + m_innerBounds.Width-1, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 1));
                 */

                spacing += owner.Seperators[j];

                graphics.FillRectangle(gapBrush, new RectangleF(new PointF(Bounds.Location.X-0.5f, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing)), new SizeF(1, CORNER_RADIUS + 2)));
                graphics.DrawLine(ghostGradientPen, new PointF(Bounds.Location.X + 3, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3), new PointF(m_innerBounds.Location.X  - 1, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3));
                graphics.DrawLine(ghostGradientPen, new PointF(Bounds.Location.X + 3, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 2), new PointF(m_innerBounds.Location.X - 1, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 2));


                graphics.DrawArc(ghostPen, new RectangleF(new PointF(Bounds.Location.X , Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 2 + 3), new SizeF(CORNER_RADIUS, CORNER_RADIUS)), 180, 90);
                graphics.DrawArc(ghostPen, new RectangleF(new PointF(Bounds.Location.X, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) - 3), new SizeF(CORNER_RADIUS, CORNER_RADIUS)), 90, 90);


                graphics.DrawLine(gradientPen, new PointF(Bounds.Location.X+3, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3), new PointF(m_innerBounds.Location.X - 1, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3));
                graphics.DrawLine(gradientPen, new PointF(Bounds.Location.X + 3, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 2), new PointF(m_innerBounds.Location.X  - 1, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 2));

                graphics.DrawArc(pen, new RectangleF(new PointF(Bounds.Location.X , Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 2 + 3), new SizeF(CORNER_RADIUS, CORNER_RADIUS)), 180, 90);
                graphics.DrawArc(pen, new RectangleF(new PointF(Bounds.Location.X , Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) - 3), new SizeF(CORNER_RADIUS, CORNER_RADIUS)), 90, 90);

                graphics.DrawLine(gapPen, new PointF(Bounds.Location.X - 1, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 1), new PointF(m_innerBounds.Location.X - 1, Bounds.Location.Y + (INPUT_VERT_SPACING * spacing) + 3 + 1));
                



            }





            int counter = 0;
            for (int i = 0; i < owner.Modifiers.Count; i++)
            {
                if (owner.Modifiers[i].HasBeenAdded == false)
                {
                    RectangleF buttonBounds = new RectangleF(Bounds.Location.X, Bounds.Location.Y + m_innerBounds.Height + 5 + ((BUTTON_HEIGHT + BUTTON_PADDING) * counter), Bounds.Width, BUTTON_HEIGHT);
                    var btn = Grasshopper.GUI.Canvas.GH_Capsule.CreateTextCapsule(buttonBounds, buttonBounds, Grasshopper.GUI.Canvas.GH_Palette.Black, owner.Modifiers[i].Name, 2, 0);
                    btn.Render(graphics, this.Selected, this.Owner.Locked, false);
                    btn.Dispose();
                    counter++;
                }

           

            }


          //  RectangleF nameCapRec = new RectangleF(m_innerBounds.Location.X, m_innerBounds.Location.Y, m_innerBounds.Width, (INPUT_VERT_SPACING * owner.Seperators[0]));
            RectangleF nameCapRec = new RectangleF(m_innerBounds.Location.X, m_innerBounds.Location.Y, m_innerBounds.Width, m_innerBounds.Height);

            var font = new Font(canvas.Font.FontFamily,10f, FontStyle.Bold);
            
                   var nameCap = Grasshopper.GUI.Canvas.GH_Capsule.CreateTextCapsule(nameCapRec, nameCapRec, Grasshopper.GUI.Canvas.GH_Palette.Black,owner.NickName, font, GH_Orientation.vertical_center, 2, 0);
            nameCap.Render(graphics, this.Selected, this.Owner.Locked, false);
            nameCap.Dispose();

         


        }
    



        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left)
            {

                DiagramComponentWithModifiers owner = this.Owner as DiagramComponentWithModifiers;
                int counter = 0;
                for (int i = 0; i < owner.Modifiers.Count; i++)
                {
                    if (owner.Modifiers[i].HasBeenAdded == false) { 

                        RectangleF buttonBounds = new RectangleF(Bounds.Location.X, Bounds.Location.Y + m_innerBounds.Height + 5 + ((BUTTON_HEIGHT + BUTTON_PADDING) * counter), Bounds.Width, BUTTON_HEIGHT);
                    if (buttonBounds.Contains(e.CanvasLocation))
                    {
                        owner.OnPingDocument().ScheduleSolution(5, d =>
                           {
                               owner.Modifiers[i].AddModifiers(owner.Params);
                               owner.Seperators.Add(owner.Modifiers[i].ItemCount);
                               owner.Params.OnParametersChanged();
                               this.ExpireLayout();
                           });


                        return GH_ObjectResponse.Handled;
                    }

                    counter++;
                }
                }


                
               
               
            }

            return base.RespondToMouseDown(sender, e);
        }

      

    }

}


