using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    #region Components panel
    [Header("Dialogue panel")]
    [SerializeField]
    #pragma warning disable 0649
    private GameObject _dialoguePnl;
    #pragma warning restore 0649

    private TextMeshProUGUI _dialogueText, _nameText, _continueText;
    private Button _continueButton, _skipButton;
    #endregion

    private string _name;
    private List<string> _dialogueList;
    private int _dialogueIdx;

    private SpawnController _spawn;
    private GameObject _spawnedTutorialObject;
    private TutorialManager TutorialManager;

    private static DialogueManager _instance { get; set; } // Singleton
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        #region Check and initialize components
        if (_dialoguePnl == null)
        {
            Debug.Log("Dialogue Manager has no Dialogue Panel");
        }

        _dialogueText = _dialoguePnl.GetComponentInChildren<TextMeshProUGUI>();
        if(_dialogueText == null)
        {
            Debug.LogWarning("Panel's first children has no TMP");
        }
        else
        {
            _dialogueText.text = "Dialogue txt component was found succesfully";
        }

        _nameText = _dialoguePnl.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        if (_nameText == null)
        {
            Debug.LogWarning("Panel's second children has no child with TMP");
        }
        else
        {
            _nameText.text = "???";
        }

        _continueButton = _dialoguePnl.transform.GetChild(2).GetComponent<Button>();
        if (_continueButton == null)
        {
            Debug.LogWarning("Panel's third children has no Button component");
        }
        else
        {
            _continueText = _continueButton.GetComponentInChildren<TextMeshProUGUI>();
            if (_continueText == null)
            {
                Debug.LogWarning("Continue button has no TMP children");
            }
            else
            {
                _continueText.text = "Continue";
            }
        }

        #endregion
        _continueButton.onClick.AddListener(delegate { ContinueDialogue(); });
        _dialoguePnl.SetActive(false);


        _spawn = GameObject.FindObjectOfType<SpawnController>();
        TutorialManager = GameObject.FindObjectOfType<TutorialManager>();
        _skipButton = _dialoguePnl.transform.GetChild(3).GetComponent<Button>();
        _skipButton.onClick.AddListener(delegate { SkipDialogue(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDialogue(string name, string[] dialogue)
    {
        _name = name;
        _nameText.text = name;
        _dialogueList = new List<string>(dialogue.Length); // Empty list of size #
        _dialogueList.AddRange(dialogue);
        _dialogueIdx = 0;
        _continueText.text = "Continue";
        _dialoguePnl.SetActive(true);
        ShowDialogue();
    }

    private void ShowDialogue()
    {
        _dialogueText.text = _dialogueList[_dialogueIdx];
    }

    private void ContinueDialogue()
    {
        if (_dialogueIdx == _dialogueList.Count - 1)
        {
            _dialoguePnl.SetActive(false);
            TutorialManager.SetFinishedTutorial(true);
            if (_spawnedTutorialObject != null)
            {
                Destroy(_spawnedTutorialObject); // Make sure any objects spawned by the tutorial are always properly destroyed and deallocated
            }
        } else if (_dialogueIdx == _dialogueList.Count - 2)
        {
            _dialogueIdx++;
            ShowDialogue();
            _continueText.text = "Leave";
        }
        else
        {
            _dialogueIdx++;
            ShowDialogue();
            Debug.Log("List is on: " + _dialogueIdx + " of " + _dialogueList.Count);
        }

        SpecialEvent(_dialogueIdx);
    }

    private void SkipDialogue()
    {
        _dialogueIdx = _dialogueList.Count - 1;
        ContinueDialogue();
    }

    private void SpecialEvent(int index)
    {
        if(index == 15)
        {
            // Spawn Ice Wisp
            _spawnedTutorialObject = _spawn.SpawnTutorialObject(1);
        } else if(index == 16)
        {
            // Despawn Ice Wisp
            _spawn.DespawnTutorialObject(_spawnedTutorialObject);
            // Spawn Fire Wisp
            _spawnedTutorialObject = _spawn.SpawnTutorialObject(0);
        } else if(index == 17)
        {
            // Despawn Fire Wisp
            _spawn.DespawnTutorialObject(_spawnedTutorialObject);
            // Spawn Toxic Wisp
            _spawnedTutorialObject = _spawn.SpawnTutorialObject(2);
        } else if(index == 18)
        {
            // Despawn Toxic Wisp
            _spawn.DespawnTutorialObject(_spawnedTutorialObject);
            // Spawn Electric Wisp
            _spawnedTutorialObject = _spawn.SpawnTutorialObject(3);
        } else if(index == 20)
        {
            // Despawn Electric Wisp
            _spawn.DespawnTutorialObject(_spawnedTutorialObject);
            // Spawn Capuchas
            _spawnedTutorialObject = _spawn.SpawnTutorialObject(4);
        } else if(index == 25)
        {
            // Despawn Capuchas
            _spawn.DespawnTutorialObject(_spawnedTutorialObject);
        } else if(index == 26)
        {
            // Spawn Medical Kit
            _spawnedTutorialObject = _spawn.SpawnTutorialObject(5);
            
        } else if(index == 28)
        {
            // Despawn Medical Kit
            _spawn.DespawnTutorialObject(_spawnedTutorialObject);
        }
    }
}
