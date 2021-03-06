﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitive : MonoBehaviour
{
  public Color MyColor = new Color(1f, 1f, 1f, 1.0f);
  public Vector3 Pivot;
  public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix)
  {
    Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
    Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
    Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
    Matrix4x4 m = nodeMatrix * p * trs * invp;
    GetComponent<Renderer>().material.SetMatrix("MyXformMat", m);
    GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
  }

  public Vector3 GetPosition()
  {
    return transform.localPosition;
  }
}