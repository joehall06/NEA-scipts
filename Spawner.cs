using UnityEngine;

public class Spawner : MonoBehaviour
{
    // array to hold the different object prefabrications 
    // created within Unity
    public GameObject[] objectPrefabs;

    // float holding the minimum time limit before each spawn
    private float spawnObstacleCountdown;
    private float spawnPUCountdown;

    // boolean allowing the spawning feature to be stopped / resumed
    private bool enableSpawning = true;

    // float holding the y-axis offset value when called
    private float objectOffsetY;
    // float holding the x-axis offset value when called
    private float objectOffsetX;


    // Start is called before the first frame update
    void Start()
    {
        spawnObstacleCountdown = 2f;
        spawnPUCountdown = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnObstacleCheck();
        SpawnPUCheck();
    }

    // checks if obstacles can be spawned
    private void SpawnObstacleCheck()
    {
        if (spawnObstacleCountdown > 0)
        {
            // ensures a minimum timeframe of 1 second is between each obstacle spawning
            spawnObstacleCountdown -= Time.deltaTime;
        }
        else
        {

            // spawns after the time remaining is 0 seconds
            ChooseObsacle();

            // reset the spawn countdown for the next obstacle spawning
            // adds a random number in the range of [0,1] to the countdown timing
            // makes predicting when the next obstacle will spawn harder
            spawnObstacleCountdown = UnityEngine.Random.Range(6, 8);
        }
    }

    // stores offset for x and y values for each unique object
    // when spawned ensure that they are all at a similar level
    private (float objectOffsetY, float objectOffsetX) ObjectOffset(int prefabIndex)
    {
        switch (prefabIndex)
        {
            case 0:
                objectOffsetY = 1.7f;
                objectOffsetX = 21f;
                break;
            case 1:
                objectOffsetY = 2.29f;
                objectOffsetX = 21f;
                break;
            case 2:
                objectOffsetY = 0f;
                objectOffsetX = 21f;
                break;
            case 3:
                objectOffsetY = 0.05f;
                objectOffsetX = 21f;
                break;
            case 4:
                objectOffsetY = 1.75f;
                objectOffsetX = 18f;
                break;
            case 5:
                objectOffsetY = 2.7f;
                objectOffsetX = 18f;
                break;
            case 6:
                objectOffsetY = 3.5f;
                objectOffsetX = 21f;
                break;
            case 7:
                objectOffsetY = 2.95f;
                objectOffsetX = 25f;
                break;
            case 8:
                objectOffsetY = 1.65f;
                objectOffsetX = 21f;
                break;
            case 9:
                objectOffsetY = 4.66f;
                objectOffsetX = 21f;
                break;
            case 10:
                objectOffsetY = 2.2f;
                objectOffsetX = 21f;
                break;
            case 11:
                objectOffsetY = 2.8f;
                objectOffsetX = 21f;
                break;
            case 12:
                objectOffsetY = 2.5f;
                objectOffsetX = 26f;
                break;
            case 13:
                objectOffsetY = 2.74f;
                objectOffsetX = 22f;
                break;
            case 14:
                objectOffsetY = -0.62f;
                objectOffsetX = 29f;
                break;
            case 15:
                objectOffsetY = 1.97f;
                objectOffsetX = 23f;
                break;
            case 16:
                objectOffsetY = 2.17f;
                objectOffsetX = 21f;
                break;
            case 17:
                objectOffsetY = 1.29f;
                objectOffsetX = 24f;
                break;
            case 18:
                objectOffsetY = 1.82f;
                objectOffsetX = 21f;
                break;
            case 19:
                objectOffsetY = 9.08f;
                objectOffsetX = 31f;
                break;
        }

        // returns the y-axis offset value to be used in a calculation
        return (objectOffsetY, objectOffsetX);
    }

    // spawns object
    private void ChooseObsacle()
    {
        if (enableSpawning) 
        {
            // generates a random variable between 0 (inclusive) and 20 (exclusive)
            // for each game object within the 'objectPrefabs' array
            int randomIndex = UnityEngine.Random.Range(0, 19);

            // retrieves the values held in the function 'ObjectOffset'
            (float offsetY, float offsetX) = ObjectOffset(randomIndex);

            // transforms the position of the object when spawned to a new position
            // uses the prefab y-axis adjustments contained in a function (ObjectOffsetY)
            transform.position = transform.position + new Vector3(offsetX, offsetY, 0);

            // spawm that object 
            // spawned at spawner's position
            // 'Quaternion' regards the objects rotation (in this case, default)
            Instantiate(objectPrefabs[randomIndex], transform.position, Quaternion.identity);

            // resets the position of the object before the next object is spawned
            // stops the objects from increasingly gaining height
            transform.position = transform.position + new Vector3(-offsetX, -offsetY, 0);
        }
    }

    // checks if power up can be spawned
    private void SpawnPUCheck()
    {
        if (spawnPUCountdown > 0)
        {
            // ensures a minimum timeframe of 1 second is between each obstacle spawning
            spawnPUCountdown -= Time.deltaTime;
        }
        else
        {

            // spawns after the time remaining is 0 seconds
            ChoosePUObject();

            // reset the spawn countdown for the next obstacle spawning
            // adds a random number in the range of [0,1] to the countdown timing
            // makes predicting when the next obstacle will spawn harder
            spawnPUCountdown = UnityEngine.Random.Range(15, 25);
        }
    }

    // spawns power up
    private void ChoosePUObject()
    {
        if (enableSpawning)
        {
            // generates a random variable between 20 (inclusive) and 24 (exclusive)
            // for each game object within the 'objectPrefabs' array
            int randomIndex = UnityEngine.Random.Range(20, 24);

            float offsetY = UnityEngine.Random.Range(0.5f, 3f);

            // transforms the position of the object when spawned to a new position
            // uses the prefab y-axis adjustments contained in a function (ObjectOffsetY)
            transform.position = transform.position + new Vector3(0, offsetY, 0);

            // spawm that object 
            // spawned at spawner's position
            // 'Quaternion' regards the objects rotation (in this case, default)
            Instantiate(objectPrefabs[randomIndex], transform.position, Quaternion.identity);

            // resets the position of the object before the next object is spawned
            // stops the objects from increasingly gaining height
            transform.position = transform.position + new Vector3(0, -offsetY, 0);
        }
    }
}






