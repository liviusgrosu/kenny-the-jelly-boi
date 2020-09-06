using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public GameObject triggerObj;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            triggerObj.GetComponent<ITrigger>().TriggerObj(true);
            Destroy(this.gameObject);
        }
    }
}
