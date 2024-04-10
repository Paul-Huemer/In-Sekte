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
    private int processEveryNthFrame = 1; // Change this to process every nth frame if performanc is an issue
    private Vector2[] vectorList;

    [SerializeField] private PolygonCollider2D PolygonCollider;
    [SerializeField] private float CurveAccuracy = 10f;

    [SerializeField] private float MinArea = 500f;


    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        // flip image y axis
        
        

        
        image = OpenCvSharp.Unity.TextureToMat(input);

        Cv2.Flip(image, image, FlipMode.Y);
        Cv2.CvtColor(image, image, ColorConversionCodes.BGR2GRAY);


        Cv2.FindContours(image, out Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);

        // invert it back

        PolygonCollider.pathCount = 0; // Remove all colliders stored in the component

        foreach (Point[] contour in contours)
        {
            Point[] points = Cv2.ApproxPolyDP(contour, CurveAccuracy, true);
            var area = Cv2.ContourArea(contour);
            
            if (area > MinArea)
            {
                drawContour(image, new Scalar(128, 128,128), 2, points);
                // Add the collider to the PolygonCollider component
                PolygonCollider.pathCount++;
                PolygonCollider.SetPath(PolygonCollider.pathCount-1, toVector2(points));
            }
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
