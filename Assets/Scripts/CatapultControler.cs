using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultControler : MonoBehaviour {

    public GameObject meatOnCatapult;

    public bool isLoaded;
    public float timeSinceLastThrow;
    public float timeBetweenThrows;
    GameObject garry;

    public void Start()
    {
        garry = GameObject.FindGameObjectWithTag("Garry");
        timeSinceLastThrow = 0.0f;
    }

    public void LoadMeat()
    {
        isLoaded = true;
        meatOnCatapult.SetActive(true);
    }

	void Update () {
        this.transform.GetChild(0).LookAt(new Vector3(garry.transform.position.x , this.transform.GetChild(0).position.y, garry.transform.position.z));
        this.transform.GetChild(0).Rotate(new Vector3(0, 90, 0));
        this.transform.GetChild(0).localPosition = new Vector3(0.0f, this.transform.GetChild(0).localPosition.y, this.transform.GetChild(0).localPosition.z);
        this.transform.localPosition = new Vector3(0.0f, this.transform.localPosition.y, this.transform.localPosition.z);
        timeSinceLastThrow += Time.deltaTime;
        if (timeSinceLastThrow > timeBetweenThrows)
        {
            if (isLoaded)
            {
                this.GetComponent<Animator>().SetTrigger("ThrowBolder");
                timeSinceLastThrow = 0.0f;
                isLoaded = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadMeat();
        }

        //Removes meat on the catapult after a short time
        if(timeSinceLastThrow > 0.3f && timeSinceLastThrow < 1.0f && meatOnCatapult.activeSelf && !isLoaded)
        {
            meatOnCatapult.SetActive(false);
        }
    }
}
