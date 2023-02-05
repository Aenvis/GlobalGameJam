using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int hitPoints;
    [SerializeField] private string tagToGetHit;
    [SerializeField] private UnityEvent deathEffect;

    private void Update()
    {
        if (hitPoints <= 0) deathEffect?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag(tagToGetHit)) return;
        else hitPoints--;
    }
}
