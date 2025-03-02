using System.Collections;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] sectionsPrefabs;

    GameObject[] sectionsPool = new GameObject[3];
    public GameObject[] sections = new GameObject[3];
    Transform playerCarTransform;
    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);
    public int sectionLength = 300;

    [SerializeField]
    BotCarSpawner botCarSpawner; 

    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabCount = sectionsPrefabs.Length;

        for (int i = 0; i < sectionsPool.Length; i++)
        {
            sectionsPool[i] = Instantiate(sectionsPrefabs[i % prefabCount]);
            sectionsPool[i].SetActive(false);
        }

        // Adding first sections
        for (int i = 0; i < sections.Length; i++)
        {
            GameObject randomSection = GetRandomSectionFromPool();
            randomSection.transform.position = new Vector3(sectionsPool[i].transform.position.x, 0, i * sectionLength);
            randomSection.SetActive(true);
            sections[i] = randomSection;
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            UpdateSectionPositions();
            yield return waitFor100ms;
        }
    }

    void UpdateSectionPositions()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            if (sections[i].transform.position.z - playerCarTransform.position.z < -sectionLength)
            {
                Vector3 lastSectionPosition = sections[i].transform.position;
                sections[i].SetActive(false);
                sections[i] = GetRandomSectionFromPool();
                sections[i].transform.position = new Vector3(lastSectionPosition.x, 0, lastSectionPosition.z + sectionLength * sections.Length);
                sections[i].SetActive(true);

                //Spawning bot cars
                botCarSpawner.SpawnCarsAtSections();
            }
        }
    }

    GameObject GetRandomSectionFromPool()
    {
        int randomIndex = Random.Range(0, sectionsPool.Length);
        bool isNewSectionFound = false;

        while (!isNewSectionFound)
        {
            if (!sectionsPool[randomIndex].activeInHierarchy)
            {
                isNewSectionFound = true;
            }
            else
            {
                randomIndex++;
                if (randomIndex > sectionsPool.Length - 1) randomIndex = 0;
            }
        }

        return sectionsPool[randomIndex];
    }
}