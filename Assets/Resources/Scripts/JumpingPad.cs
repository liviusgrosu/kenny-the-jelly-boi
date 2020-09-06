using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JumpingPad : MonoBehaviour
{
    Transform[] jumpPositions;
    int currentPositionIndex = -1;
    bool isJumping;
    GameObject entity;

    public float jumpSpeed = 1.0f;
    public float journeyTime = 1.0f;
    float startTime;

    public Transform startPos, endPos;
    Transform centrePos;

    Vector3 centerPoint;
    Vector3 startRelCenter;
    Vector3 endRelCenter;

    private void Start() {
        GetArchCenter(Vector3.up * 0.1f, startPos.position, endPos.position);
    }

    private void Update() {
        if (isJumping) {

            if (Vector3.Distance(entity.transform.position, endPos.position) > 0.0001f) {
                float fracComplete = (Time.time - startTime) / journeyTime * jumpSpeed;
                entity.transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * jumpSpeed);
                entity.transform.position += centerPoint;
            }
            else
                isJumping = false;
        }
    }


    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            entity = other.gameObject;
            isJumping = true;
            currentPositionIndex = 1;
            startTime = Time.time;
        }
    }

    public void GetArchCenter(Vector3 direction, Vector3 startPos, Vector3 endPos) {
        centerPoint = (startPos + endPos) * 0.5f;
        centerPoint -= direction;

        startRelCenter = startPos - centerPoint;
        endRelCenter = endPos - centerPoint;
    }
}
