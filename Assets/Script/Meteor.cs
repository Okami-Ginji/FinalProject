using UnityEngine;

public class Meteor : MonoBehaviour
{
    private GameObject target;
    private Rigidbody2D rb;
    public float speed = 10f;

    public void SetTarget(GameObject enemy)
    {
        target = enemy;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                Explode();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        if (target != null && target.CompareTag("Enemy"))
        {
            target.GetComponent<EnemyAI>().ChangeHealth(-50, transform.position);
            target.GetComponent<EnemyAISeries>().ChangeHealth(-50, transform.position);
        }

        Destroy(gameObject);
    }

}
