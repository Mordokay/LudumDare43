using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour {

    int bones;
    int flesh;
    int cookedAnimal;
    int tears;

    public Text bonesText;
    public Text fleshText;
    public Text cookedAnimalText;
    public Text tearsText;

    public bool bolderSelected;
    public bool plateSelected;
    public bool catapultSelected;
    public bool boneRemoverSelected;
    public bool baseballBatSelected;
    public bool happyThoughtsSelected;
    public bool barbecueGrillSelected;

    public List<GameObject> constructButtons;
    public Color selectedColor;

    public GameObject bolderPrefab;
    public GameObject platePrefab;
    public GameObject catapultPrefab;

    private void Start()
    {
        bones = 0;
        flesh = 0;
        cookedAnimal = 0;
        tears = 0;

        UpdateUI();
    }

    public void ResetPlayerData()
    {
        bones = 0;
        flesh = 0;
        cookedAnimal = 0;
        tears = 0;
        RemoveAllSelectedConstructs();
    }

    void RemoveAllSelectedConstructs()
    {
        foreach(GameObject btn in constructButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
        bolderSelected = false;
        plateSelected = false;
        catapultSelected = false;
        boneRemoverSelected = false;
        baseballBatSelected = false;
        happyThoughtsSelected = false;
        barbecueGrillSelected = false;
}

    public void SelectConstruct(int i)
    {
        RemoveAllSelectedConstructs();
        constructButtons[i].GetComponent<Image>().color = selectedColor;
        switch (i)
        {
            case 0:
                bolderSelected = true;
                break;
            case 1:
                plateSelected = true;
                break;
            case 2:
                catapultSelected = true;
                break;
            case 3:
                boneRemoverSelected = true;
                break;
            case 4:
                baseballBatSelected = true;
                break;
            case 5:
                happyThoughtsSelected = true;
                break;
            case 6:
                barbecueGrillSelected = true;
                break;
        }
    }

    public void IncrementInventory(int bones, int flesh, int cooked, int tears)
    {
        this.bones += bones;
        this.flesh += flesh;
        this.cookedAnimal += cooked;
        this.tears += tears;

        UpdateUI();
    }

    void UpdateUI()
    {
        bonesText.text = this.bones.ToString();
        fleshText.text = this.flesh.ToString();
        cookedAnimalText.text = this.cookedAnimal.ToString();
        tearsText.text = this.tears.ToString();
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.I))
        {
            IncrementInventory(1, 1, 1, 1);
        }
        //Only allow raycast if mouse is not over the UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //left click stabs an animal
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Animal")))
                {
                    Transform objectHit = hit.transform;
                    //Debug.Log(objectHit.name);

                    //Atacks the animal, removing a portion of that animals life;
                }

                if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Terrain")))
                {
                    Transform objectHit = hit.transform;
                    //Check if there is no decorative or sniaml in that tile
                    if(objectHit.gameObject.GetComponent<TerrainTile>().decorativeType == -1 &&
                        objectHit.gameObject.GetComponent<TerrainTile>().animalType == -1 &&
                        !objectHit.gameObject.GetComponent<TerrainTile>().hasConstruct)
                    {
                        if (bolderSelected)
                        {
                            if (objectHit.gameObject.GetComponent<TerrainTile>().terrainType == 0)
                            {
                                objectHit.gameObject.GetComponent<TerrainTile>().hasConstruct = true;
                                Instantiate(bolderPrefab, hit.transform);
                            }
                        }
                        else if (plateSelected)
                        {
                            if (objectHit.gameObject.GetComponent<TerrainTile>().terrainType == 0)
                            {
                                objectHit.gameObject.GetComponent<TerrainTile>().hasConstruct = true;
                                Instantiate(platePrefab, hit.transform);
                            }
                        }
                        else if (catapultSelected)
                        {
                            if (objectHit.gameObject.GetComponent<TerrainTile>().terrainType != 0)
                            {
                                objectHit.gameObject.GetComponent<TerrainTile>().hasConstruct = true;
                                Instantiate(catapultPrefab, hit.transform);
                            }
                        }
                        else if (boneRemoverSelected)
                        {

                        }
                        else if (baseballBatSelected)
                        {

                        }
                        else if (happyThoughtsSelected)
                        {

                        }
                        else if (barbecueGrillSelected)
                        {

                        }
                    }
                    //Debug.Log(objectHit.name);

                    //Atacks the animal, removing a portion of that animals life;
                }
            }

            //right click grabs an animal
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Terrain")))
                {
                    Transform objectHit = hit.transform;
                }
            }
            //checks if player is holding an animal. It player is holding an animal, the animal is dragged to the position of the mouse
            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Terrain")))
                {
                    Transform objectHit = hit.transform;
                }
            }
        }
    }
}