using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalControler : MonoBehaviour {

    public List<Transform> patrolPoints;
    int currentPoint;
    public float animalWaitTime;
    float timeSinceAnimalWaited;
    bool isWaiting;
    public float animalSpeed;

    public float lootType;
    public float currentHealth;
    public float maxHealth;
    public Slider healthSlider;

    public int dropBones;
    public int dropFlesh;
    public int dropTears;

    public bool beingGrabbed;

    PlayerData pd;

    private void Awake()
    {
        pd = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerData>();
        beingGrabbed = false;
        currentHealth = maxHealth;
        patrolPoints = this.transform.parent.parent.gameObject.GetComponent<AnimalSpawner>().patrolPoints;
        int currentPoint = Random.Range(0, patrolPoints.Count);
        this.transform.position = patrolPoints[currentPoint].position;

        isWaiting = true;
        timeSinceAnimalWaited = Time.timeSinceLevelLoad;
        
    }

    public void PlayerAtack(float attackDamage)
    {
        if (!healthSlider.gameObject.activeSelf)
        {
            healthSlider.gameObject.SetActive(true);
        }
        currentHealth -= attackDamage;
        healthSlider.value = currentHealth / maxHealth;
        if (currentHealth <= 0.0f)
        {
            DropLoot();
            Destroy(this.gameObject);
        }
    }

    void DropLoot()
    {
        if (lootType == 0)
        {
            GameObject myLoot = Instantiate(Resources.Load("FullLoot", typeof(GameObject))) as GameObject;
            myLoot.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            myLoot.GetComponent<LootManager>().InitializeLoot(dropBones, dropFlesh, dropTears);
        }
        else
        {
            GameObject myLoot = Instantiate(Resources.Load("SnakeLoot", typeof(GameObject))) as GameObject;
            myLoot.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            myLoot.GetComponent<LootManager>().InitializeLoot(dropBones, dropFlesh, dropTears);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("BoneRemover") || other.gameObject.tag.Equals("BasebalBat") || other.gameObject.tag.Equals("BarbecueGril"))
        {
            //Debug.Log("BoneRemover XXXXX: " + other.transform.parent.name);
            other.transform.parent.gameObject.GetComponent<ConstructControler>().AnimalEntry(this.gameObject);
            //Destroy(this.gameObject);
        }
    }

    void Update()
    {
        healthSlider.transform.parent.LookAt(healthSlider.transform.parent.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        if (!beingGrabbed)
        {
            if (isWaiting)
            {
                timeSinceAnimalWaited += Time.deltaTime;
                if (timeSinceAnimalWaited > animalWaitTime)
                {
                    isWaiting = false;
                    timeSinceAnimalWaited = 0.0f;
                    currentPoint = Random.Range(0, patrolPoints.Count);
                    this.GetComponent<Animator>().SetBool("isWalking", true);
                    Vector3 dir = patrolPoints[currentPoint].position - this.transform.position;
                    this.transform.LookAt(patrolPoints[currentPoint].position);
                    this.transform.Rotate(new Vector3(0, 90, 0));
                    //this.transform.rotation =
                    //          Quaternion.Euler(-90, 0, Vector2.SignedAngle(new Vector2(dir.x, dir.z), new Vector2(0.0f, -1.0f)));

                }

            }
            else if (Vector3.Distance(patrolPoints[currentPoint].position, this.transform.position) < 0.001f)
            {
                this.transform.position = patrolPoints[currentPoint].position;
                isWaiting = true;
                this.GetComponent<Animator>().SetBool("isWalking", false);
            }
            else
            {
                // The step size is equal to speed times frame time.
                float step = animalSpeed * Time.deltaTime;

                // Move our position a step closer to the target.
                transform.position = Vector3.MoveTowards(this.transform.position, patrolPoints[currentPoint].position, step);
            }
        }
    }
}
