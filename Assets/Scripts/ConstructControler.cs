using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        isWorking = false;
        canCollectPile = false;
        pd = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerData>();
    }

    public void AnimalEntry()
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
        }
    }

    public void Interact()
    {
        if(!isWorking && canCollectPile)
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
