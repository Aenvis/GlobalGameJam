using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class WarriorMushroom : Enemy
{
    public WarriorMushroom(string name, int life) : base(name, life)
    {
        name = "Warrior Mushroom";
        life = 20;
        itemDrops.AddItem(new Item("Grzybnia", 10, 1));
    }

    public override void Move()
    {
        // Code to move the warrior left or right by 50 units
    }

    public override void Die()
    {
        base.Die();
        // Add code to play death animation or sound
    }
}

