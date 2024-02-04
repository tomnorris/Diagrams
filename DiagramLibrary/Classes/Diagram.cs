using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class Diagram
    {
        public static Color DefaultColor = Color.Black;
        public static float DefaultLineWeight = 1f;
        public static float DefaultTextScale = 10f;
        public static Color SelectedColor = Color.FromArgb(128, Color.ForestGreen);

        private PointF m_Location;
        private int m_Width;
        private int m_Height;
        private DiagramText m_Title;
        private string m_TitleFont = "Arial";
      
        private Color m_BackgroundColour;
        private Color m_FrameColour;
        private float m_FrameLineWeight;
    

        private List<DiagramObject> m_Objects;


        static public string LibraryVersion()
        {
            return typeof(Diagram).Assembly.GetName().Version.ToString();
        }



        public DiagramText Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }


        public List<DiagramObject> Objects
        {
            get { return m_Objects; }
            
        }



        public Diagram Duplicate()
        {
            Diagram diagram = new Diagram();
            diagram.m_Width = m_Width;
            diagram.m_Height = m_Height;
            diagram.m_Objects = m_Objects;
            diagram.m_FrameColour = m_FrameColour;
            diagram.Title = m_Title;
            diagram.m_TitleFont = m_TitleFont;
            diagram.m_FrameColour = m_FrameColour;
            diagram. m_FrameLineWeight = m_FrameLineWeight;


            return diagram;
        }

        public Diagram() { }


        public static Diagram Create(int width, int height, DiagramText title, Color backgroundColour,float frameLineWeight, Color frameColour)
        {
                      return Create( width,  height, title,  backgroundColour, frameLineWeight, frameColour, new PointF(0,0) );
        }


        //Do not use the location to set the diagram location in the RhinoPreview, use the xfrom in the DrawinRhinoPreview method, the diagram location is used to compensate for single object diagram's locations
        public static Diagram Create(int width, int height, DiagramText title, Color backgroundColour, float frameLineWeight, Color frameColour, PointF location)
        {
            Diagram diagram = new Diagram();
            diagram.m_Width = width;
            diagram.m_Height = height;
            diagram.m_Objects = new List<DiagramObject>();
            diagram.m_Title = title;
            diagram.m_BackgroundColour = backgroundColour;
            diagram.m_FrameColour = frameColour;
            diagram.m_FrameLineWeight = frameLineWeight;


            diagram.m_Location = location;
            return diagram;
        }

        public void AddObjects(Grasshopper.Kernel.GH_Component component, List<object> objs, double tolernace)
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
                    AddDiagramObjectFromGoo(component,objs[i]);

                }



            }

        }


        public void AddDiagramObject(DiagramObject obj)
        {

            m_Objects.Add(obj);
        }


        public void AddDiagramObjectFromGoo(Grasshopper.Kernel.GH_Component component, object obj)
        {
            if (obj == null) {
                component.AddRuntimeMessage(Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning, "Cannont add null object to diagram");
                return;
            }

            var goo = obj as Grasshopper.Kernel.Types.IGH_Goo;


            goo.CastTo(out Diagram diagram);
            if (diagram == null) { return;  }
            if (diagram.m_Objects.Count == 0) { return;  }
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

                    DiagramCurve dCurve = DiagramCurve.Create(crv, DefaultColor, DefaultLineWeight);
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
            m_Objects.AddRange(DiagramFilledCurve.CreateFromBrep(brep, DefaultColor, DefaultColor, DefaultLineWeight));
        }



        public BoundingBox GetGeometryBoundingBox()
        {
            Plane pl = Plane.WorldXY;
            return new Rectangle3d(pl, m_Width, m_Height).ToNurbsCurve().GetBoundingBox(false);
        }

        public Size GetBoundingSize(double scale)
        {
            BoundingBox bb = GetGeometryBoundingBox();
            int width = (int)((bb.Max.X - bb.Min.X) * scale);
            int height = (int)((bb.Max.Y - bb.Min.Y) * scale);
            return new Size(width, height);
        }


        private DiagramFilledRectangle GetBackground() {
            Plane plane = Plane.WorldXY;
            plane.Origin = new Point3d(m_Location.X, m_Location.Y, 0);
            Rectangle3d rec = new Rectangle3d(plane, m_Width, m_Height);
          return DiagramFilledRectangle.Create(rec, m_BackgroundColour, m_FrameColour, m_FrameLineWeight);

        }

        public Bitmap DrawBitmap(Grasshopper.Kernel.GH_Component component, double scale) // be careful all the Y dimentions need to be be subtracted from the the hieght at this is drawn upside down
        {
            //TODO DrawBitmap needs to pass on the scale and draw accordingly
            Size sz = GetBoundingSize(scale);

            Bitmap btm = new Bitmap(sz.Width, sz.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);



            using (var graphics = Graphics.FromImage(btm))
            {
                graphics.TranslateTransform(-m_Location.X, -m_Location.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
                graphics.FillRectangle(Brushes.White, new RectangleF(0, 0, this.m_Width, this.m_Height));// text displays badly without this, if no background is set
                GetBackground().DrawBitmap(component,graphics);
                foreach (DiagramObject obj in m_Objects)
                {
                    obj.DrawBitmap(component,graphics);

                }


            }

         btm.RotateFlip(RotateFlipType.RotateNoneFlipY);


            return btm;

        }


        public void DrawRhinoPreview(Grasshopper.Kernel.GH_Component component, Rhino.Display.DisplayPipeline pipeline, double tolernace, Transform xform, bool colorOverride)
        {
            if (m_Location != PointF.Empty) {
                xform = Transform.Multiply(xform,Transform.Translation(new Vector3d(-m_Location.X, -m_Location.Y, 0)));
            }

            GetBackground().DrawRhinoPreview(component,pipeline, tolernace, xform, colorOverride);

            if (m_Title != null)
            {
                PointF pt = new PointF(0, this.m_Height);
                DiagramText title = m_Title;
title.Location = pt;
                if (title.TextSize < 0) {
                    title.TextSize = this.m_Width / 20;
                }
                  
                title.DrawRhinoPreview(component,pipeline, tolernace, xform, colorOverride);
            }

            foreach (DiagramObject obj in m_Objects)
            {
                obj.DrawRhinoPreview(component,pipeline, tolernace, xform, colorOverride);

            }



        }



    }
}
