using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalKit : Interactable
{
    PlayerController _player;
    AudioSource _sfx;
    Renderer _renderer;
    Collider _trigger;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        _sfx = this.GetComponent<AudioSource>();
        _renderer = this.GetComponent<Renderer>();
        _trigger = this.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up, 45 * Time.deltaTime);
    }
    public override void Interact()
    {
        _player.Heal(50); // Heal 50 HP upon pick-up
        _sfx.Play(); // Play pick-up SFX
        _renderer.enabled = false; // Hide model
        _trigger.enabled = false; // Once model is hidden, don't allow pick-up to avoid multiple instance of healing
        Invoke("Despawn", _sfx.clip.length); // Call object destroy after SFX has finished playing
    }

    private void Despawn()
    {
        Destroy(this);
    }
}
