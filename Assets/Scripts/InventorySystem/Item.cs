using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item
{
    public string Name { get; private set; }
    public int Value { get; private set; }
    public int Quantity { get; private set; }

    public Item(string name, int value, int quantity)
    {
        Name = name;
        Value = value;
        Quantity = quantity;
    }
}
