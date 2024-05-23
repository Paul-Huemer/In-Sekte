using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureKiller : MonoBehaviour
{
    public float maxTimeInDanger = 2.0f;
    public float killTime = 2.0f;

    public bool instantKill = false;

    public bool deathParticles = true;

    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(instantKill) {
        if (collision.gameObject.GetComponent<Creature>())
        {
            // add a time in danger
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
        

        // creatureToKill.GetComponent<Creature>().creatureAudioSource.PlayOneShot(creatureToKill.GetComponent<Creature>().deathSound);
        
        if(deathParticles) {
            creatureToKill.GetComponent<Creature>().deathParticles.Play();
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
