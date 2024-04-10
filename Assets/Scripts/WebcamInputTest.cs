using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class WebcamInputTest : MonoBehaviour
{
    // Start is called before the first frame update
    WebCamTexture webcamTexture;
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[1].name);
        webcamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.mainTexture = webcamTexture;
    }
}
