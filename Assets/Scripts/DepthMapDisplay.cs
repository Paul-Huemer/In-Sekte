using UnityEngine;
using Windows.Kinect;
using OpenCvSharp;
using OpenCvSharp.Demo;


public class DepthMapDisplay : MonoBehaviour
{
    public Material depthMapMaterial; // Material to apply the depth map texture
    public GameObject Surface; // Surface to apply the depth map texture

    private KinectSensor sensor;
    private DepthFrameReader depthReader;
    private BodyFrameReader bodyReader;
    private ushort[] depthData;
    private Texture2D depthTexture;
    private Mat image;

    public float thresholdValue = 1000.0f;

    void Start()
    {
        sensor = KinectSensor.GetDefault();
        if (sensor != null)
        {
            depthReader = sensor.DepthFrameSource.OpenReader();
            bodyReader = sensor.BodyFrameSource.OpenReader();
            if (!sensor.IsOpen)
            {
                sensor.Open();
            }

            var frameDesc = sensor.DepthFrameSource.FrameDescription;
            depthData = new ushort[frameDesc.LengthInPixels];
            depthTexture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);
        }
    }

    void Update()
    {
        if (depthReader != null)
        {
            var depthFrame = depthReader.AcquireLatestFrame();
            if (depthFrame != null)
            {
                depthFrame.CopyFrameDataToArray(depthData);
                depthFrame.Dispose();
                depthFrame = null;
            }
        }

        if (bodyReader != null)
        {
            var bodyFrame = bodyReader.AcquireLatestFrame();
            if (bodyFrame != null)
            {
                Body[] bodies = new Body[sensor.BodyFrameSource.BodyCount];
                bodyFrame.GetAndRefreshBodyData(bodies);
                bodyFrame.Dispose();
                bodyFrame = null;
            }
        }

        UpdateDepthTexture();
    }

    void UpdateDepthTexture()
{
    if (depthData == null)
        return;

    Color[] colors = new Color[depthData.Length];
    ushort minDepth = 200;
    ushort maxDepth = 2100;

    for (int i = 0; i < depthData.Length; i++)
    {
        ushort depth = depthData[i];

        // Map depth to a color value between 0 and 255
        byte colorValue = (depth > minDepth && depth < maxDepth) ? (byte)255 : (byte)0;
        colors[i] = new Color32(colorValue, colorValue, colorValue, 255);
    }

    depthTexture.SetPixels(colors);
    depthTexture.Apply();
    depthMapMaterial.mainTexture = depthTexture; // Apply depth map texture to material

    // Convert depth texture to Mat
    image = OpenCvSharp.Unity.TextureToMat(depthTexture);

    // Perform your OpenCV operations on the 'image' Mat here

    // Apply the processed texture back to a GameObject if needed
    Surface.GetComponent<Renderer>().material.mainTexture = OpenCvSharp.Unity.MatToTexture(image);
}
}