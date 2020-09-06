using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool?[] surrBlockCond;
    Vector3[] playerDir = new Vector3[9];
    int playerLayerMask;

    CameraControls cam;

    EntitySlidingMechanic slideMechanic;
    EntityFallingMechanic fallingMechanic;

    //Arch Movement
    public float journeyTime = 1.0f;
    public float speed;
    int previousDir;
    int previousRelDir;

    float startTime;
    Vector3 centerPoint;
    Vector3 startRelCenter;
    Vector3 endRelCenter;

    bool isMoving;
    bool startMoving;

    bool isFalling;
    bool isPrepForFalling;
    bool isPrepToSlide;
    bool isPrepToHalt;
    bool isSailing;
    bool isPrepareToDie;
    bool isDead;
    bool isGrabbing;
    bool isAdvancingLevel;
    
    string nextLevelName;
    public float fallingSpeed = 3f;

    Rigidbody rb;
    Collider col;

    Vector3 bottomDir;
    RaycastHit bottomRay;

    Vector3 newPos;

    public Transform characterModel;
    Animator characterAnimation;

    public float cutOffFallAnimTime;

    public PlayerExpersionChange expersions;

    public float idleAnimStartTime = 1.0f;
    float currIdleTime = 0.0f;

    bool finishSpawnAnim;
    bool stillJumping;

    Vector2 forwardVec, backwardVec, leftVec, rightVec;
    Vector2 relForwardVec, relBackwardVec, relLeftVec, relRightVec;
    float relForwardAngle = 0;
    int relForward = 4, relBackward = 3, relLeft = 1, relRight = 2;
    int currFacingDirection = 1;

    bool goLeft, goRight, goDown, goUp;

    public MasterAudioSystem audioSource;

    private void Start() {
        playerLayerMask = LayerMask.GetMask("PlayerBlock");

        bottomDir = -transform.up;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        slideMechanic = GetComponent<EntitySlidingMechanic>();
        fallingMechanic = GetComponent<EntityFallingMechanic>();
        characterAnimation = characterModel.GetComponent<Animator>();

        forwardVec = new Vector2(0,1);
        backwardVec = new Vector2(0,-1);
        leftVec = new Vector2(-1,0);
        rightVec = new Vector2(1,0);

        relForwardVec = forwardVec;
        relBackwardVec = backwardVec;
        relLeftVec = leftVec;
        relRightVec = rightVec;
    }

    // Update is called once per frame
    void Update()
    {
        surrBlockCond = new bool?[5];
        float moveX = 0.0f, moveZ = 0.0f;

        if (!finishSpawnAnim && !characterAnimation.GetCurrentAnimatorStateInfo(0).IsName("Player Spawn")) {
            finishSpawnAnim = true;
            characterAnimation.applyRootMotion = true;
        }

        if(stillJumping && !characterAnimation.GetCurrentAnimatorStateInfo(0).IsName("Player Hop")) {
            stillJumping = false;
            characterModel.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (!isMoving && !isDead) {

            if (!fallingMechanic.IsDoneFalling()) {
                goLeft = false;
                goRight = false;
                goDown = false;
                goUp = false;
            }

            if (!fallingMechanic.IsPlayerFalling() && !slideMechanic.IsSliding() && !isSailing && !isMoving && finishSpawnAnim) {

                if (currIdleTime <= idleAnimStartTime) currIdleTime += Time.deltaTime;
                else {
                    if (characterAnimation.GetCurrentAnimatorStateInfo(0).IsName("Player"))
                        characterAnimation.SetTrigger("Idle");
                }

                if (goLeft) {
                    goLeft = false;

                    characterModel.eulerAngles = new Vector3(0, relForwardAngle, 0);

                    surrBlockCond = CheckPlayerSurrondings(relLeft);
                    previousRelDir = relLeft;
                    previousDir = 1;

                    moveX = relLeftVec.x;
                    moveZ = relLeftVec.y;
                    startMoving = true;

                    StopIdling();
                }
                if (goRight) {
                    goRight = false;

                    characterModel.eulerAngles = new Vector3(0, relForwardAngle + 180f, 0);

                    surrBlockCond = CheckPlayerSurrondings(relRight);
                    previousRelDir = relRight;
                    previousDir = 2;

                    moveX = relRightVec.x;
                    moveZ = relRightVec.y;
                    startMoving = true;

                    StopIdling();
                    print("going right");

                }
                if (goDown) {
                    goDown = false;

                    characterModel.eulerAngles = new Vector3(0, relForwardAngle + 270f, 0);

                    surrBlockCond = CheckPlayerSurrondings(relBackward);
                    previousRelDir = relBackward;
                    previousDir = 3;

                    moveX = relBackwardVec.x;
                    moveZ = relBackwardVec.y;
                    startMoving = true;

                    StopIdling();
                }
                if (goUp) {
                    goUp = false;

                    characterModel.eulerAngles = new Vector3(0, relForwardAngle + 90f, 0);
                    surrBlockCond = CheckPlayerSurrondings(relForward);
                    previousRelDir = relForward;
                    previousDir = 4;

                    moveX = relForwardVec.x;
                    moveZ = relForwardVec.y;
                    startMoving = true;

                    StopIdling();
                }
            }

            if (startMoving) {
                characterAnimation.applyRootMotion = true;
                newPos = Vector3.zero;

                Vector3 translationAmount = Vector3.zero;

                if (CheckValidMovemement(surrBlockCond) == 0) {
                    
                    translationAmount = new Vector3(moveX, 1f, moveZ);
                    ToggleGrabbingState(false);
                }
                else if (CheckValidMovemement(surrBlockCond) == 1) translationAmount = new Vector3(moveX, 0f, moveZ);
                else if (CheckValidMovemement(surrBlockCond) == -1) translationAmount = Vector3.zero;
                else {
                    translationAmount = new Vector3(moveX, 0f, moveZ);
                    isPrepForFalling = true;
                    ToggleGrabbingState(false);
                }

                newPos = transform.position + translationAmount;

                GetArchCenter(Vector3.up * 0.1f, transform.position, newPos);
                startTime = Time.time;

                isMoving = true;
                startMoving = false;

                characterAnimation.SetTrigger("Hop");
                audioSource.PlayPlayerSound("Hop");
                stillJumping = true;

                if(isGrabbing) {
                    GetComponent<DragObject>().MoveBlock(previousRelDir);
                }
            }
        }
        else {
            if (Vector3.Distance(transform.position, newPos) > 0.0001f) {
                float fracComplete = (Time.time - startTime) / journeyTime * speed;
                transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
                transform.position += centerPoint;

                if (isPrepForFalling && characterAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= (cutOffFallAnimTime)) PauseHoppingAnimation();
            }
            else {
                isMoving = false;

                if (isPrepToSlide) {
                    slideMechanic.ToggleSlide(true, previousDir);
                    isPrepToSlide = false;
                }

                if (isPrepForFalling) {
                    fallingMechanic.ToggleFall(true);
                    isPrepForFalling = false;
                }

                if (isPrepareToDie) {

                    KillThePlayer();
                    isPrepareToDie = false;
                }
            }
        }
    }
    
    //1 - Left
    //2 - Right
    //3 - Forward
    //4 - Backward
    bool?[] CheckPlayerSurrondings(int dir) {
        RaycastHit topBlock, upperBlock, middleBlock, lowerBlock, lowestBlock;
        Vector3 topBlockDir, upperBlockDir, middleBlockDir, lowerBlockDir, lowestBlockDir;

        topBlockDir = upperBlockDir = middleBlockDir = lowerBlockDir = lowestBlockDir = Vector3.zero;

        bool?[] surrBlockCond = new bool?[5];

        middleBlockDir = upperBlockDir = lowerBlockDir = lowestBlockDir = -characterModel.right;

        topBlockDir = transform.up;


        //Top
        if (Physics.Raycast(transform.position, topBlockDir, out topBlock, 1f)) {
            if (topBlock.collider.gameObject != null) {
                if (topBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) surrBlockCond[0] = true;
                if (!topBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) surrBlockCond[0] = false;
            }
            else surrBlockCond[0] = null;
        }

        //Upper block check
        if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), upperBlockDir, out upperBlock, 1f)) {
            if (upperBlock.collider.GetComponent<IValidStandingSpace>() != null) {
                if (upperBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) surrBlockCond[1] = true;
                if (!upperBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) surrBlockCond[1] = false;
            }
            else surrBlockCond[1] = null;
        }

        //Middle block check
        if (Physics.Raycast(transform.position, middleBlockDir, out middleBlock, 1f)) {
            if (middleBlock.collider.gameObject != null) {
                if (middleBlock.collider.tag == "Movable" && middleBlock.collider.GetComponent<MovableBlock>() != null) {
                    if (isGrabbing) surrBlockCond[2] = false;
                    else if (!isGrabbing && middleBlock.collider.GetComponent<MovableBlock>().CanBeMoved(dir)) surrBlockCond[2] = false;
                    else surrBlockCond[2] = true;
                }
                else {
                    if (middleBlock.collider.GetComponent<IValidStandingSpace>() != null) {
                        if (middleBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) surrBlockCond[2] = true;
                        if (!middleBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) surrBlockCond[2] = false;
                    }
                }
            }
            else surrBlockCond[2] = null;
        }

        //Lower block check
        if (Physics.Raycast(transform.position + new Vector3(0, -1f, 0), lowerBlockDir, out lowerBlock, 1f)) {
            if (lowerBlock.collider.gameObject != null) {
                if (lowerBlock.collider.GetComponent<IValidStandingSpace>() != null && lowerBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) {
                    surrBlockCond[3] = true;

                    if (lowerBlock.collider.tag == "Sliding Block") isPrepToSlide = true;
                    if (lowerBlock.collider.GetComponent<IValidStandingSpace>().IsDeadlyToStandOn()) {
                        isPrepareToDie = true;
                    }
                }
                if (!lowerBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) surrBlockCond[3] = false;
            }
            else surrBlockCond[3] = null;
        }

        //Lowest block check
        if (Physics.Raycast(transform.position + new Vector3(0, -2f, 0), lowestBlockDir, out lowestBlock, 1f)) {
            if (lowestBlock.collider.gameObject != null) {
                if (lowestBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) surrBlockCond[4] = true;
                if (!lowestBlock.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) surrBlockCond[4] = false;
            }
            else surrBlockCond[4] = null;
        }
        return surrBlockCond;
    }

    int CheckValidMovemement(bool?[] validBlocks) {
        if(validBlocks[2] == true) {
            if (validBlocks[1] == true) return -1;
            else {
                if (validBlocks[0] == true) return -1;
                else return 0;
            }
        }
        else {
            if (validBlocks[3] == true) return 1;
            else {
                return 3;
            }
        }
    }

    //1 - Clockwise
    //0 - Counter-clockwise
    public void ChangeDirection(int dir) {
        if(dir == 1) {
            currFacingDirection++;
            if (currFacingDirection > 4) currFacingDirection = 1;
        } else {
            currFacingDirection--;
            if (currFacingDirection < 1) currFacingDirection = 4;
        }

        switch(currFacingDirection) {
            case 1:
                relForward = 4;
                relBackward = 3;
                relLeft = 1;
                relRight = 2;

                relForwardVec = forwardVec;
                relBackwardVec = backwardVec;
                relLeftVec = leftVec;
                relRightVec = rightVec;

                relForwardAngle = 0f;
                break;
            case 2:
                relForward = 2;
                relBackward = 1;
                relLeft = 4;
                relRight = 3;

                relForwardVec = rightVec;
                relBackwardVec = leftVec;
                relLeftVec = forwardVec;
                relRightVec = backwardVec;

                relForwardAngle = 90f;
                break;
            case 3:
                relForward = 3;
                relBackward = 4;
                relLeft = 2;
                relRight = 1;

                relForwardVec = backwardVec;
                relBackwardVec = forwardVec;
                relLeftVec = rightVec;
                relRightVec = leftVec;

                relForwardAngle = 180f;
                break;
            case 4:
                relForward = 1;
                relBackward = 2;
                relLeft = 3;
                relRight = 4;

                relForwardVec = leftVec;
                relBackwardVec = rightVec;
                relLeftVec = backwardVec;
                relRightVec = forwardVec;

                relForwardAngle = 270f;
                break;
        }
    }

    void ToggleBlockLayers() {
        RaycastHit upperBlock, lowerBlock;

        if (Physics.Raycast(transform.position, transform.up, out upperBlock, 1f)) {
            if (upperBlock.collider.gameObject != null) {
                if (upperBlock.collider.gameObject.layer == 8) upperBlock.collider.gameObject.layer = 0;
                else upperBlock.collider.gameObject.layer = 8;
            }
        }

        if (Physics.Raycast(transform.position, -transform.up, out lowerBlock, 1f)) {
            if (lowerBlock.collider.gameObject != null) {
                if (lowerBlock.collider.gameObject.layer == 8) lowerBlock.collider.gameObject.layer = 0;
                else lowerBlock.collider.gameObject.layer = 8;
            }
        }
    }

    public void GetArchCenter(Vector3 direction, Vector3 startPos, Vector3 endPos) {
        centerPoint = (startPos + endPos) * 0.5f;
        centerPoint -= direction;

        startRelCenter = startPos - centerPoint;
        endRelCenter = endPos - centerPoint;
    }

    public bool IsPlayerMoving() {
        return isMoving;
    }

    public void LetPlayerFall() {
        fallingMechanic.ToggleFall(true);
    }

    public void ToggleSailingState(bool state) {
        isSailing = state;
    }

    public void KillThePlayer() {
        isDead = true;
        StopIdling();
        characterAnimation.SetTrigger("Die");
        audioSource.PlayPlayerSound("Death");
        col.enabled = false;
    }

    public bool IsPlayerDead() {
        return isDead;
    }

    public void AdvanceThePlayer(string name) {

        nextLevelName = name;
        isAdvancingLevel = true;
        characterModel.gameObject.SetActive(false);
        audioSource.PlayPlayerSound("Next Level");
        col.enabled = false;
    }

    public string GetNextLevelName() {
        return nextLevelName;
    }

    public bool IsAdvancingLevel() {
        return isAdvancingLevel;
    }

    public void PauseHoppingAnimation() {
        characterAnimation.speed = 0f;
    }

    public void ContinueHoppingAnimation() {
        characterAnimation.speed = 1f;
    }

    public void ChangeToHappyFace() {
        expersions.TriggerChangeOfFaces("happy");
    }

    public void ToggleGrabbingState(bool state) {
        isGrabbing = state;
    }

    public bool GetGrabbingState() {
        return isGrabbing;
    }

    void StopIdling() {
        currIdleTime = 0.0f;
        if (characterAnimation.GetCurrentAnimatorStateInfo(0).IsName("Player Idle"))
            characterAnimation.SetTrigger("StopIdle");
    }

    public void PressDirectionButton(int index) {
        switch(index) {
            case 1:
                goLeft = true;
                break;
            case 2:
                goRight = true;
                break;
            case 3:
                goDown = true;
                break;
            case 4:
                goUp = true;
                break;
        }
    }
}
