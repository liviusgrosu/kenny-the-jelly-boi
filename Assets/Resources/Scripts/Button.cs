using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool onState;
    public GameObject colourChangingObj;
    Renderer rend;
    Animator anim;

    public Material onMat, offMat;
    //1 - perma trigger
    //2 - stay trigger
    //3 - toggle trigger
    public int triggerType;

    public string[] validTriggerEntities;

    public GameObject triggerObj;

    MasterAudioSystem audioSource;

    void Start() {
        rend = colourChangingObj.GetComponent<Renderer>();
        anim = GetComponent<Animator>();

        if (onState) rend.material = onMat;
        else rend.material = offMat;
    }

    private void Update() {
        if (audioSource == null && GameObject.Find("Player Sounds") != null)
            audioSource = GameObject.Find("Player Sounds").GetComponent<MasterAudioSystem>();
    }

    void OnTriggerEnter(Collider colliderObj) {
        foreach (string validEntity in validTriggerEntities)
		{
			if(validEntity == colliderObj.gameObject.tag)
			{
                CalculateState(0);
			}
		}
    }

    void OnTriggerExit(Collider colliderObj) {
        foreach (string validEntity in validTriggerEntities) 
        {
            if (validEntity == colliderObj.gameObject.tag) 
            {
                CalculateState(1);
            }
        }
    }
	
    //enterExitState
    //  1 - enter
    //  2 - exit
	void CalculateState(int enterExitState)
	{
        if(enterExitState == 0) {
            if (triggerType == 3) ToggleState();
            else if (!onState) ToggleState();
        }
        else if(enterExitState == 1) {
            if (triggerType == 2) ToggleState();
            if (triggerType == 3) ToggleAnim(false);
        }
	}

    void ToggleState() {
        onState = !onState;

        triggerObj.GetComponent<ITrigger>().TriggerObj(onState);

        ToggleAnim(onState);

        audioSource.PlayerMovableBlockTriggerSounds("Button");
    }

    void ToggleAnim(bool state) {

        if (state) {
            rend.material = onMat;
            anim.SetTrigger("Activate");
        }
        else {
            rend.material = offMat;
            anim.SetTrigger("Deactivate");
        }
    }
}
