using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySqlManager : MonoBehaviour {

    public GameObject uploadButton;
    public InputField mapName;
    public InputField creatorName;
    public InputField password;
    public GameObject warningMySql;

    string addMap = "https://www.mordokay.com/webgl/Games/LD43/addMap.php";
    string getMapData = "https://www.mordokay.com/webgl/Games/LD43/getMapData.php";
    string listMaps = "https://www.mordokay.com/webgl/Games/LD43/listMaps.php";

    public string GenerateMapData()
    {
        string data = "";
        int mapWidth = this.GetComponent<MapMaker>().mapWidth;
        int mapHeight = this.GetComponent<MapMaker>().mapHeight;
        GameObject[,] mapArray = this.GetComponent<MapMaker>().mapArray;
        List<GameObject> garryPath = this.GetComponent<MapMaker>().garryPath;

        data = mapName.text + "***" + creatorName.text + "***" + password.text + "***" + mapWidth.ToString() + " " + mapHeight.ToString() + "***";
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                data += mapArray[i, j].GetComponent<TerrainTile>().terrainType + " ";
            }
        }
        data = data.Substring(0, data.Length - 1);
        data += "***";
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                data += mapArray[i, j].GetComponent<TerrainTile>().animalType + " ";
            }
        }
        data = data.Substring(0, data.Length - 1);
        data += "***";
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                data += mapArray[i, j].GetComponent<TerrainTile>().decorativeType + " ";
            }
        }
        data = data.Substring(0, data.Length - 1);
        data += "***";
        foreach (GameObject pathNode in garryPath)
        {
            data += Mathf.RoundToInt(pathNode.GetComponent<TerrainTile>().transform.position.x) + " " +
                        Mathf.RoundToInt(pathNode.GetComponent<TerrainTile>().transform.position.z) + ",";
        }
        data = data.Substring(0, data.Length - 1);

        return data;
    }

    public void UploadMap()
    {
        //if there is no Garry and Pit of Hell
        if (!this.GetComponent<MapMaker>().GarryPlaced || !this.GetComponent<MapMaker>().pitOfHellPlaced)
        {
            warningMySql.SetActive(true);
            warningMySql.GetComponent<Text>().text = "You didn't create Garry or the Pit of Hell!";
            return;
        }
        else
        {
            string data = GenerateMapData();
            StartCoroutine(AddMapEnumerator(mapName.text, data.Trim()));
        }
    }

    public IEnumerator AddMapEnumerator(string name, string data)
    {     
        string post_url = addMap + "?name=" + WWW.EscapeURL(name) + "&data=" + WWW.EscapeURL(data);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;

        //if there is no Garry and Pit of Hell
        if(!this.GetComponent<MapMaker>().GarryPlaced || !this.GetComponent<MapMaker>().pitOfHellPlaced)
        {
            warningMySql.SetActive(true);
            warningMySql.GetComponent<Text>().text = "That Map Already Exists!";
        }
        //Map already exists
        if(hs_post.text == "44")
        {
            warningMySql.SetActive(true);
            warningMySql.GetComponent<Text>().text = "That Map Already Exists!";
        }
        //Success
        else if(hs_post.text == "1")
        {
            this.GetComponent<MapMaker>().saveMapPanel.SetActive(false);
            warningMySql.SetActive(false);
        }
        //Other possible Errors
        else
        {
            warningMySql.SetActive(true);
            warningMySql.GetComponent<Text>().text = hs_post.text;
        }
    }

    void Update () {
		if(mapName.text != "" && creatorName.text != "" && password.text != "")
        {
            uploadButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            uploadButton.GetComponent<Button>().interactable = false;
        }
	}
}
