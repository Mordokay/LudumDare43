using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySqlManager : MonoBehaviour {

    public GameObject uploadButton;
    public InputField mapName;
    public InputField creatorName;
    public InputField password;

    string addMap = "http://mordokay.com/LudumDare43/addMap.php";
    string getMaps = "http://mordokay.com/LudumDare43/getMaps.php";

    public void UploadMap()
    {
        string data = "";
        int mapWidth = this.GetComponent<MapMaker>().mapWidth;
        int mapHeight = this.GetComponent<MapMaker>().mapHeight;
        GameObject[,] mapArray = this.GetComponent<MapMaker>().mapArray;
        List<GameObject> garryPath = this.GetComponent<MapMaker>().garryPath;

        data = mapName.text + "&&" + creatorName.text + "&&" + password.text + "&&" + mapWidth.ToString() + " " + mapHeight.ToString() + "&&";

        StartCoroutine(AddMapEnumerator(data.Trim()));
    }

    public IEnumerator AddMapEnumerator(string data)
    {
        string post_url = addMap + "?data=" + WWW.EscapeURL(data);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;

        Debug.Log(hs_post.text);
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
