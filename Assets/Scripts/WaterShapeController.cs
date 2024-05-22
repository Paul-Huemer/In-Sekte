using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaterSprite : MonoBehaviour
{
    [SerializeField] 
    private float springStiffness = 0.1f;
    [SerializeField]
    private List<WaterSpring> springs = new();

    private int cornersCount = 2;
    [SerializeField]
    private SpriteShapeController spriteShapeController;
    [SerializeField]
    private int WavesCount = 6;

    private void SetWaves() {
        Spline waterSpline = spriteShapeController.spline;
        int waterPointsCount = waterSpline.GetPointCount();
    }


    void FixedUpdate()
    {
        foreach (WaterSpring waterSpringComponent in springs)
        {
            waterSpringComponent.WaveSpringUpdate(springStiffness, 0.1f);
        }
    }

    void Start()
    {
        SetWaves();
    }

    



    
}
