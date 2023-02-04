using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    private bool showInventory = false;
    private Inventory inventory;
    private List<Texture2D> itemTextures;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        itemTextures = new List<Texture2D>();

        // Load item textures into the itemTextures list
        // ...
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            showInventory = !showInventory;
        }
    }

    private void OnGUI()
    {
        if (showInventory)
        {
            int x = 10;
            int y = 10;
            int padding = 5;
            int size = 50;

            for (int i = 0; i < inventory.items.Count; i++)
            {
                GUI.DrawTexture(new Rect(x, y, size, size), itemTextures[i]);
                x += size + padding;
            }
        }
    }
}

