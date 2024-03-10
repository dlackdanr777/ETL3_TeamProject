using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonHandler<GameManager>
{
    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }
}
