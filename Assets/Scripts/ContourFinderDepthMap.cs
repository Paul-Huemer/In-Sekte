using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using OpenCvSharp.Demo;
using Windows.Kinect;



public class ContourFinderDepthMap : WebCamera
{
    private Mat image;
    private int frameCount = 0;
    private int processEveryNthFrame = 2; // Change this to process every nth frame

    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        frameCount++;
        if (frameCount % processEveryNthFrame == 0)
        {
            image = OpenCvSharp.Unity.TextureToMat(input);
        }

        if(output == null)
        {
            output = OpenCvSharp.Unity.MatToTexture(image);
        }
        else
        {
            OpenCvSharp.Unity.MatToTexture(image, output);
        }

        return true;
    }
}
