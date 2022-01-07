using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
       
        _playerAnimator = GetComponentInChildren<Animator>();
        if(_playerAnimator == null)
        {
            Debug.LogWarning("Player has no first children with Animator component");
        }
        // SetSpeed(0.8f);

    }

    public void SetSpeed(float speed)
    {
        _playerAnimator.SetFloat("Speed", speed);
    }

    /* public void SetDirection(int direction)
    {
        _playerAnimator.SetInteger("Direction", direction);
    }

    public void SetDirection(string direction)
    {
        int directionInt = 0;
        if(direction == "Forward")
        {
            directionInt = 0;
        } else if(direction == "Backwards")
        {
            directionInt = -1;
        } else if(direction == "Left")
        {
            directionInt = 1;
        } else if(direction == "Right")
        {
            directionInt = 2;
        }
        _playerAnimator.SetInteger("Direction", directionInt);
    } */

    public void SetDirection(float x, float y)
    {
        _playerAnimator.SetFloat("Horizontal", x);
        _playerAnimator.SetFloat("Vertical", y);
    }
}
