using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFallingMechanic : MonoBehaviour
{
    bool isPlayer;
    bool isFalling, isCloseToLanding, doneJumping;
    public float fallingSpeed = 3f;

    public float finalFallingSpeed = 3f;
    float startTime;
    float journeyLength;
    float fractionOfJourney;
    float distCovered;

    Vector3 finalFallingInitialPosition, finalFallingEndPosition;

    Vector3 bottomBlockDir;
    RaycastHit bottomBlockRay;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        bottomBlockDir = -transform.up;
        rb = GetComponent<Rigidbody>();
        if (transform.GetComponent<Player>() != null) isPlayer = true;
        doneJumping = true;
    }

    // Update is called once per frame
    void Update() {
        if (isFalling) {
            if (Physics.Raycast(transform.position, bottomBlockDir, out bottomBlockRay, 1f)) {

                ToggleFall(false); //Remove if needed
                InitializeFinalFallingVariables(bottomBlockRay.collider.transform);
                isCloseToLanding = true;
                if(isPlayer) transform.GetComponent<Player>().ContinueHoppingAnimation();
            }
        }
        if(isCloseToLanding) {
            if(Vector3.Distance(transform.position, finalFallingEndPosition) < 0.01f) {
                isCloseToLanding = false;
                doneJumping = true;
                transform.position = bottomBlockRay.collider.transform.position + new Vector3(0, 1f, 0);

                if (bottomBlockRay.collider.GetComponent<IValidStandingSpace>().IsDeadlyToStandOn()) {
                    if (GetComponent<Player>() != null) GetComponent<Player>().KillThePlayer();
                    if (GetComponent<MovableBlock>() != null) GetComponent<MovableBlock>().KillTheBox();
                }

                if(GetComponent<MovableBlock>() != null) {
                    GetComponent<MovableBlock>().audioSource.PlayMovableBlockSound("Land");
                }
            } else {
                distCovered = (Time.time - startTime) * finalFallingSpeed;
                fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(finalFallingInitialPosition, finalFallingEndPosition, fractionOfJourney);
            }
        }
    }

    public void ToggleFall(bool state) {
        isFalling = state;
        doneJumping = false;
        rb.isKinematic = !state;

        if (isFalling) CalculateFallingSpeed();
        else rb.velocity = new Vector3(0, 0, 0);
    }

    void CalculateFallingSpeed() {
        rb.AddForce(bottomBlockDir * fallingSpeed, ForceMode.Impulse);
    }

    void InitializeFinalFallingVariables(Transform landingBlockPos) {
        finalFallingInitialPosition = transform.position;
        finalFallingEndPosition = landingBlockPos.position + new Vector3(0, 1f, 0);
        startTime = Time.time;
        journeyLength = Vector3.Distance(finalFallingInitialPosition, finalFallingEndPosition);
    }

    public bool IsPlayerFalling() {
        return isFalling;
    }

    public bool IsDoneFalling() {
        return doneJumping;
    }
}
