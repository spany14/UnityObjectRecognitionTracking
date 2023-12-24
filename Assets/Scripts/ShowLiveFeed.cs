using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLiveFeed : MonoBehaviour
{
    
    [SerializeField]
    private RawImage rawImage;

    WebCamTexture camTexture;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // for debugging purposes, prints available devices to the console
        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log("Webcam available: " + devices[i].name);
        }

        //Renderer rend = this.GetComponentInChildren<Renderer>();

        // assuming the first available WebCam is desired
        
        camTexture = new WebCamTexture(devices[0].name,640,480,30);

        //rend.material.mainTexture = tex;
        this.rawImage.texture = camTexture;

        camTexture.Play();

       
    }

    // Update is called once per frame
    void Update()
    {
        if(!(camTexture.isPlaying))
        {
            Debug.LogError("CameraTexture stopped unknowingly.");
        }
        
    }
}
