using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace DiagramsForGrasshopper
{

  

    public abstract class ModifiersBase
    {
        protected List<IGH_Param> m_Params = new List<IGH_Param>();

        public abstract string Name { get; }
        public abstract int ItemCount { get; }
        public bool HasBeenAdded = false;

        public void AddModifiers(GH_ComponentParamServer Params) {

            for (int i = 0; i < m_Params.Count; i++)
            {
               Params.RegisterInputParam(m_Params[i]);
            }
            HasBeenAdded = true;

        }


        public abstract void GetValues(IGH_DataAccess DA, IGH_Component componant);


        public static IGH_Param CreateParam(IGH_Param baseParam, string Name, string NickName, string Description)
        {
            baseParam.Name = Name;
            baseParam.NickName = NickName;
            baseParam.Description = Description;
            baseParam.Access = GH_ParamAccess.item;
            baseParam.Optional = true;

            return baseParam;

        }


       

       





    }



}
