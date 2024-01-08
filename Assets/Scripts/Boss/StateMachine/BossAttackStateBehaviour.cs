using UnityEngine;

public class BossAttackStateBehaviour : StateMachineBehaviour
{

    [SerializeField] private BossAttackState _state;
    public BossAttackState AttackState => _state;

    protected BossController _boss;

    protected BossAttackData _data;

    protected AnimationClip _clip;


    public void Init(BossController boss, BossAttackData data)
    {
        _boss = boss;
        _data = data;
    }


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for(int i = 0, count = _data.Frames.Length; i < count; i++)
        {
            _data.Frames[i].SetIsStarted = false;
            _data.Frames[i].SetIsFinished = false;
        }
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        float currentTime = _clip.length * stateInfo.normalizedTime;
        int _currentFrame = Mathf.RoundToInt(_clip.frameRate * currentTime);

        for(int i = 0, count = _data.Frames.Length; i < count; i++)
        {
            if (_data.Frames[i].StartFrame <= _currentFrame && !_data.Frames[i].GetIsStarted)
            {
                _data.Start(i);
                Debug.Log("공격 실행");
                _data.Frames[i].SetIsStarted = true;
            }

            else if (_data.Frames[i].FinishedFrame <= _currentFrame && !_data.Frames[i].GetIsFinished)
            {
                _data.End(i);
                Debug.Log("종료 실행");
                _data.Frames[i].SetIsFinished = true;
            }
        }
       
    }

}
