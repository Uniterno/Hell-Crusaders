using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Camera variables
    [SerializeField]
    private Transform _player, _playerCamera, _focusPoint;
    #endregion
    #region Zoom
    [SerializeField]
    private float _zoom = -7;
    private float _zoomSpeed = 5;
    [SerializeField]
    private float _zoomMin = -10, _zoomMax = 0;
    private float _autoZoom = -7;
    #endregion
    #region Rotate camera
    [SerializeField]
    private float _camRotX, _camRotY;
    [SerializeField]
    private float _cameraSensitivity = 8;
    #endregion

    HUDController HUD;
    private TutorialManager TutorialManager;
    // Start is called before the first frame update
    void Start()
    {
        #region Assignment check
        if(_player == null)
        {
            Debug.LogWarning("Player wasn't assigned on CameraController script");
        }
        if (_playerCamera == null)
        {
            Debug.LogWarning("Player camera wasn't assigned on CameraController script");
        }
        if (_focusPoint == null)
        {
            Debug.LogWarning("Focus point wasn't assigned on CameraController script");
        }

        #endregion

        #region Assign Relationship
        _playerCamera.transform.SetParent(_focusPoint);
        _focusPoint.transform.SetParent(_player);
        #endregion

        #region Initialize camera and focus
        _playerCamera.transform.localPosition = new Vector3(0, 0, _zoom);
        _focusPoint.transform.localPosition = new Vector3(0, 1, 0);
        _playerCamera.LookAt(_focusPoint);
        #endregion

        HUD = GameObject.FindObjectOfType<HUDController>();
        TutorialManager = GameObject.FindObjectOfType<TutorialManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialManager.GetFinishedTutorial())
        {
            #region Zoom
            _zoom += Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
            _zoom = Mathf.Clamp(_zoom, _zoomMin, _zoomMax);
            _playerCamera.transform.localPosition = new Vector3(0, 0, _zoom);
            #endregion

            #region Rotate camera
            // If right click
            if (Input.GetMouseButton(1))
            {  // 0 Left, 1 Right, 2 Wheel
                _camRotX += Input.GetAxis("Mouse X") * _cameraSensitivity; // Rotate Player's Y
                _camRotY += Input.GetAxis("Mouse Y") * _cameraSensitivity; // Rotate FocusPoint's X
                _player.transform.localRotation = Quaternion.Euler(0, _camRotX, 0);
                _focusPoint.transform.localRotation = Quaternion.Euler(_camRotY, 0, 0);

            }
            #endregion

            #region Auto-adjust
            RaycastHit hit; // Check if there's an object in between camera and player
            if (Physics.Linecast(_playerCamera.transform.position, _player.position, out hit))
            {
                // If there is, and that item is not the player itself, another camera component, a bullet or an invisible object
                // then zoom in to counter the object and be able to see even with it in the way
                // Uses AutoZoom as another instance of Zoom to keep the player's selected zoom when the view is cleared
                if (hit.transform.tag != "Player" && hit.transform.tag != "Camera Controller" && hit.transform.tag != "Main Camera" && hit.transform.tag != "Bullet" && hit.transform.tag != "Invisible")
                {
                    _autoZoom = _zoomMax - 1.67f;
                    _autoZoom = Mathf.Clamp(_autoZoom, _zoomMin, _zoomMax);
                    _playerCamera.transform.localPosition = new Vector3(0, 0, _autoZoom);
                }

            }
            #endregion

            #region Aim (disabled)
            /* if (Input.GetKey(KeyCode.Mouse1)) // Shoot on mouse click
            {
                _playerCamera.transform.localPosition = new Vector3(0, 0, _zoomMax + 0.33f);
                HUD.ShowAim();
            }
            else
            {
                HUD.ShowAim(false);
            } */
            #endregion
        }
    }
}
