using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleControler : MonoBehaviour {

    public float health;
    float currentHealth;

    GarryControler gc;
    public float strengthUpgrade;

    public GameObject slider;

    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("Garry").GetComponent<GarryControler>();
        currentHealth = health;
    }

    public void TakeDamage(float damage)
    {
        if (!slider.activeSelf)
        {
            slider.SetActive(true);
        }
        slider.transform.parent.LookAt(slider.transform.parent.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);

        currentHealth -= damage;
        slider.GetComponent<Slider>().value = currentHealth / health;
        if (currentHealth < 0)
        {
            gc.UpgradeStrength(strengthUpgrade);
            this.transform.GetComponentInParent<TerrainTile>().hasConstruct = false;
            Destroy(this.gameObject);
        }
    }
}
