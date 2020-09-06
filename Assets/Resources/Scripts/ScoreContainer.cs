using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreContainer : MonoBehaviour
{
    public int scoreValue;

    private void OnTriggerStay(Collider other) {
        if(other.tag == "Player") {
            other.gameObject.GetComponent<ScoreSystem>().AddToScore(scoreValue);
            other.gameObject.GetComponent<Player>().ChangeToHappyFace();
            Destroy(this.gameObject);
        }
    }
}
