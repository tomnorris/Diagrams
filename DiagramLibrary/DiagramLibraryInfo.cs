﻿using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace DiagramLibrary
{
    public class DiagramLibraryInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "DiagramLibrary";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("5287dd55-f3d2-4d0d-97cf-0c90bd1c81a5");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
