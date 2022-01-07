using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float _despawnTimeout = 10f;
    private ScoreController _scoreController;
    private GameObject _player;
    private SpawnController _spawnController;
    private GameObject _m4;

    private void Awake()
    {
       this._scoreController = GameObject.Find("ScoreIndicator").GetComponent<ScoreController>();
       this._player = GameObject.Find("Player");
       this._spawnController = GameObject.Find("Spawn").GetComponent<SpawnController>();
       this._m4 = GameObject.Find("M4");
    
    }
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(this.GetComponent<Collider>(), _player.GetComponent<Collider>());
        this.transform.rotation = _player.transform.GetChild(0).transform.rotation;
        // this.transform.position = _player.transform.position;
        this.transform.position = _m4.transform.position; // Adjust to weapon, not character
        

        Invoke("Timeout", _despawnTimeout);

    }

    // Update is called once per frame
    void Update()
    {
        /* Vector3 movement = new Vector3(1, 0, 1);
        transform.Translate(movement * 1 * Time.deltaTime); */
        
        
        

        this.transform.position += this.transform.forward * 50f * Time.deltaTime;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            float remainingHP = 999;
            // Decrease enemy HP
            if (collision.gameObject.name.StartsWith("FireWisp"))
            {
                FireWisp shotEnemy = collision.gameObject.GetComponent<FireWisp>();
                remainingHP = shotEnemy.ReceiveDamage(100);
                
            } else if (collision.gameObject.name.StartsWith("IceWisp"))
            {
                IceWisp shotEnemy = collision.gameObject.GetComponent<IceWisp>();
                remainingHP = shotEnemy.ReceiveDamage(100);
            } else if (collision.gameObject.name.StartsWith("ToxicWisp"))
            {
                ToxicWisp shotEnemy = collision.gameObject.GetComponent<ToxicWisp>();
                remainingHP = shotEnemy.ReceiveDamage(100);
            }
            else if (collision.gameObject.name.StartsWith("ElectricWisp"))
            {
                ElectricWisp shotEnemy = collision.gameObject.GetComponent<ElectricWisp>();
                remainingHP = shotEnemy.ReceiveDamage(100);
            }
            else if (collision.gameObject.name.StartsWith("Capuchas"))
            {
                Capuchas shotEnemy = collision.gameObject.GetComponent<Capuchas>();
                remainingHP = shotEnemy.ReceiveDamage(100);
            }

            if (remainingHP <= 0)
            {
                _spawnController.DespawnEnemy(collision.gameObject);
                this._scoreController.AddScore(Mathf.RoundToInt(2 * Mathf.Log(Mathf.Pow(_spawnController.GetCurrentRound() + 1, 3f))));
            }
            
        }
    }

    private void Timeout()
    {
        Destroy(this.gameObject);
    }

}
