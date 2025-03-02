using System.Collections;
using UnityEngine;

public class BotCarSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] botCarPrefabs; 

    [SerializeField]
    RoadSpawner roadSpawner; 

    float spawnDistance; 

    [SerializeField]
    int carsPerSection;

    private Transform playerTransform; 
    private float laneWidth = 3.4f;
    private int lastRandomLane; 
    private int lastSpawnedCarIndex = -1; 

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(CarSpawner());
    }

    IEnumerator CarSpawner()
    {
        yield return new WaitForSeconds(.1f);
        SpawnCarsAtSections();
    }

    public void SpawnCarsAtSections()
    {
        int playerSectionIndex = GetPlayerSectionIndex(); 

        for (int i = 0; i < roadSpawner.sections.Length; i++)
        {
            GameObject section = roadSpawner.sections[i];

            if (section.activeInHierarchy && i != playerSectionIndex) 
            {
                for (int j = 0; j < carsPerSection; j++)
                {
                    Vector3 spawnPosition = GetValidSpawnPosition(section, j);
                    if (spawnPosition != Vector3.zero)
                    {
                        Instantiate(GetRandomCarPrefab(), spawnPosition, Quaternion.identity);
                    }
                }
            }
        }
    }

    private int GetPlayerSectionIndex()
    {
        for (int i = 0; i < roadSpawner.sections.Length; i++)
        {
            if (roadSpawner.sections[i].activeInHierarchy)
            {
                if (Mathf.Abs(roadSpawner.sections[i].transform.position.z - playerTransform.position.z) < (roadSpawner.sectionLength / 2))
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private Vector3 GetValidSpawnPosition(GameObject section, int carIndex)
    {
        spawnDistance = Random.Range(35, 45);
        int attemptCount = 0;
        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        while (!validPositionFound && attemptCount < 10) 
        {
            int randomValue;
            do
            {
                randomValue = Random.Range(0, 2) == 0 ? -1 : 1; // 0 -> -1, 1 -> 1
            } while (randomValue == lastRandomLane); 

            lastRandomLane = randomValue; 

            float randomLane = randomValue * laneWidth;
            spawnPosition = new Vector3(randomLane, 0f, section.transform.position.z + carIndex * spawnDistance);

            if (CheckDistanceToExistingCars(spawnPosition))
            {
                validPositionFound = true;
            }

            attemptCount++;
        }

        return validPositionFound ? spawnPosition : Vector3.zero; 
    }

    private GameObject GetRandomCarPrefab()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, botCarPrefabs.Length); 
        } while (randomIndex == lastSpawnedCarIndex); 
        lastSpawnedCarIndex = randomIndex; 

        return botCarPrefabs[randomIndex]; 
    }

    private bool CheckDistanceToExistingCars(Vector3 newSpawnPosition)
    {
        BotCarController[] existingCars = FindObjectsOfType<BotCarController>();

        foreach (BotCarController car in existingCars)
        {
            if (Vector3.Distance(car.transform.position, newSpawnPosition) < spawnDistance)
            {
                return false; 
            }
        }

        return true; 
    }
}