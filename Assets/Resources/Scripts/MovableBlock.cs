using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlock : MonoBehaviour, IValidStandingSpace 
{
    public bool isValidToStand;

    bool isFalling;
    public float fallingSpeed = 3f;

    Rigidbody rb;

    Vector3 bottomDir;
    RaycastHit bottomRay;

    EntitySlidingMechanic slidingMechanic;
    EntityFallingMechanic fallingMechanic;

    int previousDirection;

    bool isPrepForFalling;
    bool isPrepForSliding;
    bool isPrepToDie;

    GameObject playerObj;
    bool isGrabbed;

    public bool isIndestructible;

    public Renderer outlineRend;
    public Material readyToDragMat, DraggingMat;

    bool isMoving;
    Vector3 newPos;

    float startTime;
    public float journeyTime = 1.0f;
    public float speed;

    public MasterAudioSystem audioSource;

    void Start() {
        bottomDir = -transform.up;

        rb = GetComponent<Rigidbody>();

        slidingMechanic = GetComponent<EntitySlidingMechanic>();
        fallingMechanic = GetComponent<EntityFallingMechanic>();

        outlineRend.enabled = false;
    }

    void Update() {

        if(audioSource == null && GameObject.Find("Player Sounds") != null)
            audioSource = GameObject.Find("Player Sounds").GetComponent<MasterAudioSystem>();

        if (isPrepToDie)
        {
            KillTheBox();
            isPrepToDie = false;
        }

        if (isPrepForFalling && !isMoving) {
            fallingMechanic.ToggleFall(true);
            isPrepForFalling = false;
        }

        if(isPrepForSliding) {
            slidingMechanic.ToggleSlide(true, previousDirection);
            isPrepForSliding = false;
        }

        if(isMoving) {
            if (Vector3.Distance(transform.position, newPos) > 0.0001f) {
                float fracComplete = (Time.time - startTime) / journeyTime * speed;
                transform.position = Vector3.Lerp(transform.position, newPos, fracComplete * speed);
            } else {
                isMoving = false;
            }
        }
    }

    public bool CanBeMoved(int dir) {
        float moveX = 0f, moveZ = 0f;

        previousDirection = dir;

        switch (dir) {
            case 1:
                moveX = -1f;
                moveZ = 0f;
                break;
            case 2:
                moveX = 1f;
                moveZ = 0f;
                break;
            case 3:
                moveX = 0f;
                moveZ = -1f;
                break;
            case 4:
                moveX = 0f;
                moveZ = 1f;
                break;
        }
        if (CheckValidMovemement(CheckBlockSurrondings(dir)) == 0) {
            isMoving = true;
            newPos = transform.position + new Vector3(moveX, 0f, moveZ);
            startTime = Time.time;

            if(audioSource != null) audioSource.PlayMovableBlockSound("Move");

            return true;
        }
        return false;
    }

    //1 - Left
    //2 - Right
    //3 - Up
    //4 - Down
    bool? CheckBlockSurrondings(int dir) {
        RaycastHit middleBlock, lowerBlock;
        Vector3 middleBlockDir;

        middleBlockDir = Vector3.zero;

        bool? surrBlockCond = null;

        if (dir == 1) middleBlockDir = -transform.right;
        if (dir == 2) middleBlockDir = transform.right;
        if (dir == 3) middleBlockDir = -transform.forward;
        if (dir == 4) middleBlockDir = transform.forward;

        //Middle block check
        if (Physics.Raycast(transform.position, middleBlockDir, out middleBlock, 1f)) {
            if (middleBlock.collider.gameObject != null) {
                if (isGrabbed) surrBlockCond = false;
                else if (middleBlock.collider.GetComponent<WorldBlock>().isValidToStand) surrBlockCond = true;
                else if (!middleBlock.collider.GetComponent<WorldBlock>().isValidToStand) surrBlockCond = false;
            }
            else surrBlockCond = null;
        }

        //Lower block check
        if (Physics.Raycast(transform.position + new Vector3(0, -1f, 0), middleBlockDir, out lowerBlock, 1f)) {
            if (lowerBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) {
                if (lowerBlock.collider.tag == "Sliding Block") isPrepForSliding = true;
            }
            if (lowerBlock.collider.GetComponent<IValidStandingSpace>().IsDeadlyToStandOn()) isPrepToDie = true;
        }
        else {
            isPrepForFalling = true;
            ToggleGrabbingState(false);
        }

        return surrBlockCond;
    }

    int CheckValidMovemement(bool? validBlock) {
        if (validBlock == true) {
            if (isPrepForFalling) isPrepForFalling = false;
            return -1;
        }
        else return 0;
    }


    public bool IsValidToStandOn() {
        return isValidToStand;
    }

    public bool IsDeadlyToStandOn()
    {
        return false;
    }

    public void ToggleGrabbingState(bool state) {
        isGrabbed = state;
    }

    public bool GetGrabbingState() {
        return isGrabbed;
    }

    public void KillTheBox() {
        if(!isIndestructible) Destroy(this.gameObject);
    }

    //0 - Inactive
    //1 - Ready to grab
    //2 - Is already grabbing
    public void ToggleDragColours(int mode) {
        
        switch(mode) {
            case 0:
                outlineRend.enabled = false;
                break;
            case 1:
                outlineRend.enabled = true;
                outlineRend.material = readyToDragMat;
                break;
            case 2:
                outlineRend.enabled = true;
                outlineRend.material = DraggingMat;
                break;
        }
    }
}
