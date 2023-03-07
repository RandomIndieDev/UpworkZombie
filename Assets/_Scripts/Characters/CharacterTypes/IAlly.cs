using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlly
{
    void SubscribeToGroup(GroupManager manager);
    bool UpdateClosestTarget();

    void HealFlatAmount(float healAmt);
    void ChangeState(CharacterStates state);
}
