using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAudioSystem : MonoBehaviour {
    public AudioSource source;

    public AudioClip playerHop, playerDeath, playerAdvanceLevel, playerStartLevel;
    public AudioClip gemPickUp;
    public AudioClip doorOpening;
    public AudioClip buttonPress, MovableBlockTrigger;
    public AudioClip movableBlockSlide, movableBlockLand;


    public void PlayPlayerSound(string soundName) {
        if (source.isPlaying) source.Stop();

        switch(soundName) {
            case "Hop":
                source.PlayOneShot(playerHop);
                break;
            case "Death":
                source.PlayOneShot(playerDeath);
                break;
            case "Next Level":
                source.PlayOneShot(playerAdvanceLevel);
                break;
            case "Spawn":
                source.PlayOneShot(playerStartLevel);
                break;
        }
    }

    public void PlayOtherSounds (string soundName) {
        if (source.isPlaying) source.Stop();

        switch (soundName) {
            case "Gem Pickup":
                source.PlayOneShot(gemPickUp);
                break;
            case "Door":
                source.PlayOneShot(doorOpening);
                break;
        }
    }

    public void PlayerMovableBlockTriggerSounds(string soundName) {
        if (source.isPlaying) source.Stop();

        switch (soundName) {
            case "Button":
                source.PlayOneShot(buttonPress);
                break;
            case "Moving Block Trigger":
                source.PlayOneShot(MovableBlockTrigger);
                break;
        }
    }

    public void PlayMovableBlockSound(string soundName) {
        if (source.isPlaying) source.Stop();

        switch (soundName) {
            case "Move":
                source.PlayOneShot(movableBlockSlide);
                break;
            case "Land":
                source.PlayOneShot(movableBlockLand);
                break;

        }
    }
}
