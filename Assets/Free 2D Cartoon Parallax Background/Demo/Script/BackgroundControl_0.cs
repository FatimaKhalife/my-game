using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControl_0 : MonoBehaviour
{
    [Header("BackgroundNum 0 -> 3")]
    public int backgroundNum;
    public Sprite[] Layer_Sprites;
    private GameObject[] Layer_Object = new GameObject[5];
    private int max_backgroundNum = 3;

    void Start()
    {
        // Find the Layer Objects in the scene
        for (int i = 0; i < Layer_Object.Length; i++)
        {
            Layer_Object[i] = GameObject.Find("Layer_" + i);
            if (Layer_Object[i] == null)
            {
                Debug.LogError("Layer_" + i + " not found in the scene! Check object names.");
            }
        }

        // Validate sprites
        if (Layer_Sprites == null || Layer_Sprites.Length < (backgroundNum * 5) + Layer_Object.Length)
        {
            Debug.LogError("Layer_Sprites array is not assigned or too short! Ensure it contains enough sprites.");
        }

        ChangeSprite();
    }

    void Update()
    {
        // Change background using arrow keys (for testing)
        if (Input.GetKeyDown(KeyCode.RightArrow)) NextBG();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) BackBG();
    }

    void ChangeSprite()
    {
        if (Layer_Object[0] == null || Layer_Sprites.Length <= backgroundNum * 5)
        {
            Debug.LogError("Cannot change sprite: Layer_Object[0] is null or Layer_Sprites index is out of bounds.");
            return;
        }

        // Change Layer_0 sprite
        SpriteRenderer sr0 = Layer_Object[0].GetComponent<SpriteRenderer>();
        if (sr0 != null)
        {
            sr0.sprite = Layer_Sprites[backgroundNum * 5];
        }
        else
        {
            Debug.LogError("Layer_0 is missing a SpriteRenderer component!");
        }

        // Change sprites for Layer_1 to Layer_4
        for (int i = 1; i < Layer_Object.Length; i++)
        {
            if (Layer_Object[i] == null || Layer_Sprites.Length <= (backgroundNum * 5) + i)
            {
                Debug.LogError("Skipping Layer_" + i + " due to missing object or sprite.");
                continue;
            }

            Sprite changeSprite = Layer_Sprites[backgroundNum * 5 + i];
            SpriteRenderer sr = Layer_Object[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = changeSprite;
            }
            else
            {
                Debug.LogError("Layer_" + i + " is missing a SpriteRenderer component!");
            }

            // Change sprites for child objects (if they exist)
            for (int j = 0; j < 2; j++)  // Assuming each Layer has 2 children
            {
                if (Layer_Object[i].transform.childCount > j)
                {
                    SpriteRenderer childSR = Layer_Object[i].transform.GetChild(j).GetComponent<SpriteRenderer>();
                    if (childSR != null)
                    {
                        childSR.sprite = changeSprite;
                    }
                    else
                    {
                        Debug.LogError("Child " + j + " of Layer_" + i + " is missing a SpriteRenderer!");
                    }
                }
            }
        }
    }

    public void NextBG()
    {
        backgroundNum = (backgroundNum + 1) % (max_backgroundNum + 1);
        ChangeSprite();
    }

    public void BackBG()
    {
        backgroundNum = (backgroundNum - 1 + (max_backgroundNum + 1)) % (max_backgroundNum + 1);
        ChangeSprite();
    }
}
