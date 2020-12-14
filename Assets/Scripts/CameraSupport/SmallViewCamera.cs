using UnityEngine;
using System.Collections;

public class SmallViewCamera : MonoBehaviour
{
  public Transform selectedNode = null;
  private void OnPreCull()
  {
    Camera cam = gameObject.GetComponent<Camera>();
    cam.cullingMatrix = Matrix4x4.Ortho(-99999, 99999, -99999, 99999, 0.001f, 99999) *
                        Matrix4x4.Translate(Vector3.forward * -99999 / 2f) *
                        cam.worldToCameraMatrix;
  }

  void OnPreRender()
  {
    // Debug.Log("OnPreRender:" + name);
    Matrix4x4 r = Matrix4x4.identity;
    // Set the rows, for rotation matrices, this is identical to setColumn followed by inverse
    r.SetRow(0, transform.right);
    r.SetRow(1, transform.up);
    r.SetRow(2, -transform.forward);

    Matrix4x4 t = Matrix4x4.TRS(-transform.position, Quaternion.identity, Vector3.one);

    Matrix4x4 u = r * t;
    Shader.SetGlobalMatrix("CameraViewMatrix", u);
  }

  void Start()
  {
  }

  void Update()
  {
    transform.forward = -selectedNode.forward;
    RePositionCamera();
  }

  void RePositionCamera()
  {
    transform.localPosition = selectedNode.position + selectedNode.forward * 2;
  }
}