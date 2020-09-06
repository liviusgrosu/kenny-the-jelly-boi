using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAdvanceBlock : MonoBehaviour
{
    public string nextSceneName;

    public GameObject playerModel, cannonModel;
    public Animator playerAnim, cannonAnim;

    private void Update() {
        if(Input.GetKey(KeyCode.Space)) {

        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.GetComponent<Player>().AdvanceThePlayer(nextSceneName);
            TriggerAnimation();
        }
    }

    public void TriggerAnimation() {
        playerModel.SetActive(true);
        playerAnim.SetTrigger("Blast Off");
        cannonAnim.SetTrigger("Blast Off");
    }
}
