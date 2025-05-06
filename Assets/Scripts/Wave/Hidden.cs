using UnityEngine;

using System.Collections;
using static UnityEngine.Rendering.DebugUI.Table;

public class Hidden : MonoBehaviour

{

    public SpriteRenderer sprite;
    private Coroutine fadeCoroutine; // Store the fade coroutine



    private void Start()

    {

        sprite.enabled = false;

    }



    public void Reveal()

    {

        print("Revealed!");



        // Stop any previous fade coroutine before starting a new one

        if (fadeCoroutine != null)

        {

            StopCoroutine(fadeCoroutine);

        }



        sprite.enabled = true;



        // Start a new fade coroutine

        fadeCoroutine = StartCoroutine(FadeOut());

    }

    IEnumerator FadeOut()

    {

        Color color = sprite.color;

        float fadeDuration = 3f; // Duration of the fade

        float elapsedTime = 0f;



        while (elapsedTime < fadeDuration)

        {

            elapsedTime += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            sprite.color = color;

            yield return null;

        }
        sprite.enabled = false;
    }



}
