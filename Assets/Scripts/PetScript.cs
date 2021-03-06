﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetScript : MonoBehaviour
{
  NodePrimitive Myself = null;
  public bool IsPet;

  public Vector3 minSize = Vector3.one * 2f;

  void Awake()
  {
    Myself = GetComponent<NodePrimitive>();
  }
  // Start is called before the first frame update
  void Start()
  {
    if (IsPet) transform.localScale = minSize;

        RenderSelf();
    SetPos();
  }

  // Update is called once per frame
  void Update()
  {
        RenderSelf();
    GrowthDecay();
    SetPos();
  }

    void RenderSelf()
    {
        Matrix4x4 i = Matrix4x4.identity;
        if (Myself != null) Myself.LoadShaderMatrix(ref i);
    }
  void SetPos()
  {
    Vector3 pos = new Vector3(transform.position.x, 0f, transform.position.z);
    if (IsPet) pos += new Vector3(0f, 0.5f * transform.localScale.y, 0f);
    else pos += new Vector3(0f, transform.localScale.y + .01f, 0f);
    transform.position = pos;
  }

  public void Grow()
  {
    if (!IsPet) return;
    transform.localScale *= 1.1f;
  }

  void GrowthDecay()
  {
    if (!IsPet) return;
    if (Time.frameCount % 300 != 0) return;
    Vector3 newSize = transform.localScale * .99f;
    if (newSize.x < minSize.x) return;
    transform.localScale = newSize;

  }
}
