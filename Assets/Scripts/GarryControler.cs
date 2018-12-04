using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarryControler : MonoBehaviour {

    public float strength;
    public float positivity;
    public float health;

    Vector3 goalPos;
    public int currentPathPoint;

    MapMaker mapMaker;
    public float garrySpeed;

    bool isWalking;
    bool isAtacking;
    public bool isRecovering;

    bool destroyingObstacle;
    float timeSinceLastAtack;
    public float timeBetweenAtacks;

    public bool garryActivated;
    GameObject objectToAtack;

    public Slider healthBar;
    public Slider positivityBar;
    public Text strengthNumberText;

    private void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(4).GetChild(2).GetChild(0).GetComponent<Slider>();
        positivityBar = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(4).GetChild(2).GetChild(1).GetComponent<Slider>();
        strengthNumberText = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(4).GetChild(2).GetChild(2).GetComponent<Text>();
        garryActivated = false;
        goalPos = Vector3.zero;
        mapMaker = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapMaker>();
        currentPathPoint = 0;
        strength = 1;
        health = 100;
        transform.position = new Vector3(transform.position.x, 0.85f, transform.position.z);
    }

    public void UpgradeStrength(float strengthUpgrade)
    {
        strength += strengthUpgrade;
        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        if (!isRecovering)
        {
            health -= damage;
            UpdateUI();
        }
    }

    public void GainPositivity(float positivityValue)
    {
        positivity += positivityValue;
        UpdateUI();
        if(positivity >= 100)
        {
            mapMaker.ShowWin();
        }
    }

    void UpdateUI()
    {
        positivityBar.value = positivity / 100.0f;
        positivityBar.gameObject.GetComponentInChildren<Text>().text = positivity.ToString();
        healthBar.value = health / 100.0f;
        healthBar.gameObject.GetComponentInChildren<Text>().text = health.ToString();
        strengthNumberText.text = strength.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("PlateOfMeat") || other.gameObject.tag.Equals("MeatBolder"))
        {
            objectToAtack = other.gameObject;
            timeSinceLastAtack = 0.0f;
            destroyingObstacle = true;
        }
    }

    void Update ()
    {
        if(health <= 0)
        {
            isRecovering = true;
            this.GetComponent<Animator>().SetTrigger("recovering");
        }
        if (isRecovering)
        {
            if(health < 100)
            {
                health += strength * 2 * Time.deltaTime;
                UpdateUI();
            }
            else
            {
                health = 100;
                isRecovering = false;
                this.GetComponent<Animator>().SetTrigger("walking");
            }
        }
        else if (garryActivated)
        {
            if (mapMaker.gameStarted)
            {
                if (destroyingObstacle)
                {
                    timeSinceLastAtack += Time.deltaTime;
                    if (objectToAtack != null && timeSinceLastAtack > timeBetweenAtacks)
                    {
                        objectToAtack.GetComponent<ObstacleControler>().TakeDamage(strength);
                        this.GetComponent<Animator>().SetTrigger("attacking");
                        timeSinceLastAtack = 0.0f;
                    }
                    else if(objectToAtack == null)
                    {
                        destroyingObstacle = false;
                        this.GetComponent<Animator>().SetTrigger("walking");
                    }
                }
                else
                {

                    if (mapMaker.garryPath.Count > 2 && currentPathPoint < mapMaker.garryPath.Count)
                    {
                        if (Mathf.Abs(mapMaker.garryPath[currentPathPoint].transform.position.x - this.transform.position.x) < 0.05f &&
                        Mathf.Abs(mapMaker.garryPath[currentPathPoint].transform.position.z - this.transform.position.z) < 0.05f)
                        {
                            currentPathPoint += 1;
                            if (currentPathPoint == mapMaker.garryPath.Count)
                            {
                                mapMaker.ShowLost();
                            }
                            else
                            {
                                goalPos = mapMaker.garryPath[currentPathPoint].transform.position;

                                this.transform.LookAt(new Vector3(goalPos.x, this.transform.position.y, goalPos.z));
                                this.transform.Rotate(new Vector3(0, -90, 0));
                                this.GetComponent<Animator>().SetTrigger("walking");
                            }
                        }
                        else
                        {
                            // The step size is equal to speed times frame time.
                            float step = garrySpeed * Time.deltaTime;

                            // Move our position a step closer to the target.
                            transform.position += (new Vector3(goalPos.x, 0.85f, goalPos.z) - transform.position).normalized * step;
                        }
                    }
                }
            }
        }
	}
}
