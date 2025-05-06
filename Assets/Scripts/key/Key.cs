using UnityEngine;

public class Key : MonoBehaviour
{
    private bool isFollowing = false;
    private Transform player;

    void Update()
    {
        if (isFollowing && player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.position + new Vector3(0.5f, 0, 0), Time.deltaTime * 5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Key collected!");
            isFollowing = true;
            player = collision.transform;
            transform.SetParent(player); // Makes the key a child of the player
        }
    }
}
