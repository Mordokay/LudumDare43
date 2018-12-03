using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour {

    public List<Transform> patrolPoints;

    public int animalMaxCount;
    public Transform animalHolder;
    public float animalRespawnTime;
    float timeSinceLastRespawn;
    GameObject animalPrefab;
    public string animalName;

    void Start () {
        animalPrefab = Resources.Load<GameObject>("AnimalObjects/" + animalName);
        patrolPoints = new List<Transform>();
        animalHolder = this.transform.GetChild(1);
        foreach (Transform child in this.transform.GetChild(0))
        {
            patrolPoints.Add(child);
            timeSinceLastRespawn = 0.0f;
        }
	}
	
	void Update () {
        if (animalHolder.childCount < animalMaxCount) {
            timeSinceLastRespawn += Time.deltaTime;
            if (timeSinceLastRespawn > animalRespawnTime)
            {
                //Instantiate bunny
                GameObject myAnimal = Instantiate(animalPrefab, animalHolder) as GameObject;
                timeSinceLastRespawn = 0.0f;
            }
        }
	}
}
