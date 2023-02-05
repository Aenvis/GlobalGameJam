using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;

public class InterfaceInventory : MonoBehaviour
{
    private bool showInventory = false;
    private Inventory inventory;
    [SerializeField] List<Texture2D> itemTextures;
    public static InterfaceInventory Instance;
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
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

    public void AddItem(Item item)
    {
        inventory.AddItem(item);
    }

    
    private void OnGUI()
    {
        if (showInventory)
        {   
            Debug.Log("OnGUI");

            float x = 70;
            float y = Screen.height / 4f;
            float padding = 5;
            float size = 50;
            
            GUI.backgroundColor = Color.yellow;
            
            bool isButtonCliced = GUI.Button(new Rect(x, y, 100f, 30f), "Button");
            if (isButtonCliced == true)
            {
                Debug.Log("IsButtonCliced");
            }
            

            for (int i = 0; i < inventory.items.Count; i++)
            {
                GUI.DrawTexture(new Rect(x, y, size, size), itemTextures[i]);
                x += size + padding;
                
            }
            
        }
    }
}

