using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalControler : MonoBehaviour {

    public List<Transform> patrolPoints;
    int currentPoint;
    public float animalWaitTime;
    float timeSinceAnimalWaited;
    bool isWaiting;
    public float animalSpeed;

    private void Awake()
    {
        int currentPoint = Random.Range(0, patrolPoints.Count);
        patrolPoints = this.transform.parent.parent.gameObject.GetComponent<AnimalSpawner>().patrolPoints;

        this.transform.position = patrolPoints[currentPoint].position;

        isWaiting = true;
        timeSinceAnimalWaited = Time.timeSinceLevelLoad;
    }

    void Update () {
        if (isWaiting)
        {
            timeSinceAnimalWaited += Time.deltaTime;
            if(timeSinceAnimalWaited > animalWaitTime)
            {
                isWaiting = false;
                timeSinceAnimalWaited = 0.0f;
                currentPoint = Random.Range(0, patrolPoints.Count);
            }

        }
		else if(Vector3.Distance(patrolPoints[currentPoint].position, this.transform.position) < 0.001f)
        {
            this.transform.position = patrolPoints[currentPoint].position;
            isWaiting = true;
        }
        else
        {
            // The step size is equal to speed times frame time.
            float step = animalSpeed * Time.deltaTime;

            // Move our position a step closer to the target.
            transform.position = Vector3.MoveTowards(this.transform.position, patrolPoints[currentPoint].position, step);
        }
	}
}
