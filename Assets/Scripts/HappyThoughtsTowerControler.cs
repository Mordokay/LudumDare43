using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappyThoughtsTowerControler : MonoBehaviour {

    public GameObject orbOnTower;

    public bool isLoaded;
    public float timeSinceLastThrow;
    public float timeBetweenThrows;
    GameObject garry;
    PlayerData pd;

    public GameObject ProjectilePos;
    GameObject projectilePrefab;
    public Slider timeBeforeOperationalSlider;

    public void Start()
    {
        projectilePrefab = Resources.Load("OrbBullet", typeof(GameObject)) as GameObject;
        pd = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerData>();
        garry = GameObject.FindGameObjectWithTag("Garry");
        timeSinceLastThrow = 0.0f;
    }

    public void LoadOrb()
    {
        isLoaded = true;
        orbOnTower.SetActive(true);
    }

    public void Interact()
    {
        if (pd != null && pd.tears > 0)
        {
            //Decrements flesh
            pd.DecrementInventory(0, 0, 0, 1);

            LoadOrb();
        }
    }

    void Update()
    {
        timeSinceLastThrow += Time.deltaTime;

        timeBeforeOperationalSlider.transform.parent.LookAt(timeBeforeOperationalSlider.transform.parent.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        timeBeforeOperationalSlider.value = timeSinceLastThrow / timeBetweenThrows;


        if (timeSinceLastThrow > timeBetweenThrows)
        {
            if (isLoaded && garry.GetComponent<GarryControler>().garryActivated)
            {
                //Instantiate projectile and makes projective more towards garry
                GameObject myBullet = Instantiate(projectilePrefab) as GameObject;
                myBullet.transform.position = ProjectilePos.transform.position;
                timeSinceLastThrow = 0.0f;
                isLoaded = false;
            }
        }

        //Removes orb on the catapult after a short time
        if (timeSinceLastThrow > 0.3f && timeSinceLastThrow < 1.0f && orbOnTower.activeSelf && !isLoaded)
        {
            orbOnTower.SetActive(false);
        }
    }
}
