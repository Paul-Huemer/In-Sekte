using UnityEngine;
using Windows.Kinect;

public class DepthMapDisplay : MonoBehaviour
{
    public Material depthMapMaterial; // Material to apply the depth map texture
    public float maxDepth = 4.0f; // Maximum depth value in meters
    public float threshold = 2.0f; // Threshold distance in meters

    private KinectSensor sensor;
    private DepthFrameReader depthReader;
    private BodyFrameReader bodyReader;
    private ushort[] depthData;
    private Texture2D depthTexture;
    private Body[] bodies;

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
                bodies = new Body[sensor.BodyFrameSource.BodyCount];
                bodyFrame.GetAndRefreshBodyData(bodies);
                bodyFrame.Dispose();
                bodyFrame = null;
            }
        }

        UpdateDepthTexture();
    }

    void UpdateDepthTexture()
    {
        if (depthData == null || bodies == null)
            return;

        Color[] colors = new Color[depthData.Length];
        for (int i = 0; i < depthData.Length; i++)
        {
            ushort depth = depthData[i];
            float normalizedDepth = depth / thresholdValue; // Normalize depth value to meters

            // Map depth to black (closer than threshold) or white (farther than threshold)
            byte colorValue = (normalizedDepth <= threshold) ? (byte)0 : (byte)255;
            colors[i] = new Color32(colorValue, colorValue, colorValue, 255);
        }

        depthTexture.SetPixels(colors);
        depthTexture.Apply();
        depthMapMaterial.mainTexture = depthTexture; // Apply depth map texture to material
    }


    int GetDepthIndex(int x, int y)
    {
        var depthFrameDesc = sensor.DepthFrameSource.FrameDescription;
        int depthIndex = y * depthFrameDesc.Width + x;
        return depthIndex;
    }

    void OnDestroy()
    {
        if (depthReader != null)
        {
            depthReader.Dispose();
            depthReader = null;
        }

        if (bodyReader != null)
        {
            bodyReader.Dispose();
            bodyReader = null;
        }

        if (sensor != null)
        {
            if (sensor.IsOpen)
            {
                sensor.Close();
            }
            sensor = null;
        }
    }
}
