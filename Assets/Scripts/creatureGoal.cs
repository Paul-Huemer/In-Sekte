using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class creatureGoal : MonoBehaviour
{
    private CreatureSpawner creatureSpawner;
    private int maxCreatureCount;
    public int creatureCount = 0;
    public AudioClip[] creatureWinSounds;
    public ParticleSystem winParticles;

    [SerializeField] public int minCreaturesToWin = 1; // Min Färglets to reach goal for the next scene to load
    [SerializeField] public int sceneBuildIndex = 1; // Scene Index you can look that up in the build menu
    [SerializeField] public GameObject winningText; // WinnignText Object inside the scene that has to be activated

    public Animation GoalWiggleAnimation;
    void Start()
    {
        creatureWinSounds = Resources.LoadAll<AudioClip>("CreatureWin");
        creatureSpawner = GameObject.Find("CreatureSpawner").GetComponent<CreatureSpawner>();
        maxCreatureCount = creatureSpawner.maxCreatureCount;
        winningText.SetActive(false);



    }




    // Update is called once per frame

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Creature>())
        {
            if (collision.gameObject.GetComponent<Creature>().isInvincible == false)
            {
                winParticles.Play();
                GoalWiggleAnimation.Play();

                collision.gameObject.GetComponent<Creature>().creatureAudioSource.PlayOneShot(creatureWinSounds[Random.Range(0, creatureWinSounds.Length)]);
                creatureCount += 1;
                collision.gameObject.GetComponent<Creature>().isInvincible = true;
                StartCoroutine(DestroyCreature(collision.gameObject));

            // Decrement maxCreatureCount in the CreatureSpawner object
                creatureSpawner.maxCreatureCount -= 1;

            }

            
            
            
        }

        if (creatureCount >= minCreaturesToWin)
        {
            print("Switching Scene to  " + sceneBuildIndex);
            winningText.SetActive(true);
            // SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single); // Loads scene specified with sceneBuildIndex

            Invoke("LoadMyLevel", 2);
        }
    }

    private void LoadMyLevel()
    {
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single); // Loads scene specified with sceneBuildIndex
    }
    //  destroy creature by making it smaller gradually
    IEnumerator DestroyCreature(GameObject creatureToDestroy)
{
    float shrinkDuration = 0.5f;
    float timeElapsed = 0;
    Vector3 originalScale = creatureToDestroy.transform.localScale;
    while (timeElapsed < shrinkDuration)
    {
        if (creatureToDestroy != null)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / shrinkDuration;
            creatureToDestroy.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
        }
        else
        {
            yield break;
        }
        yield return null;
    }

    yield return new WaitForSeconds(2);

    if (creatureToDestroy != null)
    {
        Destroy(creatureToDestroy);
    }
}
}
