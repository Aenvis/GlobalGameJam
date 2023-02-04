using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected string enemyName;
    protected int life;
    protected Item drop;

    protected virtual void Start()
    {
        FillItem();
    }
    protected abstract void FillItem();

    public void FixedUpdate()
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
        InterfaceInventory.Instance.AddItem(drop);
    }

}

