using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelSpawner : MonoBehaviour
{
    public GameObject[] characterModels;
    public float intervalTiming;
    public float deathSpeed = 10f;
    public float dragSpeed = 10f;
    public float angularDragSpeed = 2f;

    //false - forwards
    //true  - backwards
    bool forwBack;
    public Transform pos1, pos2, currPos;


    public float spawnerSpeed = 1.0f;
    float startTime;
    float journeyLength;
    float fractionOfJourney;
    float distCovered;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject models in characterModels)
        {
            models.GetComponent<Rigidbody>().drag = dragSpeed;
            models.GetComponent<Rigidbody>().angularDrag = angularDragSpeed;
            
        }

        InvokeRepeating("SpawnCharacterModel", 0f, intervalTiming);
        ReinitializeTimeVariables();
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, currPos.position) < 0.01f)
            ReinitializeTimeVariables();
        else 
        {
            distCovered = (Time.time - startTime) * spawnerSpeed;
            fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Slerp(transform.position, currPos.position, fractionOfJourney);
        }
    }

    void ReinitializeTimeVariables() {
        startTime = Time.time;
        forwBack = !forwBack;
        if(!forwBack)
        {
            journeyLength = Vector3.Distance(transform.position, pos1.position);
            currPos = pos1;
        }
        else
        { 
            journeyLength = Vector3.Distance(transform.position, pos2.position);
            currPos = pos2;
        }
    }


    void SpawnCharacterModel()
    {
        GameObject insta = Instantiate(characterModels[Random.Range(0,4)], transform.position, transform.rotation);
        insta.transform.rotation = Random.rotation;
        Destroy(insta, deathSpeed);
    }
}
