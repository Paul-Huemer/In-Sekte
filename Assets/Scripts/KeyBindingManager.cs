using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindingManager : MonoBehaviour
{
    // R to reload the level
    public KeyCode reloadKey = KeyCode.R;
    
    // Escape to quit the game with pressing ESC twice in 1 second
    public KeyCode quitKey = KeyCode.Escape;

    // Skip to the next level with the N key
    public KeyCode nextLevelKey = KeyCode.N;



    // Update is called once per frame
    void Update()
    {
        // Reload the level
        if (Input.GetKeyDown(reloadKey))
        {
            print("Reloading the scene");
            // Reload the current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        // Quit the game
        if (Input.GetKeyDown(quitKey))
        {
            // Start the coroutine to wait for a second to press the ESC key again
            StartCoroutine(WaitForQuitKey());
        }

        // Skip to the next level
        if (Input.GetKeyDown(nextLevelKey))
        {
            // Load the next scene
            print("Loading next scene");
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1 < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
            } else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
            
        }
    }


    // IEnumerator to wait for a second to press the ESC key again
    IEnumerator WaitForQuitKey()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1.0f);

        if (Input.GetKeyDown(quitKey))
        {
            Debug.Log("Quitting the game");
            Application.Quit();
        }
    }



}
