using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureKiller : MonoBehaviour
{
    public float killTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Creature>())
        {
            if(collision.gameObject.GetComponent<Creature>().isInvincible)
            {
                return;
            }
            StartCoroutine(SlowlyKillCreature(collision.gameObject));
        }
    }

    IEnumerator SlowlyKillCreature(GameObject creatureToKill)
    {
        float timeElapsed = 0;
        Vector3 originalScale = creatureToKill.transform.localScale;
        while (timeElapsed < killTime)
        {
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
