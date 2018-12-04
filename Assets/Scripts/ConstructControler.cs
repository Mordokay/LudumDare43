using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructControler : MonoBehaviour {

    public GameObject meatChunk;
    public GameObject collectionPile;
    PlayerData pd;

    bool isWorking;
    bool canCollectPile;
    
    public float timeToProcessMeat;
    float timeSinceStartedWorking;

    public int bonesPerCicle;
    public int meatPerCicle;
    public int platePerCicle;
    public int tearsPerCicle;

    public int collectedBones;
    public int collectedMeat;
    public int collectedPlate;
    public int collectedTears;

    public Slider workingRemainingTimeSlider;
    private void Start()
    {

        isWorking = false;
        canCollectPile = false;
        pd = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerData>();
    }

    public void AnimalEntry(GameObject animal)
    {
        if (!isWorking)
        {
            timeSinceStartedWorking = 0.0f;
            meatChunk.SetActive(true);
            isWorking = true;
            if (!this.gameObject.tag.Equals("BarbecueGril"))
            {
                this.GetComponent<Animator>().SetBool("isWorking", true);
            }

            pd.animalBeingGrabbed = null;
            pd.grabbingAnimal = false;
            Destroy(animal);
        }
    }

    public void Interact()
    {
        if(canCollectPile)
        {
            collectionPile.SetActive(false);
            pd.IncrementInventory(collectedBones, collectedMeat, collectedPlate, collectedTears);
            collectedBones = 0;
            collectedMeat = 0;
            collectedPlate = 0;
            collectedTears = 0;
        }
    }

    void UpdatePile()
    {
        collectedBones += bonesPerCicle;
        collectedMeat += meatPerCicle;
        collectedPlate += platePerCicle;
        collectedTears += tearsPerCicle;
    }

	void Update () {
        if (isWorking)
        {
            workingRemainingTimeSlider.transform.parent.LookAt(workingRemainingTimeSlider.transform.parent.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
            workingRemainingTimeSlider.value = timeSinceStartedWorking / timeToProcessMeat;

            timeSinceStartedWorking += Time.deltaTime;
            if(timeSinceStartedWorking > timeToProcessMeat)
            {
                meatChunk.SetActive(false);
                collectionPile.SetActive(true);
                canCollectPile = true;
                if (!this.gameObject.tag.Equals("BarbecueGril"))
                {
                    this.GetComponent<Animator>().SetBool("isWorking", false);
                }
                isWorking = false;
                UpdatePile();
            }
        }
    }
}
