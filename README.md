# zMisc.Unity 
### (miscellaneous script collection and their decrtiptions)
This repository is a random collection of snippets I use for various projects. This repository is mainly hosting the document you are reading right now, in case some one ever finds a file of unknown purpose and tracks is back here - where descriptions of each script are provided.
Note that it won't necesarily be the source of the lastest version of each script (they tend to get updated as need arises in anotether parts of the zUtils codebase

# zCameraInspectorHelper.cs
This is really a hack, not a script per say, it gives you 7 sliders for controlling camera position, rotation, fov and distance from point of interest, and resets them to middle position as soon as you move them. The difference is forwarded to transform of the camera. This enables very simple fuss-free positioning of a camera (Framing) without using brutal methods like Transform inspector, or moving it manually in scene view.
The point is - this is not runtime, this is editor time, but without a slightest reference to UnityEditor, scriptableObjects etc. It doesn't even need to [ExecuteInEditMode]

![Alt text](/Screenshots/cameraInspector.png?raw=true?raw=true "Screenshot")
# zCameraController.cs
still work in progress (no map edge detection/ wall detection) but a pretty functional stab at a camera controller script

# TimeRamp.cs

TimeRamp is an simplistic animation utility I wrote a while ago and somehow end up using it pretty much everywhere.
Its basically a struct that holds a Time.time when an event was triggered, and a desired duration. It provides a timeramp.value getter which gives you normalized values in the 0-1 range, and a timeramp.isRunning bool which is false if last ramp played through (reaching 0 or 1) and true if the value is still chaning.

The idea was that it's very cheap to run in Update (just two bool checks per update if not running), typical use involves using AnimationCurve to shape the actual animation

>vois Update()<br/>
>{<br/>
> if (timeRamp.isRunning)<br/>
>  {<br/>
>  float mappedValue=myAnimationCurve.Evaluate(timeRamp.value);<br/>
>  transform.localScale=new Vector3(mappedValue,mappedValue,mappedValue);<br/>
> }<br/>
>}

its optimised to avod unnecesary operation (like getting current Time.time, dividing it by difference between start and end etc), provides a basic callback functionality, but primarly it can provide consistent behaviour when the direction of the animatino changes midway.
To trigger the start we run timeRamp.GoOne(), or timeRamp.GoZero() methods - imagine a panel that slides in and out of screen. If the animation is mid way through, and you changed your mind, timeRamp is your friend (the alternative would be to stop a coroutine, or deal with other stuff that some more advanved animation frameworks provide).
There are several helper methosd available like GoOneIn(float seconds), or JumpZero)



# zDragResizeRect.cs

UI utility that adds a panel on the selected side of the parent rect, and listens for drag events. When dragged, it resizes the panel with a typical 'windows' logic, with anchroing and pivoting based on where the user clicked.
A Bit more sophisticated class, able to control an array of dragResizables, zDraggable, has its own respository

Handles matching the pivots, makes sure the image is there, adds layoutelement with ignore layout just in case, checks if IColorProvider is available in parent, if targetRect is null it takes its parent rect etc

![Alt text](/Screenshots/dragResize.png?raw=true?raw=true "Screenshot")



# zKeyMap
Super tiny keyboard mapping utility - avoids checking for Input.GetKeyDown in Update, your mono will recieve a call when it happens
I hate having scripts on the payroll that do nothing but check for keyboard

so Use this

>void Start()
>   {
>      zKeyMap.map(this, method, mappedKey);
>   }

instead of the recommended method

>void Update()
>    {
>        if (Input.GetKeyDown(mappedKey)) method();
>    }
    
Heres a video demo
    
[![](http://img.youtube.com/vi/eSrLbbu6xas/0.jpg)](http://www.youtube.com/watch?v=eSrLbbu6xas)
  
Could probably use a bit more null conditions checking. 

# zRectExtensions

A few extension methods for RectTransform I was missing for a long time. Enables simple setting the piviot (as Vector2 - like you would with normal RectTransform) but also 
seperately for horizontal and vertical i.e. rect.setPivotX(0) setAnchorY(1) with a super sweet addition of not messing up currrent positioning (most of the time), so it behaves a bit like Inspector in the Editor

# zScrollRect.cs

Inheriting from Unity ScrollRect it disables the drag behaviour (I hate it), finds its own slider, and has some minor improvements

# BenchmarkClones.cs

Clone a gameobject n times, wait for k frames and display FPS. super simple

#SimpleRotate.cs

Rotates stuff with constant motion.

# zNode and zNodeController.cs

I noticed that for a lot of project I am instancing UI objects from templates, that end up in a horizontalLayoutGroup. This is just an attempt to limit boilerplate each time I am doing this. 
It provides basic templating functionality - it tries to populate a list of templates based on objects that inherit from zNode in its children, creates a dictionary of them, handles instancing, parenting, and few other misc things.

# zAnimateLayout.cs
work in progress - uses TimeRamp to animate UI properties

#zHoverColorProvider.cs
Got tired of scripting color picking logic for UI elements. IColorProvider is an interface providing four basic colours (neutral, hovered, active, disabled), and a callback Action when neutral is changed (via OnValidate) to enable live preview.
The idea is that child objects can look for any MonoBehaviour implementing IColorProvier and 'synchronize' their colors



# zResourceLoader.cs
static classes to load resources that I use for other scripts, like cursors







