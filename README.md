# Diagrams
An Open Source digram library for Grasshopper and Rhino.

## Why do we need diagrams?
There are two main goals with this project:
1) There are times when we don't want to leave the grasshopper canvas to see information which may be generated inside a complex componant. In this senario having an image drawn on canvas can reveal information which otherwise would need displaying within the rhino space which potentially cluttering up the viewport. An example is a diagram which shows the build up of a wall's materials for a UValue calculation.
2) The second is to enable repeatable diagrams within the Rhino World Space which is useful when creating reports and layouts.

## What are the Constrains?
Any diagram object must be reproducable both in the System.Drawing.Graphic Library and also the Rhino.Display.DisplayPipeline

## The Two Halves
We want to encourage developers to ustilize this library within their own projects for all your diagram needs but we also want to give grasshopper users who are none devs the ability to build there own diagrams from componants.

-  `DiagramsLibrary` **-  For Developers** Include the Diagrams.dll within your project and write the logic for the diagrams within your componants, then you can use the `RhinoDiagram` and `CanvasDiagram` Componants from `DiagramsForGrasshopper` to view your diagrams.
-  `DiagramsForGrasshopper` **- For Grasshopper Users** This is a small libray of compnants which utilizle the DiagramsLibray so a Grasshopper user can build complex diagrams from strach. 

## What Objects are currently supported?

- **Curves** Any Rhino/ Grasshopper Curve, for the canvas view these will be converted to polylines
- **Filled Curves aka Solid Hatches**  If a Brep is input then a filled curve is created for each face, if a curve is added it will be filled in solid.
- **Images**
- **Text**
- **Tables**

This is an open source project and contributors are welcome!



