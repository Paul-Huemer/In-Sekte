using OpenCvSharp;
using OpenCvSharp.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContourFinder : WebCamera 
{
    [SerializeField] private FlipMode ImageFlip;
    [SerializeField] private float Threshold = 96.4f;
    [SerializeField] private bool ShowProcessingImage = true;
    [SerializeField] private float CurveAccuracy = 10f;
    [SerializeField] private float MinArea = 500f;
    [SerializeField] private PolygonCollider2D PolygonCollider;
    
    private Mat image;
    private Mat processImage = new Mat();
    private Point[][] contours;
    private HierarchyIndex[] hierarchy;
    private Vector2[] vectorList;
    
    //Cool processing stuff!

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        
        

        
        
    }
    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        image = OpenCvSharp.Unity.TextureToMat(input);

        
        
        
        Cv2.Flip(image, image, ImageFlip); // Flip the image
        Cv2.CvtColor(image, processImage, ColorConversionCodes.BGR2GRAY); // Convert the image to grayscale
        Cv2.Threshold(processImage, processImage, Threshold, 255, ThresholdTypes.BinaryInv); // Put a threshold on the image
        Cv2.FindContours(processImage, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null); // Find contours from the grayscale image
        
        PolygonCollider.pathCount = 0; // Remove all colliders stored in the component
        foreach (Point[] contour in contours)
        {
            Point[] points = Cv2.ApproxPolyDP(contour, CurveAccuracy, true);
            var area = Cv2.ContourArea(contour);
            
            if (area > MinArea)
            {
                drawContour(processImage, new Scalar(128, 128,128), 2, points);
                // Add the collider to the PolygonCollider component
                PolygonCollider.pathCount++;
                PolygonCollider.SetPath(PolygonCollider.pathCount-1, toVector2(points));
            }
        }
            
        if (output == null)
        {
            output = OpenCvSharp.Unity.MatToTexture(ShowProcessingImage ? processImage : image );
        } else
        {
            OpenCvSharp.Unity.MatToTexture(ShowProcessingImage ? processImage : image, output);
        }
        return true;
    }
    /**
     * toVector2[]
     * takes a OpenCV Point[] Class and converts it into a Unity Vector2[].
     */
    private Vector2[] toVector2(Point[] points)
    {
        vectorList = new Vector2[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            vectorList[i] = new Vector2(points[i].X, points[i].Y);
        }
        return vectorList;
    }
    
    /**
     * drawContour()
     * uses the @param Points to draw onto the @param Image using @param Color and a Line @param Thickness.
     */
    private void drawContour(Mat Image, Scalar Color, int Thickness, Point[] Points)
    {
        for (int i=1; i < Points.Length; i++)
        {
            Cv2.Line(Image, Points[i-1], Points[i], Color, Thickness);
        }
        Cv2.Line(Image, Points[Points.Length-1], Points[0], Color, Thickness);
    }
}
