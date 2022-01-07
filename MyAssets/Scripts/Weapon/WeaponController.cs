using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    #region Ammo
    [SerializeField]
    private int _availableAmmoSlot = 30;
    [SerializeField]
    private int _maximumAmmoPerSlot = 30;
    #endregion

    #region Shooting
    [SerializeField]
    private float _shootingSpeed = 5; // Bullets per second
    private float _originalShootingSpeed = 5; // Bullets per second
    #endregion

    #region Reloading
    private float _reloadingSpeed = 1; // Seconds
    private bool _shootingInCooldown = false; // Whether shooting is in cooldown
    private bool _currentlyReloading = false; // Whether player is currently reloading
    #endregion

    #region SFX
    private AudioSource _weaponShootSFX;
    private AudioSource _noAmmoSFX;
    private AudioSource _reloadSFX;
    private float _sfxNoAmmoCooldownTimer = 0.2f;
    private bool _sfxNoAmmoInCooldown = false;
    #endregion

    // float _aimAnimCooldown = 0.5f;

    [SerializeField]
    private GameObject _bulletPrefab; // Prefab for the bullet GameObject to shoot
    private PlayerAnimation _playerAnimator;
    private HUDController HUD; // HUD reference to update Ammo indicator
    private TutorialManager TutorialManager;

    private void Awake()
    {
        // Initialize SFX
        _weaponShootSFX = gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        _noAmmoSFX = gameObject.transform.GetChild(1).GetComponent<AudioSource>();
        _reloadSFX = gameObject.transform.GetChild(2).GetComponent<AudioSource>();
        _playerAnimator = GameObject.Find("Player").GetComponent<PlayerAnimation>();
        TutorialManager = GameObject.FindObjectOfType<TutorialManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameObject.Find("Player").transform.position; // Set Weapon initial position to player's
        HUD = GameObject.FindObjectOfType<HUDController>(); // Get HUD Controller script
    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialManager.GetFinishedTutorial())
        {
            #region Shoot
            if (Input.GetKey(KeyCode.Mouse0)) // Shoot on mouse click
            {
                if (_availableAmmoSlot > 0)
                {
                    if (!_shootingInCooldown && !_currentlyReloading)
                    {
                        _availableAmmoSlot--;
                        HUD.UpdateAmmo(_availableAmmoSlot);
                        Shoot();
                        _shootingInCooldown = true;
                        Invoke("ResetCooldown", 1 / _shootingSpeed);
                    }
                }
                else
                {
                    if (!_sfxNoAmmoInCooldown)
                    {
                        _sfxNoAmmoInCooldown = true;
                        PlayNoAmmoSFX();
                        Invoke("ResetNoAmmoCooldown", _sfxNoAmmoCooldownTimer);
                    }

                }
            }
            #endregion
            #region Reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!_shootingInCooldown)
                {
                    if (_availableAmmoSlot != _maximumAmmoPerSlot && !_currentlyReloading)
                    {
                        _currentlyReloading = true;
                        PlayReloadSFX();
                        Invoke("Reload", _reloadingSpeed);
                    }
                }
            }
            #endregion

            /* _aimAnimCooldown -= 1 * Time.deltaTime;
            if (_aimAnimCooldown <= 0)
            {
                _playerAnimator.SetAiming(false);
            } */
        }
    }

    void Shoot()
    {
        // _playerAnimator.SetAiming(true);
        // _aimAnimCooldown = 0.5f;
        PlayShootSFX();
        NewBullet();
    }

    void Reload()
    {
        _availableAmmoSlot = _maximumAmmoPerSlot;
        HUD.UpdateAmmo(_availableAmmoSlot);
        _currentlyReloading = false;
    }

    void ResetCooldown()
    {
        _shootingInCooldown = false;
    }


    #region Play SFX
    public void PlayShootSFX()
    {
        if(_weaponShootSFX == null)
        {
            Debug.LogWarning("Shoot Audio SFX can't be found");
        } else
        {
            _weaponShootSFX.Play();
        }
    }

    public void PlayNoAmmoSFX()
    {
        if (_noAmmoSFX == null)
        {
            Debug.LogWarning("No Ammo Audio SFX can't be found");
        }
        else
        {
            _noAmmoSFX.Play();
        }
    }

    public void PlayReloadSFX()
    {
        if (_reloadSFX == null)
        {
            Debug.LogWarning("Reload SFX can't be found");
        }
        else
        {
            _reloadSFX.Play();
        }
    }
    #endregion

    public void ResetNoAmmoCooldown()
    {
        _sfxNoAmmoInCooldown = false;
    }

    void NewBullet()
    {
        Instantiate(_bulletPrefab, this.transform.GetChild(3));
        // GameObject bullet = Instantiate(gameObject.transform.GetChild(3).GetChild(0).gameObject, this.transform.GetChild(3));

    }

    public void AdjustShootingSpeed(float adjustRate, int time)
    {
        if (_shootingSpeed == _originalShootingSpeed)
        {
            _shootingSpeed *= adjustRate;

            HUD.UpdateEffects("Zapped", time);
            StartCoroutine(ZappedCountdown(time));
           
            Invoke("ResetShootingSpeed", time);
        }
    }

    IEnumerator ZappedCountdown(int remainingSeconds)
    {
        while (remainingSeconds >= 0)
        {
            yield return new WaitForSeconds(1);
            remainingSeconds--;
            HUD.UpdateEffects("Zapped", remainingSeconds);
        }
    }

    private void ResetShootingSpeed()
    {
        _shootingSpeed = _originalShootingSpeed;
    }


}
