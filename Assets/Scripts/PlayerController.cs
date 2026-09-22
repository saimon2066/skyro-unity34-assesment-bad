using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public int Health = 50;
    
    [SerializeField] private float _speed = 5.5f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootCooldown = 0.2f;
    float lastShot;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > lastShot + _shootCooldown)
            {
                lastShot = Time.time;
                Shoot();
            }
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
            rb.linearVelocity = new Vector2(12f, 0f);
            var col = b.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.12f;
            Destroy(b, 1.6f);
        }
    }

    // void OnCollisionEnter2D(Collision2D collision2D)
    // {
    //     if (collision2D.gameObject.GetComponent<EnemyController>() != null)
    //     {
    //         Health = Health - 4;
    //         var g = GameObject.FindObjectOfType<GameManager>();
    //         if (g != null) g.DamagePlayer(0);
    //         var hpGo = GameObject.Find("HPText");
    //         if (hpGo != null) hpGo.GetComponent<Text>().text = "hp " + Health;
    //     }
    // }
}
