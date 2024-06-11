using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureKiller : MonoBehaviour
{
    public float maxTimeInDanger = 2.0f;
    public float killTime = 2.0f;

    public bool instantKill = false;

    public bool deathParticles = true;

    public bool isWater = false;
    public bool isFire = false;
    public bool isSpike = false;

    public AudioSource DeathObjectAudioSource;

    public AudioClip[] creatureImpaleSounds;
    public AudioClip burnSound;

    void Start()
    {
        // CreatureDeaths/Spikes
        creatureImpaleSounds = Resources.LoadAll<AudioClip>("CreatureSpikeDeath");
        burnSound = Resources.Load<AudioClip>("CreatureFireDeath");
        print("The number of impale sounds is: " + creatureImpaleSounds.Length);
        // Get audioSource from own object if it exists
        if (GetComponent<AudioSource>())
        {
            DeathObjectAudioSource = GetComponent<AudioSource>();
        }


        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(instantKill) {
        if (collision.gameObject.GetComponent<Creature>())
        {
            // add a time in danger
            collision.gameObject.GetComponent<Creature>().isBasicallyDead = true;
            collision.gameObject.GetComponent<Creature>().timeInDanger += Time.deltaTime;

            if(collision.gameObject.GetComponent<Creature>().isInvincible)
            {
                return;
            }

            StartCoroutine(SlowlyKillCreature(collision.gameObject));

            // StartCoroutine(SlowlyKillCreature(collision.gameObject));
        }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!instantKill){
        
        if (collision.gameObject.GetComponent<Creature>())
        {
            // add a time in danger
            collision.gameObject.GetComponent<Creature>().timeInDanger += Time.deltaTime;

            if (collision.gameObject.GetComponent<Creature>().isInvincible)
            {
                return;
            }

            StartCoroutine(SlowlyKillCreature(collision.gameObject));
        }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(!instantKill) {
        if (collision.gameObject.GetComponent<Creature>())
        {
            collision.gameObject.GetComponent<Creature>().timeInDanger = 0;
        }
        }
    }

    public IEnumerator SlowlyKillCreature(GameObject creatureToKill)
    {
        // add a time in danger
        if(!instantKill) {
            creatureToKill.GetComponent<Creature>().timeInDanger += Time.deltaTime;
        if (creatureToKill.GetComponent<Creature>().timeInDanger < maxTimeInDanger)
        {
            yield break;
        }   
        }

         creatureToKill.GetComponent<Creature>().isBasicallyDead = true;
        

        // creatureToKill.GetComponent<Creature>().creatureAudioSource.PlayOneShot(creatureToKill.GetComponent<Creature>().deathSound);
        
        if(deathParticles) {
            if(creatureToKill.GetComponent<Creature>().playedDeathParticles == false)
            {
                creatureToKill.GetComponent<Creature>().deathParticles.Play();
                creatureToKill.GetComponent<Creature>().playedDeathParticles = true;
                if(isFire) {
                    // play random fire sound
                    DeathObjectAudioSource.PlayOneShot(burnSound);

                }
                if(isSpike) {
                    // play random impale sound
                    DeathObjectAudioSource.PlayOneShot(creatureImpaleSounds[Random.Range(0, creatureImpaleSounds.Length)]);
                }
            }
            // creatureToKill.GetComponent<Creature>().deathParticles.Play();
        }


        float timeElapsed = 0;
        Vector3 originalScale = creatureToKill.transform.localScale;
        while (timeElapsed < killTime)
        {
            if (creatureToKill == null)
            {
                yield break;
            }
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / killTime;
            creatureToKill.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }
        // wait 1 second 
        
        yield return new WaitForSeconds(1.0f);
        Destroy(creatureToKill);
    }
}
