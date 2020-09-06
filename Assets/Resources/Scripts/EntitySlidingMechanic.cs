using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySlidingMechanic : MonoBehaviour
{
    bool isSliding;
    int previousPlayerDirection = 0;

    Vector3 bottomBlockDir, slidingDir;
    RaycastHit bottomBlockRay, forwardBlockRay, previousBottomBlockRay;

    Rigidbody rb;
    EntityFallingMechanic fallingMechanic;

    public float slidingSpeed = 1f;

    private void Start() {
        bottomBlockDir = -transform.up;
        rb = GetComponent<Rigidbody>();
        fallingMechanic = GetComponent<EntityFallingMechanic>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(isSliding) {
            CalculateSlidingSpeed();
            //Bottom block check
            if (Physics.Raycast(transform.position, bottomBlockDir, out bottomBlockRay, 1f)) {
                previousBottomBlockRay = bottomBlockRay;
                if (bottomBlockRay.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) {
                    if (bottomBlockRay.collider.tag != "Sliding Block") {
                        ToggleSlide(false, -1);
                        transform.position = bottomBlockRay.collider.transform.position + new Vector3(0, 1f, 0);
                    }
                }
            }
            else {
                ToggleSlide(false, -1);
                transform.position = previousBottomBlockRay.collider.transform.position + slidingDir;
                fallingMechanic.ToggleFall(true);
            }

            if (Physics.Raycast(transform.position, slidingDir, out forwardBlockRay, 1f)) {
                if (forwardBlockRay.collider.gameObject != null) {
                    if (forwardBlockRay.collider.GetComponent<IValidStandingSpace>().IsValidToStandOn()) {
                        if (forwardBlockRay.collider.tag != "Sliding Block") {
                            ToggleSlide(false, -1);
                            transform.position = forwardBlockRay.collider.transform.position - slidingDir;
                        }
                    }
                }
            }
        }
    }

    public void ToggleSlide(bool state, int playerDirection) {
        previousPlayerDirection = playerDirection;
        isSliding = state;

        if (isSliding) {
            if (previousPlayerDirection == 1) slidingDir = -transform.right;
            if (previousPlayerDirection == 2) slidingDir = transform.right;
            if (previousPlayerDirection == 3) slidingDir = -transform.forward;
            if (previousPlayerDirection == 4) slidingDir = transform.forward;
        }
    }

    void CalculateSlidingSpeed() {
        rb.MovePosition(transform.position + slidingDir * Time.fixedDeltaTime * slidingSpeed);
    }

    public bool IsSliding() {
        return isSliding;
    }
}
