using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    GameObject playerInsta;
    public GameObject playerObj;

    private void Start() {
        SpawnPlayer();
    }

    public void SpawnPlayer() {
        Destroy(playerInsta);
        playerInsta = Instantiate(playerObj, transform.position, Quaternion.identity);
    }
}
