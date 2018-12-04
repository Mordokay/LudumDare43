using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    string listMaps = "http://mordokay.com/LudumDare43/listMaps.php";
    string deleteMap = "http://mordokay.com/LudumDare43/deleteMap.php";

    public GameObject mainMenuPanel;
    public GameObject mapListPanel;
    public GameObject listingItem;
    public GameObject requestPasswordPanel;
    public InputField mapPassword;
    string mapForDelete;
    public GameObject passwordSubmitButton;
    public Text passwordSubmitFeedback;
    public bool toggleSound;
    public Text muteButtonText;

    private void Start()
    {
        mapForDelete = "";
        toggleSound = true;
    }

    public void ToggleSound()
    {
        toggleSound = !toggleSound;
        if (toggleSound)
        {
            AudioListener.volume = 1f;
            muteButtonText.text = "Sound: On";
        }
        else
        {
            AudioListener.volume = 0f;
            muteButtonText.text = "Sound: Off";
        }
    }

    public IEnumerator ListMaps()
    {
        foreach(Transform child in mapListPanel.transform.GetChild(0).GetChild(0))
        {
            Destroy(child.gameObject);
        }

        WWW hs_post = new WWW(listMaps);
        yield return hs_post;

        if (hs_post.text.Trim() != "")
        {
            string[] maps = hs_post.text.Split(new string[] { "&&" }, StringSplitOptions.None);
            foreach (string map in maps)
            {
                string[] myMapData = map.Trim().Split(new string[] { "***" }, StringSplitOptions.None); ;
                GameObject myListing = Instantiate(listingItem, mapListPanel.transform.GetChild(0).GetChild(0)) as GameObject;
                myListing.GetComponent<MapListingData>().mapName = myMapData[0];
                myListing.GetComponent<MapListingData>().creator = myMapData[1];
                myListing.GetComponent<MapListingData>().password = myMapData[2];
                myListing.transform.GetChild(0).GetComponentInChildren<Text>().text = "Map Name: " + myMapData[0] +
                    System.Environment.NewLine + "Creator: " + myMapData[1];
            }
        }
        //Debug.Log(hs_post.text);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMapCreator()
    {
        PlayerPrefs.SetInt("StartGame", 0);
        SceneManager.LoadScene(1);
    }

    public void LoadGame(string name)
    {
        PlayerPrefs.SetInt("StartGame", 1);
        PlayerPrefs.SetString("MapName", name);
        SceneManager.LoadScene(1);
    }

    public IEnumerator DeleteMap(string name)
    {
        string post_url = deleteMap + "?name=" + WWW.EscapeURL(name) + "&password=" + WWW.EscapeURL(mapPassword.text);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;


        if (hs_post.text == "1")
        {
            ShowMapListPanel();
        }
        else
        {
            passwordSubmitFeedback.text = hs_post.text;
        }
        //Debug.Log(hs_post.text);
    }

    public void RequestMapDelete(string mapName)
    {
        mapForDelete = mapName;
        requestPasswordPanel.SetActive(true);
    }

    public void SubmitMapPasswordForDelete()
    {
        StartCoroutine(DeleteMap(mapForDelete));
    }

    public void ShowMapListPanel()
    {
        requestPasswordPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        mapListPanel.SetActive(true);
        StartCoroutine(ListMaps());
    }

    public void ShowMainMenuPanel()
    {
        requestPasswordPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        mapListPanel.SetActive(false);
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(ListMaps());
        }

        if(mapPassword.text != "")
        {
            passwordSubmitButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            passwordSubmitButton.GetComponent<Button>().interactable = false;
        }
    }

}
