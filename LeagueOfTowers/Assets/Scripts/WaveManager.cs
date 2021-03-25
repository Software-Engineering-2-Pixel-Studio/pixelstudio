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

    private ObjectPool pool;
    //private bool WaveActive;

    private PhotonView view;    //PhotonView object for synchronize between each view object in the scene

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
            this.startWaveButton.SetActive(true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private IEnumerator SpawnWave()
    {
        MapManager.Instance.GeneratePath();
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
    public ObjectPool GetPool()
    {
        return this.pool;
    }

    public List<Monster> GetActiveMonsters()
    {
        return this.activeMonsters;
    }

    public int GetWaves()
    {
        return this.waves;
    }



    public void StartWave()
    {
        waves++;
        waveText.text = string.Format("{0}", waves);
        StartCoroutine(SpawnWave());

        startWaveButton.SetActive(false);
    }

    public void removeMonster(Monster monster)
    {
        activeMonsters.Remove(monster);
        if(!isWaveActive()){
            startWaveButton.SetActive(true);
        }
    }

    // private bool WaveActive{
    //     get{
    //         return activeMonsters.Count > 0;
    //     }
    // }
}
