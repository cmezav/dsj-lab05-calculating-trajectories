using UnityEngine;

public class AIShell : MonoBehaviour
{
    public GameObject explosion;

    Rigidbody rb;
    Transform target;
    bool hitRegistered;

    public void Initialize(Transform enemy, Transform shooter)
    {
        target = enemy;

        // Evita colisiones con el tanque que disparó.
        foreach (Collider shellCollider in GetComponentsInChildren<Collider>())
        {
            foreach (Collider tankCollider in
                     shooter.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(shellCollider, tankCollider);
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 10f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hitRegistered || target == null) return;

        Transform collided = collision.collider.transform;

        if (collided != target && !collided.IsChildOf(target))
            return;

        hitRegistered = true;

        Debug.Log($"Hit enemy: {target.name}", gameObject);

        if (explosion != null)
        {
            GameObject effect = Instantiate(
                explosion, transform.position, Quaternion.identity
            );
            Destroy(effect, 0.5f);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.001f)
            transform.forward = rb.linearVelocity.normalized;
    }
}