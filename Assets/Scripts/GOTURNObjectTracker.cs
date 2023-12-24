using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GOTURNObjectTracker : MonoBehaviour
{
    public static Vector2 CameraRes;
    private bool _ready;

    // Start is called before the first frame update
    void Start()
    {
        int camWidth = 0, camHeight = 0;
        int result = GOTURNrackerInterop.Initialise(ref camWidth, ref camHeight);
        if (result < 0)
        {
            if (result == -2)
            {
                Debug.LogWarningFormat("[{0}] Failed to open camera stream.", GetType());
            }
            return;
        }

        CameraRes = new Vector2(camWidth, camHeight);

        _ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_ready)
            return;

        unsafe
        {
            GOTURNrackerInterop.Track_GOTURN();
        }
    }


    void OnApplicationQuit()
    {
        if (_ready)
        {
            GOTURNrackerInterop.StopTracking();
        }
    }

}

internal static class GOTURNrackerInterop
{
    [DllImport("OpenCVObjectTracker")]
    internal static extern int Initialise(ref int ouputCameraWidth, ref int outputCameraHeight);

    [DllImport("OpenCVObjectTracker")]
    internal static extern void StopTracking();

    [DllImport("OpenCVObjectTracker")]
    internal unsafe static extern void Track_GOTURN();
}
