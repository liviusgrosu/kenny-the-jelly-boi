using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    RaycastHit ray;

    GameObject movingBlockObj;

    public MenuSystem menu;

    bool isGrabbing;
    bool readyToDrag;

    private void Update() {
        if (!isGrabbing) {
            if (Physics.Raycast(transform.position, -transform.right, out ray, 1f) ||
                Physics.Raycast(transform.position, transform.right, out ray, 1f) ||
                Physics.Raycast(transform.position, -transform.forward, out ray, 1f) ||
                Physics.Raycast(transform.position, transform.forward, out ray, 1f)) {
                if (ray.collider.tag == "Movable") {
                    movingBlockObj = ray.collider.gameObject;
                    movingBlockObj.GetComponent<MovableBlock>().ToggleDragColours(1);
                    readyToDrag = true;
                    //return;
                }
            }
            else {
                if(movingBlockObj != null) movingBlockObj.GetComponent<MovableBlock>().ToggleDragColours(0);
                movingBlockObj = null;
                readyToDrag = false;
            }
        }
        else {
            if(!GetComponent<Player>().GetGrabbingState() || !movingBlockObj.GetComponent<MovableBlock>().GetGrabbingState()) {
                Release();
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "Movable") {
                    if (readyToDrag) {
                        Grab();
                    }
                    else if (isGrabbing) {
                        Release();
                    }
                }
            }
        }
    }

    public void MoveBlock(int direction) {
        movingBlockObj.GetComponent<MovableBlock>().CanBeMoved(direction);
    }

    public void Grab() {
        isGrabbing = true;
        readyToDrag = false;
        GetComponent<Player>().ToggleGrabbingState(true);
        movingBlockObj.GetComponent<MovableBlock>().ToggleGrabbingState(true);
        movingBlockObj.GetComponent<MovableBlock>().ToggleDragColours(2);
    }

    public void Release() {
        isGrabbing = false;
        GetComponent<Player>().ToggleGrabbingState(false);
        movingBlockObj.GetComponent<MovableBlock>().ToggleGrabbingState(false);
        movingBlockObj.GetComponent<MovableBlock>().ToggleDragColours(1);
    }
}
