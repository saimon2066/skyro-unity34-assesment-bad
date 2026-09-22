using UnityEngine;
using UnityEngine.UI;

// game manager !! dont touch if it works
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject enemyPrefab;
    public GameObject prefab2;
    public float spawnEvery = 1.337f;
    public int score = 0;
    public int HP = 37;
    public bool paused;
    public Text hpTxt;
    public Text scoreTxt;
    public HudStuff hud;
    public float t;
    public int wave = 1;
    public bool gameOver;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        t = spawnEvery;
        HP = 37;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
        }

        var p = GameObject.Find("player");
        var ply = FindObjectOfType<PlayerController>();
        var hpGo = GameObject.Find("HPText");
        var scGo = GameObject.Find("ScoreText");
        hud = FindObjectOfType<HudStuff>();

        if (hpGo != null) hpTxt = hpGo.GetComponent<Text>();
        if (scGo != null) scoreTxt = scGo.GetComponent<Text>();
        if (ply != null) HP = ply.Health;

        if (hpTxt != null) hpTxt.text = "hp " + HP;
        else if (hud != null) hud.upd("hp " + HP);

        // DIAGNOSTIKA DEV2-05 — ScoreText sa tu zámerne neaktualizuje.
        // Po opravenej kolízii + addScore uvidíš zmenu len ak napojíš UI
        // (addScore už Text nastavuje — alebo doplň sem).
        // hint: scoreTxt
        if (hud != null) hud.upd("hp " + HP);

        if (gameOver)
        {
            if (hpTxt != null) hpTxt.text = "dead lol  hp 0";
            return;
        }

        if (HP <= 0)
        {
            gameOver = true;
            Debug.Log("you died lol");
            Time.timeScale = 0.2f;
            return;
        }

        t -= Time.deltaTime;
        if (t <= 0f)
        {
            t = spawnEvery;
            spawn1();
        }

        /*
        // old spawn dont delete i might need it !!!!! 12.3.2024
        void SpawnEnemiesOLD()
        {
            int n = 5;
            for (int i = 0; i < n; i++)
            {
                float x = Random.Range(-8f, 8f);
                float y = Random.Range(-4f, 4f);
                Vector3 pos = new Vector3(x, y, 0);
                // GameObject e = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // e.transform.position = pos;
                if (enemyPrefab != null)
                {
                    GameObject ee = (GameObject)Instantiate(enemyPrefab, pos, Quaternion.identity);
                    ee.name = "GameObject";
                    // ee.GetComponent<eNemy>().speed = 3;
                    // ee.GetComponent<eNemy>().hp = 10;
                }
                else
                {
                    Debug.Log("no prefab lol");
                }
            }
            wave++;
            spawnEvery = spawnEvery * 0.95f;
            if (spawnEvery < 0.2f) spawnEvery = 0.2f;
        }

        // StartCoroutine... InvokeRepeating("spawn1", 1f, 1.337f);
        */
    }

    public void spawn1()
    {
        Vector3 pos = new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0);
        var p = GameObject.Find("player");
        if (p != null && Vector3.Distance(pos, p.transform.position) < 1.5f)
        {
            pos.x += 3f;
        }

        try
        {
            GameObject e = Instantiate(enemyPrefab, pos, Quaternion.identity);
            e.name = "GameObject";
        }
        catch
        {
        }
    }

    public void addScore(int x)
    {
        score = score + x;
        var scGo = GameObject.Find("ScoreText");
        if (scGo != null) scGo.GetComponent<Text>().text = "score:" + score;
    }

    public void hitPlayer(int dmg)
    {
        var ply = FindObjectOfType<PlayerController>();
        if (ply != null)
        {
            ply.Health = ply.Health - dmg;
            HP = ply.Health;
        }
        else HP = HP - dmg;

        var hpGo = GameObject.Find("HPText");
        if (hpGo != null) hpGo.GetComponent<Text>().text = "hp " + HP;

        var hud2 = FindObjectOfType<HudStuff>();
        if (hud2 != null) hud2.upd("hp " + HP);
    }
}
