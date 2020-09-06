using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpersionChange : MonoBehaviour
{
    public GameObject regEyeLeft, regEyeRight, deadEyeLeft, deadEyeRight;
    public GameObject regMouth, supriseMouth, deadMouth, happyMouth;

    bool isInIdleFace, isInDeadFace, isInSupriseFace, isInHappyFace;

    public float supriseMouthExposureTime;
    public float happySmileExposureTime;

    float timer = 0.0f;

    private void Start() {
        isInIdleFace = true;
        ChangeFaces();
    }

    private void Update() {
        if(isInHappyFace) {
            timer -= Time.deltaTime;
            if(timer <= 0.0f) {
                isInIdleFace = true;
                isInHappyFace = false;
                ChangeFaces();
            }
        }
    }

    public void TriggerChangeOfFaces(string faceType) {
        switch(faceType) {
            case "happy":
                if (isInIdleFace) isInIdleFace = false;

                isInHappyFace = true;
                timer = happySmileExposureTime;
                ChangeFaces();
                break;
        }
    }

    public void ChangeFaces() {
        regEyeLeft.SetActive(false);
        regEyeRight.SetActive(false);
        deadEyeLeft.SetActive(false);
        deadEyeRight.SetActive(false);

        regMouth.SetActive(false);
        supriseMouth.SetActive(false);
        deadMouth.SetActive(false);
        happyMouth.SetActive(false);

        if(isInIdleFace) {
            regEyeLeft.SetActive(true);
            regEyeRight.SetActive(true);
            regMouth.SetActive(true);
            return;
        }

        if (isInHappyFace) {
            regEyeLeft.SetActive(true);
            regEyeRight.SetActive(true);
            happyMouth.SetActive(true);
            return;
        }

        if (isInSupriseFace) {
            deadEyeLeft.SetActive(true);
            deadEyeRight.SetActive(true);
            supriseMouth.SetActive(true);
            return;
        }

        if (isInDeadFace) {
            deadEyeLeft.SetActive(true);
            deadEyeRight.SetActive(true);
            deadMouth.SetActive(true);
            return;
        }
    }
}
