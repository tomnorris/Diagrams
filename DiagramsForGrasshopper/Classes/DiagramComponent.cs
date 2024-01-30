﻿using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;


namespace DiagramsForGrasshopper
{
    public abstract class DiagramComponent : ReportBaseComponent

    {


        protected bool m_VersionChecked = false;
        protected  Transform m_Xform = Transform.ZeroTransformation;
      protected  DiagramLibrary.Diagram m_Diagram = null;

        public DiagramComponent(string a, string b, string c, string d, string e)
           : base(a, b, c, d, e) // pass through the GH_Comp
        {
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            pManager.AddGenericParameter("Diagram Objects", "DObjs", "The Diagram Objects Created by this componant", GH_ParamAccess.list);

        }

       

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            m_Diagram = null;
            m_Xform = Transform.ZeroTransformation;
            this.CheckLibraryVersion(DA);


            m_Diagram = DiagramSolveInstance(DA);



            DA.SetData(1, m_Diagram);
        }

        public virtual DiagramLibrary.Diagram DiagramSolveInstance(IGH_DataAccess DA) { return null; }

        

        public override BoundingBox ClippingBox
        {

            get
            {
              
                if (m_Diagram != null) { return m_Diagram.GetGeometryBoundingBox(); } else { return BoundingBox.Empty; }
            }
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
          
            if (m_Diagram == null) { return; }

            if (!this.Locked)
            {

                m_Diagram.DrawRhinoPreview(args.Display, GH_Component.DocumentTolerance(), m_Xform, this.m_attributes.Selected);



            }
        }




        private void CheckLibraryVersion(IGH_DataAccess DA)
        {
            if (m_VersionChecked == false)
            {
                string libraryVersion = DiagramLibrary.Diagram.LibraryVersion();
                string componantLibraryVersion = "1.0.0.0";
                VersionComparision compareVersion = CompareVersion(libraryVersion, componantLibraryVersion);
                switch (compareVersion)
                {

                    case VersionComparision.VersionMatch:

                        m_VersionChecked = true;

                        break;

                    case VersionComparision.LibraryVersionIsNewer:
                        this.AddUsefulMessage(DA, "ERROR01: The Diagrams Library (.dll) currently loaded in grasshopper is newer than the library this component was built against therefore results might be unpredicatble, error prone and newer features will be ignored. Check for an update on Food for Rhino or the Package Manager. Library Version: "+ libraryVersion + ", Componant was Built Against Version:" + componantLibraryVersion, GH_RuntimeMessageLevel.Warning);
                        break;
                    case VersionComparision.ComponantVersionIsNewer:
                        this.AddUsefulMessage(DA, "ERROR02: This Component was built against a newer version of the Diagrams Library (.dll) than the one currently loaded in grasshopper therefore results might be unpredicatble, error prone and newer features will be ignored. Check for an update on Food for Rhino or the Package Manager for any packages that create diagrams. Library Version: " + libraryVersion + ", Componant was Built Against Version:" + componantLibraryVersion, GH_RuntimeMessageLevel.Warning);
                        break;
                }




            }
        }

        private VersionComparision CompareVersion(string libraryVersion, string componantVersion)
        {
            string[] libraryVersionSplit = libraryVersion.Split('.');
            string[] componantVersionSplit = componantVersion.Split('.');

            for (int i = 0; i < libraryVersionSplit.Length; i++)
            {
                if (i > componantVersionSplit.Length)
                {
                    return VersionComparision.LibraryVersionIsNewer;
                }
                int libraryInt = Int32.Parse(libraryVersionSplit[i]);
                int componantInt = Int32.Parse(componantVersionSplit[i]);

                if (libraryInt > componantInt)
                {
                    return VersionComparision.LibraryVersionIsNewer;
                }

                if (libraryInt < componantInt)
                {
                    return VersionComparision.ComponantVersionIsNewer;
                }

            }

            return VersionComparision.VersionMatch;

        }

        private enum VersionComparision
        {
            VersionMatch,
            LibraryVersionIsNewer,
            ComponantVersionIsNewer
        }





    }

}
