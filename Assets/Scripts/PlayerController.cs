using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5.5f;
    public int Health = 37;
    [SerializeField] private GameObject _bulletPrefab;
    public float fireWait = 0.18f;
    float lastShot;
    float lastDir = 1f;
    public HudStuff hud;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // void Start()
    // {
    //     DontDestroyOnLoad(this);
    // }

    void Update()
    {
        // ============================================================
        // DIAGNOSTIKA DEV2-02 — POHYB CHÝBA (zámerne)
        // Doplň: Horizontal / Vertical (Input Manager OK na tento task)
        // alebo Input System. Posuň transform. Pozri README.
        // ============================================================
        /*

        */

        // streľba ostáva — overíš, že Play beží, aj keď sa ešte nehýbeš
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > lastShot + fireWait)
            {
                lastShot = Time.time;
                Shoot();
            }
        }

        // also write hud from here because gm is laggy sometimes??
        var hpGo = GameObject.Find("HPText");
        if (hpGo != null)
        {
            hpGo.GetComponent<Text>().text = "hp " + Health;
        }
        hud = FindObjectOfType<HudStuff>();
        if (hud != null)
        {
            hud.upd("hp " + Health);
        }

        var g = FindObjectOfType<GameManager>();
        if (g != null)
        {
            g.HP = Health;
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rigidbody.linearVelocity = moveVector.normalized * _speed;
    }

    void Shoot()
    {
        try
        {
            GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.parent = null;
            
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

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.GetComponent<EnemyController>() != null || collision2D.gameObject.GetComponent<eNemy2>() != null)
        {
            Health = Health - 4;
            var g = GameObject.FindObjectOfType<GameManager>();
            if (g != null) g.hitPlayer(0);
            var hpGo = GameObject.Find("HPText");
            if (hpGo != null) hpGo.GetComponent<Text>().text = "hp " + Health;
        }
    }
}
