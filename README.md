# Diagrams
An Open Source digram library for Grasshopper and Rhino.

# Why do we need diagrams?
There are two main goals with this project:
1) There are times when we don't want to leave the grasshopper canvas to see information which may be generated inside a complex componant. In this senario having an image drawn on canvas can reveal information which otherwise would need displaying within the rhino space which potentially cluttering up the viewport. An example is a diagram which shows the build up of a wall's materials for a UValue calculation.
2) The second is to enable repeatable diagrams within the Rhino World Space which is useful when creating reports and layouts.

# What are the Constrains?
Any diagram object must be reproducable both in the System.Drawing.Graphic Library and also the Rhino.Display.DisplayPipeline

# What Objects are currently supported?

- **Curves** Any Rhino/ Grasshopper Curve, for the canvas view these will be converted to polylines
- **Filled Curves aka Solid Hatches**  If a Brep is input then a filled curve is created for each face, if a curve is added it will be filled in solid.
- **Images**
- **Text**
- **Tables**

This is an open source project and contributors are welcome!



