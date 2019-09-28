using UnityEngine;
using System.Collections;

public class BattleController : MonoBehaviour {
    public UnityEngine.UI.Text Word; //バトル中のテキスト用の変数
    public UnityEngine.UI.Text Hp; //ユニティちゃんのHPを表示するテキスト用の変数
    public UnityEngine.UI.Text EnemyHp; //敵のHPを表示するテキスト用の変数
    public UnityEngine.UI.Slider HpBar; //ユニティちゃんのHPバーを表示するスライダー用の変数　
    public UnityEngine.UI.Slider EnemyHpBar; //敵のHPバーを表示するスライダー用の変数
    private Animator anim; //ユニティちゃんのAnimatorコンポーネント用の変数
    private Animator spiderAnim; //敵キャラクターのAnimatorコンポーネント用の変数
    public CharacterController charaCon;  // キャラクターコンポーネント用の変数
    private GameObject GameController;
    public GameObject[] button; //ボタンのGameObject用の変数
    public GameObject carsol; //カーソルのGameObject用の変数
    public GameObject spider; //敵キャラクターのGameObject用の変数
    public GameObject gobutton; //GAMEOVER時のボタンのGameObject用の変数
    private GameObject Unitychan; //ユニティちゃんのGameObject用の変数
    private PlayerController pc; //プレイヤーコントローラー用の変数
    private GameController gc;
    private Vector3 battlemove = Vector3.zero;    // キャラ移動量
    public float Spider_life; //敵のHP
    public float Spider_attack; //敵の攻撃力
    public float Spider_deffence; //敵の防御力
    public float speed; //移動スピード
    private float damegeCheck; //受けたダメージ量
    public float life_now; //ユニティちゃんの現在HP
    public float Spiderlife_now; //敵キャラクターの現在HP
    private int Select; //コマンドボタンがどこを選択しているか
    private int enemySelect; //敵の攻撃コマンド

    private AudioSource battleBGM;
    private AudioSource sounddamege;
    private AudioSource soundenemydamege;
    private AudioSource soundenemyattack;
    private AudioSource soundcursor;
    private AudioSource soundennemydown;
    private AudioSource soundselect;
    private AudioSource sounderror;
    private AudioSource soundattack;
    private AudioSource soundexattack;
    private AudioSource soundcharge;

    public GameObject effectattack;
    public GameObject effectskill1;
    public GameObject effectskill2;
    public GameObject effectskill3;
    public GameObject charge1;
    public GameObject charge2;

    private int isBattleMoveChara = 0; //ユニティちゃんが移動する状態のフラグ管理
    private int isBattleMoveEnemy = 0; //敵キャラクターが移動する状態のフラグ管理
    private int isWord = 0; //バトル中のテキスト用のフラグ管理
    private bool playerTurn = true; //ユニティちゃん側の行動を管理するフラグ
    private bool enemyTurn = false; //敵キャラクター側の行動を管理するフラグ
    private bool chargecounter = false; //ユニティちゃんがチャージをしたかどうかを管理するフラグ
    private bool enemychargecounter = false; //敵がチャージをしたかどうかを管理するフラグ
                                             // Use this for initialization
    void Start() {
        Select = 0;
        carsol.transform.position = button[Select].transform.position;
        Unitychan = GameObject.Find("SD_unitychan_humanoid");
        GameController = GameObject.Find("GameController");
        pc = Unitychan.GetComponent<PlayerController>();
        gc = GameController.GetComponent<GameController>();
        charaCon = GetComponent<CharacterController>();
        anim = Unitychan.GetComponent<Animator>();
        spiderAnim = spider.GetComponent<Animator>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        soundcursor = audioSources[0];
        sounddamege = audioSources[1];
        soundenemydamege = audioSources[2];
        soundenemyattack = audioSources[3];
        soundennemydown = audioSources[4];
        soundselect = audioSources[5];
        sounderror = audioSources[6];
        soundattack = audioSources[7];
        soundexattack = audioSources[8];
        soundcharge = audioSources[9];

        life_now = pc.life;
        Spiderlife_now = Spider_life;
        spiderAnim.SetBool("Run", false);
        spiderAnim.SetBool("Attack", false);
        spiderAnim.SetBool("Death", false);
        spiderAnim.SetBool("Skill", false);
        spiderAnim.SetBool("Def", false);
    }

    // Update is called once per frame
    void Update() {
        isWordCheck(); //どのテキストを表示するかをチェックする
        HpBar.value = life_now; //HPバーをユニティちゃんの現在のlifeに応じて変化させる
        Hp.text = "ユニティちゃん:" + life_now.ToString() + "/" + pc.life.ToString(); //ユニティちゃんのHPを数字で表示
        EnemyHpBar.value = Spiderlife_now; //HPバーを敵の現在のlifeに応じて変化させる
        EnemyHp.text = "Forest Spider:" + Spiderlife_now.ToString() + "/" + Spider_life; //敵のHPを数字で表示

        if (life_now < 0) life_now = 0;
        if (Spiderlife_now < 0) Spiderlife_now = 0;
        if (gc.GetIsGameComplete())
        {
            enemyTurn = false;
        }
        if (Spiderlife_now == 0)
        {
            StartCoroutine("EnemyDeath");
        }
        if(life_now == 0)
        {
            StartCoroutine("Death");
        }
        if (gc.GetIsGameComplete()) { return; }
        if (gc.GetIsGameOver()) { return; }
        if (Select == 0)
        {
            isWord = 1;
        }
        else if (Select == 1)
        {
            isWord = 2;
        }
        else if (Select == 2)
        {
            isWord = 3;
        }
        if (isBattleMoveChara == 1)
        {
            pc.transform.Translate(transform.forward * 1.0f * speed * Time.deltaTime);
        }
        else if (isBattleMoveChara == 2)
        {
            pc.transform.Translate(transform.forward * -1.0f * speed * Time.deltaTime);
        }
        
        if (isBattleMoveEnemy == 1)
        {
            spider.transform.Translate(transform.forward * 1.0f * speed * Time.deltaTime);
        }
        else if (isBattleMoveEnemy == 2)
        {
            spider.transform.Translate(transform.forward * -1.0f * speed * Time.deltaTime);
        }
        if (playerTurn == true)
        {
            if (Input.GetKeyDown("down"))
            {
                if (Select != 2 && Select < 3)
                {
                    soundcursor.PlayOneShot(soundcursor.clip);
                    Select += 1;
                    carsol.transform.position = button[Select].transform.position;
                }
            }
            if (Input.GetKeyDown("up"))
            {
                if (Select != 0 && Select < 3)
                {
                    soundcursor.PlayOneShot(soundcursor.clip);
                    Select -= 1;
                    carsol.transform.position = button[Select].transform.position;
                }
            }
            if (Input.GetKeyDown("z") && pc.isGoal == true && pc.sp > 0)
            {
                switch (Select)
                {
                    case 0:
                        StartCoroutine("Attack");
                        soundselect.PlayOneShot(soundselect.clip);
                        break;
                    case 1:
                        StartCoroutine("Skill");
                        soundselect.PlayOneShot(soundselect.clip);
                        break;
                    case 2:
                        StartCoroutine("Charge");
                        soundselect.PlayOneShot(soundselect.clip);
                        break;
                }
            } else if (Input.GetKeyDown("z") && pc.isGoal == true && pc.sp == 0)
            {
                switch (Select)
                {
                    case 0:
                        StartCoroutine("Attack");
                        soundselect.PlayOneShot(soundselect.clip);
                        break;
                    case 1:
                        StartCoroutine("SpDry");
                        sounderror.PlayOneShot(sounderror.clip);
                        break;
                    case 2:
                        StartCoroutine("Charge");
                        soundselect.PlayOneShot(soundselect.clip);
                        break;
                }
            }
        }
        if (enemyTurn == true && enemychargecounter == false)
        {
            enemySelect = Random.Range(0, 100); //0~100をランダム取りで敵の行動が決定する
            if (0 <= enemySelect && enemySelect <= 59) //0~59の場合通常攻撃
            {
                StartCoroutine("EnemyAttack");
            } else if (60 <= enemySelect && enemySelect <= 84) //60~84の場合強攻撃
            {
                StartCoroutine("EnemySkill");
            }
            else //それ以外の場合チャージ
            {
                StartCoroutine("EnemyCharge");
            }

        }else if(enemyTurn == true && enemychargecounter == true)
        {
            enemySelect = Random.Range(0, 100); //0~100をランダム取りで敵の行動が決定する
            if (0 <= enemySelect && enemySelect <= 59) //0~59の場合通常攻撃
            {
                StartCoroutine("EnemyAttack");
            }
            else if (60 <= enemySelect && enemySelect <= 100) //60~100の場合強攻撃
            {
                StartCoroutine("EnemySkill");
            }
        }
    }

    public void isWordCheck()
    {
        if (isWord == 1)
        {
            Word.text = "敵に通常攻撃をします";
        } else if (isWord == 2)
        {
            Word.text = "スキルポイントを1消費して2倍のダメージを与えます";
        } else if (isWord == 3)
        {
            Word.text = "1ターン防御力が半分になりますが次の攻撃を2.5倍します";
        } else if (isWord == 4)
        {
            Word.text = "ユニティちゃんの通常攻撃!";
        } else if (isWord == 5)
        {
            Word.text = "ユニティちゃんのスキル攻撃!";
        } else if (isWord == 6)
        {
            Word.text = "ユニティちゃんは力を溜めている…";
        } else if (isWord == 7)
        {
            Word.text = "敵の通常攻撃";
        } else if (isWord == 8)
        {
            Word.text = "敵の強攻撃!!";
        } else if (isWord == 9)
        {
            Word.text = "敵は力を溜めている…!";
        } else if (isWord == 10)
        {
            Word.text = "ユニティちゃんは" + damegeCheck.ToString() + "のダメージを受けた";
        } else if (isWord == 11)
        {
            Word.text = "敵に" + damegeCheck.ToString() + "のダメージを与えた";
        } else if (isWord == 12)
        {
            Word.text = "スキルポイントが足りません";
        } else if(isWord == 13)
        {
            Word.text = "Forest Spiderを倒した！";
        }else if(isWord == 14)
        {
            Word.text = "ユニティちゃんは倒された…";
        }
    }

    IEnumerator SpDry()
    {
        Select = 3;
        isWord = 12;
        yield return new WaitForSeconds(1.0f);
        isWord = 0;
        Select = 1;
    }

    IEnumerator Death()
    {
        GameObject.Find("BGM3").GetComponent<AudioSource>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        isWord = 14;
        anim.SetBool("Death", true);
        GameObject.Find("BGM5").GetComponent<AudioSource>().enabled = true;
        yield return new WaitForSeconds(2.0f);
        gc.go.SetActive(true);
        gobutton.SetActive(true);
    }

    IEnumerator EnemyDeath()
    {
        if (Select == 0)
        {
            GameObject.Find("BGM3").GetComponent<AudioSource>().enabled = false;
            yield return new WaitForSeconds(1.6f);
            isWord = 13;
            Destroy(spider);
            yield return new WaitForSeconds(1.0f);
            anim.SetBool("Salte", true);
            GameObject.Find("BGM4").GetComponent<AudioSource>().enabled = true;
            yield return new WaitForSeconds(2.5f);
            gc.gl.SetActive(true);
            gobutton.SetActive(true);
        }
        else
        {
            GameObject.Find("BGM3").GetComponent<AudioSource>().enabled = false;
            yield return new WaitForSeconds(1.6f);
            isWord = 13;
            Destroy(spider);
            yield return new WaitForSeconds(1.0f);
            anim.SetBool("Salte", true);
            GameObject.Find("BGM4").GetComponent<AudioSource>().enabled = true;
            yield return new WaitForSeconds(2.5f);
            gc.gl.SetActive(true);
            gobutton.SetActive(true);
        }

    }
    
    IEnumerator Attack()
    {
        if (chargecounter == true && enemychargecounter == true)
        {
            playerTurn = false;
            Select = 3;
            isWord = 4;
            anim.SetBool("Run", true);
            isBattleMoveChara = 1;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            damegeCheck = pc.attack * 2.5f - Spider_deffence / 2;
            if (damegeCheck < 0)
            {
                damegeCheck = 0;
            }
            damegeCheck = Mathf.Round(damegeCheck);
            Spiderlife_now = Spiderlife_now - damegeCheck;
            yield return new WaitForSeconds(0.1f);
            soundattack.PlayOneShot(soundattack.clip);
            Instantiate(
                effectattack, effectattack.transform.position, Quaternion.Euler(439.26f, 51.62f, 207f)
                );
            spiderAnim.SetBool("Death", true);
            isWord = 11;
            yield return new WaitForSeconds(1.5f);
            spiderAnim.SetBool("Death", false);
            anim.SetBool("Run", true);
            isBattleMoveChara = 2;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            yield return new WaitForSeconds(0.8f);
            chargecounter = false;
            Select = 0;
            enemyTurn = true;
        } else if (chargecounter == true && enemychargecounter == false)
        {
            playerTurn = false;
            Select = 3;
            isWord = 4;
            anim.SetBool("Run", true);
            isBattleMoveChara = 1;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            damegeCheck = pc.attack * 2.5f - Spider_deffence;
            if (damegeCheck < 0)
            {
                damegeCheck = 0;
            }
            damegeCheck = Mathf.Round(damegeCheck);
            Spiderlife_now = Spiderlife_now - damegeCheck;
            yield return new WaitForSeconds(0.1f);
            soundattack.PlayOneShot(soundattack.clip);
            spiderAnim.SetBool("Death", true);
            isWord = 11;
            Instantiate(
                effectattack, effectattack.transform.position, Quaternion.Euler(439.26f, 51.62f, 207f)
                );
            yield return new WaitForSeconds(1.5f);
            spiderAnim.SetBool("Death", false);
            anim.SetBool("Run", true);
            isBattleMoveChara = 2;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            yield return new WaitForSeconds(0.8f);
            Select = 0;
            chargecounter = false;
            enemyTurn = true;
        } else if (chargecounter == false && enemychargecounter == true)
        {
            playerTurn = false;
            Select = 3;
            isWord = 4;
            anim.SetBool("Run", true);
            isBattleMoveChara = 1;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            damegeCheck = pc.attack - Spider_deffence / 2;
            if (damegeCheck < 0)
            {
                damegeCheck = 0;
            }
            damegeCheck = Mathf.Round(damegeCheck);
            Spiderlife_now = Spiderlife_now - damegeCheck;
            yield return new WaitForSeconds(0.1f);
            soundattack.PlayOneShot(soundattack.clip);
            spiderAnim.SetBool("Death", true);
            Instantiate(
                effectattack, effectattack.transform.position, Quaternion.Euler(439.26f, 51.62f, 207f)
                );
            isWord = 11;
            yield return new WaitForSeconds(1.5f);
            spiderAnim.SetBool("Death", false);
            anim.SetBool("Run", true);
            isBattleMoveChara = 2;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            yield return new WaitForSeconds(0.8f);
            Select = 0;
            chargecounter = false;
            enemyTurn = true;
        } else if (chargecounter == false && enemychargecounter == false)
        {
            playerTurn = false;
            Select = 3;
            isWord = 4;
            anim.SetBool("Run", true);
            isBattleMoveChara = 1;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            damegeCheck = pc.attack - Spider_deffence;
            if (damegeCheck < 0)
            {
                damegeCheck = 0;
            }
            damegeCheck = Mathf.Round(damegeCheck);
            Spiderlife_now = Spiderlife_now - damegeCheck;
            isBattleMoveChara = 0;
            yield return new WaitForSeconds(0.1f);
            soundattack.PlayOneShot(soundattack.clip);
            spiderAnim.SetBool("Death", true);
            Instantiate(
                effectattack, effectattack.transform.position, Quaternion.Euler(439.26f, 51.62f, 207f)
                );
            isWord = 11;
            yield return new WaitForSeconds(1.5f);
            spiderAnim.SetBool("Death", false);
            anim.SetBool("Run", true);
            isBattleMoveChara = 2;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            yield return new WaitForSeconds(0.8f);
            Select = 0;
            chargecounter = false;
            enemyTurn = true;
        }
    }

    IEnumerator Skill()
    {
        if (chargecounter == true && enemychargecounter == true)
        {
            playerTurn = false;
            pc.sp -= 1;
            Select = 3;
            isWord = 5;
            isBattleMoveChara = 1;
            anim.SetBool("Run", true);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            damegeCheck = pc.attack * 5 - Spider_deffence / 2;
            if (damegeCheck < 0)
            {
                damegeCheck = 0;
            }
            damegeCheck = Mathf.Round(damegeCheck);
            Spiderlife_now = Spiderlife_now - damegeCheck;
            isBattleMoveChara = 0;
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill1, effectskill1.transform.position, Quaternion.Euler(440.23f, 52.58f, 207.82f)
                );
            soundexattack.PlayOneShot(soundexattack.clip);
            spiderAnim.SetBool("Death", true);    
            isWord = 11;
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill2, effectskill2.transform.position, Quaternion.Euler(438.84f, 51.31f, 207.69f)
                );
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill3, effectskill3.transform.position, Quaternion.Euler(439.9f, 52.3f, 209.72f)
                );
            yield return new WaitForSeconds(1.8f);
            spiderAnim.SetBool("Death", false);
            anim.SetBool("Run", true);
            isBattleMoveChara = 2;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            yield return new WaitForSeconds(0.8f);
            Select = 1;
            chargecounter = false;
            enemyTurn = true;
        } else if (chargecounter == true && enemychargecounter == false)
        {
            playerTurn = false;
            pc.sp -= 1;
            Select = 3;
            isWord = 5;
            isBattleMoveChara = 1;
            anim.SetBool("Run", true);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            damegeCheck = pc.attack * 5 - Spider_deffence;
            if (damegeCheck < 0)
            {
                damegeCheck = 0;
            }
            damegeCheck = Mathf.Round(damegeCheck);
            Spiderlife_now = Spiderlife_now - damegeCheck;
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill1, effectskill1.transform.position, Quaternion.Euler(440.23f, 52.58f, 207.82f)
                );
            soundexattack.PlayOneShot(soundexattack.clip);
            spiderAnim.SetBool("Death", true);
            isWord = 11;
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill2, effectskill2.transform.position, Quaternion.Euler(438.84f, 51.31f, 207.69f)
                );
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill3, effectskill3.transform.position, Quaternion.Euler(439.9f, 52.3f, 209.72f)
                );
            yield return new WaitForSeconds(1.8f);
            spiderAnim.SetBool("Death", false);
            anim.SetBool("Run", true);
            isBattleMoveChara = 2;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            yield return new WaitForSeconds(0.8f);
            Select = 1;
            chargecounter = false;
            enemyTurn = true;
        } else if (chargecounter == false && enemychargecounter == true)
        {
            playerTurn = false;
            pc.sp -= 1;
            Select = 3;
            isWord = 5;
            isBattleMoveChara = 1;
            anim.SetBool("Run", true);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            damegeCheck = pc.attack * 2 - Spider_deffence / 2;
            if (damegeCheck < 0)
            {
                damegeCheck = 0;
            }
            damegeCheck = Mathf.Round(damegeCheck);
            Spiderlife_now = Spiderlife_now - damegeCheck;
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill1, effectskill1.transform.position, Quaternion.Euler(440.23f, 52.58f, 207.82f)
                );
            soundexattack.PlayOneShot(soundexattack.clip);
            spiderAnim.SetBool("Death", true);
            isWord = 11;
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill2, effectskill2.transform.position, Quaternion.Euler(438.84f, 51.31f, 207.69f)
                );
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill3, effectskill3.transform.position, Quaternion.Euler(439.9f, 52.3f, 209.72f)
                );
            yield return new WaitForSeconds(1.8f);
            spiderAnim.SetBool("Death", false);
            anim.SetBool("Run", true);
            isBattleMoveChara = 2;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            yield return new WaitForSeconds(0.8f);
            Select = 1;
            chargecounter = false;
            enemyTurn = true;
        } else if (chargecounter == false && enemychargecounter == false)
        {
            playerTurn = false;
            pc.sp -= 1;
            Select = 3;
            isWord = 5;
            isBattleMoveChara = 1;
            anim.SetBool("Run", true);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            damegeCheck = pc.attack * 2 - Spider_deffence;
            if (damegeCheck < 0)
            {
                damegeCheck = 0;
            }
            damegeCheck = Mathf.Round(damegeCheck);
            Spiderlife_now = Spiderlife_now - damegeCheck;
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill1, effectskill1.transform.position, Quaternion.Euler(440.23f, 52.58f, 207.82f)
                );
            soundexattack.PlayOneShot(soundexattack.clip);
            spiderAnim.SetBool("Death", true);
            isWord = 11;
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill2, effectskill2.transform.position, Quaternion.Euler(438.84f, 51.31f, 207.69f)
                );
            yield return new WaitForSeconds(0.1f);
            Instantiate(
                effectskill3, effectskill3.transform.position, Quaternion.Euler(439.9f, 52.3f, 209.72f)
                );
            yield return new WaitForSeconds(1.8f);
            //isWord = 11;
            spiderAnim.SetBool("Death", false);
            anim.SetBool("Run", true);
            isBattleMoveChara = 2;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Run", false);
            isBattleMoveChara = 0;
            yield return new WaitForSeconds(0.8f);
            Select = 1;
            chargecounter = false;
            enemyTurn = true;
        }
    }

    IEnumerator Charge()
    {
        playerTurn = false;
        chargecounter = false;
        Select = 3;
        isWord = 6;
        anim.SetBool("Salte", true);
        Instantiate(
            charge1, charge1.transform.position, Quaternion.Euler(440f, 50f, 200f)
            );
        soundcharge.PlayOneShot(soundcharge.clip);
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("Salte", false);
        yield return new WaitForSeconds(0.5f);
        soundcharge.Stop();
        Select = 2;
        chargecounter = true;
        enemyTurn = true;
    }

    IEnumerator EnemyAttack()
    {
        if (chargecounter == true && enemychargecounter == true)
        {
            if (Select == 0)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack * 2 - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 0;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 1)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack * 2 - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 1;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 2)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack * 2 - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 2;
                enemychargecounter = false;
                playerTurn = true;
            }
        }
        else if (chargecounter == true && enemychargecounter == false)
        {
            if (Select == 0)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack - pc.defence / 2;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 0;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 1)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack - pc.defence / 2;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 1;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 2)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack - pc.defence / 2;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 2;
                enemychargecounter = false;
                playerTurn = true;
            }
        }
        else if (chargecounter == false && enemychargecounter == true)
        {
            if (Select == 0)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack * 2.5f - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 0;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 1)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack * 2.5f - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 1;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 2)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack * 2.5f - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 2;
                enemychargecounter = false;
                playerTurn = true;
            }
        }else if(chargecounter == false && enemychargecounter == false)
        {
            if (Select == 0)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 0;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 1)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 1;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 2)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 7;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.3f);
                sounddamege.PlayOneShot(sounddamege.clip);
                anim.SetBool("Damege", true);
                damegeCheck = Spider_attack - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(0.8f);
                isWord = 10;
                spiderAnim.SetBool("Attack", false);
                anim.SetBool("Damege", false);
                yield return new WaitForSeconds(0.8f);
                isBattleMoveEnemy = 2;
                spiderAnim.SetBool("Run", true);
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.25f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 2;
                enemychargecounter = false;
                playerTurn = true;
            }
        }
    }
    IEnumerator EnemySkill()
    {
        if (chargecounter == true && enemychargecounter == true)
        {
            if (Select == 0)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 5 - pc.defence / 2;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 0;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 1)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 5 - pc.defence / 2;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 1;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 2)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 5 - pc.defence / 2;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 2;
                enemychargecounter = false;
                playerTurn = true;
            }
        } else if (chargecounter == true && enemychargecounter == false)
        {
            if (Select == 0)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 2 - pc.defence / 2;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 0;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 1)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 2 - pc.defence / 2;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 1;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 2)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 2 - pc.defence / 2;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 2;
                enemychargecounter = false;
                playerTurn = true;
            }
        }else if(chargecounter == false && enemychargecounter == true)
        {
            if (Select == 0)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 5 - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 0;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 1)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 5 - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 1;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 2)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 5 - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 2;
                enemychargecounter = false;
                playerTurn = true;
            }
        }else if(chargecounter == false && enemychargecounter == false)
        {
            if (Select == 0)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 2.5f - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 0;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 1)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 2.5f - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 1;
                enemychargecounter = false;
                playerTurn = true;
            }
            else if (Select == 2)
            {
                enemyTurn = false;
                spiderAnim.SetBool("Run", true);
                Select = 3;
                isWord = 8;
                isBattleMoveEnemy = 1;
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                spiderAnim.SetBool("Skill", true);
                yield return new WaitForSeconds(0.3f);
                soundenemydamege.PlayOneShot(soundenemydamege.clip);
                anim.SetBool("Down", true);
                damegeCheck = Spider_attack * 2.5f - pc.defence;
                if (damegeCheck < 0)
                {
                    damegeCheck = 0;
                }
                damegeCheck = Mathf.Round(damegeCheck);
                life_now = life_now - damegeCheck;
                yield return new WaitForSeconds(1.3f);
                isWord = 10;
                spiderAnim.SetBool("Skill", false);
                anim.SetBool("Down", false);
                anim.SetBool("GetUp", true);
                yield return new WaitForSeconds(0.5f);
                spiderAnim.SetBool("Run", true);
                isBattleMoveEnemy = 2;
                anim.SetBool("GetUp", false);
                yield return new WaitForSeconds(0.3f);
                spiderAnim.SetBool("Run", false);
                isBattleMoveEnemy = 0;
                yield return new WaitForSeconds(0.5f);
                Select = 2;
                enemychargecounter = false;
                playerTurn = true;
            }
        }
    }
        IEnumerator EnemyCharge()
    {
            if (Select == 0)
            {
                enemyTurn = false;
                enemychargecounter = false;
                Select = 3;
                isWord = 9;
                spiderAnim.SetBool("Def", true);
                soundcharge.PlayOneShot(soundcharge.clip);
            Instantiate(
              charge2, charge2.transform.position, Quaternion.Euler(439.72f, 50f, 209.44f)
            );
            yield return new WaitForSeconds(1.5f);
                spiderAnim.SetBool("Def", false);
                yield return new WaitForSeconds(0.5f);
                soundcharge.Stop();
                enemychargecounter = true;
                Select = 0;
                playerTurn = true;
            } else if (Select == 1)
            {
                enemyTurn = false;
                enemychargecounter = false;
                Select = 3;
                isWord = 9;
                spiderAnim.SetBool("Def", true);
                soundcharge.PlayOneShot(soundcharge.clip);
            Instantiate(
              charge2, charge2.transform.position, Quaternion.Euler(439.72f, 50f, 209.44f)
            );
            yield return new WaitForSeconds(1.5f);
                spiderAnim.SetBool("Def", false);
                yield return new WaitForSeconds(0.5f);
                soundcharge.Stop();
                enemychargecounter = true;
                Select = 1;
                playerTurn = true;
            } else if (Select == 2)
            {
                enemyTurn = false;
                enemychargecounter = false;
                Select = 3;
                isWord = 9;
                spiderAnim.SetBool("Def", true);
                soundcharge.PlayOneShot(soundcharge.clip);
            Instantiate(
              charge2, charge2.transform.position, Quaternion.Euler(439.72f, 50f, 209.44f)
            );
            yield return new WaitForSeconds(1.5f);
                spiderAnim.SetBool("Def", false);
                yield return new WaitForSeconds(0.5f);
                soundcharge.Stop();
                enemychargecounter = true;
                Select = 2;
                playerTurn = true;
            }
        }
    }
