using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capuchas : Enemy
{
    GameObject _enemiesList;

    [SerializeField]
    private GameObject _magicFieldPrefab; // Prefab for the magical field GameObject to summon

    protected override void Start()
    {
        if (_attackSound == null)
        {
            _attackSound = this.gameObject.GetComponent<AudioSource>();
        }
        if(_enemiesList == null)
        {
            _enemiesList = GameObject.Find("Spawn");
        }
        _attackRange = 0.6f; // Capuchas have less attack range by default
        _attackDamage = 0;
    }
    protected override void Move()
    {
        if(_enemiesList.transform.childCount > 1)
        {
            Transform FollowingEnemy = GetFollowingEnemy();
            transform.LookAt(2 * transform.position - FollowingEnemy.position);
            transform.position = Vector3.MoveTowards(transform.position, FollowingEnemy.position, _movementSpeed * Time.deltaTime);
        }
        else
        {
            transform.LookAt(2 * transform.position - _player.position);
            transform.position = Vector3.MoveTowards(transform.position, _player.position, _movementSpeed * Time.deltaTime);
        }
        
    }
    protected override void Quirk() // Effects unique to this kind of Enemy
    {
        HealAllies();
    }

    void HealAllies()
    {
        SpawnHealingCircle();
    }

    void SpawnHealingCircle()
    {
        if(this.transform.childCount == 0)
        {
            Instantiate(_magicFieldPrefab, this.transform);
        }
    }

    private Transform GetFollowingEnemy()
    {
        Transform FollowingEnemy = _enemiesList.transform.GetChild(0).GetChild(0);
        if (FollowingEnemy == this.transform)
        {
            if (_enemiesList.transform.childCount > 2)
            {
                FollowingEnemy = _enemiesList.transform.GetChild(0).GetChild(1);
            }
            else
            {
                FollowingEnemy = _player;
            }
        }
        return FollowingEnemy;
    }

    protected override bool IsCloseEnoughToAttack()
    {
        return Vector3.Distance(GetFollowingEnemy().position, transform.position) <= _attackRange;
    }
}
