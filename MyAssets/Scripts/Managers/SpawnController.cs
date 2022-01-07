using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    #region Enemy Prefabs
    [SerializeField]
    private GameObject _fireWispPrefab;

    [SerializeField]
    private GameObject _iceWispPrefab;

    [SerializeField]
    private GameObject _toxicWispPrefab;

    [SerializeField]
    private GameObject _electricWispPrefab;

    [SerializeField]
    private GameObject _capuchasPrefab;
    #endregion

    [SerializeField]
    private GameObject _medicalKit;

    private HUDController HUD;
    private GameObject Spawnpoints;
    private AudioSource VictorySound;
    private TutorialManager TutorialManager;
    private GameObject TutorialObjectPlaceholder;
    private GameObject MainCamera;
    private VictoryManager VictoryManager;


    #region Round and enemies
    [SerializeField]
    private int _round = 0;
    private int _maxEnemies = 0;
    private int _waitingEnemies = 0;
    // private int _remainingEnemies = 0;
    private int _subroundMinEnemies = 0;
    private int _spawnedEnemies = 0;
    private int _spawnedCapuchas = 0;
    private List<string> _spawnableEnemies = new List<string>() { "Fire Wisp", "Ice Wisp", "Toxic Wisp", "Electric Wisp"};
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        HUD = GameObject.FindObjectOfType<HUDController>();
        Spawnpoints = this.transform.GetChild(1).gameObject;
        VictorySound = this.GetComponent<AudioSource>();
        TutorialManager = GameObject.FindObjectOfType<TutorialManager>();
        TutorialObjectPlaceholder = GameObject.Find("TutorialObjectPlaceholder");
        MainCamera = GameObject.Find("Main Camera");
        VictoryManager = GameObject.FindObjectOfType<VictoryManager>();
        NewRound();
    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialManager.GetFinishedTutorial())
        {
            if (_waitingEnemies > 0)
            {
                if (_spawnedEnemies < _subroundMinEnemies)
                {
                    _waitingEnemies--;
                    SpawnNewEnemy(_round * 2);
                    HUD.UpdateRemainingEnemies(_spawnedEnemies + _waitingEnemies);
                }
            }
            else
            {
                if (_spawnedEnemies == 0)
                {
                    NewRound();
                }
            }
        }
    }

    private void NewRound()
    {
        _spawnedCapuchas = 0;
        _round++;
        _maxEnemies = _round * 10;
        if (_round != 1)
        {
            VictorySound.Play();
            SpawnMedicalKit();
        }
        if(_round > 10)
        {
            Won();
        }
        
        _waitingEnemies = _maxEnemies;
        _subroundMinEnemies = Mathf.RoundToInt(_maxEnemies / 3);
        HUD.UpdateRound(_round);
        HUD.UpdateRemainingEnemies(_spawnedEnemies + _waitingEnemies);
    }

    public void DespawnEnemy(GameObject target)
    {
        Destroy(target);
        _spawnedEnemies--;

        HUD.UpdateRemainingEnemies(_spawnedEnemies + _waitingEnemies);
    }

    void SpawnNewEnemy(int CapuchasLimit)
    {
        _spawnedEnemies++;
        List<string> SpawnableEnemies = _spawnableEnemies;

        if (_spawnedCapuchas <= CapuchasLimit) // If the amount of Capuchas spawned at a time doesn't exceed the limit for the round:
        {
            SpawnableEnemies.Add("Capuchas"); // Add Capuchas to spawneable enemies.
        }

        int RandomOption = Random.Range(0, SpawnableEnemies.Count); // Select a random enemy to spawn.
        Debug.Log("_spawnedCapuchas / CapuchasLimit: " + _spawnedCapuchas + "/" + CapuchasLimit);
        Debug.Log("Random Option: " + RandomOption);
        GameObject ToSpawnPrefab = GetEnemyPrefab(RandomOption);
        GameObject EnemyInstance = Instantiate(ToSpawnPrefab, this.transform.GetChild(0));
        RandomizeStartingPosition(EnemyInstance);
        if (SpawnableEnemies.Contains("Capuchas")) // If Capuchas was added to the list:
        {
            SpawnableEnemies.RemoveAt(SpawnableEnemies.Count - 1); // Remove from list to avoid it to keep several copies and causing issues.
            // Uses RemoveAt instead of Remove for performance reasons, as Count - 1 is always guaranteed to be Capuchas when it's contained within the list.
        }
    }

    GameObject GetEnemyPrefab(int Index)
    {
        // List of enemy's prefabs to instantiate. Capuchas must always be the last one to ensure limit is enforced properly.
        List<GameObject> SpawnList = new List<GameObject>() { _fireWispPrefab, _iceWispPrefab, _toxicWispPrefab, _electricWispPrefab, _capuchasPrefab};
        return SpawnList[Index];
    }

    public int GetCurrentRound()
    {
        return this._round;
    }

    private Transform GetRandomStartingPosition()
    {
        List<Transform> PotentialPositions = new List<Transform>(){ };
        for (int i = 0; i < Spawnpoints.transform.childCount; i++)
        {
            PotentialPositions.Add(Spawnpoints.transform.GetChild(i));
        }
        int RandomOption = Random.Range(0, PotentialPositions.Count);
        return PotentialPositions[RandomOption];
    }

    private void RandomizeStartingPosition(GameObject Instance)
    {
        Instance.transform.position = GetRandomStartingPosition().position;
    }

    private GameObject SpawnMedicalKit()
    {
        return Instantiate(_medicalKit, GameObject.Find("Interactable").transform);
    }

    public GameObject SpawnTutorialObject(int Index)
    {

        if(Index == 5) // Medical Kit is not an enemy
        {
            GameObject MedicalKit = SpawnMedicalKit();
            MedicalKit.transform.position = TutorialObjectPlaceholder.transform.position - (Vector3.up * 0.3f);
            return MedicalKit;
        }
        else
        {
            GameObject ToSpawnPrefab = GetEnemyPrefab(Index);
            GameObject EnemyInstance = Instantiate(ToSpawnPrefab, this.transform.GetChild(0));
            Destroy(EnemyInstance.GetComponent<Enemy>());
            EnemyInstance.transform.position = TutorialObjectPlaceholder.transform.position;
            EnemyInstance.transform.LookAt(2 * EnemyInstance.transform.position - MainCamera.transform.position);
            return EnemyInstance;
        }

    }

    public void DespawnTutorialObject(GameObject TutorialObject)
    {
        Destroy(TutorialObject);
    }

    public void Won()
    {
        VictoryManager.Won();
    }
}