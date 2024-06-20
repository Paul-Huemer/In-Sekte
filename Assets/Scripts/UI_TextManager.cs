using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;  // Add this line to use TextMeshPro

public class UI_TextManager : MonoBehaviour
{
    public TextMeshProUGUI currentStatsText;  
    public CreatureSpawner creatureSpawner;  
    public creatureGoal creatureGoal;
    public int maxCreatureCount = 0;

    public int creatureInGoal = 0;





    // Start is called before the first frame update
    void Start()
    {
        creatureSpawner = FindObjectOfType<CreatureSpawner>();  
        maxCreatureCount = creatureSpawner.maxCreatureCount;

        creatureGoal = FindObjectOfType<creatureGoal>();
        creatureInGoal = creatureGoal.creatureCount;



        // seatch for the TextMeshPro text field in the scene
        currentStatsText = FindObjectsOfType<TextMeshProUGUI>().FirstOrDefault(t => t.name == "CurrentStatsText");
        currentStatsText.text = ""; 
    }


    void Update()
    {
        creatureInGoal = creatureGoal.creatureCount;

        currentStatsText.text = "Rescued Creatures: " + creatureInGoal + "/" + maxCreatureCount + " ";
        


    }
}
