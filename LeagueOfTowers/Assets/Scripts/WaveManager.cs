using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField] private GameObject startWaveButton; //reference to the StartWave button

    private List<Monster> activeMonsters = new List<Monster>(); //the list of the monsters in the wave

    private int waves = 0;   //counter for the Waves
    
    [SerializeField] private Text waveText; //holds the reference to the text that shows the number of waves

    private ObjectPool pool;    //pool of objects to that holds the Monster prefabs

    private PhotonView view;    //PhotonView object for synchronize between each view object in the scene

    private Coroutine waveCoroutine; //to control the coroutine of the wave

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
            switch (monsterIndex)
            {
                case 0:
                    type = "TrainingDummy";
                    break;
                case 1:
                    type = "Scarecrow";
                    break;
                default:
                    break;
            }

            //get the monster object from the pool of objects control by this WaveManager
            Monster monster = pool.GetObject(type).GetComponent<Monster>();
            monster.Spawn();

            activeMonsters.Add(monster);

            yield return new WaitForSeconds(2.5f);
        }
        
    }
    private bool isWaveActive()
    {
        return this.activeMonsters.Count > 0;
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
    public List<Monster> GetActiveMonsters()
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
        waves++;
        waveText.text = string.Format("{0}", waves);
        waveCoroutine = StartCoroutine(SpawnWave());

        startWaveButton.SetActive(false);
    }

    /*
        Methods to stop the wave coroutine when gameover
    */
    public void StopWave()
    {
        //stop producing new monsters on the map
        StopCoroutine(waveCoroutine);
        // for(int i=0; i<=activeMonsters.Count; i++){
        //         activeMonsters[i].SetActive(false);
        //         activeMonsters.Remove(activeMonsters[i]);
        // }
        // activeMonsters[0].SetActive(false);
        // activeMonsters.Remove(activeMonsters[0]);
    }

    /*
        Method to remove monster from the list of active monsters and 
        also display wave button again if all of active monsters are gone.
    */
    public void removeMonster(Monster monster)
    {
        activeMonsters.Remove(monster);
        if(!isWaveActive()){
            startWaveButton.SetActive(true);
        }
    }
}
