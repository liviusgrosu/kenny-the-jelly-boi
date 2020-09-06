using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlockSpawner : MonoBehaviour
{
    Animator spawnerAnim;
    public GameObject movableBlockObj; 
    GameObject movableBlockObjInsta;
    public Transform instaPoint;

    bool shouldSpawn = false;

    public GameObject prop;

    public bool isIndestructible;

    void Start()
    {
        spawnerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnerAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && movableBlockObjInsta == null)
        {
            if(shouldSpawn)
            {
                movableBlockObjInsta = Instantiate(movableBlockObj, instaPoint.position, Quaternion.identity);
                movableBlockObjInsta.GetComponent<MovableBlock>().isIndestructible = isIndestructible;
                shouldSpawn = false;
            }
            else 
            {
                spawnerAnim.SetTrigger("Spawn");
                shouldSpawn = true;     
            }
        }
        else if(spawnerAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && movableBlockObjInsta != null)
        {
            prop.gameObject.SetActive(false);
        }
    }
}
