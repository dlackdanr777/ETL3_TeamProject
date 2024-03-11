using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossAiStateChange : BossStateMachineBehaviour
{
    [SerializeField] private BossAIState _changeState;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _boss.ChangeAiState(_changeState);
    }

}
