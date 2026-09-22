using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public float speed = 5.5f;
    public int hp = 37;
    public GameObject prefab;
    public float fireWait = 0.18f;
    float lastShot;
    float lastDir = 1f;
    public HudStuff hud;

    void Start()
    {
        DontDestroyOnLoad(this);
        hp = 37;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0.01f) lastDir = Mathf.Sign(h);

        transform.position += new Vector3(h, v, 0) * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            if (hp <= 0) return;
            if (Time.time > lastShot + fireWait)
            {
                lastShot = Time.time;
                shoot();
            }
        }

        // also write hud from here because gm is laggy sometimes??
        var hpGo = GameObject.Find("HPText");
        if (hpGo != null)
        {
            hpGo.GetComponent<Text>().text = "hp " + hp;
        }
        hud = FindObjectOfType<HudStuff>();
        if (hud != null)
        {
            hud.upd("hp " + hp);
        }

        var g = FindObjectOfType<gm>();
        if (g != null)
        {
            g.HP = hp;
        }
    }

    void shoot()
    {
        try
        {
            var b = Instantiate(prefab, transform.position, Quaternion.identity);
            b.transform.parent = null;
        }
        catch
        {
            GameObject b = new GameObject("bullet");
            b.transform.position = transform.position;
            b.transform.parent = null;
            var sr = b.AddComponent<SpriteRenderer>();
            var my = GetComponent<SpriteRenderer>();
            if (my != null) sr.sprite = my.sprite;
            sr.color = new Color(1f, 1f, 0.2f, 1f);
            sr.sortingOrder = 10;
            var rb = b.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(lastDir * 12f, 0f);
            var col = b.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.12f;
            Destroy(b, 1.6f);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.GetComponent<eNemy>() != null)
        {
            hp = hp - 4;
            var g = GameObject.FindObjectOfType<gm>();
            if (g != null) g.hitPlayer(0);
            var hpGo = GameObject.Find("HPText");
            if (hpGo != null) hpGo.GetComponent<Text>().text = "hp " + hp;
        }
    }
}
