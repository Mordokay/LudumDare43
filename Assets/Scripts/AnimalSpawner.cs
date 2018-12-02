using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour {

    public List<Transform> patrolPoints;

    int animalCount;
    public int animalMaxCount;
    public Transform animalHolder;
    public float animalRespawnTime;
    float timeSinceLastRespawn;
    GameObject animalPrefab;
    public string animalName;

    void Start () {
        animalPrefab = Resources.Load<GameObject>("AnimalObjects/" + animalName);
        animalCount = 0;
        patrolPoints = new List<Transform>();
        animalHolder = this.transform.GetChild(1);
        foreach (Transform child in this.transform.GetChild(0))
        {
            patrolPoints.Add(child);
            timeSinceLastRespawn = 0.0f;
        }
	}
	
	void Update () {
        timeSinceLastRespawn += Time.deltaTime;
        if(timeSinceLastRespawn > animalRespawnTime && animalCount < animalMaxCount)
        {
            //Instantiate bunny
            GameObject myAnimal = Instantiate(animalPrefab, animalHolder) as GameObject;
            animalCount += 1;
            timeSinceLastRespawn = 0.0f;
        }
	}
}
