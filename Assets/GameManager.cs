using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public int enemyCount;

   private void Start()
   {
      enemyCount = FindObjectsOfType<HorizontalEnemyMovement>().Length;
   }

   private void Update()
   {
      if (enemyCount <= 0)
         SceneManager.LoadScene("Menu");
   }

   public void KillEnemy()
   {
      enemyCount--;
   }
}
