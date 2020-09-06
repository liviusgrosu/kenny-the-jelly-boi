using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBlock : MonoBehaviour, IValidStandingSpace
{
    public bool isValidToStand;
    public bool isDeadlyToStand;

    public bool IsValidToStandOn() {
        return isValidToStand;
    }

    public bool IsDeadlyToStandOn() {
        return isDeadlyToStand;
    }
}
