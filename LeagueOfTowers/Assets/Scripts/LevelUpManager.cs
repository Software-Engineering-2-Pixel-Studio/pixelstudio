using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LevelUpManager : Singleton<LevelUpManager>
{
    //fields
    [SerializeField] private int expPoints;
    [SerializeField] private Text expPointsDisplayText;
    [SerializeField] private int playerLevel;
    [SerializeField] private Text playerLevelDisplayText;
    [SerializeField] private int techTokens;
    [SerializeField] private Text techTokensDisplayText;

    private int expRequireToLevelUP = 50;
    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        this.view = this.GetComponent<PhotonView>();
        this.expPoints = 0;
        this.playerLevel = 1;
        this.techTokens = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //get/set
    public int GetExpPoints()
    {
        return this.expPoints;

    }
    //events

    //actions
    public void AddExpPoints(int earnEXP)
    {
        // this.expPoints += earnEXP;
        // this.expPointsDisplayText.text = string.Format("Exp: {0} / {1}", this.expPoints.ToString(), this.expRequireToLevelUP);

        // if(this.expPoints >= this.expRequireToLevelUP)
        // {
        //     LevelUp();
            
        // }
        this.view.RPC("addExpRPC", RpcTarget.All, earnEXP);
    }

    private void LevelUp()
    {   
        //increase level by 1
        this.playerLevel += 1;
        this.playerLevelDisplayText.text = string.Format("Level: {0}", this.playerLevel);
        //increae tech token by 1
        this.addTechToken();
        //reset exp back to 0
        this.expPoints = 0;
        this.expPointsDisplayText.text = string.Format("Exp: {0} / {1}", this.expPoints.ToString(), this.expRequireToLevelUP);
    }

    private void addTechToken()
    {
        this.techTokens++;
        this.techTokensDisplayText.text = string.Format("Tokens: {0}", this.techTokens.ToString());
    }

    public void SpendToken()
    {
        this.techTokens--;
        this.techTokensDisplayText.text = string.Format("Tokens: {0}", this.techTokens.ToString());
    }

    //punRPC
    [PunRPC]
    private void addExpRPC(int earnEXP)
    {
        this.expPoints += earnEXP;
        this.expPointsDisplayText.text = string.Format("Exp: {0} / {1}", this.expPoints.ToString(), this.expRequireToLevelUP);

        if(this.expPoints >= this.expRequireToLevelUP)
        {
            LevelUp();
        }
    }
}
