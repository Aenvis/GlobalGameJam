using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
    protected string name;
    protected int life;
    protected Inventory itemDrops;

    public Enemy(string name, int life)
    {
        this.name = name;
        this.life = life;
        itemDrops = new Inventory();
    }

    public abstract void Move();

    public  void FixedUpdate()
    {
        if (life <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        DropItems();
    }

    public void DropItems()
    {
        // Code to drop items when the enemy dies
    }

    public Inventory GetItemDrops()
    {
        return itemDrops;
    }
}

