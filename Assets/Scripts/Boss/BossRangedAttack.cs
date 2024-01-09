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

    [Tooltip("������ ���� ����(ex. 0.1�� ��� ��� �ִ� ��� ��� 0.1�ʸ��� ������)")]
    [Range(0.1f, 10f)]
    [SerializeField] private float _damageInterval;

    [Tooltip("ȸ������ ���� ���� ��ġ ���� ����(���ϰ�� ����, ������ ��� ȸ���� ����")]
    [SerializeField] private bool _isAbsoluteSpawnPos;

    private Projectile _currentProjectile;


    public override void StartAttack()
    {
        Vector3 spawnPos = _isAbsoluteSpawnPos ? GetAbsoluteSpawnPos() : GetSpawnPos();

        _currentProjectile = Instantiate(_projectilePrefab, spawnPos, Quaternion.identity);
        _currentProjectile.Init("boss", _damage, _damageInterval);

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

    //ȸ������ ����Ͽ� ��ǥ ã��
    private Vector3 GetAbsoluteSpawnPos()
    {       
        Vector3 spawnPos = transform.position + transform.rotation * _spawnPos;
        return spawnPos;
    }

}
