using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrigger {
    void TriggerObj(bool state);
}

public interface IValidStandingSpace {
    bool IsValidToStandOn();
    bool IsDeadlyToStandOn();
}
