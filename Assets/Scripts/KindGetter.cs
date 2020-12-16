using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KindGetter : MonoBehaviour
{
  public SceneNode nodeType = null;
  void Start()
  {
    Debug.Assert(nodeType != null);
  }
}
