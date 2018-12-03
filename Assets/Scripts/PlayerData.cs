using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour {

    public int bones;
    public int flesh;
    public int cookedAnimal;
    public int tears;

    public Text bonesText;
    public Text fleshText;
    public Text cookedAnimalText;
    public Text tearsText;

    public bool bolderSelected;
    public bool plateSelected;
    public bool catapultSelected;
    public bool boneRemoverSelected;
    public bool baseballBatSelected;
    public bool barbecueGrillSelected;
    public bool happyThoughtsSelected;

    public List<GameObject> constructButtons;
    public Color selectedColor;

    public GameObject bolderPrefab;
    public GameObject platePrefab;
    public GameObject catapultPrefab;
    public GameObject BoneRemoverPrefab;
    public GameObject basebalBatChamberPrefab;
    public GameObject barbecueGrilPrefab;
    public GameObject HappyThoughtsPrefab;

    public LayerMask terrainLayer;
    public LayerMask animalLayer;
    public LayerMask lootLayer;
    public LayerMask moveLayer;
    public LayerMask constructLayer;

    public bool grabbingAnimal;
    public GameObject animalBeingGrabbed;
    public Vector3 animalGrabbedLastPos;

    public Text bolderTextR1;
    public Text bolderTextR2;
    public Text plateR1;
    public Text catapultR1;
    public Text boneRemoverR1;
    public Text boneRemoverR2;
    public Text basebalBatR1;
    public Text basebalBatR2;
    public Text barbecueGrilR1;
    public Text barbecueGrilR2;
    public Text happyThoughtsR1;
    public Text happyThoughtsR2;
    public Text happyThoughtsR3;

    public Button bolderButton;
    public Button plateButton;
    public Button catapultButton;
    public Button boneRemoverButton;
    public Button basebalBatButton;
    public Button barbecueGrilButton;
    public Button happyThoughtsButton;

    private void UpdateUI()
    {
        bolderTextR1.color = Color.black;
        bolderTextR2.color = Color.black;
        plateR1.color = Color.black;
        catapultR1.color = Color.black;
        boneRemoverR1.color = Color.black;
        boneRemoverR2.color = Color.black;
        basebalBatR1.color = Color.black;
        basebalBatR2.color = Color.black;
        barbecueGrilR1.color = Color.black;
        barbecueGrilR2.color = Color.black;
        happyThoughtsR1.color = Color.black;
        happyThoughtsR2.color = Color.black;
        happyThoughtsR3.color = Color.black;

        if(flesh >= 8 && bones >= 2)
        {
            bolderButton.interactable = true;
        }
        else 
        {
            bolderButton.interactable = false;
            if (flesh < 8)
            {
                bolderTextR1.color = Color.red;
            }
            if(bones < 2)
            {
                bolderTextR2.color = Color.red;
            }
        }

        if(cookedAnimal > 0)
        {
            plateButton.interactable = true;
        }
        else
        {
            plateR1.color = Color.red;
            plateButton.interactable = false;
        }

        if(bones >= 10)
        {
            catapultButton.interactable = true;
        }
        else
        {
            catapultR1.color = Color.red;
            catapultButton.interactable = false;
        }

        if(bones >= 8 && tears >= 2)
        {
            boneRemoverButton.interactable = true;
        }
        else
        {
            boneRemoverButton.interactable = false;
            if (bones < 8)
            {
                boneRemoverR1.color = Color.red;
            }
            if (tears < 2)
            {
                boneRemoverR2.color = Color.red;
            }
        }

        if (bones >= 2 && flesh >= 8)
        {
            basebalBatButton.interactable = true;
        }
        else
        {
            basebalBatButton.interactable = false;
            if (bones < 2)
            {
                basebalBatR1.color = Color.red;
            }
            if(flesh < 8)
            {
                basebalBatR2.color = Color.red;
            }
        }

        if (flesh >= 10 && tears >= 3)
        {
            barbecueGrilButton.interactable = true;
        }
        else
        {
            barbecueGrilButton.interactable = false;
            if (flesh < 10)
            {
                barbecueGrilR1.color = Color.red;
            }
            if (tears < 3)
            {
                barbecueGrilR2.color = Color.red;
            }
        }

        if (bones >= 10 && flesh >= 5 && tears >= 5)
        {
            happyThoughtsButton.interactable = true;
        }
        else
        {
            happyThoughtsButton.interactable = false;
            if (bones < 10)
            {
                happyThoughtsR1.color = Color.red;
            }
            if (flesh < 5)
            {
                happyThoughtsR2.color = Color.red;
            }
            if (tears < 5)
            {
                happyThoughtsR3.color = Color.red;
            }
        }


        bonesText.text = this.bones.ToString();
        fleshText.text = this.flesh.ToString();
        cookedAnimalText.text = this.cookedAnimal.ToString();
        tearsText.text = this.tears.ToString();
    }

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
                barbecueGrillSelected = true;
                break;
            case 6:
                happyThoughtsSelected = true;
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

    public void DecrementInventory(int bones, int flesh, int cooked, int tears)
    {
        this.bones -= bones;
        this.flesh -= flesh;
        this.cookedAnimal -= cooked;
        this.tears -= tears;

        UpdateUI();
    }

    void Update () {
        if (this.GetComponent<MapMaker>().gameStarted)
        {
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

                    if (Physics.Raycast(ray, out hit, 100.0f, animalLayer))
                    {
                        GameObject objectHit = hit.rigidbody.gameObject;
                        //Debug.Log("Animal: " + objectHit.name);

                        objectHit.GetComponent<AnimalControler>().PlayerAtack(10);
                        //Atacks the animal, removing a portion of that animals life;
                    }
                    else if (Physics.Raycast(ray, out hit, 100.0f, lootLayer))
                    {
                        GameObject objectHit = hit.transform.parent.gameObject;
                        //Debug.Log("Loot: " + objectHit.name);

                        objectHit.GetComponent<LootManager>().MoveToCollector();
                        //Atacks the animal, removing a portion of that animals life;
                    }
                    else if (Physics.Raycast(ray, out hit, 100.0f, terrainLayer))
                    {
                        Transform objectHit = hit.transform;
                        //Check if there is no decorative or sniaml in that tile
                        if (objectHit.gameObject.GetComponent<TerrainTile>().decorativeType == -1 &&
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
                                if (objectHit.gameObject.GetComponent<TerrainTile>().terrainType != 0)
                                {
                                    objectHit.gameObject.GetComponent<TerrainTile>().hasConstruct = true;
                                    Instantiate(BoneRemoverPrefab, hit.transform);
                                }
                            }
                            else if (baseballBatSelected)
                            {
                                if (objectHit.gameObject.GetComponent<TerrainTile>().terrainType != 0)
                                {
                                    objectHit.gameObject.GetComponent<TerrainTile>().hasConstruct = true;
                                    Instantiate(basebalBatChamberPrefab, hit.transform);
                                }
                            }
                            else if (barbecueGrillSelected)
                            {
                                if (objectHit.gameObject.GetComponent<TerrainTile>().terrainType != 0)
                                {
                                    objectHit.gameObject.GetComponent<TerrainTile>().hasConstruct = true;
                                    Instantiate(barbecueGrilPrefab, hit.transform);
                                }
                            }
                            else if (happyThoughtsSelected)
                            {
                                if (objectHit.gameObject.GetComponent<TerrainTile>().terrainType != 0)
                                {
                                    objectHit.gameObject.GetComponent<TerrainTile>().hasConstruct = true;
                                    Instantiate(HappyThoughtsPrefab, hit.transform);
                                }
                            }
                        }
                    }

                    if (Physics.Raycast(ray, out hit, 100.0f, constructLayer))
                    {
                        GameObject objectHit = hit.transform.gameObject;
                        if (objectHit.tag.Equals("BoneRemover") || objectHit.tag.Equals("BasebalBat") || objectHit.tag.Equals("BarbecueGril"))
                        {
                            objectHit.gameObject.GetComponent<ConstructControler>().Interact();
                        }
                    }

                    RemoveAllSelectedConstructs();
                }

                //right click grabs an animal
                if (Input.GetMouseButtonDown(1))
                {
                    if (grabbingAnimal)
                    {
                        //Debug.Log("Release An Animal");
                        grabbingAnimal = false;
                        animalBeingGrabbed.GetComponent<AnimalControler>().beingGrabbed = false;
                        animalBeingGrabbed.GetComponent<Animator>().enabled = true;

                        animalBeingGrabbed.transform.position = animalGrabbedLastPos;
                    }
                    else
                    {
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray, out hit, 100.0f, animalLayer))
                        {
                            Transform objectHit = hit.transform;
                            //Debug.Log("GrabsAnAnimal : " + objectHit.gameObject.name);
                            grabbingAnimal = true;
                            animalBeingGrabbed = objectHit.gameObject;
                            animalBeingGrabbed.GetComponent<AnimalControler>().beingGrabbed = true;
                            animalBeingGrabbed.GetComponent<Animator>().enabled = false;

                            animalGrabbedLastPos = animalBeingGrabbed.transform.position;
                        }
                    }
                }

                if (grabbingAnimal)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, 100.0f, moveLayer))
                    {
                        animalBeingGrabbed.transform.position =hit.point;
                    }
                }
            }
        }
    }
}