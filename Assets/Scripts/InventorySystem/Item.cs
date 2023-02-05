using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item
{
    public string Name { get; private set; }
    public int Value { get; private set; }
    public int Quantity { get; private set; }
    
    public Texture2D Texture { get; private set; }

    public Item(string name, int value, int quantity, Texture2D texture)
    {
        Name = name;
        Value = value;
        Quantity = quantity;
        Texture = texture;
    }
}
