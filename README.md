# zMisc.Unity 
### (miscellaneous script collection and their decrtiptions)
This repository is a random collection of snippets I use for various projects. This repository is mainly hosting the document you are reading right now, in case some one ever finds a file of unknown purpose and tracks is back here - where descriptions of each script are provided.
Note that it won't necesarily be the source of the lastest version of each script (they tend to get updated as need arises in anotether parts of the zUtils codebase


# Selection Bookmarks
![selection screenshot](/Screenshots/selectionBookmarks.png?raw=true?raw=true)

An EditorWindow that lets you remember a number of gameObjects and quickly switch active selection between them. You can also Ping (highlight in hierarchy without selection) a given object to make sure this is the one you want to select, rearrange favourites, and toggle gameobjects on and off

## Select objects named the same
![selection screenshot](/Screenshots/selectObjectsNamedTheSame.png?raw=true?raw=true)

How many times you had to make the same change to dozens od objects somewhere in the hierarchy tree, that happened to be called exactly the same. Too many times. This simple tool crawls the hierarchy in the editor and selects objects named the same, like Text or Image or Button, up to a desired depth up the tree (not to select ALL THE objects, but only ones within a given depth limit from currently selected

## RectExtensions / zExtensons
A large and fast growing collection of extension methods, out of which probably the most useful ones are for working with RectTransform (RectTransform extensions), theres shortcuts for setting just one dimension x/y for a rect - setSizeX, setLocalX, setRelativeLocalY, setRelativeSizeX (relative ones take 0-1 values and take into account the parent rect size), theres a whole set of Anchor setting methods (the min/max convention used in unity just consfues me so theres setAnchorRight, setAnchorLeft, setAnchorX, setPivotX etc
theres  GameObject[] getChildren, Transfom.RemoveChildren(), Color.Random(), Color.Alpha(float), rectTransform.AddChild (will make sure it streches), and some other stuff, it just grows

## SafeSlider
If you just control something with a slider, Unity is just fine, but when you want to use the same slider to both display current value (so when a value changes in code, it gets updated on a slider), it tends to create nasty feedback loops (resulting in stack overflows), where something changes the slider, but moving the slider triggers the state change, that triggers the slider change etc.
Safe slider mutes its recieving input just for the time the event is sent - this means its safe to use the same slider to both control and display a value.


# Hierarchy tools

![hierarchytools](/Screenshots/hierarchyTools.png?raw=true?raw=true)

I am not a great fan of some default behavious of UnityEditor, for exapmle why does duplicating an object moves it to the bottom of the current hierarchy level (now you can duplicate in place - leaving the clone as next sibling to active object), there's plaste in place (keeps localposition instead of absolute position), create a duplicate of the object but without its children, hiding objects from a given layer in hierarchy (via HideFlags), Duplicate in place, Paste as child, create Image child, 

# Event Extensions
An ever growing collection of smail tools meant to ease the proces of wiring an event network in Unity Editor without having to create a new file for evry tiny bit of functionality. 

![event_extensions screenshot](/Screenshots/EventExtensions.png?raw=true?raw=true)

Starting from a set of UnityEvent<type>() definitons, such as FloatEvent (an equivalent of for example SliderEvent), StringEvent (useful to distribute strings) etc, we move on to some basic logic functionality (like EventGate, which triggers either one event, or another, based on some logic state, that is of course changable via other events), touching some very useful tiny scripts that take an integer (or a float which is rounded to int) and if they have a children wich such sibling number, it gets activated, while other children get deactivated), ending on quite flexible Event Processor, which basically performs a y=(x+a)*b+c operation of floats, with a b d c settable via function calls, so you can take for example a slider value, add 0.1f to it, multiply by 0.9f and trigger another FloatEvent with a processed value.

![event_extensions screenshot](/Screenshots/EventTools.png?raw=true?raw=true)

The list is likely to grow as I keep using them in projects.


# Range.cs
![Range](/Screenshots/Range.png?raw=true?raw=true)

A simple class with a CustomPropertyDrawer for setting up ranges. Supports dragging the range, dragging in/out handles.

## Timeline.cs
Timeline plus a set of helper utilities is a way to incorporate functionality related to playing back time - from obvious support of loops, setting speeds, handling both loose loop ands and constant loop duration scenario, it can both relay its current time via global static getter (double or single precision), and/or trigger UntiyEvents when new time is reached. It also has some support for displaying such timelines, offering public events that spread new pixel per second ratios for elements that need to scale.


## Layout helpers
Creates a vertical or horitontal layout with some sample elements (Image with random color), adding Layout Elements to sections, makins sure layout group respects them. Groundwork for a future autolayout graphics helper .


## zCameraInspectorHelper.cs
This is really a hack, not a script per say, it gives you 7 sliders for controlling camera position, rotation, fov and distance from point of interest, and resets them to middle position as soon as you move them. The difference is forwarded to transform of the camera. This enables very simple fuss-free positioning of a camera (Framing) without using brutal methods like Transform inspector, or moving it manually in scene view.
The point is - this is not runtime, this is editor time, but without a slightest reference to UnityEditor, scriptableObjects etc. It doesn't even need to [ExecuteInEditMode]

![zcamera screenshot](/Screenshots/cameraInspector.png?raw=true?raw=true)

## zCameraController.cs
still work in progress (no map edge detection/ wall detection) but a pretty functional stab at a camera controller script, with middle mouse button movement, scroll movement, rotation around a pivot in front of you.

## TimeRamp.cs

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



## zKeyMap
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

## zScrollRect.cs

Inheriting from Unity ScrollRect it disables the drag behaviour (I hate it), finds its own slider, and has some minor improvements

## BenchmarkClones.cs

Clone a gameobject n times, wait for k frames and display FPS. super simple

## zNode and zNodeController.cs

I noticed that for a lot of project I am instancing UI objects from templates, that end up in a horizontalLayoutGroup. This is just an attempt to limit boilerplate each time I am doing this. 
It provides basic templating functionality - it tries to populate a list of templates based on objects that inherit from zNode in its children, creates a dictionary of them, handles instancing, parenting, and few other misc things.

## zHoverColorProvider.cs
Got tired of scripting color picking logic for UI elements. IColorProvider is an interface providing four basic colours (neutral, hovered, active, disabled), and a callback Action when neutral is changed (via OnValidate) to enable live preview.
The idea is that child objects can look for any MonoBehaviour implementing IColorProvier and 'synchronize' their colors


## SimpleRotate.cs

Rotates stuff with constant motion. The HelloWorld script of unity but I wrote it one too many times, its on github now

## zResourceLoader.cs
static classes to load resources that I use for other scripts, like cursors

