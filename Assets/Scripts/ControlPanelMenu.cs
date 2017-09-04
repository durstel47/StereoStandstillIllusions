using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MotionType : int
{
    None = 0, Rotation = 1, Scaling = 2, Shearing = 4, RoationScaling = 3, RotationShearing = 5, ScalingShearing = 6, All = 7
}

public class ControlPanelMenu : MonoBehaviour {


    public bool bStarted = false;
    public MotionType wheelMotion = MotionType.None;
    public MotionType maskMotion = MotionType.None;
    private MotionType flickerMaskMotion = MotionType.None;
    private StereoDisplay disp;

    private Text illusionTitle;
    private Dropdown depthMapDD;
    private Slider wheelRotSpeedSl;
    private Text wheelRotSpeedLb;
    private Slider wheelScalingFreqSl;
    private Text wheelScalingFreqLb;
    private Slider wheelShearingAmpSl;
    private Text wheelShearingAmpLb;
    private Slider wheelShearingFreqSl;
    private Text wheelShearingFreqLb;
    private Slider wheelShearingAngSl;
    private Text wheelShearingAngLb;
    private Slider wheelSizeSl;
    private Text wheelSizeLb;
    private Slider maxDisparitySl;
    private Text maxDisparityLb;
    private Toggle markerTg;
    private Text markerLb;
    private Toggle crossTg;
    private Dropdown textureDD;
    private Slider maskRotSpeedSl;
    private Text maskRotSpeedLb;
    private Slider maskScalingFreqSl;
    private Text maskScalingFreqLb;
    private Slider maskShearingAmpSl;
    private Text maskShearingAmpLb;
    private Slider maskShearingFreqSl;
    private Text maskShearingFreqLb;
    private Slider maskShearingAngSl;
    private Text maskShearingAngLb;
    private Slider maskXOffsetSl;
    private Text maskXOffsetLb;
    private Slider maskYOffsetSl;
    private Text maskYOffsetLb;
    private Slider textureScaleSl;
    private Text textureScaleLb;
    private Slider maskSizeSl;
    private Text maskSizeLb;
    private Toggle flickerTg;
    private Button resetButton;


    /*******************************************
    private float dfltWheelSpeed = 12f;
    private float dfltWheelTemporalFreq = 0.4f;
    private float dflthWeelSize = 50f;
    private float dfltMaxDisparity = 10f;
    private float dfltMaskSpeed = 0f;
    private float dfltMaskTemporalFreq = 0.0f;
    private float dfltMaskSize = 100f;
    private float dfltTemporalScale = 50f;
    **************************************/
    private void Awake()
    {
        //access to stereo display sript
        disp = GameObject.Find("SpriteObject").GetComponent<StereoDisplay>();
    }

    // Use this for initialization
    void Start()
    {
 
        illusionTitle = GameObject.Find("IllusionTitle").GetComponent<Text>();

        depthMapDD = GameObject.Find("DepthMapDD").GetComponent<Dropdown>();
        depthMapDD.ClearOptions();
        depthMapDD.options.Add(new Dropdown.OptionData("Rotating Sectors"));
        depthMapDD.options.Add(new Dropdown.OptionData("Scaling Rings"));
        depthMapDD.options.Add(new Dropdown.OptionData("Rotating R-handed Spirals"));
        depthMapDD.options.Add(new Dropdown.OptionData("Rotating L-handed Spirals"));
        depthMapDD.options.Add(new Dropdown.OptionData("Scaling R-handed Spirals"));
        depthMapDD.options.Add(new Dropdown.OptionData("Scaling L-handed Spirals"));
        depthMapDD.options.Add(new Dropdown.OptionData("Rotating Wheel 4 Spokes"));
        depthMapDD.options.Add(new Dropdown.OptionData("Rotating Wheel 8 Spokes"));
        depthMapDD.options.Add(new Dropdown.OptionData("Rotating Wheel 16 Spokes"));
        depthMapDD.options.Add(new Dropdown.OptionData("Rotating Wheel 32 Spokes"));
        depthMapDD.options.Add(new Dropdown.OptionData("Shearing Sectors"));
        depthMapDD.options.Add(new Dropdown.OptionData("Shearing Rings"));
        depthMapDD.options.Add(new Dropdown.OptionData("Shearing R-handed Spirals"));
        depthMapDD.options.Add(new Dropdown.OptionData("Shearing L-handed Spirals"));
        depthMapDD.onValueChanged.AddListener((value) => { OnChangedDepthMap(value); });
        Text mapLb = GameObject.Find("DepthMapDD").GetComponentInChildren<Text>();
        mapLb.text = "Rotating Sectors";
        //depthMapDD.onValueChanged.Invoke(0);

        textureDD = GameObject.Find("TextureDD").GetComponent<Dropdown>();
        textureDD.ClearOptions();
        textureDD.options.Add(new Dropdown.OptionData("Random Dots"));
        textureDD.options.Add(new Dropdown.OptionData("Periodic RD"));
        textureDD.options.Add(new Dropdown.OptionData("Ash Leaves"));
        textureDD.options.Add(new Dropdown.OptionData("Bakhtiari Rug"));
        textureDD.onValueChanged.AddListener((value) => { OnChangedTexture(value); });
        //textureDD.onValueChanged.Invoke(0);
        Text textureLb = GameObject.Find("TextureDD").GetComponentInChildren<Text>();
        textureLb.text = "Random Dots";


        wheelRotSpeedSl = GameObject.Find("WheelRotSpeedSl").GetComponent<Slider>();
        wheelRotSpeedLb = GameObject.Find("WheelRotSpeedLb").GetComponent<Text>();
        wheelScalingFreqSl = GameObject.Find("WheelScalingFreqSl").GetComponent<Slider>();
        wheelScalingFreqLb = GameObject.Find("WheelScalingFreqLb").GetComponent<Text>();
        wheelShearingAmpSl = GameObject.Find("WheelShearingAmpSl").GetComponent<Slider>();
        wheelShearingAmpLb = GameObject.Find("WheelShearingAmpLb").GetComponent<Text>();
        wheelShearingFreqSl = GameObject.Find("WheelShearingFreqSl").GetComponent<Slider>();
        wheelShearingFreqLb = GameObject.Find("WheelShearingFreqLb").GetComponent<Text>();
        wheelShearingAngSl = GameObject.Find("WheelShearingAngSl").GetComponent<Slider>();
        wheelShearingAngLb = GameObject.Find("WheelShearingAngLb").GetComponent<Text>();
        wheelSizeSl = GameObject.Find("WheelSizeSl").GetComponent<Slider>();
        wheelSizeLb = GameObject.Find("WheelSizeLb").GetComponent<Text>();

        maxDisparitySl = GameObject.Find("MaxDisparitySl").GetComponent<Slider>();
        maxDisparityLb = GameObject.Find("MaxDisparityLb").GetComponent<Text>();
        markerTg = GameObject.Find("MarkerTg").GetComponent<Toggle>();
        markerLb = GameObject.Find("MarkerTg").GetComponentInChildren<Text>();
        crossTg = GameObject.Find("CrossTg").GetComponent<Toggle>();

        maskRotSpeedSl = GameObject.Find("MaskRotSpeedSl").GetComponent<Slider>();
        maskRotSpeedLb = GameObject.Find("MaskRotSpeedLb").GetComponent<Text>();
        maskScalingFreqSl = GameObject.Find("MaskScalingFreqSl").GetComponent<Slider>();
        maskScalingFreqLb = GameObject.Find("MaskScalingFreqLb ").GetComponent<Text>();
        maskShearingAmpSl = GameObject.Find("MaskShearingAmpSl").GetComponent<Slider>();
        maskShearingAmpLb = GameObject.Find("MaskShearingAmpLb").GetComponent<Text>();
        maskShearingFreqSl = GameObject.Find("MaskShearingFreqSl").GetComponent<Slider>();
        maskShearingFreqLb = GameObject.Find("MaskShearingFreqLb").GetComponent<Text>();
        maskShearingAngSl = GameObject.Find("MaskShearingAngSl").GetComponent<Slider>();
        maskShearingAngLb = GameObject.Find("MaskShearingAngLb").GetComponent<Text>();
        maskSizeSl = GameObject.Find("MaskSizeSl").GetComponent<Slider>();
        maskSizeLb = GameObject.Find("MaskSizeLb").GetComponent<Text>();
        maskXOffsetSl = GameObject.Find("MaskXOffsetSl").GetComponent<Slider>();
        maskXOffsetLb = GameObject.Find("MaskXOffsetLb").GetComponent<Text>();
        maskYOffsetSl = GameObject.Find("MaskYOffsetSl").GetComponent<Slider>();
        maskYOffsetLb = GameObject.Find("MaskYOffsetLb").GetComponent<Text>();

        textureScaleSl = GameObject.Find("TextureScaleSl").GetComponent<Slider>();
        textureScaleLb = GameObject.Find("TextureScaleLb").GetComponent<Text>();
        flickerTg = GameObject.Find("FlickerToggle").GetComponent<Toggle>();
        resetButton = GameObject.Find("ResetBtn").GetComponent<Button>();


        wheelRotSpeedSl.onValueChanged.AddListener((value) => { OnChangedWheelRotVel(value); });
        wheelRotSpeedSl.onValueChanged.Invoke(disp.wheelRotVel);

        wheelScalingFreqSl.onValueChanged.AddListener((value) => { OnChangedWheelScalingFreq(value); });
        wheelScalingFreqSl.onValueChanged.Invoke(disp.wheelScalingFreq);

        wheelShearingAmpSl.onValueChanged.AddListener((value) => { OnChangedWheelShearingAmp(value); });
        //wheelShearingAmpLb.text = "Wheel Shearing Amplitude: " + wheelShearingAmpSl.value.ToString("F2") + " %";
        wheelShearingAmpSl.onValueChanged.Invoke(disp.wheelShearingAmpPct);

        wheelShearingFreqSl.onValueChanged.AddListener((value) => { OnChangedWheelShearingFreq(value); });
        wheelShearingFreqLb.text = "Wheel Shearing Frequency: " + wheelShearingFreqSl.value.ToString("F2") + "Hz";

        wheelShearingAngSl.onValueChanged.AddListener((value) => { OnChangedWheelShearingAng(value); });
        wheelShearingAngLb.text = "Wheel Shearing Angle: " + wheelShearingAngSl.value.ToString("F1") + " °";

        wheelSizeSl.onValueChanged.AddListener((value) => { OnChangedWheelSize(value); });
        wheelSizeLb.text = "Wheel Size: " + disp.wheelSizePct.ToString("F2") + " %";

        maxDisparitySl.onValueChanged.AddListener((value) => { OnChangedMaxDisparity(value); });
        maxDisparityLb.text = "Maximal Disparity:" + disp.maxDisparity.ToString("F1") + " pels";

        markerTg.onValueChanged.AddListener((value) => { OnChangedMarkerTg(value); });
        crossTg.onValueChanged.AddListener((value) => { OnChangedCrossTg(value); });

        maskRotSpeedSl.onValueChanged.AddListener((value) => { OnChangedMaskRotVel(value); });
        maskRotSpeedSl.onValueChanged.Invoke(disp.maskRotVel);

        maskScalingFreqSl.onValueChanged.AddListener((value) => { OnChangedMaskScalingFreq(value); });
        maskScalingFreqSl.onValueChanged.Invoke(disp.maskScalingFreq);

        maskShearingAmpSl.onValueChanged.AddListener((value) => { OnChangedMaskShearingAmp(value); });
        maskShearingAmpLb.text = "Mask Shearing Amp: " + maskShearingAmpSl.value.ToString("F0") + " %";

        maskShearingFreqSl.onValueChanged.AddListener((value) => { OnChangedMaskShearingFreq(value); });
        maskShearingFreqLb.text = "Mask Shearing Frequency: " + maskShearingFreqSl.value.ToString("F2") + "Hz";

        maskShearingAngSl.onValueChanged.AddListener((value) => { OnChangedMaskShearingAng(value); });
        maskShearingAngLb.text = "Mask Shearing Angle: " + maskShearingAngSl.value.ToString("F1") + " °";

        maskSizeSl.onValueChanged.AddListener((value) => { OnChangedMaskSize(value); });
        maskSizeLb.text = "Mask Size: " + disp.maskSizePct.ToString("F2") + " %";

        maskXOffsetSl.onValueChanged.AddListener((value) => { OnChangedMaskXOffset(value); });
        maskXOffsetLb.text = "Mask's Center X-Offset: " + disp.maskXOffsetPct.ToString("F2") + " %";

        maskYOffsetSl.onValueChanged.AddListener((value) => { OnChangedMaskYOffset(value); });
        maskYOffsetLb.text = "Mask's Center Y-Offset: " + disp.maskYOffsetPct.ToString("F2") + " %";

        textureScaleSl.onValueChanged.AddListener((value) => { OnChangedTextureScale(value); });
        textureScaleLb.text = "Texture Scale:" + disp.textureScalePct.ToString("F2") + " %";

        flickerTg.onValueChanged.AddListener((value) => { OnChangedFlickerTg(value); }); //

        resetButton.onClick.AddListener(() => { OnClickedReset(); });

        OnChangedDepthMap((int)DepthMaps.RotatingSectors);
        crossTg.gameObject.SetActive(false);

    }



    // Update is called once per frame
    void Update () {
		
	}

    public void OnChangedDepthMap(int iMap)
    {
        disp.OnChangedDepthMap(iMap);

        DepthMaps idMaps = (DepthMaps)iMap;
        ShowWheelRotationControls(false);
        ShowWheelScalingControls(false);
        ShowWheelShearingControls(false);
        ShowMaskRotatingControls(false);
        ShowMaskScalingControls(false);
        ShowMaskShearingControls(false);
        if (maskXOffsetSl != null)
        {
            maskXOffsetSl.onValueChanged.Invoke(0f);
            maskYOffsetSl.onValueChanged.Invoke(0f);
        }



        //Note scaling mask controls are controlled by the choide of textures: there only available for the periodic random dots texture
        crossTg.isOn = false;
        crossTg.gameObject.SetActive(false);
        markerLb.text = "Markers";
        if (!markerTg.isOn)
            disp.OnChangedMarkerTg(false);
        wheelMotion = MotionType.None;
        //maskMotion = maskMotion & MotionType.Scaling; //see note above
        maskMotion = MotionType.None;
    
        switch (idMaps)
        {
            case DepthMaps.RotatingSectors:
                illusionTitle.text = "Stereo Rotation Standstill and Related Illusions";
                ShowWheelRotationControls(true);
                ShowMaskRotatingControls(true);
                break;
            case DepthMaps.ScalingRings:
                ShowWheelScalingControls(true);
                ShowMaskRotatingControls(true);
                textureDD.onValueChanged.Invoke(textureDD.value);
                markerLb.text = "Circles";
                illusionTitle.text = "Stereo Scaling and Related Illusions";
                break;
            case DepthMaps.RotatingRHSpirals:
                ShowWheelRotationControls(true);
                ShowMaskRotatingControls(true);
                illusionTitle.text = "Stereo Spiraling Motion and Related Illusions";
                break;
            case DepthMaps.RotatingLHSpirals:
                ShowWheelRotationControls(true);
                ShowMaskRotatingControls(true);
                illusionTitle.text = "Stereo Spiraling Motion and Related Illusions";
                break;
            case DepthMaps.ScalingLHSpirals:
                ShowWheelScalingControls(true);
                ShowMaskRotatingControls(true);
                textureDD.onValueChanged.Invoke(textureDD.value);
                markerLb.text = "Ridges";
                illusionTitle.text = "Stereo Spiraling Motion and Related Illusions";
                break;
            case DepthMaps.ScalingRHSpirals:
                ShowWheelScalingControls(true);
                ShowMaskRotatingControls(true);
                textureDD.onValueChanged.Invoke(textureDD.value);
                markerLb.text = "Ridges";
                illusionTitle.text = "Stereo Spiraling Motion and Related Illusions";
                break;
            case DepthMaps.RotatingWheel4Spokes:
                ShowWheelRotationControls(true);
                illusionTitle.text = "Stereo Rotation Standstill and Related Illusions";
                break;
            case DepthMaps.RotatingWheel8Spokes:
                ShowWheelRotationControls(true);
                ShowMaskRotatingControls(true);
                illusionTitle.text = "Stereo Rotation Standstill and Related Illusions";
                break;
            case DepthMaps.RotatingWheel16Spokes:
                ShowWheelRotationControls(true);
                ShowMaskRotatingControls(true);
                illusionTitle.text = "Stereo Rotation Standstill and Related Illusions";
                break;
            case DepthMaps.RotatingWheel32Spokes:
                ShowWheelRotationControls(true);
                ShowMaskRotatingControls(true);
                illusionTitle.text = "Stereo Rotation Standstill and Related Illusions";
                break;
            case DepthMaps.ShearingSectors:
                ShowWheelShearingControls(true);
                ShowMaskShearingControls(true);
                illusionTitle.text = "Stereo Shearing Standstill and Related Illusions";
                break;
            case DepthMaps.ShearingRings:
                markerLb.text = "Circles";
                crossTg.gameObject.SetActive(true);
                ShowWheelShearingControls(true); illusionTitle.text = "Stereo Shearing Standstill and Related Illusions";
                ShowMaskShearingControls(true);
                break;
            case DepthMaps.ShearingLHSpirals:
                ShowWheelShearingControls(true); illusionTitle.text = "Stereo Shearing Standstill and Related Illusions";
                ShowMaskShearingControls(true);
                break;
            case DepthMaps.ShearingRHSpirals:
                ShowWheelShearingControls(true); illusionTitle.text = "Stereo Shearing Standstill and Related Illusions";
                ShowMaskShearingControls(true);
                break;
            default:
                break;

        }

        //UI Controls start settings
    }

    private void ShowWheelRotationControls(bool isOn)
    {
        if (isOn)
        {
            wheelMotion |= MotionType.Rotation;
            wheelRotSpeedLb.gameObject.SetActive(true);
            wheelRotSpeedSl.gameObject.SetActive(true);
        }
        else
        {
            wheelRotSpeedLb.gameObject.SetActive(false);
            wheelRotSpeedSl.gameObject.SetActive(false);
        }
    }

    private void ShowWheelScalingControls(bool isOn)
    {
        if (isOn)
        {
            wheelMotion |= MotionType.Scaling;
            markerLb.text = "Circles";
            crossTg.gameObject.SetActive(true);
            wheelScalingFreqLb.gameObject.SetActive(true);
            wheelScalingFreqSl.gameObject.SetActive(true);
        }
        else
        {
            wheelScalingFreqLb.gameObject.SetActive(false);
            wheelScalingFreqSl.gameObject.SetActive(false);
            markerLb.text = "Markers";
            crossTg.isOn = false;
            crossTg.gameObject.SetActive(false);
        }
    }

    private void ShowWheelShearingControls(bool isOn)
    {
        if (isOn)
        {
            wheelMotion |= MotionType.Shearing;
            wheelShearingAmpLb.gameObject.SetActive(true);
            wheelShearingAmpSl.gameObject.SetActive(true);
            wheelShearingFreqLb.gameObject.SetActive(true);
            wheelShearingFreqSl.gameObject.SetActive(true);
            wheelShearingAngLb.gameObject.SetActive(true);
            wheelShearingAngSl.gameObject.SetActive(true);
        }
        else
        {
            wheelShearingAmpLb.gameObject.SetActive(false);
            wheelShearingAmpSl.gameObject.SetActive(false);
            wheelShearingFreqLb.gameObject.SetActive(false);
            wheelShearingFreqSl.gameObject.SetActive(false);
            wheelShearingAngLb.gameObject.SetActive(false);
            wheelShearingAngSl.gameObject.SetActive(false);
        }
    }

    private void ShowMaskRotatingControls(bool isOn)
    {
        if (isOn)
        {
            maskMotion |= MotionType.Rotation;
            maskRotSpeedLb.gameObject.SetActive(true);
            maskRotSpeedSl.gameObject.SetActive(true);
            ShouldIShowOffsetControls();
        }
        else
        {
            maskMotion &= ~MotionType.Rotation;
            maskRotSpeedSl.value = 0f;
            maskRotSpeedLb.gameObject.SetActive(false);
            maskRotSpeedSl.gameObject.SetActive(false);
        }
    }

    private void ShowMaskScalingControls(bool isOn)
    {
        if (isOn)
        {
            maskMotion |= MotionType.Scaling;
            maskScalingFreqLb.gameObject.SetActive(true);
            maskScalingFreqSl.gameObject.SetActive(true);
        }
        else
        {
            maskMotion &= ~MotionType.Scaling;
            maskScalingFreqSl.value = 0f;
            maskScalingFreqLb.gameObject.SetActive(false);
            maskScalingFreqSl.gameObject.SetActive(false);
        }
    }
    
    private void ShowMaskShearingControls(bool isOn)
    {
        if(isOn)
        {
            maskMotion |= MotionType.Shearing;
            maskShearingAmpLb.gameObject.SetActive(true);
            maskShearingAmpSl.gameObject.SetActive(true);
            maskShearingFreqLb.gameObject.SetActive(true);
            maskShearingFreqSl.gameObject.SetActive(true);
            maskShearingAngLb.gameObject.SetActive(true);
            maskShearingAngSl.gameObject.SetActive(true);
            ShouldIShowOffsetControls();
        }
        else
        {  
            maskMotion &= ~MotionType.Shearing;
            maskShearingAmpSl.value = 0f;
            //maskShearingAmpSl.onValueChanged.Invoke(0f);
            maskShearingAmpLb.gameObject.SetActive(false);
            maskShearingAmpSl.gameObject.SetActive(false);
            maskShearingFreqLb.gameObject.SetActive(false);
            maskShearingFreqSl.gameObject.SetActive(false);
            maskShearingAngLb.gameObject.SetActive(false);
            maskShearingAngSl.gameObject.SetActive(false);

        }
    }

    public void OnChangedTexture(int iTex)
    {
        
        Textures idTex = (Textures)iTex;
        //switch flicker off to avoid offsets problems
        flickerTg.onValueChanged.Invoke(false);
        flickerTg.isOn = false;
        ShowMaskScalingControls(false);
        flickerTg.gameObject.SetActive(false);
        wheelMotion = wheelMotion & (~MotionType.Scaling); 
        switch (idTex)
        {
            case Textures.RandomDots:
                flickerTg.gameObject.SetActive(true);
                ChangeToBestTextureScale(100f);
                break;
            case Textures.PeriodicRandomDots:
                if (maskXOffsetSl != null)
                {
                    maskXOffsetSl.onValueChanged.Invoke(0f);
                    maskYOffsetSl.onValueChanged.Invoke(0f);
                }
                ShowOffsetControls(false);
                ShowMaskScalingControls(true);
                ChangeToBestTextureScale(50f);
                break;
            case Textures.AshLeaves:
                ChangeToBestTextureScale(50f);
                break;
            case Textures.BakhtiariRug:
                ChangeToBestTextureScale(50f);
                break;
            default:
                break;
        }
        disp.OnChangedTexture(iTex);
    }
    private void ChangeToBestTextureScale(float scalePct)
    {
        textureScaleSl.onValueChanged.Invoke(scalePct);
        textureScaleSl.value = scalePct;
    }

    public void OnChangedWheelRotVel(float vel)
    {
        disp.wheelRotVel = vel;
        wheelRotSpeedLb.text = "Wheel Rotational Velocity: " + disp.wheelRotVel.ToString("F1") + " °/s";
    }

    public void OnChangedWheelScalingFreq(float freq)
    {
        disp.wheelScalingFreq = freq;
        wheelScalingFreqLb.text = "Wheel Scaling Frequency: " + disp.wheelScalingFreq.ToString("F2") + " Hz";
    }
   
    public void OnChangedWheelShearingAmp(float ampPct)
    {
        disp.OnChangedWheelShearAmp(ampPct);
        wheelShearingAmpLb.text = "Wheel Shearing Amplitude: " + ampPct.ToString("F0") + " %";
    }

    public void OnChangedWheelShearingFreq(float freq)    {
        disp.wheelShearingFreq = freq;
        wheelShearingFreqLb.text = "Wheel Shearing Frequency: " + disp.wheelShearingFreq.ToString("F2") + " Hz";
    }

    public void OnChangedWheelShearingAng(float ang)
    {
        disp.wheelShearingAngle = ang;
        wheelShearingAngLb.text = "Wheel Shearing Angle: " + disp.wheelShearingAngle.ToString("F0") + " °";
    }

    public void OnChangedWheelSize(float sizePct)
    {
        disp.OnChangedWheelSize(sizePct);
        wheelSizeLb.text = "Wheel Size: " + sizePct.ToString("F2") + " %";
    }

    public void OnChangedMaxDisparity(float nPels)
    {
        disp.OnChangedMaxDisparity(nPels);
        maxDisparityLb.text = "Maximal Disparity: " + nPels.ToString() + " pel";
    }

    public void OnChangedMarkerTg(bool isOn)
    {
        if (isOn)
        {
            disp.OnChangedCrossTg(false);
            crossTg.isOn = false;
        }
        disp.OnChangedMarkerTg(isOn);
        markerTg.isOn = isOn;

    }

    public void OnChangedCrossTg(bool isOn)
    {
        if (isOn)
        {
            disp.OnChangedMarkerTg(false);
            markerTg.isOn = false;
        }
        disp.OnChangedCrossTg(isOn);
        crossTg.isOn = isOn;

    }

    public void OnChangedMaskRotVel(float vel)
    {
        disp.maskRotVel = vel;
        maskRotSpeedLb.text = "Mask Rotational Velocity: " + vel.ToString("F1") + " °/s";
        ShouldIShowOffsetControls();
    }

    private void ShouldIShowOffsetControls()
    {
        bool isOn;
        if ((Mathf.Abs(disp.maskRotVel) > 0.1f) 
            && (flickerTg.isOn == false) 
            && ((maskMotion & MotionType.Scaling) == 0)
            //|| (Mathf.Abs(disp.maskScalingFreq) > 0.01f)
            || (Mathf.Abs(disp.maskShearingAmp) > 0.01f))
            isOn = true;
        else
        {
            isOn = false;
        }
        ShowOffsetControls(isOn);
    }

    private void ShowOffsetControls(bool isOn)
    {
        if (isOn)
        {
            maskXOffsetLb.gameObject.SetActive(true);
            maskXOffsetSl.gameObject.SetActive(true);
            maskYOffsetLb.gameObject.SetActive(true);
            maskYOffsetSl.gameObject.SetActive(true);

        }
        else
        {
            maskXOffsetLb.gameObject.SetActive(false);
            maskXOffsetSl.gameObject.SetActive(false);
            maskYOffsetLb.gameObject.SetActive(false);
            maskYOffsetSl.gameObject.SetActive(false);
        }
    }
    
    /*************************************************************
    private void ShowAddedMaskShearingControls(bool isOn)
    {
        if (isOn)
        {
            maskShearingFreqLb.gameObject.SetActive(true);
            maskShearingFreqSl.gameObject.SetActive(true);
            maskShearingAngLb.gameObject.SetActive(true);
            maskShearingAngSl.gameObject.SetActive(true);
            //maskRotSpeedLb.gameObject.SetActive(false);
            //maskRotSpeedSl.gameObject.SetActive(false);
        }
        else
        {
            maskShearingFreqLb.gameObject.SetActive(false);
            maskShearingFreqSl.gameObject.SetActive(false);
            maskShearingAngLb.gameObject.SetActive(false);
            maskShearingAngSl.gameObject.SetActive(false);
        }
    }
    *****************************************************/

    public void OnChangedMaskScalingFreq(float freq)
    {
        disp.maskScalingFreq = freq;
        maskScalingFreqLb.text = "Mask Scaling Frequency: " + freq.ToString("F2") + " Hz";
        ShouldIShowOffsetControls();
    }

    public void OnChangedMaskShearingAmp(float ampPct)
    {
        //bool bShow = ampPct > 1f;
        //if (!bShow)
        //    ampPct = 0f;
        disp.OnChangedMaskShearingAmp(ampPct);
        maskShearingAmpLb.text = "Mask Shearing Amplitude: " + ampPct.ToString("F2") + " %";
        ShouldIShowOffsetControls();
    }

    public void OnChangedMaskShearingFreq(float freq)
    {
        disp.maskShearingFreq = freq;
        maskShearingFreqLb.text = "Mask Shearing Frequency: " + disp.maskShearingFreq.ToString("F1") + " Hz";
    }

    public void OnChangedMaskShearingAng(float ang)
    {
        disp.maskShearingAngle = ang;
        maskShearingAngLb.text = "Mask Shearing Angle: " + disp.maskShearingAngle.ToString("F0") + " °";
    }

    public void OnChangedMaskSize(float sizePct)
    {
        disp.OnChangedMaskSize(sizePct);
        maskSizeLb.text = "Mask Size: " + sizePct.ToString("F0") + " %";
    }

    public void OnChangedMaskXOffset(float xPct)
    {
        if (textureScaleSl.value < 100f)
        {
            textureScaleSl.onValueChanged.Invoke(100f);
            textureScaleSl.value = 100f;
        }
        disp.OnChangedMaskXOffset(xPct);
        maskXOffsetLb.text = "Mask's center X-Offset: " + xPct.ToString("F2") + " %";
    }

    public void OnChangedMaskYOffset(float yPct)
    {
        if (textureScaleSl.value < 100f)
        {
            textureScaleSl.onValueChanged.Invoke(100f);
            textureScaleSl.value = 100f;
        }
        disp.OnChangedMaskYOffset(yPct);
        maskYOffsetLb.text = "Mask's center Y-Offset: " + yPct.ToString("F2") + " %";
    }


    public void OnChangedTextureScale(float pctScale)
    {
        if (pctScale < 100f)
        {
            if ((Mathf.Abs(maskXOffsetSl.value) > 0f) 
                || (Mathf.Abs(maskYOffsetSl.value) > 0f) 
                || flickerTg.isOn)
                pctScale = 100f;
            textureScaleSl.value = pctScale;
        }
        disp.OnChangedTextureScale(pctScale);
        textureScaleLb.text = "Texture Scale: " + pctScale.ToString("F0") + " %";
    }

   public void OnChangedFlickerTg(bool isOn)
   {
        maskXOffsetSl.value = 0f;
        maskYOffsetSl.value = 0f;
        disp.OnChangedPseudoFlickerToggle(isOn);
        if (isOn)
        {
            flickerMaskMotion = maskMotion; // Savw motion flags for restoring it after leaving flicker
            ShowMaskRotatingControls(false);
            ShowMaskShearingControls(false);
            maskSizeSl.gameObject.SetActive(false);
            maskSizeLb.gameObject.SetActive(false);
            ShowOffsetControls(false);
            if (textureScaleSl.value < 100f)
            {
                textureScaleSl.onValueChanged.Invoke(100f);
                textureScaleSl.value = 100f;
            }
        }
        else
        {
            maskSizeSl.gameObject.SetActive(true);
            maskSizeLb.gameObject.SetActive(true);

            if ((flickerMaskMotion & MotionType.Rotation) > 0)
            {
                ShowMaskRotatingControls(true);
                //ShowOffsetControls(true);
            }
            if ((flickerMaskMotion & MotionType.Shearing) > 0)
            {
                ShowMaskShearingControls(true);
                //ShowOffsetControls(true);
            }

        }
    }

    public void ReenableControls()
    {
        //wheelRotSpeedLb.gameObject.SetActive(true);
        wheelRotSpeedSl.gameObject.SetActive(true);
        //wheelScalingFreqLb.gameObject.SetActive(true);
        wheelScalingFreqSl.gameObject.SetActive(true);
        //maskScalingFreqLb.gameObject.SetActive(true);
        maskScalingFreqSl.gameObject.SetActive(true);

    }
 
    public void OnClickedReset()
    {
        //ReenableControls();
        flickerTg.onValueChanged.Invoke(false);
        flickerTg.isOn = false;
        markerTg.onValueChanged.Invoke(false);
        markerTg.isOn = false;
        maskXOffsetSl.onValueChanged.Invoke(0f);
        maskXOffsetSl.value = 0f;
        maskYOffsetSl.onValueChanged.Invoke(0f);
        maskYOffsetSl.value = 0f;
        ShowOffsetControls(false);
        wheelRotSpeedSl.onValueChanged.Invoke(12f);
        wheelRotSpeedSl.value = 12f;
        wheelScalingFreqSl.onValueChanged.Invoke(0.4f);
        wheelScalingFreqSl.value = 0.4f;
        wheelShearingAmpSl.onValueChanged.Invoke(20f);
        wheelShearingAmpSl.value = 20f;
        wheelSizeSl.onValueChanged.Invoke(100f);
        wheelSizeSl.value = 100f;
        maxDisparitySl.onValueChanged.Invoke(15f);
        maxDisparitySl.value = 15f;
        maskRotSpeedSl.onValueChanged.Invoke(0f);
        maskRotSpeedSl.value = 0f;
        maskScalingFreqSl.onValueChanged.Invoke(0f);
        maskScalingFreqSl.value = 0f;
        maskShearingAmpSl.onValueChanged.Invoke(0f);
        maskShearingAmpSl.value = 0f;
        maskShearingFreqSl.onValueChanged.Invoke(0.4f);
        maskShearingFreqSl.value = 0.4f;
        maskShearingAngSl.onValueChanged.Invoke(0f);
        maskSizeSl.onValueChanged.Invoke(100f);
        maskSizeSl.value = 100f;
        textureScaleSl.onValueChanged.Invoke(100f);
        textureScaleSl.value = 100f;
        OnChangedDepthMap(0);
        depthMapDD.value = 0;
        OnChangedTexture(0);
        textureDD.value = 0;
    }


    void OnDestroy()
    {

        ReenableControls();
        depthMapDD.onValueChanged.RemoveAllListeners();
        textureDD.onValueChanged.RemoveAllListeners();
        wheelRotSpeedSl.onValueChanged.RemoveAllListeners();
        wheelScalingFreqSl.onValueChanged.RemoveAllListeners();
        wheelShearingAmpSl.onValueChanged.RemoveAllListeners();
        wheelShearingFreqSl.onValueChanged.RemoveAllListeners();
        wheelShearingAngSl.onValueChanged.RemoveAllListeners();
        wheelSizeSl.onValueChanged.RemoveAllListeners();
        maxDisparitySl.onValueChanged.RemoveAllListeners();
        markerTg.onValueChanged.RemoveAllListeners();
        crossTg.onValueChanged.RemoveAllListeners();
        maskRotSpeedSl.onValueChanged.RemoveAllListeners();
        maskScalingFreqSl.onValueChanged.RemoveAllListeners();
        maskShearingAmpSl.onValueChanged.RemoveAllListeners();
        maskShearingFreqSl.onValueChanged.RemoveAllListeners();
        maskShearingAngSl.onValueChanged.RemoveAllListeners();
        textureScaleSl.onValueChanged.RemoveAllListeners();
        maskSizeSl.onValueChanged.RemoveAllListeners();
        flickerTg.onValueChanged.RemoveAllListeners();
        resetButton.onClick.RemoveAllListeners();
    }
}
