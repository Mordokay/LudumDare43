using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarryControler : MonoBehaviour {

    float strength;
    float energy;
    float psychic;
    float health;

    int currentPathPoint;

    private void Start()
    {
        currentPathPoint = 0;
        strength = 1;
        energy = 1;
        psychic = 1;
        health = 100;
    }

    void Update ()
    {

	}
}
