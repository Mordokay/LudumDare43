using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour {


    public int bones;
    public int flesh;
    public int tears;

    GameObject collector;
    public bool movingToCollector;

    private void Start()
    {
        movingToCollector = false;
        collector = GameObject.FindGameObjectWithTag("Collector");
    }

    public void InitializeLoot(int bones, int flesh, int tears)
    {
        this.bones = bones;
        this.flesh = flesh;
        this.tears = tears;
    }
    
    public void MoveToCollector()
    {
        movingToCollector = true;
    }

    private void Update()
    {
        if (movingToCollector)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, collector.transform.position, Time.deltaTime * 10.0f);
        }
    }
}