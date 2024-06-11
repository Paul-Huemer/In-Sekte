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

    public KeyCode destroyWebCamTexture = KeyCode.D;
    public GameObject webcamTexture;

    void Start()
    {
        // Find the web cam texture object in the scene
        webcamTexture = GameObject.Find("RawImage");
    }



    // Update is called once per frame
    void Update()
    {
        // Reload the level
        if (Input.GetKeyDown(reloadKey))
        {
            print("Reloading the scene");
            // Reload the current scene
            Invoke("DestroyWebCamTexture", 0.8f);
            Invoke("ReloadScene", 1.0f);

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
            // kill 
            // Load the next scene
            print("Loading next scene");
        
            // wait 2 seconds before loading the next scene
            Invoke("DestroyWebCamTexture", 1.8f);
            Invoke("LoadNextScene", 2.0f);
            
        }

        

        
    }

    void DestroyWebCamTexture()
    {
        Destroy(webcamTexture);
    }

    void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    

    // Load the next scene
    void LoadNextScene()
    {
        // if it is the last level then load the first level
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else
        {
            // Load the next scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
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
