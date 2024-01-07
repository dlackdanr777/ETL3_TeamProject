using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangedAttack : BossAttack
{
    [Tooltip("��ȯ�� ���Ÿ� ���ݿ� ����ü")]
    [SerializeField] private Projectile _projectilePrefab;

    [Tooltip("���� ��ġ(Boss ��ġ ����)")]
    [SerializeField] private Vector3 _spawnPos;

    [Tooltip("ȸ������ ���� ���� ��ġ ���� ����(���ϰ�� ����, ������ ��� ȸ���� ����")]
    [SerializeField] private bool _isAbsoluteSpawnPos;

    [Tooltip("����ü ��ȯ�� ��Ȱ��ȭ�Ǳ� ������ �ð�")]
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
