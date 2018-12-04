using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControler : MonoBehaviour {

    public float speed;
    GameObject garry;
    public bool isOrb;

    private void Start()
    {
        garry = GameObject.FindGameObjectWithTag("Garry");
    }

    void Update () {
        this.transform.position = Vector3.MoveTowards(this.transform.position, garry.transform.position, speed * Time.timeScale);
        if (Vector3.Distance(this.transform.position, garry.transform.position) < 0.1f)
        {
            if (isOrb)
            {
                garry.GetComponent<GarryControler>().GainPositivity(1.0f);
            }
            else
            {
                garry.GetComponent<GarryControler>().TakeDamage(10.0f);
            }
            Destroy(this.gameObject);
        }
	}
}
