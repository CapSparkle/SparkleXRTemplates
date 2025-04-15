# SparkleXRTemplates

__! The project development is stoped at 2021 !__
<br />
__! You should consider it as a proof of concept or a prototype without any prooven stability !__ 


<br />

This framework for Unity XR development:
- Providing modules truly usable out-of-the box. Which are not ones (from well-known frameworks) with a bunch of responsibilities and thousands of codelines to be rewritten for each project.  
- Having flexible architecture simple to extend
- Maintaining at least sequence cohesion and data structure coupling for all modules.
- Offering modern comfortable UX leveraging advanced* selection system

Example presented in Assets/SparkleXR/SparkleXRTemplates/Examples/Template. 
<br />
This should be run using it with MagicLeap or with MagicLeap Lab Simulator

draw.io UML class diagrams:
- for core library modules
![image](https://github.com/user-attachments/assets/1db5764f-657f-4b1c-859d-96db934a3146)

https://drive.google.com/file/d/1lsO_GHqIjDzBl5lddKW1vuFHeGU6Kvwp/view?usp=sharing

- for MagicLeap astra miner game example
https://drive.google.com/file/d/1lL2xttva5YtjcTJ-lUWvRUfO_8NbFa3v/view?usp=sharing


## Advanced selection system
Advanced selection system allows to combine several XR input sources to create comfortable user experience
<br />
Scheme #1 Foreground noise-resistant selection
![Example_1](https://github.com/CapSparkle/SparkleXRTemplates/assets/25351821/a8ff7ef7-61bb-440f-9b86-ad3c1c70c858)
<br />
This algorithm "understands" users intentions using both her eyes focus point and hand pointing direction. In space with tight interactive object placement simple raycast selecting algoritm tend to select the several objects to interact with. But high priority space by sight focus point resolves ambiguosity
<br />
<br />
Scheme #2 Accuracy-asisntant selection
![Example_2](https://github.com/CapSparkle/SparkleXRTemplates/assets/25351821/600578e9-ba32-44f2-bbbe-e7865f2af94c)
Algorithm decreases requirements for pointing accuracy using additional spherecast selector for hand finger. If there is no objects selected by ray ("finger_1" selector) it checks if there any objects selected by "finger_2". And it also prioritises objects from "high priority space" by eye focus point.
<br />
<br />
In the following example green cube is selected. 
![Illustrate](https://github.com/CapSparkle/SparkleXRTemplates/assets/25351821/7e28d9ea-7cdf-49e8-808f-0801b90933e8)
<br />
User have missed to point straight at it, but thanks to eye focus point data (visualized as sphere on screenshot) and additional "finger_2" selector it is stil selected. We don't expect amazing accuracy from the user - we try to understand him instead! 
<br />
<br />
You can configure your own selecting system creating logical rule (Disjunctive normal form) for aggregating selecting results from several selectors <br /> (You also can have as much selectors as you want and configure them with their own selecting predicates)
![unnamed](https://github.com/CapSparkle/SparkleXRTemplates/assets/25351821/14a82102-f865-4de6-bc63-3a5a57c9ba66)
<br />
For example here we see the followng configuration:
- Check if there is something selected by HandRaycast and EyeGaze
- Then check if there is something selected only by HandRaycast
- Then check if there is something selected by HandRaycast and EyeGaze selector
<br />
If at any step there will be selected object it will be choosen to interact with (prioritizing first rule over second, second over third and so on)
