using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public Transform path;
    //false - forwards
    //true  - backwards
    public bool pathDir;
    Transform[] pathNodes;
    public int currPathIndex = 0;

    public float aiSpeed = 1.0f;
    float startTime;
    float journeyLength;
    float fractionOfJourney;
    float distCovered;

    RaycastHit forwardRay;
    Vector3 forwardDir;

    // Start is called before the first frame update
    void Start()
    {
        pathNodes = path.GetComponentsInChildren<Transform>();
        pathNodes = pathNodes.Skip(1).ToArray();
        ReinitializeTimeVariables();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Physics.Raycast(transform.position, forwardDir, out forwardRay, 1f))
        {
            if(forwardRay.collider.tag == "Movable")
            {
                if(!pathDir) currPathIndex--;
                else currPathIndex++;
                if (currPathIndex >= pathNodes.Length) {
                    currPathIndex = 0;
                }
                if (currPathIndex < 0) currPathIndex = pathNodes.Length - 1;
                ReinitializeTimeVariables();
                pathDir = !pathDir;
            }
        }

        if(Vector3.Distance(transform.position, pathNodes[currPathIndex].position) < 0.01f) {
            if(!pathDir) currPathIndex++;
            else currPathIndex--;
            if (currPathIndex >= pathNodes.Length) {
                currPathIndex = 0;
            }
            if (currPathIndex < 0) currPathIndex = pathNodes.Length - 1;
            ReinitializeTimeVariables();
        } else {
            distCovered = (Time.time - startTime) * aiSpeed;
            fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, pathNodes[currPathIndex].position, fractionOfJourney);
        }
    }

    void ReinitializeTimeVariables() {
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, pathNodes[currPathIndex].position);
        forwardDir = pathNodes[currPathIndex].position - transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            other.GetComponent<Player>().KillThePlayer();
        }
    }
}
