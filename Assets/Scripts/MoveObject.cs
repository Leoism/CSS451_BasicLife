using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
  public bool isSelected = false;

  // Update is called once per frame
  void Update()
  {
    if (!isSelected) return;
    Move();
  }

  void Move()
  {
    if (Input.GetKey(KeyCode.A))
    {
      transform.localPosition += -transform.right.normalized * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.W))
    {
      transform.localPosition += transform.forward.normalized * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.S))
    {
      transform.localPosition += -transform.forward.normalized * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.D))
    {
      transform.localPosition += transform.right.normalized * Time.deltaTime;
    }
  }
}
