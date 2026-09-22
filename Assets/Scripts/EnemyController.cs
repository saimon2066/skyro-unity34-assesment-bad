using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed = 2.4f;
    private int _health = 3;
    private float _attackCooldown;

    void Update()
    {
        var p = GameObject.Find("player");
        if (p == null)
        {
            p = FindObjectOfType<PlayerController>() != null ? FindObjectOfType<PlayerController>().gameObject : null;
        }
        if (p != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, p.transform.position, _speed * Time.deltaTime);
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
            if (g != null) g.DamagePlayer(7);
        }

        if (other.gameObject.name == "bullet" || other.gameObject.name.Contains("bullet"))
        {
            _health = _health - 1;
            Destroy(other.gameObject);
            if (_health <= 0)
            {
                // ============================================================
                // DIAGNOSTIKA DEV2-05 — SKÓRE NENAPOJENÉ (zámerne)
                // Po opravenej kolízii (DEV2-03) enemy zomrie, ale score
                // nerastie, kým nezavoláš gm.addScore / napojíš ScoreText.
                // ============================================================
                GameManager.Instance.AddScore(10);
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
            GameManager.Instance.DamagePlayer(_damage);
        }
    }
}
