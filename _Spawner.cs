using Unity.VisualScripting;
using UnityEngine;

public class _Spawner : MonoBehaviour
{
    // array to hold the different object prefabrications 
    // created within Unity
    public GameObject[] objectPrefabs;

    // array to hold the individual spawn probalities for each project
    // each index matches with the corresponding index in the 'objectPrefabs' array
    private float[] spawnChance;

    // float holding the minimum time limit before each object spawning
    public float spawnCountdown = 1f;

    // boolean allowing the spawning feature to be stopped / resumed
    private bool enableSpawning = true;

    // float holding the y-axis offset value when called
    private float objectOffsetY;


    // Start is called before the first frame update
    void Start()
    {
        // inilitiaze the array to match the size of 'objectPrefabs' array
        spawnChance = new float[objectPrefabs.Length];

        // assigning the spawn chances which can be adjusted
        // uses probality (in %)
        // ensure that the probalities add up to 1
        spawnChance[0] = 0.15f;
        spawnChance[1] = 0.15f;
        spawnChance[2] = 0.15f;
        spawnChance[3] = 0.15f;
        spawnChance[4] = 0.15f;
        spawnChance[5] = 0.15f;
    }

    void Update()
    {
        // continually checks to see if objects can be spawned
        SpawnCheck();
    }

    // checks if obstacles can be spawned
    private void SpawnCheck()
    {
        if (spawnCountdown > 0)
        {
            // ensures a minimum timeframe of 1 second is between each obstacle spawning
            spawnCountdown -= Time.unscaledDeltaTime;
        }
        else
        {

            // spawns after the time remaining is 0 seconds
            ChooseObject();

            // reset the spawn countdown for the next obstacle spawning
            // adds a random number in the range of [0,1] to the countdown timing
            // makes predicting when the next obstacle will spawn harder
            spawnCountdown = 1 + Random.value;  
        }
    }

    // calculates the y-axis offset values for each object
    private float ObjectOffsetY(int prefabIndex)
    {
        if (prefabIndex == 0)
        {
            objectOffsetY = 0.36f;
        }
        else if (prefabIndex == 1) {
            objectOffsetY = 0.07f;
        }
        else if (prefabIndex == 2)
        {
            objectOffsetY = 0.15f;
        }
        else if (prefabIndex == 3)
        {
            objectOffsetY = 1.8f;
        }
        else if (prefabIndex == 4)
        {
            objectOffsetY = 1.3f;
        }
        else if (prefabIndex == 5)
        {
            objectOffsetY = 1.4f;
        }

        // returns the y-axis offset value to be used in a calculation
        return objectOffsetY;
    }

    // chooses which obstacle to spawn
    private void ChooseObject()
    {
        // check is spawning is currently enabled
        if (enableSpawning) 
        {
            // generates a random float between 0 and 1 to determine which object to spawn
            float randomChance = Random.value;

            // loop through each object prefab and its corresponding spawn chance
            for (int i = 0; i < objectPrefabs.Length; i++)
            {
                // check if current object's spawn chance within the array is less than the spawn chance value
                if (randomChance < spawnChance[i])
                {
                    // calls function which spawns the object
                    SpawnObject(i);
                    break;
                }

                // subtract the current spawn chance from the random value
                // reduces the chance of the same obstacle repeating
                randomChance -= spawnChance[i];
            }
        }
    }

    // spawns object
    private void SpawnObject(int index)
    {
        // transforms the position of the object when spawned to a new position
        // uses the prefab y-axis adjustments contained in a function (ObjectOffsetY)
        transform.position = transform.position + new Vector3(0, ObjectOffsetY(index), 0);

        // spawm that object 
        // spawned at spawner's position
        // 'Quaternion' regards the objects rotation (in this case, default)
        Instantiate(objectPrefabs[index], transform.position, Quaternion.identity);

        // resets the position of the object before the next object is spawned
        // stops the objects from increasingly gaining height
        transform.position = transform.position + new Vector3(0, -(ObjectOffsetY(index)), 0);
    }

}


