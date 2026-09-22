using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _damage;
    public float speed = 2.4f;
    public int hp = 3;
    float _attackCooldown;

    void Update()
    {
        var p = GameObject.Find("player");
        if (p == null)
        {
            p = FindObjectOfType<PlayerController>() != null ? FindObjectOfType<PlayerController>().gameObject : null;
        }
        if (p != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, p.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        if (other.gameObject.name == "player" || other.GetComponent<PlayerController>() != null)
        {
            if (Time.time < _attackCooldown) return;
            _attackCooldown = Time.time + 0.4f;
            var g = FindObjectOfType<GameManager>();
            if (g != null) g.hitPlayer(7);
        }

        if (other.gameObject.name == "bullet" || other.gameObject.name.Contains("bullet"))
        {
            hp = hp - 1;
            Destroy(other.gameObject);
            if (hp <= 0)
            {
                // ============================================================
                // DIAGNOSTIKA DEV2-05 — SKÓRE NENAPOJENÉ (zámerne)
                // Po opravenej kolízii (DEV2-03) enemy zomrie, ale score
                // nerastie, kým nezavoláš gm.addScore / napojíš ScoreText.
                // ============================================================
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            if (Time.time < _attackCooldown)
            {
                return;
            }
            _attackCooldown = Time.time + 0.55f;
            GameManager.Instance.hitPlayer(_damage);
        }
    }
}
