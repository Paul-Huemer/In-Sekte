using System;
using System.Collections;
using UnityEngine;

public class CigaretteBurning : MonoBehaviour
{

    public GameObject cigaretteMask;
    public GameObject cigarette;

    public bool isOnFire = false;

    public float maxBurnTime = 3f;

    public float minMaskXPos = -2.9f;
private float burnSpeed; // the speed at which the mask moves
public bool isBurned = false;

public GameObject burnSprite;
public GameObject burnMask;

public ParticleSystem IgniteParticle;
public ParticleSystem BurnParticle;

public ParticleSystem CiggySmoke;

public Boolean isFresh = true;






void Start()
{
    cigarette = this.gameObject;
    // calculate the burn speed based on the max burn time
    burnSpeed = Mathf.Abs(minMaskXPos) / maxBurnTime;


}

void Update()
{
    // if burn mask is not active and it is on fire, set it to active
    if (isOnFire) {


    
    if (isFresh)
    {
        // burnMask.SetActive(true);
        cigarette.GetComponent<Animator>().enabled = true;
        burnSprite.GetComponent<SpriteRenderer>().enabled = true;
        CiggySmoke.Play();
        IgniteParticle.Play();
        BurnParticle.Play();
        isFresh = false;
    }
    
    // show burn sprite renderer 
    
    
    // enable animator of the cigarette
    
    
    float step = burnSpeed * Time.deltaTime;
    Vector3 targetLocalPosition = new Vector3(minMaskXPos, cigaretteMask.transform.localPosition.y, cigaretteMask.transform.localPosition.z);
    cigaretteMask.transform.localPosition = Vector3.MoveTowards(cigaretteMask.transform.localPosition, targetLocalPosition, step);
    
    if (cigaretteMask.transform.localPosition.x == minMaskXPos)
    {
        if (!isBurned)
        {
            isBurned = true;
            BurnParticle.Stop();
            CiggySmoke.Stop();

            IgniteParticle.Play();
            // disable the burn sprite totally
            //  burnsprite to get to size 0 within 0.2 seconds
            StartCoroutine(ShrinkBurnSprite());
            


            
            
        }
        }
    }

    IEnumerator ShrinkBurnSprite()
{
    float duration = 0.2f; // duration of the shrinking
    float elapsed = 0; // time elapsed since the start of the shrinking

    Vector3 initialScale = burnSprite.transform.localScale; // initial scale of the burnSprite

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime; // update the elapsed time
        float t = elapsed / duration; // calculate the progress

        // linearly interpolate the scale from the initial scale to 0
        burnSprite.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);

        yield return null; // wait for the next frame
    }

    // ensure the scale is exactly 0 at the end
    burnSprite.transform.localScale = Vector3.zero;
}

}










    
}