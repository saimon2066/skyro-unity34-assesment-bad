using UnityEngine;

public class eNemy2 : MonoBehaviour
{
    public float speed = 1.1f;
    public int hp = 8;
    float hitCd;

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
            if (Time.time < hitCd) return;
            hitCd = Time.time + 0.4f;
            var g = FindObjectOfType<GameManager>();
            if (g != null) g.hitPlayer(7);
        }

        if (other.gameObject.name == "bullet" || other.gameObject.name.Contains("bullet"))
        {
            hp = hp - 1;
            Destroy(other.gameObject);
            if (hp <= 0)
            {
                // DEV2-05 — skóre zámerne odpojené (rovnako ako eNemy)
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.GetComponent<PlayerController>() != null)
        {
            if (Time.time < hitCd) return;
            hitCd = Time.time + 0.55f;
            var g = FindObjectOfType<GameManager>();
            if (g != null) g.hitPlayer(3);
        }
    }
}
