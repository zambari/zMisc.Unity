using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class zGrapher : MonoBehaviour
{
    RawImage rawImage;
    public Texture2D texture;
    [Range(0, 1)]
    float currentValue;
    [Range(0, 10)]

    int xDim = 256;
    int yDim = 556;
    public Color bg = Color.black;
    public Color trace = Color.green;
	public Color trace2 = Color.red;

    [Range(0, 1)]
    public float val;
	
    [Range(0, 1)]
    public float val2;
	
    //	float scrollSpeed;
    Color32[] pixelsA;
    Color32[] pixelsB;
    public bool showingA;
    public bool sizeFromRect = false;
    [Range(1, 0)]
    public float updateSpeed = 0.2f;
    public int rowIndex2;
    int rowIndex;
    int scaledValue;
    float _traceValue;
	public float minValue=0;
	public float maxValue=1;
    public float traceValue
    {
        get { return _traceValue; }
        set
        {
            if (_traceValue != value)
                _traceValue = value;
				val=value;
            if (draw) updateRow();
        }

    }
	float _traceValue2;
    public float traceValue2
    {
        get { return _traceValue2; }
        set
        {
            if (_traceValue2 != value)
                _traceValue2 = value;
				val2=value;
            if (draw) updateRow2();
        }

    }
    void Start()
    {
        if (sizeFromRect)
        {
            RectTransform r = GetComponent<RectTransform>();
            xDim = (int)r.rect.width;
            yDim = (int)r.rect.height;
        }
        rawImage = GetComponent<RawImage>();
        texture = new Texture2D(xDim, yDim);
        pixelsA = new Color32[xDim * yDim];
        pixelsB = new Color32[pixelsA.Length];

        for (int i = 0; i < pixelsA.Length; i++)
        {
            pixelsB[i] = bg ;//* Random.value;
        }

        rawImage.texture = texture;

        offsetAndDraw();
        StartCoroutine(graphDrawer());
    }

    void offsetAndDraw()
    {
        texture.SetPixels32(pixelsA);
        texture.Apply();
        int offsetindex = rowIndex * xDim;
        Color32[] source = pixelsB;
        Color32[] dest = (pixelsA);
        for (int i = 0; i < source.Length; i++)
        {

            dest[offsetindex] = source[i];
            offsetindex++;
            if (offsetindex >= dest.Length) offsetindex = 0;

        }

    }
    public bool draw = true;
    IEnumerator graphDrawer()
    {

        while (true)
        {
            rowIndex++;
            rowIndex2--;
            if (rowIndex2 < 0) rowIndex2 = yDim - 1;
            if (rowIndex >= yDim - 1) rowIndex = 0;


            updateRow();
            clearRow();
            offsetAndDraw();
            yield return (draw ? new WaitForSeconds(updateSpeed) : new WaitForSeconds(3));
        }
    }	
 void plotPointRow(float val, Color c)
    {
		val=(val-minValue)/(maxValue-minValue);
        scaledValue = (int)(val * xDim);
        if (scaledValue < 0) scaledValue = 0;
        if (scaledValue > xDim - 1) scaledValue = xDim - 1;
        for (int i = 0; i < xDim-1; i++)
            if (scaledValue == i)
                pixelsB[xDim * rowIndex2 + i] = trace;
    }
    void updateRow()
    {
        scaledValue = (int)(val * xDim);
        if (scaledValue < 0) scaledValue = 0;
        if (scaledValue > xDim - 1) scaledValue = xDim - 1;
        for (int i = 0; i < xDim-1; i++)
            if (scaledValue == i)
                pixelsB[xDim * rowIndex2 + i] = trace;
    }
	   void updateRow2()
    {
        scaledValue = (int)(val2 * xDim);
        if (scaledValue < 0) scaledValue = 0;
        if (scaledValue > xDim - 1) scaledValue = xDim - 1;
        for (int i = 0; i < xDim-1; i++)
            if (scaledValue == i)
                pixelsB[xDim * rowIndex2 + i] = trace2;
    }    void clearRow()
    { int o=xDim * (rowIndex2-1) ;
	if (o<0) o=xDim * (yDim-1);
        for (int i = 0; i < xDim; i++)
            pixelsB[o + i] = bg;
    }


}

/*
public class zGrapher : MonoBehaviour
{

    // Use this for initialization
     RawImage rawImage;
    //public RawImage rawImage2;
    [Range(1, 4)]
    public int waitFrames=1;
    [Range(1, 5)]
    public int doTimes=1;
   public texture texture;
    //texture B;
    public Material graphMaterial;
	public Material offsetMaterial;
	//public float offset;

public int xDim=256;
		public int yDim=256;
		public bool sizeFromRect;
		public bool startCoroutine;
		
		[Range(0,1)]
		public float val;
		float scrollSpeed;
    void Start()
    {
        rawImage = GetComponent<RawImage>();
        
		if (sizeFromRect)
		{
			RectTransform r=GetComponent<RectTransform>();  
		 xDim=(int)r.rect.width;
		 yDim=(int)r.rect.height;
		}

	    texture = new texture(xDim, yDim, 32);
        texture.name="scrolltexture";
		texture.wrapMode=TextureWrapMode.Repeat;
		//rawImage.material=offsetMaterial;
		offsetMaterial.mainTexture=texture;
		setMaterialParams();
		if (startCoroutine)
		StartCoroutine(runner());
    }

	void setup()
	{

	}
	public void showValue(float f)
	{
		graphMaterial.SetFloat("_y",value);
			graphMaterial.SetFloat("_x",offset);
			
            Graphics.Blit(texture, texture, graphMaterial);

            offset+=scrollSpeed;
			if (offset>1) offset-=1;
			updateOffset=true;
	}
	public float value;
	[Range(1,3)]
public float pixSize=1.2f;
		public float pixelsizex;
		public float pixelsizey;
		float offset;
		bool updateOffset;
		void Update()
		{
			if (updateOffset)
			{
			offsetMaterial.mainTextureOffset=new Vector2(offset,0);
			updateOffset=false;
			}
		}
	IEnumerator runner()
    {
	setMaterialParams();
   rawImage.texture = texture;
		//	graphMaterial.SetTexture("_MainTex2",A);
			//graphMaterial.SetFloat("_cutoff",0);
			// Graphics.Blit(A, A, graphMaterial);
			//graphMaterial.SetFloat("_cutoff",1-px0.96f);
			 
        while (true)
        {
			for (int i=0;i<doTimes;i++)
			{
		
			value=0.5f+Mathf.Sin(Time.time/4)/3+Random.value*0.1f;
			showValue(value);
			
			}
			
         for (int i=0;i<waitFrames;i++)
            yield return null;
        }
	}
	void setMaterialParams()
	{
		 pixelsizex=pixSize/xDim;
		 pixelsizey=pixSize/yDim;
  		
		  graphMaterial.SetFloat("_pixelSizeX",pixelsizex);
		  graphMaterial.SetFloat("_pixelSizeY",pixelsizey);
	}

    void Do()
    {
		
        StartCoroutine(runner());

    }
    public bool DoNow;
    void OnValidate()
    { 
	
		setMaterialParams();
			if (!Application.isPlaying) scrollSpeed=pixelsizex;
		showValue(val);
 	if (startCoroutine)
		StartCoroutine(runner());
	//	graphMaterial.SetFloat("_r",Mathf.Sqrt(pixelSize));
        
    }
    // Update is called once per frame
 
}
 */
