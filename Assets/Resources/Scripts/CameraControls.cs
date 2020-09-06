using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public GameObject playerTarget;

    public RaycastHit ray;

    public Transform origPositioning;

    bool rotateRight, rotateLeft;

    private void Start() {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        origPositioning = GameObject.Find("Original Camera Position").transform;
    }

    private void Update() {

        //Turn clockwise
        if(rotateLeft) {
            rotateLeft = false;
            playerTarget.transform.Rotate(new Vector3(0, 90f, 0));
            playerTarget.GetComponent<Player>().ChangeDirection(1);
        }

        //Turn counter-clockwise
        if (rotateRight) {
            rotateRight = false;
            playerTarget.transform.Rotate(new Vector3(0, -90f, 0));
            playerTarget.GetComponent<Player>().ChangeDirection(0);
        }

        if (Physics.Raycast(playerTarget.transform.position, origPositioning.position - playerTarget.transform.position, out ray, Vector3.Distance(origPositioning.position, playerTarget.transform.position))) {
            if (ray.collider.gameObject.tag != "Invisible Block" && ray.collider.gameObject.tag != "Movable" && ray.collider.gameObject.tag != "CameraPassThrough") {
                transform.position = ray.point;
                print(ray.collider.gameObject.tag);
            }
        }
        else {
            transform.position = origPositioning.position;
        }
    }

    //1 - Left
    //2 - Right
    public void RotateButtonPress(int index) {
        if (index == 1) rotateLeft = true;
        if (index == 2) rotateRight = true;
    }
}
