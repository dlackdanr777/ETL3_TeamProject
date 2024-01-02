using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class BossAttackStateBehaviour : StateMachineBehaviour
{

    [SerializeField] private BossAttackState _state;
    public BossAttackState AttackState => _state;

    protected BossController _boss;

    protected BossAttackData _data;

    protected AnimationClip _clip;

    private bool _isStart;

    private bool _isEnd;

    public void Init(BossController boss, BossAttackData data)
    {
        _boss = boss;
        _data = data;
    }


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _isStart = false;
        _isEnd = false;
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        float currentTime = _clip.length * stateInfo.normalizedTime;
        int _currentFrame = Mathf.RoundToInt(_clip.frameRate * currentTime);

        if (_data.StartFrame <= _currentFrame && !_isStart)
        {
            _data.Start();
            _isStart = true;
        }

        else if (_data.EndFrame <= _currentFrame && !_isEnd)
        {
            _data.End();
            _isEnd = true;
        }
    }

}
