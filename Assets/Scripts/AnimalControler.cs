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
        patrolPoints = this.transform.parent.parent.gameObject.GetComponent<AnimalSpawner>().patrolPoints;
        int currentPoint = Random.Range(0, patrolPoints.Count);
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
                this.GetComponent<Animator>().SetBool("isWalking", true);
                Vector3 dir = patrolPoints[currentPoint].position - this.transform.position;
                this.transform.LookAt(patrolPoints[currentPoint].position);
                this.transform.Rotate(new Vector3(0, 90, 0));
                //this.transform.rotation =
                  //          Quaternion.Euler(-90, 0, Vector2.SignedAngle(new Vector2(dir.x, dir.z), new Vector2(0.0f, -1.0f)));

            }

        }
		else if(Vector3.Distance(patrolPoints[currentPoint].position, this.transform.position) < 0.001f)
        {
            this.transform.position = patrolPoints[currentPoint].position;
            isWaiting = true;
            this.GetComponent<Animator>().SetBool("isWalking", false);
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
