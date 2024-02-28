using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{

  
    public class TextModifiers : ModifiersBase
    {
        private int m_ItemCount;
        public override string Name { get { return "+ Text Modifiers"; } }


        public double TextScale = Diagram.DefaultTextScale;
        public string Font = Diagram.DefaultFontName;
        public double TextPadding = Diagram.DefaultPadding;
        private int m_TextJustificationInt = 0;
        public TextJustification TextJustification = TextJustification.BottomLeft;
        public Color TextColor = Diagram.DefaultColor;
        public Color TextBackgroundColor = Color.Transparent;
        public Color TextBorderColor = Diagram.DefaultColor;
        public double TextBorderLineweight = 0;



        public override int ItemCount { get { return m_ItemCount; } }
        public TextModifiers(bool addTextScale, bool addFont,bool addPadding, bool addJustification, bool addTextColour, bool addTextBackgroundColor,bool addTextBorderColor, bool addTextBorderLineWieght)
        {
            if (addTextScale)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_Number(), "TextScale", "TxtScale", "The optional scale of the Text in the Diagram Object") as Grasshopper.Kernel.Parameters.Param_Number;
                new_param.PersistentData.Append(new GH_Number(TextScale));
                m_Params.Add(new_param);
                m_ItemCount++;
            }
         
            if (addFont)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_String(), "Font", "Font", "The optional font of the Text in the Diagram Object") as Grasshopper.Kernel.Parameters.Param_String;
                new_param.PersistentData.Append(new GH_String(Font));
                m_Params.Add(new_param);
                m_ItemCount++;
            }

            if (addPadding)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_Number(), "TextPadding", "Padding", "The optional padding of the Text in the Diagram Object") as Grasshopper.Kernel.Parameters.Param_Number;
                new_param.PersistentData.Append(new GH_Number(TextPadding));
                m_Params.Add(new_param);
                m_ItemCount++;
            }
            if (addJustification)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_Integer(), "TextJustification", "Justification", "The optional justification of the Text in the Diagram Object. Horizontal Justifications (Left, Center, Right) only take effect if Width is set, Vertical Justifications (Top, Middle, Bottom) only take effect if Height it set. Use and Interger from 0 to 10 where 0: \n Bottom Left, 1: Bottom Center, 2: Bottom Right \n 3: Middle Left, 4: Middle Center, 5: Middle Right \n 6: Top Left, 7: Top Center, 8: Top Right") as Grasshopper.Kernel.Parameters.Param_Integer;
                new_param.PersistentData.Append(new GH_Integer(m_TextJustificationInt));
                m_Params.Add(new_param);
                m_ItemCount++;
            }
            if (addTextColour)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_Colour(), "TextColor", "TxtColor", "The optional color of the Text in the Diagram Object") as Grasshopper.Kernel.Parameters.Param_Colour;
                new_param.PersistentData.Append(new GH_Colour(TextColor));
                m_Params.Add(new_param);
                m_ItemCount++;
            }
            if (addTextBackgroundColor)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_Colour(), "TextBackgroundColor", "TxtBackColor", "The optional background color of the Text in the Diagram Object") as Grasshopper.Kernel.Parameters.Param_Colour;
                new_param.PersistentData.Append(new GH_Colour(TextBackgroundColor));
                m_Params.Add(new_param);
                m_ItemCount++;
            }
            if (addTextBorderColor)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_Colour(), "TextBorderColor", "TxtBdrColor", "The optional border color of the Text in the Diagram Object. For the Border will only draw if the TextBorderLineweight is greater than zero.") as Grasshopper.Kernel.Parameters.Param_Colour;
                new_param.PersistentData.Append(new GH_Colour(TextBorderColor));
                m_Params.Add(new_param);
                m_ItemCount++;
            }
            if (addTextBorderLineWieght)
            {
                var new_param = ModifiersBase.CreateParam(new Grasshopper.Kernel.Parameters.Param_Number(), "TextBorderLineweight", "TxtBdrLnweight", "The optional border line weight of the Text in the Diagram Object. For the Border will only draw if the value is greater than zero.") as Grasshopper.Kernel.Parameters.Param_Number;
                new_param.PersistentData.Append(new GH_Number(TextBorderLineweight));
                m_Params.Add(new_param);
                m_ItemCount++;
            }


        }

        public override void GetValues(IGH_DataAccess DA)
        {
            if (this.HasBeenAdded)
            {
                DA.GetData("TextScale", ref TextScale);
                DA.GetData("Font", ref Font);
                DA.GetData("TextPadding", ref TextPadding);
                DA.GetData("TextJustification", ref m_TextJustificationInt);
                DA.GetData("TextColor", ref TextColor);
                DA.GetData("TextBackgroundColor", ref TextBackgroundColor);
                DA.GetData("TextBorderColor", ref TextBorderColor);
                DA.GetData("TextBorderLineweight", ref TextBorderLineweight);

                switch (m_TextJustificationInt)
                {
                    case 0:
                        TextJustification = TextJustification.BottomLeft;
                        break;
                    case 1:
                        TextJustification = TextJustification.BottomCenter;
                        break;
                    case 2:
                        TextJustification = TextJustification.BottomRight;
                        break;
                    case 3:
                        TextJustification = TextJustification.MiddleLeft;
                        break;
                    case 4:
                        TextJustification = TextJustification.MiddleCenter;
                        break;
                    case 5:
                        TextJustification = TextJustification.MiddleRight;
                        break;
                    case 6:
                        TextJustification = TextJustification.TopLeft;
                        break;
                    case 7:
                        TextJustification = TextJustification.TopCenter;
                        break;
                    case 8:
                        TextJustification = TextJustification.TopRight;
                        break;
                    default:
                        // Use default values
                        TextJustification = TextJustification.BottomLeft;
                        break;
                }

            }
        }
    }


}
