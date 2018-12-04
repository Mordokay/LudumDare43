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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Garry"))
        {
            if (isOrb)
            {
                other.gameObject.GetComponent<GarryControler>().GainPositivity(1.0f);
            }
            else
            {
                other.gameObject.GetComponent<GarryControler>().TakeDamage(10.0f);
            }
            Destroy(this.gameObject);
        }
    }

    void Update () {
        this.transform.position = Vector3.MoveTowards(this.transform.position, garry.transform.position, speed * Time.timeScale);
	}
}
