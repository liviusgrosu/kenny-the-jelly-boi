using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ITrigger
{
    Animator doorAnim;
    Collider col;

    MasterAudioSystem audioSource;

    // Start is called before the first frame update
    void Start()
    {
        doorAnim = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource == null && GameObject.Find("Player Sounds") != null)
            audioSource = GameObject.Find("Player Sounds").GetComponent<MasterAudioSystem>();

        if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName("Door Open")) col.enabled = false;
        else col.enabled = true;
    }

    public void TriggerObj(bool state) {
        if(state) doorAnim.SetTrigger("OpenDoor");
        else doorAnim.SetTrigger("CloseDoor");

        if(audioSource != null) audioSource.PlayOtherSounds("Door");
    }
}
