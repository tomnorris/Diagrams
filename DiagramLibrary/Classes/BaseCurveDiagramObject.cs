using System;
using System.Drawing;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;

namespace DiagramLibrary

    {


    public abstract class BaseCurveDiagramObject : DiagramObject // Objects that dreive from this class can be used as DiagramCurveEnds
    {




        public abstract BaseCurveDiagramObject SetLocationAndDirectionForDrawing(Point3d basePoint, Vector3d baseDirection, Point3d location, Vector3d rotation);

      
        
    }
}