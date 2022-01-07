using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicField : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Despawn", 1.4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!other.gameObject.name.StartsWith("Capuchas"))
            {
                Enemy enemyScript = other.GetComponent<Enemy>();
                enemyScript.Heal(50, enemyScript.GetAttackSpeed() - 0.1f, true);
            }
        }
    }

    private void Despawn()
    {
        Destroy(this.gameObject);
    }
}
