using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Z;

[System.Serializable]
public class TimelineEventBlock
{
    public DoubleEvent timeDoublePrecition;
#region propertyblock
    [Header("Time")]
    public FloatEvent timeRelativeToDuration;
    public FloatEvent timeRelativeToLoopWindow;
    [Header("Loop")]
    // public FloatEvent loopInAbsoluteChangedTo;
    public FloatEvent loopInChangedTo;
    public FloatEvent loopOutChangedTo;
    public FloatEvent loopInAbsolueChangedTo;
    public FloatEvent loopOutAbsoluteChangedTo;
    public FloatEvent durationChangedTo;
    public FloatEvent speedChangedTo;
    public FloatEvent timeJumped;
    [Header("Valueless Triggers")]
    public UnityEvent pixelUnitOrDurationChanged;
    public VoidEvent started;
    public VoidEvent stopped;
    public BoolEvent playing;

    #endregion
}

[ExecuteInEditMode]
public class Timeline : MonoBehaviour
{

#region inspectorVars
      public static Timeline main;
    [Header("Playback")]

    bool _isPlaying;
    [SerializeField]
    bool _isPlayingset;
    [ReadOnly]
    [SerializeField]
    double _time;
    [SerializeField]
    bool _loop;
    public bool pingPong;
    public bool reverse;

    [Range(0, 1)]
    public double _timeRelativeToLoopWindow;
    [Range(0, 1)]
    public double _timeRelativeToDuration;

    public bool startOnPlay;

    [Range(0, 2)]
    public float speedSet = 1;

    [Header("Loop")]
    [Range(0, 1)]
    public float inPointSet;
    [Range(0, 1)]
    public float outPointSet = 1;
    [Range(0, 1)]
    public float loopWindowSet = 1;
    public bool lockloopWindow;
 

    [ReadOnly]
    [SerializeField]
    public double inPointScaled = 0;
    [ReadOnly]
    [SerializeField]
    public double outPointScaled = 1;
    [ReadOnly]
    [SerializeField]
    double loopWindowScaled;
    [SerializeField]
    [ReadOnly]
    double _loopWindow;
    double _duration = 1;

    float timeStep = 0.1f;


    [Header("Setup")]

    [SerializeField]
    float _unitPixelSize = 200;

    //float _unitPixelSize 
    public float durationSet;
    [Header("Events")]
    public FloatEvent timeChanged;
    public TimelineEventBlock events;


#endregion

#region settersGetters

    public float unitPixelSize
    {
        get
        {
            return _unitPixelSize;
        }
        set
        {
            //   if (_unitPixelSize == value) return;
            _unitPixelSize = value;
            events.pixelUnitOrDurationChanged.Invoke();

        }



    }

   public bool setlockloopWindow
    {
        get { return lockloopWindow; }
        set { lockloopWindow = value; }
    }

    public void SetUnitSize(float f)
    {
        unitPixelSize = f;
        OnValidate();
    }
    double _inPoint;
    double _outPoint = 1;
    bool forceChange;

    float _speed = 1;

    public double time
    {
        get
        {
            return _time;
        }
        set
        {
            if (_time == value && !forceChange) return;
            //            bool large = (_time - value > 5 || _time - value < -5);
            _time = value;
            checkIfTimeInRange();
            _timeRelativeToLoopWindow = (_time - inPointScaled) / loopWindowScaled;
            _timeRelativeToDuration = _time / duration;

        }
    }
    public bool isPlaying
    {
        get
        {
            return _isPlaying;
        }
        set

        {

            if (_isPlaying != value)
            {
                _isPlaying = value;
                _isPlayingset = value;
                //  isPlaying =value
                //  Debug.Log("played "+value);
                events.playing.Invoke(_isPlaying);
                if (_isPlaying)
                    events.started.Invoke();
                else
                    events.stopped.Invoke();
            }
        }
    }

    public bool loop
    {
        get { return _loop; }
        set
        {
            _loop = value;
        }
    }

    public float speed
    {
        get { return _speed; }
        set
        {

            //if (_speed == value) return;
            _speed = value;
            speedSet = value;
            if (_speed == 1 && Application.isPlaying) isPlaying = true;
            //   sendChangeEvents();
        }
    }
    public double loopWindow
    {
        get { return _loopWindow; }
        set
        {


            if (lockloopWindow)
            {
                _loopWindow = value;
                _outPoint = _inPoint + _loopWindow;
                if (_outPoint > 1)
                {
                    _outPoint = 1;
                    _inPoint = _outPoint - _loopWindow;
                }
                outPointScaled = _outPoint * _duration;
                outPointSet = (float)_outPoint;
            }
            loopWindowScaled = _loopWindow * _duration;
            loopWindowSet = (float)_loopWindow;
            //    sendChangeEvents();

        }
    }


    public float timeRelativeToDurationFloat
    {

        get { return (float)_timeRelativeToDuration; }
        set { timeRelativeToDuration = value; }
    }
    public double timeRelativeToDuration

    {
        get
        {
            return _timeRelativeToDuration;
        }
        set
        {
            double t = value * duration;
            time = t;
            //            Debug.Log("sent time via timere");
        }
    }
    public float timeRelativeToLoopWindowFloat
    {
        get { return (float)timeRelativeToLoopWindow; }
        set { timeRelativeToLoopWindow = value; }
    }
    public double timeRelativeToLoopWindow

    {
        get
        {
            return _timeRelativeToLoopWindow;

        }
        set
        {
            double f = inPointScaled + value * loopWindowScaled;
            time = f;

        }
    }


    public float outPointFloat
    {
        get { return (float)_outPoint; }
        set
        {
            outPoint = (float)value;
        }
    }


    public float inPointFloat
    {
        get { return (float)_inPoint; }
        set
        {
            inPoint = (float)value;
        }
    }

    public float inPointFloatAndOutRelative
    {
        get { return (float)_inPoint; }
        set
        {bool t=lockloopWindow;
        lockloopWindow=true;
           
            inPoint = (float)value;
           lockloopWindow=t;
        }
    }


    public double inPoint
    {
        get { return _inPoint; }
        set
        {

            if (_inPoint == value) return;
            if (value == 1) return;
            _inPoint = value;

            if (lockloopWindow)
            {


                _outPoint = _inPoint + _loopWindow;
                if (_outPoint > 1)
                {
                    _outPoint = 1;
                    _inPoint = _outPoint - _loopWindow;
                }
                outPointScaled = _outPoint * _duration;
                outPointSet = (float)_outPoint;

            }
            else
            {

                _loopWindow = (float)(_outPoint - _inPoint);
                if (_loopWindow <= 0)
                {
                    _loopWindow = 0.01f;
                    _inPoint = _outPoint - _loopWindow;
                }
                loopWindowSet = (float)_loopWindow;
                loopWindowScaled = _loopWindow * _duration;
            }
            inPointSet = (float)_inPoint;
            inPointScaled = _inPoint * _duration;
            
            if (_time < inPointScaled)
            {
                time = inPointScaled;
            }
        _timeRelativeToLoopWindow = (_time - inPointScaled) / loopWindowScaled;
              sendChangeEvents();
            //	}
        }
    }
    public double outPoint
    {
        get { return _outPoint; }
        set
        {
            if (_outPoint == value) return;
            if (lockloopWindow)
            {
                outPointSet = (float)_outPoint;
                return;
            }
            if (value < _inPoint)
                value = _inPoint + 0.01f;

            _outPoint = value;
            outPointSet = (float)_outPoint;
            outPointScaled = _outPoint * _duration;
            _loopWindow = _outPoint - _inPoint;
            loopWindowScaled = _loopWindow * _duration;
            loopWindowSet = (float)_loopWindow;
             _timeRelativeToLoopWindow = (_time - inPointScaled) / loopWindowScaled;
            if (_time > outPointScaled)
            {
                time = outPointScaled;
            }

                  sendChangeEvents();
        }

    }

    public float durationFloat
    {
        get
        {
            return ((float)duration);
        }
    }
    public double duration
    {
        get { return _duration; }
        set
        {
            if (value <= 0) value = 1;
            if (_duration == value) return;

            _duration = value;
            durationSet = (float)_duration;
            outPointScaled = _outPoint * _duration;
            inPointScaled = _inPoint * _duration;
            loopWindowScaled = _loopWindow * _duration;

            inPoint = inPointSet;

            if (lockloopWindow)
                loopWindow = loopWindowSet;
            else
                outPoint = outPointSet;

            forceChange = true;
            time = time;
            forceChange = false;
            checkIfTimeInRange();

        }
    }

    #endregion


    #region methods
    double _lastloopWindow;
    double _lastInPoint;
    double _lastOutPoint;
    double _lastspeed;
    double _lastDuraion;
    double _lastTime;
    float _lastunitPixelSize;
    void sendChangeEvents()
    {

        if (_lastloopWindow != _loopWindow)
        {
            //            float r = _loopWindow / _lastloopWindow;
            //   loopSpeedSet = _speed / _loopWindow;
            //   events.outPointChanged.Invoke(_loopWindow);
            events.loopOutChangedTo.Invoke((float)outPoint);
            events.loopOutAbsoluteChangedTo.Invoke((float)outPointScaled);
            _lastloopWindow = _loopWindow;
        }
        if (_lastInPoint != _inPoint)
        {
            events.loopInChangedTo.Invoke((float)inPoint);
            events.loopInAbsolueChangedTo.Invoke((float)inPointScaled);
            _lastInPoint = _inPoint;
        }
        if (_lastOutPoint != _outPoint)
        {
            events.loopOutChangedTo.Invoke((float)outPoint);
            events.loopOutAbsoluteChangedTo.Invoke((float)outPointScaled);
            _lastOutPoint = _outPoint;
        }
        if (_speed != _lastspeed)
        {
            events.speedChangedTo.Invoke(_speed);
            _lastspeed = _speed;
        }
        if (_lastunitPixelSize != unitPixelSize || _duration != _lastDuraion)

        {
            _lastunitPixelSize = unitPixelSize;
            _lastDuraion = duration;
            events.durationChangedTo.Invoke((float)_duration);
            events.pixelUnitOrDurationChanged.Invoke();

        }
        if (_lastTime != _time)
        {
            _lastTime = _time;
            timeChanged.Invoke((float)_time);
            events.timeDoublePrecition.Invoke(_time);
            events.timeRelativeToDuration.Invoke((float)timeRelativeToDuration);
            events.timeRelativeToLoopWindow.Invoke((float)timeRelativeToLoopWindow);
        }

    }

    void checkIfTimeInRange()
    {

        if (time < inPointScaled)
        {
            if (loop)
            {
                time = outPointScaled;
            }
            else
                time = inPointScaled;
            //  if (!isPlaying)
            //  {
            time = inPointScaled;
            //  }
            //    else
            //     {

            if (pingPong)
                reverse = false;
            //  else


        }


        if (time > outPointScaled)
        {
            if (pingPong)
            {
                time = outPointScaled;
                reverse = true;
            }
            else
            if (loop)
                time = inPointScaled;
            else
            {
                time = inPointScaled;
                isPlaying = false;
            }
        }


    }
     public void Play()
     {
         Debug.Log("pp");
         if (isPlaying)
         {
             time = inPointScaled;
             isPlaying = false;

         }
         else
         {
             isPlaying = true;


         }
     }

    public void Stop()
    {
        if (isPlaying)
        {
            isPlaying = false;
            time = inPointScaled;
        }
        // else
        //   Play();
    }

    public void StepForward()
    {
        time += timeStep;
    }
    public void StepBackward()
    {
        time -= timeStep;
        isPlaying = false;
    }
    public void Jog(float f)
    {
        time += f;
    }
    public void JogLoop(float f)
    {
        //  f=f*f;
        bool t = lockloopWindow;
        lockloopWindow = true;
        //  Debug.Log("jog by  "+f/duration) ;
        double fd = f / duration;
        if (inPoint + fd > 0 && inPoint + fd + loopWindow < 1)
            inPoint = inPoint + fd;
        lockloopWindow = t;

    }
    public void JogAndStop(float f)
    {
        time += f;
        isPlaying = false;
    }
    public void Pause()
    {
        isPlaying = !isPlaying;
    }

    public void resetRange()
    {
        inPoint = 0;
        outPoint = 1;

    }

    void sendAllEvents()
    {

        //  events.loopWindowChangedTo.Invoke(_loopWindow);
        events.loopInChangedTo.Invoke((float)_inPoint);
        events.loopOutChangedTo.Invoke((float)_outPoint);
        events.speedChangedTo.Invoke(_speed);

        events.pixelUnitOrDurationChanged.Invoke();
        events.timeRelativeToDuration.Invoke((float)timeRelativeToDuration);
        events.timeRelativeToLoopWindow.Invoke((float)timeRelativeToLoopWindow);
        events.playing.Invoke(isPlaying);
        timeChanged.Invoke((float)_time);
        events.timeDoublePrecition.Invoke((float)_time);

    }

    #endregion

    #region mono
    void Awake()
    {
     if (main == null) main = this;
        if (!Application.isPlaying) isPlaying = false;
    }

    void OnApplicationQuit()
    {
        isPlaying = false;
        inPoint = 0;
        outPoint = 0;
        time = inPointScaled;
        _jogAmount = 0;
    }

    void OnEnable()
    {
        //   if (main == null) main = this;
        if (!Application.isPlaying) isPlaying = false;
    }
    float _jogAmount = 0;
    public float jogAmount
    {
        get
        {
            return _jogAmount;
        }
        set
        {
            while (value < -0.3f || value > 0.3f) value *= 0.5f;
            _jogAmount = value;
        }
    }
    void Update()
    {
        if (Application.isPlaying)
        {
            if (_isPlaying)
            {
                if (reverse)
                    time -= Time.deltaTime * _speed + _jogAmount;
                else
                    time += Time.deltaTime * _speed + _jogAmount;
            }
            else if (jogAmount != 0) time += _jogAmount;
        }
        sendChangeEvents();
    }
    void OnValidate()
    {

        //timeRelativeToDuration=ti;
        // return;

        isPlaying = _isPlayingset;

        if (!Application.isPlaying) isPlaying = false;

        //  if (rangeEnd < rangeStart + _duration) rangeEnd = rangeStart + _duration;


        //.  if (main == null) main = this;
        //  speed = speedSet;
        duration = durationSet;
        inPoint = inPointSet;
        loop = _loop;
        if (pingPong) loop = true;
        if (loop == false) pingPong = false;

        if (lockloopWindow)
            loopWindow = loopWindowSet;
        else
            outPoint = outPointSet;

        if (!isPlaying)
        {
            timeRelativeToLoopWindow = _timeRelativeToLoopWindow;

        }
        timeRelativeToDuration=_timeRelativeToDuration;
        //setTimeFromLoopRelative(playheadRelativeToLoop);
        /*   if (_playheadRelative != playheadRelativeToLoop)
           {

            //   if (!isPlaying)
             //      setTimeFromRelativeTime(playheadRelativeToLoop);
           }
   */
        unitPixelSize = _unitPixelSize;
        sendChangeEvents();
    }
    public void normalSpeed()
    {
        speed = 1;

    }
    public void setIn()
    {
        inPoint = timeRelativeToDuration;
    }
    public void setOut()
    {
        outPoint = timeRelativeToDuration;
    }
    public void toggleLoopMode()
    {
        lockloopWindow = !lockloopWindow;
    }
    public void fwd()
    {
        bool t = lockloopWindow;
        lockloopWindow = true;

        double f = outPoint + loopWindow;
        if (f < 1) inPoint += loopWindow;
        lockloopWindow = t;

    }
    public void rwd()
    {
        bool t = lockloopWindow;
        lockloopWindow = true;

        double f = inPoint - loopWindow;
        if (f > 0) inPoint -= loopWindow;
        lockloopWindow = t;

    }

    public void setone()
    {
        outPoint = inPoint + 1 / duration;
    }
    public void MoveToInpoint()
    {
        time = inPoint;
    }

    void Start()
    {
        isPlaying = startOnPlay;
        if (isPlaying) time = inPointScaled;
        OnValidate();
      //  sendChangeEvents();
       sendAllEvents();
    }
    #endregion
}
