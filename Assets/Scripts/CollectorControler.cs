using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorControler : MonoBehaviour {

    PlayerData pd;
    private void Start()
    {
        pd = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Loot"))
        {
            if (other.transform.parent.gameObject.GetComponent<LootManager>().movingToCollector)
            {
                pd.IncrementInventory(other.transform.parent.gameObject.GetComponent<LootManager>().bones,
                    other.transform.parent.gameObject.GetComponent<LootManager>().flesh,
                    0, other.transform.parent.gameObject.GetComponent<LootManager>().tears);
                Destroy(other.transform.parent.gameObject);
            }
        }
    }
    
}
