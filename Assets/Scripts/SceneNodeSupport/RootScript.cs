using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RootScript : MonoBehaviour
{

  public SceneNode root;
  void Start()
  {
    Debug.Assert(root != null);
  }
  void Update()
  {
    Matrix4x4 i = Matrix4x4.identity;
    root.CompositeXform(ref i);
  }
}
