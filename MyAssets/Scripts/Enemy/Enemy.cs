using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region HP, Movement, Attack and Defense variables
    protected float _maxHP = 100f;
    protected float _currentHP = 100f;
    protected float _movementSpeed = 0.86f;
    protected int _attackDamage = 3;
    protected float _attackRange = 2f;
    protected float _defense = 0f;
    protected float _attackSpeed = 1.4f;
    protected bool _healingInCooldown = false;
    #endregion

    protected Transform _player;
    protected AudioSource _attackSound;
    protected SpawnController _spawnController;
    protected Transform _weapon;

    protected bool _currentlyAttacking = false;

    [SerializeField]
    private GameObject _projectilePrefab;

    private void Awake()
    {
       _player = GameObject.Find("Player").transform;
       _attackSound = this.gameObject.GetComponent<AudioSource>();
       _spawnController = GameObject.Find("Spawn").GetComponent<SpawnController>();
       _weapon = GameObject.Find("Weapon").transform;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (_attackSound == null)
        {
            _attackSound = this.gameObject.GetComponent<AudioSource>();
        }
    }
    protected virtual void Update()
    {
        if (!IsCloseEnoughToAttack())
        {
            Move();
        }
        else
        {
            LaunchAttack();
            Quirk();
        }

        VerifyWorkingCorrectly();
    }

    protected virtual bool IsCloseEnoughToAttack()
    {
        return Vector3.Distance(_player.position, transform.position) <= _attackRange;
    }

    public float SetHP(float hp)
    {
        _currentHP = hp;
        return hp;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Enemy collisioned on something");
    }

    protected virtual void ResetAttackCooldown()
    {
        _currentlyAttacking = false;
    }

    protected virtual void Move()
    {
        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_player.position), 500 * Time.deltaTime);
        transform.LookAt(2 * transform.position - _player.position);

        transform.position = Vector3.MoveTowards(transform.position, _player.position, _movementSpeed * Time.deltaTime);
    }

    protected virtual void VerifyWorkingCorrectly()
    {
        // Make sure distance between player and enemy isn't extremely long, this would mean the enemy went stray at some point.

        if(_attackSound == null)
        {
            _attackSound = this.gameObject.GetComponent<AudioSource>();
        }

        if(Vector3.Distance(transform.position, _player.position) > 100)
        {
            _spawnController.DespawnEnemy(this.gameObject);
        }
    }

    public virtual float GetAttackDamage()
    {
        return this._attackDamage;
    }

    protected virtual void LaunchAttack()
    {
        transform.LookAt(2 * transform.position - _player.position);
        if (!_currentlyAttacking)
        {
            _currentlyAttacking = true;
            if (_attackSound != null)
            {
                _attackSound.Play();
            }

            _player.GetComponent<PlayerController>().ReceiveDamage(GetAttackDamage(), this.transform.gameObject.name);

            // RAYCAST
            /* bool rcHit = Physics.Raycast(this.transform.position, _player.position, 1 << 4);
            Debug.DrawRay(this.transform.position, _player.position);
            if (rcHit)
            {
                
                _player.GetComponent<PlayerController>().ReceiveDamage(GetAttackDamage());
            } */


            // PHYSICAL PREFAB
            /* GameObject _projectile = Instantiate(_projectilePrefab, this.transform.position, this.transform.rotation, this.transform);
            FireIceProjectileController _projectileController = _projectile.GetComponent<FireIceProjectileController>();
            _projectileController.Initalize(this.gameObject); */

            Invoke("ResetAttackCooldown", _attackSpeed);
        }
    }

    public virtual bool Heal(float amount, float cooldown, bool canShield)
    {
        if (!this._healingInCooldown && (_currentHP != _maxHP || canShield)) {
            SetHP(canShield ? (_currentHP + amount) : Mathf.Min(_currentHP + amount, _maxHP));
            this.SetHealingCooldown(cooldown);
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void SetHealingCooldown(float cooldown)
    {
        this._healingInCooldown = true;
        Invoke("ResetHealingCooldown", cooldown);
    }

    protected virtual void ResetHealingCooldown()
    {
        this._healingInCooldown = false;
    }

    public virtual float GetAttackSpeed()
    {
        return _attackSpeed;
    }

    public virtual float ReceiveDamage(float dmg)
    {
        _defense = 0.75f;
        return SetHP(_currentHP - (dmg * _defense));
    }

    protected virtual void Quirk() // Effects unique to this kind of Enemy
    {
    }
}
