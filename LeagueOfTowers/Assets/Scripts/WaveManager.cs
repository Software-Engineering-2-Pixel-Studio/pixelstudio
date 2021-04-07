using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField] private GameObject startWaveButton; //reference to the StartWave button

    //private List<Monster> activeMonsters = new List<Monster>(); //the list of the monsters in the wave
    private List<MonsterScript> activeMonsters = new List<MonsterScript>();
    private int waves = 0;   //counter for the Waves
    
    [SerializeField] private Text waveText; //holds the reference to the text that shows the number of waves

    private ObjectPool pool;    //pool of objects to that holds the Monster prefabs

    private PhotonView view;    //PhotonView object for synchronize between each view object in the scene

    private Coroutine waveCoroutine; //to control the coroutine of the wave

    private bool waveOver;      //state when wave have done spawning monster

    private float enemyHealthExtra = 0;   //base health extra for monster

    private int monsterActiveCount = 0; //number of active monster on scene (not destroyed)

    // Start is called before the first frame update
    private void Start()
    {
        view = this.GetComponent<PhotonView>();
        pool = GetComponent<ObjectPool>();

        //only MasterClient (host) can see this button
        if(PhotonNetwork.IsMasterClient){
            this.startWaveButton.SetActive(true);
        }
        else
        {
            this.startWaveButton.SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(PhotonNetwork.IsMasterClient){
            if(!isWaveActive() && waveOver){
                startWaveButton.SetActive(true);
            }
        }
    }

    /*
        wave coroutine for spawning monsters
    */
    private IEnumerator SpawnWave()
    {
        //generate the path from Spawn Tile to Base Tile
        MapManager.Instance.GeneratePath();

        //inscrease number of monsters to be spawned based on
        //number of survived waves
        for(int i = 0; i < waves; i++){
            int monsterIndex = Random.Range(0, 2);
            string type = string.Empty;
            object[] data = new object[6];
            switch (monsterIndex)
            {
                case 0:
                    type = "TrainingDummy";
                    data = MonsterData.GetDummyData();
                    break;
                case 1:
                    type = "Scarecrow";
                    data = MonsterData.GetScarecrowData();
                    break;
                default:
                    break;
            }
            data[1] = (float) data[1] + enemyHealthExtra;
            //get the monster object from the pool of objects control by this WaveManager
            //Monster monster = pool.GetObject(type).GetComponent<Monster>();
            //monster.Spawn(enemyHealth);
           

            Tile spawnTile = MapManager.Instance.GetSpawnTile();
            GameObject monsterGO = PhotonNetwork.Instantiate(type, spawnTile.GetCenterWorldPosition(), Quaternion.identity, 0, data);
            MonsterScript monster = monsterGO.GetComponent<MonsterScript>();

            if (this.waves % 3 == 0) //increase health of monsters by 2 every 3 waves
            {
                this.enemyHealthExtra += 2;
            }

            this.activeMonsters.Add(monster);

            IncreaseMonsterCount();
            yield return new WaitForSeconds(2.5f);
        }
        this.waveOver = true;
        
    }
    private bool isWaveActive()
    {
        return this.monsterActiveCount > 0;
    }

    //public methods
    /*
        Method to get ObjectPool object which stores the prefabs of monsters
        stored in this WaveManager
    */
    public ObjectPool GetPool()
    {
        return this.pool;
    }

    /*
        Method ot get lists of active monsters
    */
    public List<MonsterScript> GetActiveMonsters()
    {
        return this.activeMonsters;
    }

    /*
        Method to get number of survived waves
    */
    public int GetWaves()
    {
        return this.waves;
    }

    /*
        Methods to start wave coroutine when the start wave button is clicked
    */
    public void StartWave()
    {
        IncreaseWaveCount();
        this.waveCoroutine = StartCoroutine(SpawnWave());

        this.startWaveButton.SetActive(false);
        this.waveOver = false;
    }

    /*
        Methods to stop the wave coroutine when gameover
    */
    public void StopWave()
    {
        //stop producing new monsters on the map
        StopCoroutine(waveCoroutine);
    }

    public void DeactiveMonsters()
    {
        foreach (MonsterScript monster in activeMonsters){
            monster.ChangeIsActiveState(false);
        }
    }

    /*
        Method to remove monster from the list of active monsters and 
        also display wave button again if all of active monsters are gone.
    */
    public void RemoveMonster(MonsterScript monster)
    {
        activeMonsters.Remove(monster);
        monster.DestroyThisMonster();
        //Debug.Log("monster destroyed");
        DecreaseMonsterCount();
    }

    //punRPC methods for sync this script's fields over network
    [PunRPC]
    private void increaseWaveCountRPC()
    {
        this.waves++;
        this.waveText.text = string.Format("{0}", waves);
    }

    public void IncreaseWaveCount()
    {
        this.view.RPC("increaseWaveCountRPC", RpcTarget.All);
    }

    [PunRPC]
    private void decreaseMonsterCountPRC()
    {
        this.monsterActiveCount--;
    }

    public void DecreaseMonsterCount()
    {
        this.view.RPC("decreaseMonsterCountPRC", RpcTarget.All);
    }

    [PunRPC]
    private void increaseMonsterCountRPC()
    {
        this.monsterActiveCount++;
    }
    public void IncreaseMonsterCount()
    {
        this.view.RPC("increaseMonsterCountRPC", RpcTarget.All);
    }

}
