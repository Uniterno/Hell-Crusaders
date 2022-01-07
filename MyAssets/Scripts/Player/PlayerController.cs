using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRB;

    #region PlayerMovement
    [SerializeField]
    private float _speed, _maxSpeed = 4.9f, _maxSpeedOg;
    private bool _isRunning = false;
    private float _horizontalInput, _forwardInput;
    #endregion

    private float _maxHP = 100f;
    private float _currentHP = 100f;
    private float _poisonedTime = 0f;

    /* #region Player Jump
    [SerializeField]
    private float _jumpForce = 5;
    [SerializeField]
    private int _availableJumps = 0, _maxJumps = 2;

    private bool _jumpRequest = false;
    #endregion */ // Jump is disabled for this gamemode

    private PlayerAnimation _playerAnimation;
    private HUDController HUD;
    private DeathManager _deathManager;
    private TutorialManager TutorialManager;


    // Start is called before the first frame update
    void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
        if (_playerRB == null)
        {
            Debug.LogWarning("Player has no Rigidbody");
        }

        _playerAnimation = GetComponent<PlayerAnimation>();
        if (_playerAnimation == null)
        {
            Debug.LogWarning("Player has no PlayerAnimation");
        }
        _speed = _maxSpeed  / 2;
        _maxSpeedOg = _maxSpeed / 2;
        _isRunning = false;

        HUD = GameObject.FindObjectOfType<HUDController>();
        _deathManager = GameObject.Find("Death Manager").GetComponent<DeathManager>();
        TutorialManager = GameObject.FindObjectOfType<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialManager.GetFinishedTutorial())
        {
            _horizontalInput = Input.GetAxis("Horizontal"); // AD
            _forwardInput = Input.GetAxis("Vertical"); // WS

            if (_isRunning)
            {
                _horizontalInput = 0; // Cannot move right/left while running
                _forwardInput = Mathf.Max(0, _forwardInput);
            }

            Vector3 movement = new Vector3(_horizontalInput, 0, _forwardInput);
            _playerAnimation.SetDirection(_horizontalInput, _forwardInput);
            transform.Translate(movement * _speed * Time.deltaTime);

            float velocity = Mathf.Max(Mathf.Abs(_horizontalInput), Mathf.Abs(_forwardInput));
            velocity = velocity * _speed / _maxSpeed;
            _playerAnimation.SetSpeed(velocity);


            /* if (Input.GetKeyDown(KeyCode.Space) && _availableJumps > 0)
            {
                _jumpRequest = true;
            } */ // Jump is disabled for this gamemode

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _isRunning = !_isRunning;
                if (_isRunning)
                {
                    _speed = _maxSpeed; // 4.9 / 2
                }
                else
                {
                    _speed = _maxSpeed / 2; // 4.9
                }
            }
        }  
    }
    private void FixedUpdate()
    {
        /* if (_jumpRequest)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _availableJumps > 0)
            {
                _playerRB.velocity = Vector3.up * _jumpForce;
                _availableJumps--;
                _jumpRequest = false;
            }
        } */ // Jump is disabled for this gamemode
    }

    private void OnCollisionEnter(Collision collision)
    {
        /* if (collision.gameObject.CompareTag("Ground"))
        {
            _availableJumps = _maxJumps;
        } */ // Jump is disabled for this gamemode
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            Interactable interactScript = other.GetComponent<Interactable>();
            if(interactScript != null)
            {
                interactScript.Interact();
            }
            else
            {
                Debug.Log("Object has no script to interact");
            }
            Debug.Log("A trigger with Interactable tag was found");
        }
    }

    public void ReceiveDamage(float Damage, string Invoker)
    {
        Debug.Log("Invoked by: " + Invoker);
        this._currentHP -= Damage;
        if(_currentHP <= 0f)
        {
            _currentHP = 0f;
            Die();
        }

        HUD.UpdateHP(Mathf.RoundToInt(_currentHP));
    }

    private void Die()
    {
        
        _deathManager.Die();
    }

    public void AdjustPlayerSpeed(float adjustRate, int time, string origin)
    {
        if(_maxSpeed == _maxSpeedOg)
        {
            _maxSpeed *= adjustRate;
            if (_isRunning)
            {
                _speed = _maxSpeed; // 7
            }
            else
            {
                _speed = _maxSpeed / 2; // 3.5
            }

            if(origin == "Ice")
            {
                HUD.UpdateEffects("Frozen", time);
                StartCoroutine(FrozenCountdown(time));
            }

            if (origin == "Electric")
            {
                HUD.UpdateEffects("Shock", time);
                StartCoroutine(ShockCountdown(time));
            }


            Invoke("ResetPlayerSpeed", time);
        }
    }

    IEnumerator FrozenCountdown(int remainingSeconds)
    {
        while(remainingSeconds >= 0)
        {
            yield return new WaitForSeconds(1);
            remainingSeconds--;
            HUD.UpdateEffects("Frozen", remainingSeconds);
        }  
    }

    private void ResetPlayerSpeed()
    {
        _maxSpeed = _maxSpeedOg;
        if (_isRunning)
        {
            _speed = _maxSpeed; // 7
        }
        else
        {
            _speed = _maxSpeed / 2; // 3.5
        }
    }

    public void SetPoisoned(int time, float damage)
    {
        if (_poisonedTime <= 0f)
        {
            _poisonedTime = time;
            HUD.UpdateEffects("Poisoned", time);
            StartCoroutine(PoisonedCountdown(time, damage));
            Invoke("ResetPlayerSpeed", time);
        }
    }

    IEnumerator PoisonedCountdown(int remainingSeconds, float damage)
    {
        while (remainingSeconds > 0)
        {
            Debug.Log("Damaged played with poison by " + damage + "HP (" + _currentHP + ") . Remaining seconds: " + remainingSeconds);
            yield return new WaitForSeconds(1);
            remainingSeconds--;
            _poisonedTime--;
            HUD.UpdateEffects("Poisoned", Mathf.CeilToInt(remainingSeconds));
            PoisonDamage(damage);
        }
    }

    void PoisonDamage(float damage) // This function may look unnecessary but it's here as a placeholder for potential poison damage conditions
    {
        ReceiveDamage(damage, "Poison");
    }

    IEnumerator ShockCountdown(int remainingSeconds)
    {
        while (remainingSeconds >= 0)
        {
            yield return new WaitForSeconds(1);
            remainingSeconds--;
            HUD.UpdateEffects("Shock", remainingSeconds);
        }
    }

    public void Heal(int amount)
    {
        _currentHP += amount;
        _currentHP = Mathf.Min(_currentHP, _maxHP);
        HUD.UpdateHP(Mathf.RoundToInt(_currentHP));
    }
}
