using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangedAttack : BossAttack
{
    [Tooltip("��ȯ�� ���Ÿ� ���ݿ� ����ü")]
    [SerializeField] private Projectile _projectilePrefab;

    [Tooltip("���� ��ġ(Boss ��ġ ����)")]
    [SerializeField] private Vector3 _spawnPos;

    [Tooltip("����ü ��ȯ�� ��Ȱ��ȭ�Ǳ� ������ �ð�")]
    [SerializeField] private float _disabledTime;

    [Tooltip("ȸ������ ���� ���� ��ġ ���� ����(���ϰ�� ����, ������ ��� ȸ���� ����")]
    [SerializeField] private bool _isAbsoluteSpawnPos;

    private Projectile _currentProjectile;


    public override void StartAttack()
    {
        _currentProjectile = _isAbsoluteSpawnPos ?
            Instantiate(_projectilePrefab, GetSpawnPos(), Quaternion.identity) :
            Instantiate(_projectilePrefab, GetSpawnPos(), Quaternion.identity, transform);

        _currentProjectile.Init("boss", _damage);
        _currentProjectile.transform.parent = null;

        Destroy(_currentProjectile.gameObject, _disabledTime);
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
