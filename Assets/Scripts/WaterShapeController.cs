using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteAlways]
public class WaterShapeController : MonoBehaviour
{
    // water is from that https://www.youtube.com/watch?v=69sBjqMtZCc&t=334s Thanks MemoryLeak!!!!

    private int CorsnersCount = 2;
    [SerializeField]
    private SpriteShapeController spriteShapeController;
    [SerializeField]
    private GameObject wavePointPref;
    [SerializeField]
    private GameObject wavePoints;

    public CreatureKiller creatureKiller; // Add this line

    public float drownTime = 2.0f;

    public float waterUpdraft = 2.0f;

    [SerializeField]
    [Range(1, 100)]
    private int WavesCount;
    private List<WaterSpring> springs = new();
    // How stiff should our spring be constnat
    public float springStiffness = 0.1f;
    // Slowing the movement over time
    public float dampening = 0.03f;
    // How much to spread to the other springs
    public float spread = 0.006f;


//     void OnValidate()
// {
//     if (gameObject.activeInHierarchy)
//     {
//         StartCoroutine(CreateWaves());
//     }
// }
void Start() { 
    StartCoroutine(CreateWaves());
}

    IEnumerator CreateWaves() {
        foreach (Transform child in wavePoints.transform) {
            StartCoroutine(Destroy(child.gameObject));
        }
        yield return null;
        SetWaves();
        yield return null;
    }
    IEnumerator Destroy(GameObject go) {
        yield return null;
        DestroyImmediate(go);
    }
    private void SetWaves() { 
        Spline waterSpline = spriteShapeController.spline;
        int waterPointsCount = waterSpline.GetPointCount();

        for (int i = CorsnersCount; i < waterPointsCount - CorsnersCount; i++) {
            waterSpline.RemovePointAt(CorsnersCount);
        }

        Vector3 waterTopLeftCorner = waterSpline.GetPosition(1);
        Vector3 waterTopRightCorner = waterSpline.GetPosition(2);
        float waterWidth = waterTopRightCorner.x - waterTopLeftCorner.x;

        float spacingPerWave = waterWidth / (WavesCount+1);

        for (int i = WavesCount; i > 0 ; i--) {
            int index = CorsnersCount;

            float xPosition = waterTopLeftCorner.x + (spacingPerWave*i);
            Vector3 wavePoint = new Vector3(xPosition, waterTopLeftCorner.y, waterTopLeftCorner.z);
            waterSpline.InsertPointAt(index, wavePoint);
            waterSpline.SetHeight(index, 0.1f);
            waterSpline.SetCorner(index, false);
            waterSpline.SetTangentMode(index, ShapeTangentMode.Continuous);

        }


        // loop through all the wave points
        // plus the both top left and right corners
        
        springs = new();
        for (int i = 0; i <= WavesCount+1; i++) {
            int index = i + 1; 
            
            Smoothen(waterSpline, index);

            GameObject wavePoint = Instantiate(wavePointPref, wavePoints.transform, false);
            wavePoint.transform.localPosition = waterSpline.GetPosition(index);

            WaterSpring waterSpring = wavePoint.GetComponent<WaterSpring>();
            waterSpring.Init(spriteShapeController);
            springs.Add(waterSpring);

            // WaveSpring waveSpring = wavePoint.GetComponent<WaveSpring>();
            // waveSpring.Init(spriteShapeController);
        }

        //Splash(5,1f);
    }
    private void Smoothen(Spline waterSpline, int index)
    {
        Vector3 position = waterSpline.GetPosition(index);
        Vector3 positionPrev = position;
        Vector3 positionNext = position;
        if (index > 1) {
            positionPrev = waterSpline.GetPosition(index-1);
        }
        if (index - 1 <= WavesCount) {
            positionNext = waterSpline.GetPosition(index+1);
        }

        Vector3 forward = gameObject.transform.forward;

        float scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;

        Vector3 leftTangent = (positionPrev - position).normalized * scale;
        Vector3 rightTangent = (positionNext - position).normalized * scale;

        SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent, out leftTangent);
        
        waterSpline.SetLeftTangent(index, leftTangent);
        waterSpline.SetRightTangent(index, rightTangent);
    }
    void FixedUpdate()
    {
        foreach(WaterSpring waterSpringComponent in springs) {
            waterSpringComponent.WaveSpringUpdate(springStiffness, dampening);
            waterSpringComponent.WavePointUpdate();
        }

        UpdateSprings();

    }

    private void UpdateSprings() { 
    int count = springs.Count;
    float[] left_deltas = new float[count];
    float[] right_deltas = new float[count];

    for(int i = 0; i < count; i++) {
        if (i > 0) {
            left_deltas[i] = spread * (springs[i].transform.localPosition.y - springs[i-1].transform.localPosition.y);
            springs[i-1].velocity += left_deltas[i];
        }
        if (i < springs.Count - 1) {
            right_deltas[i] = spread * (springs[i].transform.localPosition.y - springs[i+1].transform.localPosition.y);
            springs[i+1].velocity += right_deltas[i];
        }
    }
}
    private void Splash(int index, float speed) { 
        if (index >= 0 && index < springs.Count) {
            springs[index].velocity += speed;
        }
    }

    // create a wave if creature entes the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // print("Collision with " + collision.gameObject.name);
        if (collision.gameObject.GetComponent<Creature>())
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * waterUpdraft, ForceMode2D.Impulse);
            // half the speed of the gameobject
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity / 2;
        }
        // if has BuoyantObject Script then add force to the object to make it float
        if (collision.gameObject.GetComponent<BuoyantObject>())
        {
            // half the speed of the gameobject
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity / 2;
        }
        



    }

    private void OnTriggerStay2D(Collider2D collision)
{
    if (collision.gameObject.GetComponent<BuoyantObject>())
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        // Calculate the depth of the object in the water
        float waterLevel = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;
        float objectBottom = collision.transform.position.y - collision.bounds.extents.y;
        float depth = Mathf.Clamp(waterLevel - objectBottom, 0, Mathf.Infinity);

        // Apply an upward force proportional to the depth of the object in the water
        float buoyancyForce = depth * 10f;
        rb.AddForce(Vector2.up * buoyancyForce, ForceMode2D.Force);

        // Apply a damping force to simulate water resistance
        float dampingForce = rb.velocity.y * 2f;
        rb.AddForce(Vector2.down * dampingForce, ForceMode2D.Force);

        // Apply a torque to make the object rotate towards the upright position
        float rotationDifference = 0 - rb.rotation;
        float torque = rotationDifference * 2f;
        rb.AddTorque(torque);
    }
}

    

    // count time that creature is in the water TimeInDanger += Time.deltaTime;
    // if TimeInDanger > 2.0f then kill the creature
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Creature>())
        {
            collision.gameObject.GetComponent<Creature>().timeInDanger = 0;
        }
    }

    

    

    
}
