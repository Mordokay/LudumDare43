using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapMaker : MonoBehaviour
{

    public Text mapWidthText;
    public Text mapHeightText;
    public int mapWidth;
    public int mapHeight;

    public GameObject tilePrefab;
    public GameObject[,] mapArray;
    public Transform mapHolder;

    public int selectedTile;
    public int selectedAnimal;
    public int selectedDecorative;

    public List<GameObject> TileButtons;
    public List<GameObject> AnimalButtons;
    public List<GameObject> DecorativeButtons;
    public GameObject editGarryButton;
    public GameObject titleTabButton;
    public GameObject animalTabButton;
    public GameObject decorativeTabButton;
    public Color selectedColor;

    public bool editingGarry;
    public bool editingTerrain;
    public bool editingAnimals;
    public bool editingDecorative;

    public GameObject listTerrain;
    public GameObject scrollbarTerrain;
    public GameObject listAnimal;
    public GameObject scrollbarAnimal;
    public GameObject listDecorative;
    public GameObject scrollbarDecorative;

    public GameObject GarryHolder;
    public bool GarryPlaced;
    public bool pitOfHellPlaced;

    public List<GameObject> garryPath;
    GameObject arrowsHolder;
    bool gameStarted;

    public GameObject mapMakerPanel;
    public GameObject saveMapPanel;

    string getMapData = "http://mordokay.com/LudumDare43/getMapData.php";

    private void Start()
    {
        if (PlayerPrefs.GetInt("StartGame") == 1)
        {
            LoadMap(PlayerPrefs.GetString("MapName"));
            mapMakerPanel.SetActive(false);
        }
        else
        {
            RemoveEditing();
            editingTerrain = true;
            arrowsHolder = GameObject.FindGameObjectWithTag("ArrowsHolder");
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public IEnumerator RequestMapData(string name)
    {
        string post_url = getMapData + "?name=" + WWW.EscapeURL(name);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        Debug.Log(hs_post.text);
        string[] gameData = hs_post.text.Split(new string[] { "***" }, StringSplitOptions.None);
        string[] mapSize = gameData[3].Split(' ');
        mapWidth = Int32.Parse(mapSize[0]);
        mapHeight = Int32.Parse(mapSize[1]);
        GenerateMap(mapWidth, mapHeight);

        string[] terrainIDs = gameData[4].Split(' ');
        string[] animalIDs = gameData[5].Split(' ');
        string[] decorativeIDs = gameData[6].Split(' ');
        string[] garyPathNodes = gameData[7].Split(',');

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                
                if (Int32.Parse(terrainIDs[i * mapWidth + j]) != -1)
                {
                    yield return new WaitForSeconds(0.05f);
                    mapArray[i, j].GetComponent<TerrainTile>().ChangeTile(Int32.Parse(terrainIDs[i * mapWidth + j]));
                }
                if (Int32.Parse(animalIDs[i * mapWidth + j]) != -1)
                {
                    yield return new WaitForSeconds(0.05f);
                    mapArray[i, j].GetComponent<TerrainTile>().ChangeAnimal(Int32.Parse(animalIDs[i * mapWidth + j]));
                }
                if (Int32.Parse(decorativeIDs[i * mapWidth + j]) != -1)
                {
                    yield return new WaitForSeconds(0.05f);
                    mapArray[i, j].GetComponent<TerrainTile>().ChangeDecorative(Int32.Parse(decorativeIDs[i * mapWidth + j]));
                }
            }
        }

        garryPath.Clear();
        garryPath = new List<GameObject>();
        Vector3 lastPos = Vector3.zero;
        for (int i = 0; i < garyPathNodes.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            string[] node = garyPathNodes[i].Split(' ');
            int realWidth = Int32.Parse(node[0]) + mapWidth / 2;
            int realHeight = Int32.Parse(node[1]) + mapHeight / 2;
            garryPath.Add(mapArray[realWidth, realHeight]);
            if (i == 0)
            {
                mapArray[realWidth, realHeight].GetComponent<TerrainTile>().InsertGarry();
            }
            else 
            {
                mapArray[realWidth, realHeight].GetComponent<TerrainTile>().InsertPitOfHell(garryPath[i - 1].transform.position, true, false);

                if (!pitOfHellPlaced)
                {
                    pitOfHellPlaced = true;
                }
                else
                {
                    garryPath[i - 1].GetComponent<TerrainTile>().hasPit = 0;
                    Destroy(garryPath[i - 1].GetComponent<TerrainTile>().othersHolder.GetChild(0).gameObject);
                }
            }
        }
        Debug.Log(hs_post.text);
    }

    public void LoadMap(string name)
    {
        StartCoroutine(RequestMapData(name));
    }

    public void RemoveEditing()
    {
        editingGarry = false;
        editingTerrain = false;
        editingAnimals = false;
        editingDecorative = false;
    }

    public void RemoveLists()
    {
        listTerrain.SetActive(false);
        scrollbarTerrain.SetActive(false);
        listAnimal.SetActive(false);
        scrollbarAnimal.SetActive(false);
        listDecorative.SetActive(false);
        scrollbarDecorative.SetActive(false);
    }

    public void EditGarry()
    {
        RemoveEditing();
        ClearAllSelections();
        editingGarry = true;
        editGarryButton.GetComponent<Image>().color = selectedColor;
    }

    public void EditTerrain()
    {
        RemoveEditing();
        RemoveLists();
        ClearAllSelections();
        editingTerrain = true;
        listTerrain.SetActive(true);
        scrollbarTerrain.SetActive(true);
        titleTabButton.GetComponent<Image>().color = selectedColor;
    }

    public void EditAnimals()
    {
        RemoveEditing();
        RemoveLists();
        ClearAllSelections();
        editingAnimals = true;
        listAnimal.SetActive(true);
        scrollbarAnimal.SetActive(true);
        animalTabButton.GetComponent<Image>().color = selectedColor;

    }
    public void EditDecorative()
    {
        RemoveEditing();
        RemoveLists();
        ClearAllSelections();
        editingDecorative = true;
        listDecorative.SetActive(true);
        scrollbarDecorative.SetActive(true);
        decorativeTabButton.GetComponent<Image>().color = selectedColor;
    }

    public void ClearAllSelections()
    {
        foreach (GameObject btn in TileButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
        foreach (GameObject btn in AnimalButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
        foreach (GameObject btn in DecorativeButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
        editGarryButton.GetComponent<Image>().color = Color.white;
        titleTabButton.GetComponent<Image>().color = Color.white;
        animalTabButton.GetComponent<Image>().color = Color.white;
        decorativeTabButton.GetComponent<Image>().color = Color.white;
        selectedTile = -1;
        selectedAnimal = -1;
        selectedDecorative = -1;
    }

    public void selectTile(int tileID)
    {
        foreach (GameObject btn in TileButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }

        selectedTile = tileID;
        TileButtons[tileID].GetComponent<Image>().color = selectedColor;
    }

    public void selectAnimal(int animalID)
    {
        foreach (GameObject btn in AnimalButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }

        selectedAnimal = animalID;
        AnimalButtons[animalID].GetComponent<Image>().color = selectedColor;
    }

    public void selectDecorative(int decorativeID)
    {
        foreach (GameObject btn in DecorativeButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }

        selectedDecorative = decorativeID;
        DecorativeButtons[decorativeID].GetComponent<Image>().color = selectedColor;
    }
    public void ClearMap()
    {
        selectedTile = -1;

        int childs = mapHolder.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(mapHolder.GetChild(i).gameObject);
        }
    }

    public void ResetMap()
    {
        ClearMap();
        RemoveEditing();
        RemoveLists();
        ClearAllSelections();
        garryPath.Clear();
        GarryPlaced = false;
        pitOfHellPlaced = false;
    }

    public void GenerateMap(int width, int height)
    {
        mapArray = new GameObject[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                mapArray[i, j] = Instantiate(tilePrefab) as GameObject;
                mapArray[i, j].transform.position = new Vector3(i - width / 2, 0, j - height / 2);
                mapArray[i, j].transform.parent = mapHolder;
                mapArray[i, j].name = "(" + i + "," + j + ")";
                mapArray[i, j].GetComponent<TerrainTile>().InitializeVariables();
            }
        }
    }

    public void GenerateMap()
    {
        if (mapWidthText.text != "" && mapHeightText.text != "")
        {
            ResetMap();
            for (int i = arrowsHolder.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(arrowsHolder.transform.GetChild(i).gameObject);
            }

            mapWidth = Int32.Parse(mapWidthText.text);
            mapHeight = Int32.Parse(mapHeightText.text);

            mapArray = new GameObject[mapWidth, mapHeight];

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    mapArray[i, j] = Instantiate(tilePrefab) as GameObject;
                    mapArray[i, j].transform.position = new Vector3(i - mapWidth / 2, 0, j - mapHeight / 2);
                    mapArray[i, j].transform.parent = mapHolder;
                    mapArray[i, j].name = "(" + i + "," + j + ")";
                }
            }
        }
    }

    public void SaveMap()
    {
        if (saveMapPanel.activeSelf)
        {
            saveMapPanel.SetActive(false);
        }
        else
        {
            saveMapPanel.SetActive(true);
        }
    }
    private void Update()
    {
        //Only allow raycast if mouse is not over the UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {

            //left click hold draws terrain
            if (Input.GetMouseButton(0))
            {
                if (editingTerrain || editingGarry)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Terrain")))
                    {
                        Transform objectHit = hit.transform;
                        if (editingTerrain)
                        {
                            objectHit.gameObject.GetComponent<TerrainTile>().ChangeTile(selectedTile + 1);
                        }
                        else if (editingGarry && GarryPlaced && pitOfHellPlaced)
                        {

                            if (Vector3.Distance(garryPath[garryPath.Count - 1].transform.position, objectHit.gameObject.transform.position) - 1.0f < 0.1f &&
                                objectHit.gameObject.GetComponent<TerrainTile>().hasGarry == 0 && objectHit.gameObject.GetComponent<TerrainTile>().terrainType != 0)
                            {
                                Vector3 lastPathPos = garryPath[garryPath.Count - 1].transform.position;
                                //Instantiates a new pit of doom at the new position and removes the last pit of doom
                                garryPath[garryPath.Count - 1].GetComponent<TerrainTile>().hasPit = 0;
                                Destroy(garryPath[garryPath.Count - 1].GetComponent<TerrainTile>().othersHolder.GetChild(0).gameObject);
                                garryPath.Add(objectHit.gameObject);
                                objectHit.gameObject.GetComponent<TerrainTile>().InsertPitOfHell(lastPathPos, true, false);
                            }
                        }
                    }
                }
            }

            //left click down inserts animal or decorative object
            if (Input.GetMouseButtonDown(0))
            {
                if (editingAnimals || editingDecorative || editingGarry)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Terrain")))
                    {
                        Transform objectHit = hit.transform;
                        if (objectHit.gameObject.GetComponent<TerrainTile>().terrainType != 0)
                        {
                            if (editingAnimals)
                            {
                                objectHit.gameObject.GetComponent<TerrainTile>().ChangeAnimal(selectedAnimal);
                            }
                            else if (editingDecorative)
                            {
                                objectHit.gameObject.GetComponent<TerrainTile>().ChangeDecorative(selectedDecorative);
                            }
                            else if (editingGarry)
                            {
                                if (!GarryPlaced)
                                {
                                    objectHit.gameObject.GetComponent<TerrainTile>().InsertGarry();
                                    garryPath.Add(objectHit.gameObject);
                                    GarryPlaced = true;
                                }
                                else if (!pitOfHellPlaced)
                                {
                                    if (Vector3.Distance(garryPath[0].transform.position, objectHit.gameObject.transform.position) - 1.0f < 0.1f)
                                    {
                                        garryPath.Add(objectHit.gameObject);
                                        objectHit.gameObject.GetComponent<TerrainTile>().InsertPitOfHell(garryPath[0].transform.position, true, false);
                                        pitOfHellPlaced = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //right click removes stuff
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Terrain")))
                {
                    Transform objectHit = hit.transform;


                    if (editingAnimals)
                    {
                        objectHit.gameObject.GetComponent<TerrainTile>().RemoveAnimal();
                    }
                    else if (editingDecorative)
                    {
                        objectHit.gameObject.GetComponent<TerrainTile>().RemoveDecorative();
                    }
                }
            }
            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Terrain")))
                {
                    Transform objectHit = hit.transform;
                    if (editingTerrain)
                    {
                        objectHit.gameObject.GetComponent<TerrainTile>().RemoveTile();
                    }
                    else if (editingGarry)
                    {
                        if (objectHit.gameObject.GetComponent<TerrainTile>().hasGarry == 1)
                        {
                            objectHit.gameObject.GetComponent<TerrainTile>().RemoveGarry();
                        }
                        else if (objectHit.gameObject.GetComponent<TerrainTile>().hasPit == 1)
                        {
                            objectHit.gameObject.GetComponent<TerrainTile>().RemovePitOfHell();
                        }

                    }
                }
            }
        }
    }
}