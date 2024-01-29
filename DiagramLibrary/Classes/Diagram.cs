using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class Diagram
    {
        public static Color DefaultColor = Color.Black;
        public static float DefaultLineWieght = 1f;
        public static Color SelectedColor = Color.FromArgb(128, Color.ForestGreen);

        private PointF m_Location;
        private int m_width;
        private int m_height;
        private string m_Title;
        private Color m_Colour;

      

        private DiagramFilledRectangle m_Background;

        private List<DiagramObject> m_Objects;


        public string Title
        {
            get { return m_Title; }
        }

        public Diagram Duplicate()
        {
            Diagram diagram = new Diagram();
            diagram.m_width = m_width;
            diagram.m_height = m_height;
            diagram.m_Objects = m_Objects;
            diagram.m_Colour = m_Colour;
            return diagram;
        }

        public Diagram() { }


        public static Diagram Create(int width, int height, string Title, Color backgroundColour)
        {
                      return Create( width,  height,  Title,  backgroundColour, new PointF(0,0) );
        }


        //Do not use the location to set the diagram location in the RhinoPreview, use the xfrom in the DrawinRhinoPreview method, the diagram location is used to compensate for single object diagram's locations
        public static Diagram Create(int width, int height, string Title, Color backgroundColour, PointF location)
        {
            Diagram diagram = new Diagram();
            diagram.m_width = width;
            diagram.m_height = height;
            diagram.m_Objects = new List<DiagramObject>();
            diagram.m_Title = Title;
            Plane plane = Plane.WorldXY;
            plane.Origin = new Point3d(location.X,location.Y,0);
            Rectangle3d rec = new Rectangle3d(plane, width, height);
            diagram.m_Colour = backgroundColour;
            diagram.m_Background = DiagramFilledRectangle.Create(rec, backgroundColour, false, DefaultColor, DefaultLineWieght);
            diagram.m_Location = location;
            return diagram;
        }

        public void AddObjects(List<object> objs, double tolernace)
        {
            for (int i = 0; i < objs.Count; i++)
            {



                try
                {
                    var goo = objs[i] as Grasshopper.Kernel.Types.IGH_GeometricGoo;
                    goo.CastTo(out GeometryBase geoBase);
                    AddRhinoObject(objs[i], tolernace);



                }
                catch (Exception)
                {
                    AddDiagramObjectFromGoo(objs[i]);

                }



            }

        }


        public void AddDiagramObject(DiagramObject obj)
        {

            m_Objects.Add(obj);
        }


        public void AddDiagramObjectFromGoo(object obj)
        {
            var goo = obj as Grasshopper.Kernel.Types.IGH_Goo;

            goo.CastTo(out Diagram diagram);
            m_Objects.AddRange(diagram.m_Objects);

            

        }




        public void AddRhinoObject(object obj, double tolernace)
        {
            if (obj == null) { return; }


            // if (obj is GeometryBase == false) {                return;            }

            var goo = obj as Grasshopper.Kernel.Types.IGH_GeometricGoo;
            goo.CastTo(out GeometryBase geoBase);

            switch (geoBase.ObjectType)
            {
                case Rhino.DocObjects.ObjectType.None:
                    break;
                case Rhino.DocObjects.ObjectType.Point:
                    break;
                case Rhino.DocObjects.ObjectType.PointSet:
                    break;
                case Rhino.DocObjects.ObjectType.Curve:
                    goo.CastTo(out Curve crv);

                    DiagramCurve dCurve = DiagramCurve.Create(crv, DefaultColor, DefaultLineWieght);
                    m_Objects.Add(dCurve);
                    break;
                case Rhino.DocObjects.ObjectType.Surface:
                    goo.CastTo(out Surface srf);

                    AddBrep(tolernace, srf.ToBrep());
                    break;
                case Rhino.DocObjects.ObjectType.Brep:

                    goo.CastTo(out Brep brep);
                    AddBrep(tolernace, brep);

                    break;
                case Rhino.DocObjects.ObjectType.Mesh:
                    break;
                case Rhino.DocObjects.ObjectType.Light:
                    break;
                case Rhino.DocObjects.ObjectType.Annotation:
                    break;
                case Rhino.DocObjects.ObjectType.InstanceDefinition:
                    break;
                case Rhino.DocObjects.ObjectType.InstanceReference:
                    break;
                case Rhino.DocObjects.ObjectType.TextDot:
                    break;
                case Rhino.DocObjects.ObjectType.Grip:
                    break;
                case Rhino.DocObjects.ObjectType.Detail:
                    break;
                case Rhino.DocObjects.ObjectType.Hatch:

                    break;
                case Rhino.DocObjects.ObjectType.MorphControl:
                    break;
                case Rhino.DocObjects.ObjectType.SubD:
                    break;
                case Rhino.DocObjects.ObjectType.BrepLoop:
                    break;
                case Rhino.DocObjects.ObjectType.PolysrfFilter:
                    break;
                case Rhino.DocObjects.ObjectType.EdgeFilter:
                    break;
                case Rhino.DocObjects.ObjectType.PolyedgeFilter:
                    break;
                case Rhino.DocObjects.ObjectType.MeshVertex:
                    break;
                case Rhino.DocObjects.ObjectType.MeshEdge:
                    break;
                case Rhino.DocObjects.ObjectType.MeshFace:
                    break;
                case Rhino.DocObjects.ObjectType.Cage:
                    break;
                case Rhino.DocObjects.ObjectType.Phantom:
                    break;
                case Rhino.DocObjects.ObjectType.ClipPlane:
                    break;
                case Rhino.DocObjects.ObjectType.Extrusion:
                    goo.CastTo(out Extrusion ext);

                    AddBrep(tolernace, ext.ToBrep());
                    break;
                case Rhino.DocObjects.ObjectType.AnyObject:
                    break;
                default:
                    break;
            }
        }

        private void AddBrep(double tolernace, Brep brep)
        {
            m_Objects.AddRange(DiagramFilledCurve.CreateFromBrep(brep, DefaultColor, true, DefaultColor, DefaultLineWieght));
        }



        public BoundingBox GetGeometryBoundingBox()
        {
            Plane pl = Plane.WorldXY;
            return new Rectangle3d(pl, m_width, m_height).ToNurbsCurve().GetBoundingBox(false);
        }

        public Size GetBoundingSize(double scale)
        {
            BoundingBox bb = GetGeometryBoundingBox();
            int width = (int)((bb.Max.X - bb.Min.X) * scale);
            int height = (int)((bb.Max.Y - bb.Min.Y) * scale);
            return new Size(width, height);
        }


        public Bitmap DrawBitmap(double scale) // be careful all the Y dimentions need to be be subtracted from the the hieght at this is drawn upside down
        {
            //TODO DrawBitmap needs to pass on the scale and draw accordingly
            Size sz = GetBoundingSize(scale);

            Bitmap btm = new Bitmap(sz.Width, sz.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);



            using (var graphics = Graphics.FromImage(btm))
            {
                graphics.TranslateTransform(-m_Location.X, -m_Location.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
                graphics.FillRectangle(Brushes.White, new RectangleF(0, 0, this.m_width, this.m_height));// text displays badly without this, if no background is set
                m_Background.DrawBitmap(graphics);
                foreach (DiagramObject obj in m_Objects)
                {
                    obj.DrawBitmap(graphics);

                }


            }

         btm.RotateFlip(RotateFlipType.RotateNoneFlipY);


            return btm;

        }


        public void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolernace, Transform xform, bool colorOverride)
        {
            if (m_Location != PointF.Empty) {
                xform = Transform.Multiply(xform,Transform.Translation(new Vector3d(-m_Location.X, -m_Location.Y, 0)));
            }

            m_Background.DrawRhinoPreview(pipeline, tolernace, xform, colorOverride);

            if (m_Title != null)
            {
                PointF pt = new PointF(0, this.m_height);
                DiagramText title = DiagramText.Create(m_Title, pt, Color.Black, this.m_width / 20, TextJustification.BottomLeft, Color.White, Color.Black, 1f, false, "Arial", new SizeF(-1, -1), 3, TextJustification.BottomLeft);
                title.DrawRhinoPreview(pipeline, tolernace, xform, colorOverride);
            }

            foreach (DiagramObject obj in m_Objects)
            {
                obj.DrawRhinoPreview(pipeline, tolernace, xform, colorOverride);

            }



        }



    }
}
