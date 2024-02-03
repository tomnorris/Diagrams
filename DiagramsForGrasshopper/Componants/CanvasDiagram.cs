using System;
using System.Collections.Generic;
using System.Reflection;
using Grasshopper.Kernel;
using Rhino.Geometry;
using DiagramLibrary;
using System.Windows.Forms;

namespace DiagramsForGrasshopper
{
    public class CanvasDiagram : GH_Component
    {


       // public Diagram m_Diagram = null;
        public double Scale = 1;
        public bool Update = true;
        public System.Drawing.Bitmap Bitmap = null;



        /// <summary>
        /// Initializes a new instance of the CanvasDiagram class.
        /// </summary>
        public CanvasDiagram()
          : base("CanvasDiagram", "Nickname",
              "Description",
               "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Diagram", "D", "Diagram to display inside this componant", GH_ParamAccess.item);
          //  this.Params.Input[0].ObjectChanged += RhinoDiagram_ObjectChanged;
        }
         

       
    

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
           // base.RegisterOutputParams(pManager);
            pManager.AddGenericParameter("Diagram", "D", "The Diagram in preview passed through", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Update = true;
            Diagram diagram = null;


            if (!DA.GetData(0, ref diagram))
            {
                return;
            }

            if (diagram == null)
            {
               
                return;
            }

           

            DA.SetData(0, diagram);
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Save", SaveHandler, true,false);

            var setScale = Menu_AppendItem(menu, "Set Number");
            Menu_AppendTextItem(setScale.DropDown, Scale.ToString(System.Globalization.CultureInfo.InvariantCulture), null, TextChanged, true);
        }


        private void TextChanged(Grasshopper.GUI.GH_MenuTextBox sender, string newText)
        {
            try
            {
                Scale = Convert.ToDouble(newText);
                ExpireSolution(true);
            }
            catch (Exception)
            {

                Scale = 1;
                ExpireSolution(true);
            }
           
        }

    

        private void SaveHandler(object sender, EventArgs e)
        {
            Rhino.UI.SaveFileDialog saveFileDialog = new Rhino.UI.SaveFileDialog();
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.Filter = "Image files (*.png)|*.*";
            saveFileDialog.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png";
           
            saveFileDialog.Title = "Save a file";
            if (saveFileDialog.ShowSaveDialog())
            {
                string filename = saveFileDialog.FileName;
                this.Bitmap.Save(filename);
            }
        }




        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8e8652ba-890d-431a-9306-68b0a041d4ef"); }
        }


        public override void CreateAttributes()
        {
            base.CreateAttributes();
            m_attributes = new DiagramComponentAttibutes(this);

        }


        public override GH_Exposure Exposure

        {

            get { return GH_Exposure.primary; }

        }

    }
}