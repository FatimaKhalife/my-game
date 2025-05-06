using UnityEngine;

public class SyncTriggerWithParent : MonoBehaviour
{
    public Collider2D childCollider;
    public Collider2D parentCollider;

    void Start()
    {
        childCollider = GetComponent<Collider2D>();
        if (transform.parent != null)
        {
            parentCollider = transform.parent.GetComponent<Collider2D>();
        }
    }

    void Update()
    {
        if (parentCollider != null && childCollider != null)
        {
            childCollider.isTrigger = parentCollider.isTrigger;
        }
    }
}
