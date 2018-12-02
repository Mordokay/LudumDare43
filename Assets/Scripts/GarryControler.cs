using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarryControler : MonoBehaviour {

    float strength;
    float energy;
    float psychic;
    float health;

    Vector3 goalPos;
    public int currentPathPoint;

    MapMaker mapMaker;
    public float garrySpeed;

    bool isWalking;
    bool isAtacking;
    bool isRecovering;

    private void Start()
    {
        mapMaker = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapMaker>();
        currentPathPoint = 0;
        strength = 1;
        energy = 1;
        psychic = 1;
        health = 100;
        transform.position = new Vector3(transform.position.x, 0.85f, transform.position.z);
    }

    void Update ()
    {
        if (mapMaker.gameStarted)
        {
            if (mapMaker.garryPath.Count > 2 && currentPathPoint < mapMaker.garryPath.Count)
            {
                if (Mathf.Abs(mapMaker.garryPath[currentPathPoint].transform.position.x - this.transform.position.x) < 0.05f &&
                Mathf.Abs(mapMaker.garryPath[currentPathPoint].transform.position.z - this.transform.position.z) < 0.05f)
                {
                    currentPathPoint += 1;
                    goalPos = mapMaker.garryPath[currentPathPoint].transform.position;

                    this.transform.LookAt(new Vector3(goalPos.x, this.transform.position.y, goalPos.z));
                    this.transform.Rotate(new Vector3(0, -90, 0));
                    this.GetComponent<Animator>().SetTrigger("walking");
                }
                else
                {
                    // The step size is equal to speed times frame time.
                    float step = garrySpeed * Time.deltaTime;

                    // Move our position a step closer to the target.
                    transform.position += (new Vector3(goalPos.x, 0.85f, goalPos.z) - transform.position).normalized *  step;
                }
            }
        }
	}
}
