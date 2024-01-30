This framework for Unity XR development:
- Providing modules truly usable out-of-the box. Which are not ones (from well-known frameworks) with a bunch of responsibilities and thousands of codelines to be rewritten for each project.  
- Having flexible architecture simple to extend
- Maintaining at least sequence cohesion and data structure coupling for all modules.
- Offering modern comfortable UX leveraging advanced* selection system

# SparkleXRTemplates
Example presented in Assets/SparkleXR/SparkleXRTemplates/Examples/Template. 
<br />
This should be run using it with MagicLeap or with MagicLeap Lab Simulator

draw.io UML class diagrams:
- for core library modules
https://drive.google.com/file/d/1lsO_GHqIjDzBl5lddKW1vuFHeGU6Kvwp/view?usp=sharing

- for MagicLeap astra miner game example
https://drive.google.com/file/d/1lL2xttva5YtjcTJ-lUWvRUfO_8NbFa3v/view?usp=sharing

Advanced selection system allows to combine several XR inputs to create comfortable user experience
![Example_1](https://github.com/CapSparkle/SparkleXRTemplates/assets/25351821/0d6751a3-aa61-4a14-8cf8-75341888e058)
<br />
This algorithm understands users intentions using both her eyes focus point and hand pointing direction. In space with tight interactive object placement simple raycast selecting algoritm tend to select the several objects to interact with. But high priority space by sight focus point resolves ambiguosity
