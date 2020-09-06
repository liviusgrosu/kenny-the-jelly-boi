using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMechanic : MonoBehaviour
{
    bool isOnBoat;

    public Transform startPosition, endPosition;
    Transform currStartPosition, currEndPosition;
    //true = forwards
    //false = backwards
    bool forwardBackwardsMode;

    GameObject playerObj;

    public float boatSpeed = 1.0f;
    float startTime;
    float journeyLength;
    float fractionOfJourney;
    float distCovered;

    private void FixedUpdate() {

        if (isOnBoat) {
            if (Vector3.Distance(transform.position, currEndPosition.position) < 0.001f) {
                isOnBoat = false;
                playerObj.GetComponent<Player>().ToggleSailingState(false);
                return;
            }
            else {
                distCovered = (Time.time - startTime) * boatSpeed;
                fractionOfJourney = distCovered / journeyLength;
                playerObj.transform.position = Vector3.Lerp(currStartPosition.position + new Vector3(0, 0.5f, 0), currEndPosition.position + new Vector3(0, 0.5f, 0), fractionOfJourney);
                transform.position = Vector3.Lerp(currStartPosition.position, currEndPosition.position, fractionOfJourney);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            playerObj = other.gameObject;
            isOnBoat = true;
            forwardBackwardsMode = !forwardBackwardsMode;
            playerObj.GetComponent<Player>().ToggleSailingState(true);

            if(forwardBackwardsMode) {
                currStartPosition = startPosition;
                currEndPosition = endPosition;
            } else {
                currStartPosition = endPosition;
                currEndPosition = startPosition;
            }

            ReinitializeTimeVariables();
        }
    }

    void ReinitializeTimeVariables() {
        startTime = Time.time;
        journeyLength = Vector3.Distance(currStartPosition.position, currEndPosition.position);
    }
}
