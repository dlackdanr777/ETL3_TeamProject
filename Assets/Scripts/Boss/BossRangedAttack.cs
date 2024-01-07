using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangedAttack : BossAttack
{
    [Tooltip("소환할 원거리 공격용 투사체")]
    [SerializeField] private Projectile _projectilePrefab;

    [Tooltip("생성 위치(Boss 위치 기준)")]
    [SerializeField] private Vector3 _spawnPos;

    [Tooltip("회전값에 따른 스폰 위치 변경 여부(참일경우 변경, 거짓일 경우 회전값 무시")]
    [SerializeField] private bool _isAbsoluteSpawnPos;

    [Tooltip("투사체 소환후 비활성화되기 까지의 시간")]
    [SerializeField] private float _disabledTime;

    private Projectile _currentProjectile;

    public override void StartAttack()
    {
        if (!_isAbsoluteSpawnPos)
            _currentProjectile = Instantiate(_projectilePrefab, GetSpawnPos(), Quaternion.identity, transform);

        else
            _currentProjectile = Instantiate(_projectilePrefab, GetSpawnPos(), Quaternion.identity);

        _currentProjectile.Init("boss", _damage);
        _currentProjectile.transform.parent = null;

        Destroy(_currentProjectile, _disabledTime);
    }


    public override void EndAttack()
    {
    }

    private Vector3 GetSpawnPos()
    {
        Vector3 spawnPos = transform.position + _spawnPos;
        return spawnPos;
    }

}
