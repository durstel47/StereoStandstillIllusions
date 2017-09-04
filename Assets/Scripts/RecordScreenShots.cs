using UnityEngine;
using System.Collections;


public class RecordScreenShots : MonoBehaviour {

#if !UNITY_WEBGL
    private bool bRecording;
	private bool bCapturing;
	// The folder we place all screenshots inside.
	// If the folder exists we will append numbers to create an empty folder.
	public string folder;
	public int frameRate = 25;
	public int sizeMultiplier = 1;
	private string realFolder = "";
	private string curName="";
	private int frameCount;
	
	void Awake()
	{
		Resolution res = Screen.currentResolution;
		Screen.SetResolution(res.width, res.height, true, res.refreshRate);
	}
	// Use this for initialization
	void Start () 
	{
		// (real time doesn't influence time anymore)
		Time.captureFramerate = frameRate;
		
		bRecording = false;
		bCapturing = false;
		frameCount = 0;
		folder = "I:\\Unity\\ScreenshotMovieOutput";
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!bRecording)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{	
				bRecording = true;
				StartRecording();
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				bCapturing = false;
				bRecording = false;
			}
		}
		
		if(bCapturing)
		{
			TakeScreenShot();
		}	
	}
	private void StartRecording()
	{
		
		// Find a folder that doesn't exist yet by appending numbers!
		realFolder = folder;
		int count = 1;
		while (System.IO.Directory.Exists(realFolder))
		{
			realFolder = folder + count;
			count++;
		}
		// Create the folder
		System.IO.Directory.CreateDirectory(realFolder);
		frameCount = 0;
		bCapturing = true;
		
	}
	private void TakeScreenShot()
	{
		curName = string.Format("{0}/shot {1:D04}.png", realFolder, frameCount);
		// Capture the screenshot
		Application.CaptureScreenshot(curName, sizeMultiplier);
		frameCount++;
	}
	
#endif
}
