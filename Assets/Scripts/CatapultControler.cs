using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatapultControler : MonoBehaviour {

    public GameObject meatOnCatapult;

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
        projectilePrefab = Resources.Load("MeatBullet", typeof(GameObject)) as GameObject; 
        pd = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerData>();
        garry = GameObject.FindGameObjectWithTag("Garry");
        timeSinceLastThrow = 0.0f;
    }

    public void LoadMeat()
    {
        isLoaded = true;
        meatOnCatapult.SetActive(true);
    }

    public void Interact()
    {
        if (pd != null && pd.flesh > 0)
        {
            //Decrements flesh
            pd.DecrementInventory(0, 1, 0, 0);

            LoadMeat();
        }
    }

    void Update () {
        this.transform.GetChild(0).LookAt(new Vector3(garry.transform.position.x , this.transform.GetChild(0).position.y, garry.transform.position.z));
        this.transform.GetChild(0).Rotate(new Vector3(0, 90, 0));
        this.transform.GetChild(0).localPosition = new Vector3(0.0f, this.transform.GetChild(0).localPosition.y, this.transform.GetChild(0).localPosition.z);
        this.transform.localPosition = new Vector3(0.0f, this.transform.localPosition.y, this.transform.localPosition.z);
        timeSinceLastThrow += Time.deltaTime;

        timeBeforeOperationalSlider.transform.parent.LookAt(timeBeforeOperationalSlider.transform.parent.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        timeBeforeOperationalSlider.value = timeSinceLastThrow / timeBetweenThrows;

    
        if (timeSinceLastThrow > timeBetweenThrows)
        {
            if (isLoaded && garry.GetComponent<GarryControler>().garryActivated)
            {
                this.GetComponent<Animator>().SetTrigger("ThrowBolder");

                //Instantiate projectile and makes projective more towards garry
                GameObject myBullet = Instantiate(projectilePrefab) as GameObject;
                myBullet.transform.position = ProjectilePos.transform.position;
                timeSinceLastThrow = 0.0f;
                isLoaded = false;
            }
        }

        //Removes meat on the catapult after a short time
        if(timeSinceLastThrow > 0.3f && timeSinceLastThrow < 1.0f && meatOnCatapult.activeSelf && !isLoaded)
        {
            meatOnCatapult.SetActive(false);
        }
    }
}
