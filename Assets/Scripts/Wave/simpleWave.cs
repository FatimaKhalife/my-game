using UnityEngine;
using System.Collections;

public class SimpleWave : MonoBehaviour
{
    public float expansionSpeed = 5f;
    public float fadeSpeed = 2f;
    private Vector3 initialScale;
    private SpriteRenderer spriteRenderer;
    public PlayerMovement playerMovement;
    private CircleCollider2D circleCollider; // Reference to the collider

    void Start()
    {
        initialScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>(); // Get the collider component
        gameObject.SetActive(false);
    }

    public void ActivateWave(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.localScale = initialScale;
        circleCollider.radius = initialScale.x / 2f; // Reset collider size
        StartCoroutine(ExpandAndFade());
    }

    private IEnumerator ExpandAndFade()
    {
        float progress = 0f;
        Color color = spriteRenderer.color;

        while (progress < 1f)
        {
            progress += Time.deltaTime * fadeSpeed;

            // Expand the wave
            transform.localScale += Vector3.one * expansionSpeed * Time.deltaTime;

            // Scale the collider to match the wave size
            circleCollider.radius = (transform.localScale.x) * 5f;

            // Fade the wave
            color.a = Mathf.Lerp(1f, 0f, progress);
            spriteRenderer.color = color;

            yield return null;
        }

        gameObject.SetActive(false);
        playerMovement.Done();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hidden"))
        {
            print("yes");
            other.GetComponent<Hidden>().Reveal();
        }
    }
    void OnDrawGizmos()
    {
        if (circleCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

}
