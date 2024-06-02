using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaterSpring : MonoBehaviour
{
    public float velocity = 0;
    public float force = 0;
    // normal height
    private float target_height = 0f;
    public Transform springTransform;
    [SerializeField]
    private SpriteShapeController spriteShapeController = null;
    private int waveIndex = 0;
    private List<WaterSpring> springs = new();

    private Vector3 startPos;

    public ParticleSystem splashParticles;

    public GameObject ShiluetteCollider; // The shiluette 2d collider object

    public AudioClip[] waterCollisionSounds;

    public AudioSource waveAudioSource;

    public void Start()
    {
        ShiluetteCollider = GameObject.Find("ShiluetteCollider");
        waterCollisionSounds = Resources.LoadAll<AudioClip>("CreatureDeaths");
    }




    public void Init(SpriteShapeController ssc) { 
        var index = transform.GetSiblingIndex();
        waveIndex = index+1;
        
        spriteShapeController = ssc;
        velocity = 0;
        target_height = transform.localPosition.y;

        startPos = transform.localPosition;
    }

    public void WaveSpringUpdate(float springStiffness, float dampening) { 
        var localPosition = transform.localPosition;
        // maximum extension
        var x = localPosition.y - target_height;
        var loss = -dampening * velocity;

        force = - springStiffness * x + loss;
        velocity += force;
        transform.localPosition = new Vector3(localPosition.x, localPosition.y + velocity, localPosition.z);
    }

    public void WavePointUpdate() { 
        if (spriteShapeController != null) {
            Spline waterSpline = spriteShapeController.spline;
            Vector3 wavePosition = waterSpline.GetPosition(waveIndex);
            waterSpline.SetPosition(waveIndex, new Vector3(wavePosition.x, transform.localPosition.y, wavePosition.z));
        }
    }

    // create a wave if creature entes the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if playerCollider enters the trigger
        print("Collision with " + collision.gameObject.name); 
        
    // public GameObject PlayerCollider;
        if (collision.gameObject.GetComponent<Creature>())
        {
            
            // play the splash particles on the startpos of the spring in world space 
            splashParticles.transform.position = transform.TransformPoint(startPos);
            splashParticles.Play();
            
            // play dat sound
            waveAudioSource.PlayOneShot(waterCollisionSounds[Random.Range(0, waterCollisionSounds.Length)]);

            velocity = 0.1f;
            WaveSpringUpdate(0.1f, 0.1f);
            WavePointUpdate();
        } else if (collision.gameObject == ShiluetteCollider)
        {
            velocity = 0.01f;
            WaveSpringUpdate(0.01f, 0.01f);
            WavePointUpdate();
        }

    }

    
}