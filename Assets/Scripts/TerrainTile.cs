using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour {

    public Object[] materialTiles;
    public Object[] animalPrefabs;
    public Object[] decorativePrefabs;
    public Object[] otherPrefabs;
    public int terrainType;
    public int animalType;
    public int decorativeType;
    public int hasGarry;
    public int hasPit;
    GameObject gm;
    GameObject arrowsHolder;

    Transform animalHolder;
    Transform decorativeHolder;
    public Transform othersHolder;
    Material initialMaterial;
    public GameObject garryHolder;
    public GameObject garryPrefab;

    void Start () {

        initialMaterial = this.GetComponent<Renderer>().material;
        gm = GameObject.FindGameObjectWithTag("GameManager");
        arrowsHolder = GameObject.FindGameObjectWithTag("ArrowsHolder");
        garryHolder = GameObject.FindGameObjectWithTag("GarryHolder");
        garryPrefab = Resources.Load("Garry", typeof(GameObject)) as GameObject;
        terrainType = -1;
        animalType = -1;
        decorativeType = -1;
        hasGarry = 0;
        materialTiles = Resources.LoadAll("Terrain", typeof(Material));
        animalPrefabs = Resources.LoadAll("Animal", typeof(GameObject));
        decorativePrefabs = Resources.LoadAll("Decorative", typeof(GameObject));
        otherPrefabs = Resources.LoadAll("Others", typeof(GameObject));
        animalHolder = this.transform.GetChild(0);
        decorativeHolder = this.transform.GetChild(1);
        othersHolder = this.transform.GetChild(2);
    }

    public void InitializeVariables()
    {
        Start();
    }

    public void ChangeTile(int tileID)
    {
        this.GetComponent<Renderer>().material = (Material)materialTiles[tileID];
        terrainType = tileID;
    }
    public void RemoveTile()
    {
        terrainType = -1;
        this.GetComponent<Renderer>().material = initialMaterial;
    }

    public void ChangeAnimal(int animalID)
    {
        animalType = animalID;
        decorativeType = -1;

        if (animalHolder.childCount > 0)
        {
            Destroy(animalHolder.GetChild(0).gameObject);
        }
        if (decorativeHolder.childCount > 0)
        {
            Destroy(decorativeHolder.GetChild(0).gameObject);
        }
        GameObject myAnimal = Instantiate(animalPrefabs[animalID], animalHolder) as GameObject;
    }
    public void RemoveAnimal()
    {
        animalType = -1;
        if (animalHolder.transform.childCount > 0)
        {
            Destroy(animalHolder.GetChild(0).gameObject);
        }
    }

    public void ChangeDecorative(int decorativeID)
    {
        decorativeType = decorativeID;
        animalType = -1;

        if (animalHolder.childCount > 0)
        {
            Destroy(animalHolder.GetChild(0).gameObject);
        }
        if (decorativeHolder.childCount > 0)
        {
            Destroy(decorativeHolder.GetChild(0).gameObject);
        }
        GameObject myDecorative = Instantiate(decorativePrefabs[decorativeID], decorativeHolder) as GameObject;
    }
    public void RemoveDecorative()
    {
        decorativeType = -1;
        if (decorativeHolder.transform.childCount > 0)
        {
            Destroy(decorativeHolder.GetChild(0).gameObject);
        }

    }

    public void InsertGarry()
    {
        if (animalHolder.childCount > 0)
        {
            RemoveAnimal();
        }
        if (decorativeHolder.childCount > 0)
        {
            RemoveDecorative();
        }
        hasGarry = 1;
        hasPit = 0;
        //instantiate garry hut
        Instantiate(otherPrefabs[1], othersHolder);
        ChangeTile(0);

        
        //instantiate garry object
        GameObject myGarry = Instantiate(garryPrefab, garryHolder.transform);
        myGarry.transform.position = this.transform.position + Vector3.up * 0.8f;

    }

    //Removing gary also removes the pit of hell and the entire path
    public void RemoveGarry()
    {
        hasGarry = 0;
        gm.GetComponent<MapMaker>().GarryPlaced = false;
        gm.GetComponent<MapMaker>().pitOfHellPlaced = false;

        foreach (GameObject obj in gm.GetComponent<MapMaker>().garryPath)
        {
            obj.GetComponent<TerrainTile>().RemoveTile();
        }
        //Destroys Garry
        if (othersHolder.childCount > 0)
        {
            Destroy(othersHolder.GetChild(0).gameObject);
        }
        //Destroys Pit of Hell
        if (gm.GetComponent<MapMaker>().garryPath.Count > 1)
        {
            Destroy(gm.GetComponent<MapMaker>().garryPath[gm.GetComponent<MapMaker>().garryPath.Count - 1].GetComponent<TerrainTile>().othersHolder.GetChild(0).gameObject);
        }
        gm.GetComponent<MapMaker>().garryPath.Clear();
        for(int i = arrowsHolder.transform.childCount-1; i >=0; i--)
        {
            Destroy(arrowsHolder.transform.GetChild(i).gameObject);
        }

        //Destroys actual Garry
        Destroy(garryHolder.transform.GetChild(0).gameObject);
    }

    public void InsertPitOfHell(Vector3 lastPathPos, bool addArrow, bool fromRemove)
    {
        if(animalHolder.childCount > 0)
        {
            RemoveAnimal();
        }
        if (decorativeHolder.childCount > 0)
        {
            RemoveDecorative();
        }
        hasGarry = 0;
        hasPit = 1;
        //instantiate pit of hell
        ChangeTile(0);
        GameObject myPit = Instantiate(otherPrefabs[2], othersHolder) as GameObject;
        //Nova ta a direita
        if (this.transform.position.x - lastPathPos.x > 0.0f && Mathf.Abs(this.transform.position.x - lastPathPos.x) > 0.01f)
        {
            myPit.transform.Rotate(Vector3.forward * 90.0f);

            if (addArrow && gm.GetComponent<MapMaker>().isInEditor)
            {
                GameObject myArrow = Instantiate(otherPrefabs[0], arrowsHolder.transform) as GameObject;
                myArrow.transform.position = lastPathPos + Vector3.right * 0.9f + Vector3.up * 0.7f;
                myArrow.transform.Rotate(Vector3.up * 90.0f);
            }
            if (gm.GetComponent<MapMaker>().garryPath.Count == 2 && !fromRemove)
            {
                gm.GetComponent<MapMaker>().garryPath[0].transform.GetChild(2).GetChild(0).gameObject.transform.Rotate(Vector3.up * 90.0f);
            }
        }
        //Nova ta a esquerda
        else if (this.transform.position.x - lastPathPos.x < 0.0f && Mathf.Abs(this.transform.position.x - lastPathPos.x) > 0.01f)
        {
            myPit.transform.Rotate(Vector3.forward * -90.0f);

            if (addArrow && gm.GetComponent<MapMaker>().isInEditor)
            {
                GameObject myArrow = Instantiate(otherPrefabs[0], arrowsHolder.transform) as GameObject;
                myArrow.transform.position = lastPathPos - Vector3.right * 0.9f + Vector3.up * 0.7f;
                myArrow.transform.Rotate(Vector3.up * -90.0f);
            }
            if (gm.GetComponent<MapMaker>().garryPath.Count == 2 && !fromRemove)
            {
                gm.GetComponent<MapMaker>().garryPath[0].transform.GetChild(2).GetChild(0).gameObject.transform.Rotate(Vector3.up * -90.0f);
            }
        }
        //nova ta a tras
        else if (this.transform.position.z - lastPathPos.z < 0.0f && Mathf.Abs(this.transform.position.z - lastPathPos.z) > 0.01f)
        {
            myPit.transform.Rotate(Vector3.forward * 180.0f);

            if (addArrow && gm.GetComponent<MapMaker>().isInEditor)
            {
                GameObject myArrow = Instantiate(otherPrefabs[0], arrowsHolder.transform) as GameObject;
                myArrow.transform.position = lastPathPos - Vector3.forward * 0.9f + Vector3.up * 0.7f;
                myArrow.transform.Rotate(Vector3.up * 180.0f);
            }
            if (gm.GetComponent<MapMaker>().garryPath.Count == 2 && !fromRemove)
            {
                gm.GetComponent<MapMaker>().garryPath[0].transform.GetChild(2).GetChild(0).gameObject.transform.Rotate(Vector3.up * 180.0f);
            }
        }
        //nova ta a frente
        else if (this.transform.position.z - lastPathPos.z > 0.0f && Mathf.Abs(this.transform.position.z - lastPathPos.z) > 0.01f)
        {
            if (addArrow && gm.GetComponent<MapMaker>().isInEditor)
            {
                GameObject myArrow = Instantiate(otherPrefabs[0], arrowsHolder.transform) as GameObject;
                myArrow.transform.position = lastPathPos + Vector3.forward * 0.9f + Vector3.up * 0.7f;
            }
        }
    }

    public void RemovePitOfHell()
    {
        hasPit = 0;

        if (gm.GetComponent<MapMaker>().garryPath.Count == 2)
        {
            gm.GetComponent<MapMaker>().pitOfHellPlaced = false;
            //Destroys pit of hell
            Destroy(othersHolder.GetChild(0).gameObject);
            gm.GetComponent<MapMaker>().garryPath.RemoveAt(gm.GetComponent<MapMaker>().garryPath.Count - 1);
            RemoveTile();
            //Destroys last arrow
            Destroy(arrowsHolder.transform.GetChild(arrowsHolder.transform.childCount - 1).gameObject);

            Destroy(gm.GetComponent<MapMaker>().garryPath[0].transform.GetChild(2).GetChild(0).gameObject);
            gm.GetComponent<MapMaker>().garryPath[0].GetComponent<TerrainTile>().InsertGarry();

        }
        else
        {
            gm.GetComponent<MapMaker>().pitOfHellPlaced = true;
            //Destroys pit of hell
            Destroy(othersHolder.GetChild(0).gameObject);
            gm.GetComponent<MapMaker>().garryPath.RemoveAt(gm.GetComponent<MapMaker>().garryPath.Count - 1);
            RemoveTile();

            //Destroys last arrow
            Destroy(arrowsHolder.transform.GetChild(arrowsHolder.transform.childCount - 1).gameObject);

            gm.GetComponent<MapMaker>().garryPath[gm.GetComponent<MapMaker>().garryPath.Count - 1].GetComponent<TerrainTile>().InsertPitOfHell(
                gm.GetComponent<MapMaker>().garryPath[gm.GetComponent<MapMaker>().garryPath.Count - 2].transform.position, false, true);

        }
    }
}
