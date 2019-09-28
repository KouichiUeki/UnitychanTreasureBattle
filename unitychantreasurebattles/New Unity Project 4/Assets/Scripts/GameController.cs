using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public float time;
    public float timeplus;
    public float timeminus;
    public UnityEngine.UI.Text timer;
    public UnityEngine.UI.Text State;
    public GameObject go;
    public GameObject gl;
    private GameObject Unitychan;
    private GameObject Battle;
    public Camera MainCamera;
    public Camera BattleCamera;
    public Camera GoalCamera;
    private PlayerController pc;
    private BattleController bc;
    private bool isGameOver = false;
    private bool isComplete = false;
    // Use this for initialization
    void Start()
    {
        Unitychan = GameObject.Find("SD_unitychan_humanoid");
        pc = Unitychan.GetComponent<PlayerController>();
        Battle = GameObject.Find("BattleController");
        bc = Battle.GetComponent<BattleController>();
        MainCamera.enabled = true; //メインカメラをオンにする
        BattleCamera.enabled = false; //バトルカメラは切っておく
        GoalCamera.enabled = false; //ゴールカメラも切っておく
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
        if (pc.isGoal == false)
        {
            time -= Time.deltaTime;
        }
            if (time < 0) time = 0;

            timer.text = time.ToString("N1");

        State.text = "\n攻撃力" + pc.attack.ToString() + "\n防御力" + pc.defence.ToString() + "\nスキルポイント" + pc.sp.ToString();

        if (isGameOver) { return; } //GAME OVERフラグがtrueの場合、以降の処理を行わない
            Player_DeadCheck();
            Enemy_DeadCheck();
            TimerCheck();
    }
    public void Timeplus() //時間をプラスする処理
    {
        time += timeplus;
    }
    public void Timeminus() //時間をマイナスする処理
    {
        time -= timeminus;
    }
    private void Player_DeadCheck() //プレイヤーのlifeを確認し、0ならGAME OVER
    {
        if(bc.life_now == 0)
        { 
            GameOver();
        }
    }
    private void Enemy_DeadCheck()
    {
        if(bc.Spiderlife_now == 0)
        {
            GameComplete();
        }
    }
    private void TimerCheck() //残り秒数を確認し、0ならGAME OVER
    {
        if(time == 0)
        {
            GameOver();
        }
    }
    public bool GetIsGameOver() { return isGameOver; }  //isGameOverの値を返す
    public bool GetIsGameComplete() { return isComplete; } //isGameCompleteの値を返す

    public void GameOver() //GAME OVER処理
    {
        isGameOver = true; //GAME OVERフラグをtrueにする
    }
    public void GameComplete() //GAME COMPLETE処理
    {
        isComplete = true; //GAME CONPLETEフラグをtrueにする
    }
}