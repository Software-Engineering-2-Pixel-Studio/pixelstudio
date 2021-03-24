using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SpawnMonsterManager : Singleton<SpawnMonsterManager>
{
    [SerializeField] private GameObject waveButton; //reference to the StartWave button

    private List<Monster> activeMonsters = new List<Monster>(); //the list of the monsters in the wave

    private int wave = 0;   //counter for the Waves
    
    [SerializeField] private Text waveText; //holds the reference to the text that shows the number of waves

    private bool WaveActive;

    private PhotonView view;    //PhotonView object for synchronize between each view object in the scene

    // Start is called before the first frame update
    private void Start()
    {
        view = this.GetComponent<PhotonView>();
        //only MasterClient (host) can see this button
        if(PhotonNetwork.IsMasterClient){
            this.waveButton.SetActive(true);
        }
        else
        {
            this.waveButton.SetActive(true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private IEnumerator SpawnWave()
    {
        MapManager.Instance.GeneratePath();
        for(int i = 0; i < wave; i++){
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
            //Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            //monster.Spawn();

            //activeMonsters.Add(monster);

            yield return new WaitForSeconds(2.5f);
        }
        
    }

    //public methods
    public List<Monster> GetActiveMonsters()
    {
        return this.activeMonsters;
    }

    public void StartWave()
    {
        wave++;
        waveText.text = string.Format("Wave: <color=lime>{0}</color>", wave);
        StartCoroutine(SpawnWave());

        waveButton.SetActive(false);
    }

    public void removeMonster(Monster monster)
    {
        activeMonsters.Remove(monster);
        if(!WaveActive){
            waveButton.SetActive(true);
        }
    }
}
