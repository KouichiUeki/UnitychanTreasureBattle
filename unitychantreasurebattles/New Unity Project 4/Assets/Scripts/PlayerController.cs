using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private CharacterController charaCon;       // キャラクターコンポーネント用の変数
    private Vector3 move = Vector3.zero;    // キャラ移動量.
    private GameObject GameController;
    private GameObject BattleController;
    private GameController gc;
    private GameController bc;
    public Camera MainCamera;
    public Camera BattleCamera;
    public Camera GoalCamera;
    public GameObject BattleCommand;
    public GameObject timer;
    public GameObject BattleWord;
    public GameObject HpBar;
    public GameObject EnemyHpBar;
    public bool isGoal = false;
    public float speed;         // 移動速度
    public float jumpPower;        // 跳躍力.
    private const float GRAVITY = 9.8f;         // 重力
    public float rotationSpeed;   // プレイヤーの回転速度
    public float life; //プレイヤーの体力
    public float attack; //プレイヤーの攻撃力
    public float defence; //プレイヤーの防御力
    public float skillpoint; //プレイヤーのスキルポイント
    public double sp;
    private AudioSource soundget;
    private AudioSource soundbadget;

    // Use this for initialization
    void Start()
    {
        GameController = GameObject.Find("GameController");
        BattleController = GameObject.Find("BattleController");
        gc = GameController.GetComponent<GameController>();
        charaCon = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        soundget = audioSources[0];
        soundbadget = audioSources[1];
        anim.SetBool("Damege", false);
        anim.SetBool("Salte", false);
        anim.SetBool("Down", false);
        anim.SetBool("GetUp", false);
        anim.SetBool("Death", false);
        sp = Mathf.Round(skillpoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.GetIsGameOver()) {
            anim.SetBool("Run",false);
            anim.SetBool("Jump", false);
        }
        if (gc.GetIsGameOver()) { return; } //isGameOverがtrueなら以降の処理を行わない
        if (isGoal == true) { return; } //isGoalがtrueなら以降の操作を行わない
        playerMove();
    }
    private void playerMove()
    {
        //移動量の取得
        float y = move.y;
        move = new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical"));      //上下のキー入力を取得し、移動量に代入.
        move = transform.TransformDirection(move);                          //プレイヤー基準の移動方向へ修正する.
        move *= speed;              //移動速度を乗算.

        //重力／ジャンプ処理
        move.y += y;
        if (charaCon.isGrounded)
        {                   //地面に設置していたら
            if (Input.GetKeyDown(KeyCode.Space))
            {   //ジャンプ処理.
                move.y = jumpPower;
                anim.SetBool("Jump", true);
            }
            else
            {
                anim.SetBool("Jump", false);
                    }
        }
        move.y -= GRAVITY * Time.deltaTime; //重力を代入.

        //プレイヤーの向き変更
        Vector3 playerDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);       //左右のキー入力を取得し、移動方向に代入.
        playerDir = transform.TransformDirection(playerDir);                    //プレイヤー基準の向きたい方向へ修正する.
        if (playerDir.magnitude > 0.1f)
        {
            Quaternion q = Quaternion.LookRotation(playerDir);          //向きたい方角をQuaternionn型に直す .
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);   //向きをqに向けて変化させる.
        }

        if(move.x > 0 || move.z > 0 || move.x < 0 || move.z < 0)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
        //移動処理
        charaCon.Move(move * Time.deltaTime);   //プレイヤー移動.
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedStone"))
        {
            soundget.PlayOneShot(soundget.clip);
            attack += 10;
        }
         if (other.CompareTag("BlueStone"))
        {
            soundget.PlayOneShot(soundget.clip);
            defence += 10;
        }
         if (other.CompareTag("GreenStone"))
        {
            soundget.PlayOneShot(soundget.clip);
            skillpoint += 1;
            sp = Mathf.Round(skillpoint);
        }
         if (other.CompareTag("YellowStone"))
        {
            soundget.PlayOneShot(soundget.clip);
            gc.Timeplus();
        } if (other.CompareTag("Needle"))
        {
            StartCoroutine("HitNeedle");
        }
        if (other.CompareTag("Goal"))
        {
            StartCoroutine("GoalDo");
        }
        
    }
    IEnumerator HitNeedle()
    {
        gc.Timeminus();
        anim.SetBool("Damege", true);
        soundbadget.PlayOneShot(soundbadget.clip);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("Damege", false);
    }

    IEnumerator GoalDo()
    {
        isGoal = true;
        if(gc.time > 120)
        {
            attack *= 2.0f;
            defence *= 2.0f;
            skillpoint *= 2.0f;
        }else if(gc.time > 60)
        {
            attack *= 1.5f;
            defence *= 1.5f;
            skillpoint *= 1.5f;
            sp = Mathf.Round(skillpoint);
        }else
        {
            attack *= 1.0f;
            defence *= 1.0f;
            skillpoint *= 1.0f;
        }
        GameObject.Find("BGM1").GetComponent<AudioSource>().enabled = false;
        
        anim.SetBool("Jump", false);
        anim.SetBool("Run", false);
        gameObject.transform.position = new Vector3(110, 12, 170);
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        MainCamera.enabled = false;
        GoalCamera.enabled = true;
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("BGM2").GetComponent<AudioSource>().enabled = true;
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("Salte", true);
        yield return new WaitForSeconds(3.0f);
        BattleCommand.SetActive(true);
        anim.SetBool("Salte", false);
        anim.SetBool("Jump", false);
        gameObject.transform.position = new Vector3(440, 52, 200);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        GoalCamera.enabled = false;
        BattleCamera.enabled = true;
        BattleController.SetActive(true);
        timer.SetActive(false);
        BattleWord.SetActive(true);
        HpBar.SetActive(true);
        EnemyHpBar.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        GameObject.Find("BGM2").GetComponent<AudioSource>().enabled = false;
        GameObject.Find("BGM3").GetComponent<AudioSource>().enabled = true;
    }
 }   