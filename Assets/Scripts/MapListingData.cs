using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapListingData : MonoBehaviour {

    public string mapName;
    public string creator;
    public string password;

    public GameObject gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
    }

    public void PlayMap()
    {

    }
    public void DeleteMap()
    {
        gm.GetComponent<MenuManager>().RequestMapDelete(mapName);
    }
}
