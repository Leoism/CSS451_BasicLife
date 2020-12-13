using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KindGetter : MonoBehaviour
{
  public PetScript kindObj = null;
  void Start()
  {
    Debug.Assert(kindObj != null);
  }
}
