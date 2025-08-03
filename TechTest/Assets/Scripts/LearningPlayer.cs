using System;
using System.Collections;
using Mirror;
using UnityEngine;

public class LearningPlayer : NetworkBehaviour
{
   private IEnumerator Start()
   {
      WaitForSeconds wait = new WaitForSeconds(5f);
      while (true)
      {
         // ejecutar algo
         bool isFinished = false;
         if (isFinished)
         {
            yield break;
         }
         yield return wait;
      }
   }

   override public void OnStartLocalPlayer()
   {
      var dialogueLearning = FindFirstObjectByType<Learning>();
      StartCoroutine(dialogueLearning.DialogoCoroutine());
   }

}
