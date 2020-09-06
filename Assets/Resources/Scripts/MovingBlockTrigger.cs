using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovingBlock {
    public GameObject obj;
    public Transform positions;
}

public class MovingBlockTrigger : MonoBehaviour, ITrigger {

    public MovingBlock[] blocks;
    bool isMoving;
    bool onOff;

    Vector3 startBlockPos, endBlockPos;

    int currBlockIndex = 0;

    public float movingBlockSpeed = 3f;
    float startTime;
    float journeyLength;
    float fractionOfJourney;
    float distCovered;

    MasterAudioSystem audioSource;

    public bool triggerAtStart;

    void Start() {
        if(triggerAtStart) TriggerObj(false);
    }

    void Update() {

        if (audioSource == null && GameObject.Find("Player Sounds") != null)
            audioSource = GameObject.Find("Player Sounds").GetComponent<MasterAudioSystem>();

        if (isMoving) {
            if (Vector3.Distance(blocks[currBlockIndex].obj.transform.position, endBlockPos) < 0.001f) {
                blocks[currBlockIndex].obj.GetComponent<WorldBlock>().isValidToStand = true;
                currBlockIndex++;
                if (currBlockIndex >= blocks.Length) {
                    isMoving = false;
                    return;
                }
                InitCurrMovingBlockVars();
            } else {
                distCovered = (Time.time - startTime) * movingBlockSpeed;
                fractionOfJourney = distCovered / journeyLength;
                blocks[currBlockIndex].obj.transform.position = Vector3.Lerp(startBlockPos, endBlockPos, fractionOfJourney);
            }
        }
    }

    public void TriggerObj(bool state) {
        onOff = state;
        isMoving = true;
        currBlockIndex = 0;
        InitCurrMovingBlockVars();
    }

    void InitCurrMovingBlockVars() {
        startTime = Time.time;
        if(onOff) {
            startBlockPos = blocks[currBlockIndex].obj.transform.position;
            endBlockPos = blocks[currBlockIndex].positions.GetChild(1).position;
        } else {
            startBlockPos = blocks[currBlockIndex].obj.transform.position;
            endBlockPos = blocks[currBlockIndex].positions.GetChild(0).position;
        }
        journeyLength = Vector3.Distance(startBlockPos, endBlockPos);

        if(audioSource != null) audioSource.PlayerMovableBlockTriggerSounds("Moving Block Trigger");
    }
}
