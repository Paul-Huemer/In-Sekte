using UnityEngine;
using Windows.Kinect;

public class DepthVisualizer : MonoBehaviour
{
    private KinectSensor _kinectSensor;
    private DepthFrameReader _depthFrameReader;
    private ushort[] _depthData;

    private MeshRenderer _meshRenderer;

    void Start()
    {
        _kinectSensor = KinectSensor.GetDefault();
        _depthFrameReader = _kinectSensor.DepthFrameSource.OpenReader();
        _depthData = new ushort[_kinectSensor.DepthFrameSource.FrameDescription.LengthInPixels];

        _meshRenderer = GetComponent<MeshRenderer>();

        if (_kinectSensor != null)
            _kinectSensor.Open();
    }

    void Update()
    {
        if (_depthFrameReader != null)
        {
            using (var frame = _depthFrameReader.AcquireLatestFrame())
            {
                if (frame != null)
                {
                    frame.CopyFrameDataToArray(_depthData);

                    // Update your mesh vertices based on _depthData here
                    // For example, modify the quad's vertices to represent depth values

                    // Apply the updated mesh to the MeshRenderer
                    // _meshRenderer.material.SetTexture("_MainTex", yourDepthTexture);
                }
            }
        }
    }

    void OnDestroy()
    {
        if (_depthFrameReader != null)
            _depthFrameReader.Dispose();

        if (_kinectSensor != null)
            _kinectSensor.Close();
    }
}