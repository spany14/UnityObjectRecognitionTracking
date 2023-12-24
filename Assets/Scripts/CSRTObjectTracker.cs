using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class CSRTObjectTracker : MonoBehaviour
{
    public static Vector2 cameraRes;
    private static bool startTracking = false;

    //request ID,height,width and FPS
    int webCamDeviceNo;
    public int webCamHeight;
    public int webCamWidth;
    public int webCamFPS;
    Color32[] rawImage;

    //define texture for camera,
    //colour[] stores RGBA colours in 32 bit format
    Texture2D textImg;
    private WebCamTexture webCam;
    Texture2D camTexture;
    public RawImage processedImage;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("In Start()");

        //for sedning web cam data from UNity to openCV

        WebCamDevice[] devices = WebCamTexture.devices;
        Debug.Log("No of Webcams : " + devices.Length);

        if (devices.Length>0)
        {
            webCam = new WebCamTexture(devices[webCamDeviceNo].name,webCamWidth,webCamHeight,webCamFPS);
            Debug.Log(devices[webCamDeviceNo].name);
            //start camera
            webCam.Play();

            //assign processed image from opencv
            camTexture = new Texture2D(webCamWidth, webCamHeight);
            processedImage.material.mainTexture = camTexture;

        }
        else
        {
            Debug.Log("No webcam found!");
        }

        startTracking = true;

        if (webCam == null)
        {
            Debug.Log("Webcam Texture is null");
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        //Tracking();

        if (webCam.isPlaying)
        {
            rawImage = webCam.GetPixels32();
            if (rawImage.Length != 0)
            {
                //CSRTrackerInterop.Track_CSRT_1();
                CSRTrackerInterop.Track_CSRT_2(ref rawImage, webCamWidth, webCamHeight);
                camTexture.SetPixels32(rawImage);
                camTexture.Apply();
            }
        }

    }

    void Tracking()
    {
        Debug.Log("Webcam Initilaised. Starting Tracking ...");

        /*if (!startTracking)
        {
            Debug.Log("startTracking set false");
            return;
        }*/

        //CSRTrackerInterop.Track_CSRT_1();
    }

    /*void OnApplicationQuit()
    {
        if (startTracking)
        {
            CSRTrackerInterop.StopTracking();
        }
    }*/
}

internal static class CSRTrackerInterop
{
    [DllImport("OpenCVObjectTracker")]
    internal static extern int Initialise(ref int ouputCameraWidth, ref int outputCameraHeight);

    [DllImport("OpenCVObjectTracker")]
    internal static extern void StopTracking();

    [DllImport("OpenCVObjectTracker")]
    internal unsafe static extern void Track_CSRT();

    [DllImport("OpenCVObjectTracker")]
    internal unsafe static extern void Track_CSRT_1(); // with predefined bounding boxes

    [DllImport("OpenCVObjectTracker")]
    internal static extern void Track_CSRT_2(ref Color32[] rawImage, int width, int height); // with passing camera data from unity
}
