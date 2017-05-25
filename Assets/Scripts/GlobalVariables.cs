using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {

   public int gold;

   void Awake()
   {
      DontDestroyOnLoad(transform.gameObject);
   }
}
