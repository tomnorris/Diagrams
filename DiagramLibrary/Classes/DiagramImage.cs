using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
   public  class DiagramImage : DiagramObject
    {
        public override string DiagramObjectType() { return "DiagramImage"; }

        private Image m_Image;
        private PointF m_Location;
        private SizeF m_Size;
        private string m_ImagePath;



        public static DiagramImage Create(string imagePath, PointF Location, SizeF Size)
        {
            DiagramImage diagramImage = new DiagramImage();
            diagramImage.m_Colour = Diagram.DefaultColor;
            diagramImage.m_LineWeight = Diagram.DefaultLineWieght;
            diagramImage.m_ImagePath = imagePath;            
            diagramImage.m_Image = Bitmap.FromFile(imagePath); 
            diagramImage.m_Location = Location;
            if (Size == SizeF.Empty)
            {
                diagramImage.m_Size = diagramImage.m_Image.Size;
            }
            else {
                diagramImage.m_Size = Size;

            }
            return diagramImage;
        }

        public DiagramImage Duplicate()
        {
            DiagramImage diagramImage = new DiagramImage();
            diagramImage.m_Colour = m_Colour;
            diagramImage.m_LineWeight = m_LineWeight;
            diagramImage.m_Image = m_Image;
            diagramImage.m_Location = m_Location;
            diagramImage.m_Size = m_Size;

            return diagramImage;
        }

        public override BoundingBox GetBoundingBox() {
            return new BoundingBox(m_Location.X,m_Location.Y,0, m_Location.X+ m_Size.Width, m_Location.Y + m_Size.Height, 0);
        }


        public override void DrawBitmap(Graphics g)
        {

           // Drawn Upside Down as final image is flipped
            g.ScaleTransform(1, -1);
            g.DrawImage(m_Image, m_Location.X, -m_Location.Y- m_Size.Height, m_Size.Width, m_Size.Height);
            g.ResetTransform();
        }

       

        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xfrom, bool colorOverride)
        {

            Rectangle3d rec = new Rectangle3d(Plane.WorldXY, new Interval(m_Location.X, m_Location.X + m_Size.Width), new Interval(m_Location.Y, m_Location.Y + m_Size.Height));
            Brep brep = Brep.CreateTrimmedPlane(Plane.WorldXY, rec.ToNurbsCurve());

            var texture = new Rhino.DocObjects.Texture();
            texture.FileName = m_ImagePath;
                
              var mat = new Rhino.Display.DisplayMaterial();
            

            mat.SetBitmapTexture(texture, true);
            pipeline.DrawBrepShaded(brep, new Rhino.Display.DisplayMaterial(mat));

        }

    }
}
