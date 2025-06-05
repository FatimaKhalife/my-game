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

        if (childCollider == null) Debug.LogWarning("Child collider not found on " + gameObject.name);
        if (parentCollider == null) Debug.LogWarning("Parent collider not found on " + transform.parent.name);
    }

    void Update()
    {
        print(parentCollider.isTrigger);
        if (parentCollider != null && childCollider != null)
        {
            childCollider.isTrigger = parentCollider.isTrigger;
           
            print("what");
        }
    }
}
