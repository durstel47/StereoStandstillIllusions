using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DepthMaps : int
{
    RotatingSectors, ScalingRings, RotatingRHSpirals, RotatingLHSpirals, ScalingRHSpirals, ScalingLHSpirals,
    RotatingWheel4Spokes, RotatingWheel8Spokes, RotatingWheel16Spokes, RotatingWheel32Spokes,
    ShearingSectors, ShearingRings, ShearingLHSpirals, ShearingRHSpirals
}

public enum Textures : int
{
    RandomDots, PeriodicRandomDots, AshLeaves, BakhtiariRug
}


public class StereoDisplay : MonoBehaviour {

    public int depthMapIndex = 0;

    public float wheelRotVel = 12f;
    public float wheelShearingAmpPct = 20f;
    public float wheelScalingFreq = 0.4f;
    public float wheelShearingAngle = 0.0f;
    public float wheelShearingFreq = 0.4f;
    public float wheelSizePct = 90f;
    public float maxDisparity = 10f; //in pels

    public int textureIndex = 0;

    public float maskRotVel = 0f;
    public float maskScalingFreq = 0f;
    public float maskShearingAmp = 0f;
    public float maskShearingAngle = 0f;
    public float maskShearingFreq = 0.0f;
    public float maskSizePct = 200f;
    public float maskXOffsetPct = 0f;
    public float maskYOffsetPct = 0f;
    public float textureScalePct = 100f;

    private float cSmallScale = 0.67f; //mesured from depth map with rings: scale change to reach to next inner ring 0.668

    private float wheelSize = 0.9f;
    private float textureScale = 0.01f;


    public bool isMarkers = false;
    public bool isCircles = false;
    public bool isFlicker = false;

    private SpriteRenderer rend;
    private Material mat;

    private float wheelShearingAmp = 0.0f;
    private float wheelAngle = 0f;
    private float wheelScalingPeriod = 0f;
    private float wheelCurScalingTime;
    private float wheelScale = 1f;
    private float wheelShear = 1f;

    private float maskAngle = 0f;
    private float maskScale = 2f;
    private float maskShear = 1f;
    private float maskSize = 1f;
    private float maskXOffset = 0f;
    private float maskYOffset = 0f; 
    private float maskScalingPeriod = 0f;
    private float maskCurScalingTime = 0f;

    private float OmphalosRad = 0.03f; //with of blue center with to use with logarithmic pereiodic ring texture

    void Awake()
    {
        rend = this.gameObject.GetComponent<SpriteRenderer>();
        mat = rend.material;
    }

    // Use this for initialization
    void Start () {
        rend = this.gameObject.GetComponent<SpriteRenderer>();
        mat = rend.material;
        mat.SetFloat("_MarkThreshold", 1.1f); //switch of markers 
        mat.SetFloat("_MarkinnerEnd", 0.01f);
        mat.SetFloat("_MarkouterEnd", 0.08f);
        mat.SetFloat("CrossMarkers", 0f);
        mat.SetFloat("_WheelScale", wheelScale);
        //get resources
        OnChangedDepthMap(depthMapIndex);
        OnChangedTexture(textureIndex);
        OnChangedWheelSize(wheelSizePct);
        OnChangedMaxDisparity(maxDisparity);
        OnChangedMaskSize(maskSizePct);
        OnChangedMaskXOffset(maskXOffsetPct);
        OnChangedMaskYOffset(maskYOffsetPct);
        OnChangedTextureScale(textureScalePct);
        OnChangedOmphalos(0f);

	}
	
	// Update is called once per frame
	void Update () {
        if ((depthMapIndex == (int)(DepthMaps.RotatingSectors))
            || (depthMapIndex == (int)(DepthMaps.RotatingLHSpirals))
            || (depthMapIndex == (int)(DepthMaps.RotatingRHSpirals))
            || (depthMapIndex == (int)(DepthMaps.RotatingWheel4Spokes))
            || (depthMapIndex == (int)(DepthMaps.RotatingWheel8Spokes))
            || (depthMapIndex == (int)(DepthMaps.RotatingWheel16Spokes))
            || (depthMapIndex == (int)(DepthMaps.RotatingWheel32Spokes))
            )
        {
            if (Mathf.Abs(wheelRotVel) > 0.1f)
                stepWheelRot();
        }
        else if (depthMapIndex == (int)(DepthMaps.ScalingRings))
        {
            cSmallScale = 0.67f; //measured from figure
            if (Mathf.Abs(wheelScalingFreq) > 0.05f)
                stepWheelScalingFreq();
        }
        else if ((depthMapIndex == (int)(DepthMaps.ScalingLHSpirals))
            || (depthMapIndex == (int)(DepthMaps.ScalingRHSpirals))
            )
        {
            cSmallScale = 0.392f; //emperical value. Here we are using a linear aproximatio d of an exponential function
            if (Mathf.Abs(wheelScalingFreq) > 0.05f)
                stepWheelScalingFreq();
        }
        else if ((depthMapIndex == (int)(DepthMaps.ShearingSectors))
            || (depthMapIndex == (int)(DepthMaps.ShearingRings))
            || (depthMapIndex == (int)(DepthMaps.ShearingLHSpirals))
            || (depthMapIndex == (int)(DepthMaps.ShearingRHSpirals))
            )
        {
            if (wheelShearingFreq > 0.01f)
                stepWheelShearingFreq();
        }
        if (Mathf.Abs(maskRotVel) > 1f)
            stepMaskRot();
        if (Mathf.Abs(maskShearingAmp) > 0.01f)
            stepMaskShearingFreq();
        if (textureIndex == (int)Textures.PeriodicRandomDots)
        {
            if (Mathf.Abs(maskScalingFreq) > 0.1f)
                stepMaskFreq();
        }
        else if (textureIndex == (int)Textures.RandomDots)
        {
            if (isFlicker)
                stepFlicker();
        }
     

    }

    private void stepWheelRot()
    {
        wheelAngle += Time.deltaTime * wheelRotVel;
        wheelAngle = wheelAngle % 360f;
        mat.SetFloat("_WheelAngle", wheelAngle);
    }

    private void stepWheelScalingFreq()
    {
        wheelScalingPeriod = 1f / Mathf.Abs(wheelScalingFreq);
        wheelCurScalingTime = Time.time % wheelScalingPeriod;
        float deltaScale = (1f - cSmallScale) * Mathf.Abs(wheelScalingFreq);
        float scale;
        if (wheelScalingFreq > 0f)
        {
           scale = wheelScale * (cSmallScale + deltaScale * wheelCurScalingTime);
        }
        else
        {
            scale = wheelScale * (1f - deltaScale * wheelCurScalingTime);
        }
        mat.SetFloat("_WheelScale", scale);
    }
    public void stepWheelShearingFreq()
    {
        float freq = wheelShearingFreq;
        wheelShear = 1f + wheelShearingAmp * Mathf.Cos(2f * freq * Mathf.PI * Time.time);
        mat.SetFloat("_WheelShear", wheelShear);
        mat.SetFloat("_WheelAngle", wheelShearingAngle);
    }

    private void stepMaskRot()
    {
        maskAngle += Time.deltaTime * maskRotVel;
        maskAngle = maskAngle % 360f;
        mat.SetFloat("_MaskAngle", maskAngle);
    }

    private void stepMaskFreq()
    {
        maskScalingPeriod = 1f / Mathf.Abs(maskScalingFreq);
        maskCurScalingTime = Time.time % maskScalingPeriod;
        float deltaScale = (1f - cSmallScale) * Mathf.Abs(maskScalingFreq);
        float scale;
        if (maskScalingFreq > 0f)
        {
            scale =  maskScale * (cSmallScale + deltaScale * maskCurScalingTime);
        }
        else
        {
            scale =  maskScale * (1f - deltaScale * maskCurScalingTime);
        }
        rend.material.SetFloat("_MaskScale", scale);
    }

    public void stepMaskShearingFreq()
    {
        float freq = maskShearingFreq;
        maskShear = 1f + maskShearingAmp * Mathf.Cos(2f * freq * Mathf.PI * Time.time);
        if (mat != null)
        {
            mat.SetFloat("_MaskShear", maskShear);
            rend.material.SetFloat("_MaskAngle", maskShearingAngle);
        }
    }

    private void stepFlicker()
    {
        maskXOffset = Random.Range(-0.49f, 0.49f);
        maskXOffset = Random.Range(-0.49f, 0.49f);
        maskAngle = Random.Range(-180f, 179f);
        //mat.SetFloat("_MaskAngle", 0f);
        mat.SetFloat("_MaskXPos", maskXOffset);
        mat.SetFloat("_MaskYPos", maskXOffset);
        mat.SetFloat("_MaskAngle", maskAngle);

    }

    public void OnChangedDepthMap(int iMap)
    {
        if (rend == null)
            return;
        DepthMaps idMaps = (DepthMaps)iMap;
        depthMapIndex = iMap;
        Sprite mapSprite;
        //reset scaling factors
        mat.SetFloat("_WheelScale", wheelScale);
        mat.SetFloat("_MaskScale", maskScale);
        mat.SetFloat("WheelAngle", 0f);
        mat.SetFloat("_WheelShear", 1f);
        //adapt marker threshold to current weel choice
        if (isCircles)
        {
            OnChangedCrossTg(isMarkers);
        }
        else
        {
            OnChangedMarkerTg(isMarkers);
        }
       
        switch (idMaps)
        {
            case DepthMaps.RotatingSectors:
                mapSprite = Resources.Load<Sprite>("8HalfPeriodSectors1024Gauss5");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.ScalingRings:
                mapSprite = Resources.Load<Sprite>("HalfPeriodRings1024Gauss5");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.RotatingRHSpirals:
                mapSprite = Resources.Load<Sprite>("6CCWHalfPeriodSpirals1024Gauss5");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.RotatingLHSpirals:
                mapSprite = Resources.Load<Sprite>("6CWHalfPeriodSpirals1024Gauss5");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.ScalingRHSpirals:
                mapSprite = Resources.Load<Sprite>("6CCWHalfPeriodSpirals1024Gauss5");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.ScalingLHSpirals:
                mapSprite = Resources.Load<Sprite>("6CWHalfPeriodSpirals1024Gauss5");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.RotatingWheel4Spokes:
                mapSprite = Resources.Load<Sprite>("4SpokeWheel");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.RotatingWheel8Spokes:
                mapSprite = Resources.Load<Sprite>("8SpokeWheel");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.RotatingWheel16Spokes:
                mapSprite = Resources.Load<Sprite>("16SpokeWheel");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.RotatingWheel32Spokes:
                mapSprite = Resources.Load<Sprite>("32SpokeWheel");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.ShearingSectors:
                mapSprite = Resources.Load<Sprite>("8HalfPeriodSectors1024Gauss5");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.ShearingRings:
                mapSprite = Resources.Load<Sprite>("HalfPeriodRings1024Gauss5");
                rend.sprite = mapSprite;
                break;
                break;
            case DepthMaps.ShearingRHSpirals:
                mapSprite = Resources.Load<Sprite>("6CCWHalfPeriodSpirals1024Gauss5");
                rend.sprite = mapSprite;
                break;
            case DepthMaps.ShearingLHSpirals:
                mapSprite = Resources.Load<Sprite>("6CWHalfPeriodSpirals1024Gauss5");
                rend.sprite = mapSprite;
                break;

            default:
                break;
       
        }
    }

    public void OnChangedTexture(int iTex)
    {
        Textures idTex = (Textures)iTex;
        Texture pattern;
        textureIndex = iTex;
        //reset scaling factors
        rend.material.SetFloat("_WheelScale", wheelScale);
        rend.material.SetFloat("_MaskScale", maskScale);

        //set visibulity of UI elements
        OnChangedOmphalos(0f);

        switch (idTex)
        {
            case Textures.RandomDots:
                pattern = Resources.Load("BWRandomDots2x2") as Texture;
                mat.SetTexture("_LuminancePattern", pattern);
                break;
            case Textures.PeriodicRandomDots:
                pattern = Resources.Load("BWRDRingPattern1024") as Texture;
                mat.SetTexture("_LuminancePattern", pattern);
                OnChangedOmphalos(OmphalosRad);
                break;
            case Textures.AshLeaves:
                pattern = Resources.Load("EscheBlätter1024") as Texture;
                mat.SetTexture("_LuminancePattern", pattern);
                break;
            case Textures.BakhtiariRug:
                pattern = Resources.Load("OrientalRug1024") as Texture;
                mat.SetTexture("_LuminancePattern", pattern);
                break;
            default:
                break;
        }
    }



    public void OnChangedWheelShearAmp(float ampPct)
    {
        wheelShearingAmpPct = ampPct;
        wheelShearingAmp = 0.01f * ampPct;
    }
 
    public void OnChangedWheelSize(float sizePct)
    {
        wheelSize = 0.01f * sizePct;
        mat.SetFloat("_WheelSize", wheelSize);
    }

    public void OnChangedMaskShearingAmp(float ampPct)
    {
        maskShearingAmp = 0.01f * ampPct;
        maskAngle = maskShearingAngle;          //note: shader implmentation does not  have independent rotation and shear angles
        mat.SetFloat("_MaskAngle", maskAngle);
    }

    public void OnChangedMaskSize(float sizePct)
    {
        maskSize = 0.01f * sizePct;
        mat.SetFloat("_MaskSize", maskSize);  
    }

    public void OnChangedMaskXOffset(float xPct)
    {
        maskXOffset = 0.01f * xPct;
        if (mat == null)
            return;
        mat.SetFloat("_MaskXPos", maskXOffset);
    }

    public void OnChangedMaskYOffset(float yPct)
    {
        maskYOffset = 0.01f * yPct;
        if (mat == null)
            return;
        mat.SetFloat("_MaskYPos", maskYOffset);
    }


    public void OnChangedTextureScale(float pctScale)
    {
        textureScale = 4f * 0.01f * pctScale; //Bschiss: more Texture needed for dooing mask offsets with flickering
        mat.SetFloat("_TextureScale", textureScale);
    }

    public void OnChangedMaxDisparity(float nPels)
    {
        maxDisparity = nPels;
        //arbitray scaling-> 0.001f should correspond to 1 pixel??
        float fac = 1f/1024f * nPels;
        mat.SetFloat("_DepthFactor", fac);
    }
    public void OnChangedCrossTg( bool isOn)
    {
        isCircles = isOn;
        isMarkers = !isOn;
        if (isCircles)
        {
            mat.SetFloat("_MarkIsCross", 1.0f); //set cross like marker set
            if ((depthMapIndex == (int)(DepthMaps.ScalingRings))
                || (depthMapIndex == (int)(DepthMaps.ScalingLHSpirals))
                || (depthMapIndex == (int)(DepthMaps.ScalingRHSpirals))
            )
                mat.SetFloat("_MarkThreshold", 0.8f); //set markers on
            else
                mat.SetFloat("_MarkThreshold", 0.95f);
        }
        else 
        {
            mat.SetFloat("_MarkThreshold", 1.1f); //set markers off
        }
    }

    public void OnChangedMarkerTg(bool isOn)
    {
        isMarkers = isOn;
        isCircles = !isOn;
        if (isMarkers)
        {
            mat.SetFloat("_MarkIsCross", 0.0f); //set cross like marker off for sure
            if ((depthMapIndex == (int)(DepthMaps.ScalingRings))
                || (depthMapIndex == (int)(DepthMaps.ScalingLHSpirals))
                || (depthMapIndex == (int)(DepthMaps.ScalingRHSpirals))
            )
                mat.SetFloat("_MarkThreshold", 0.85f); //set markers on
            else
                mat.SetFloat("_MarkThreshold", 0.95f);
        }
        else
        {
            mat.SetFloat("_MarkThreshold", 1.1f); //set markers off
        }
    }

    public void OnChangedPseudoFlickerToggle(bool isOn)
    {
        isFlicker = isOn;
        if(!isOn)
        {
            maskAngle = 0f;
            maskXOffset = 0f;
            maskXOffset = 0f;
            mat.SetFloat("_MaskAngle", 0f);
            mat.SetFloat("_MaskXPos", maskXOffset);
            mat.SetFloat("_MaskYPos", maskXOffset);
        }
    }

    public void OnChangedOmphalos(float rad)
    {
        mat.SetFloat("_FixPtRadius", rad);
    }

}
