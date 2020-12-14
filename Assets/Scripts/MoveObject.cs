using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
  public bool isSelected = false;
  private float speed = 5f;
  public Transform PairCamera = null;
  public Transform ColliderTransform = null;
  void Start()
  {
    Debug.Assert(PairCamera != null);
    Debug.Assert(ColliderTransform != null);
  }
  // Update is called once per frame
  void Update()
  {
    if (!isSelected) return;
    Move();
    ColliderTransform.localPosition = transform.localPosition;
  }

  void Move()
  {
    if (Input.GetKey(KeyCode.A))
    {
      Vector3 newPos = -transform.right.normalized * Time.deltaTime * speed;
      transform.localPosition += newPos;
      PairCamera.localPosition += newPos;
    }
    if (Input.GetKey(KeyCode.W))
    {
      Vector3 newPos = transform.forward.normalized * Time.deltaTime * speed;
      transform.localPosition += newPos;
      PairCamera.localPosition += newPos;
    }
    if (Input.GetKey(KeyCode.S))
    {
      Vector3 newPos = -transform.forward.normalized * Time.deltaTime * speed;
      transform.localPosition += newPos;
      PairCamera.localPosition += newPos;
    }
    if (Input.GetKey(KeyCode.D))
    {
      Vector3 newPos = transform.right.normalized * Time.deltaTime * speed;
      transform.localPosition += newPos;
      PairCamera.localPosition += newPos;
    }
  }
}
