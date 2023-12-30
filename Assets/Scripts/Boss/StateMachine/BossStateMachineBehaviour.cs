using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachineBehaviour : StateMachineBehaviour
{
    protected BossController _boss;

    public void Init(BossController boss)
    {
        _boss = boss;
    }
}
