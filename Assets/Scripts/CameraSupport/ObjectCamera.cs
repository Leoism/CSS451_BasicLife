using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectCamera : MonoBehaviour
{
  public Transform LookPoint = null;
  Camera TheCamera = null;
  const float TUMBLE_LOCK = 75f;
  const float ZOOM_MIN = 5f;
  private int fingerID = -1;
  Vector3 dTrack = Vector3.zero;
  Vector3 dTumble = Vector3.zero;
  Vector3 mouseDown = Vector3.zero;
  public Dropdown cameraMenu = null;
  public List<Camera> cameras = null;
  public List<Transform> lookAts = null;
  void Awake()
  {
    TheCamera = transform.gameObject.GetComponent<Camera>();
    Debug.Assert(cameras != null);
    TheCamera = cameras[0];
  }
  // Start is called before the first frame update
  void Start()
  {
    Debug.Assert(cameraMenu != null);
    Debug.Assert(lookAts != null);
    LookPoint = lookAts[0];
    cameraMenu.onValueChanged.AddListener(OnCameraMenuChange);
  }

  // Update is called once per frame
  void Update()
  {
    ProcessMouseEvents();
    SetCameraRotation();
  }

  void OnCameraMenuChange(int val)
  {
    MoveObject currMoveObj = LookPoint.gameObject.GetComponent<MoveObject>();
    if (currMoveObj != null)
    {
      currMoveObj.isSelected = false;
    }
    LookPoint = lookAts[val];
    cameras[val].gameObject.SetActive(true);
    TheCamera.gameObject.SetActive(false);
    TheCamera = cameras[val];
    if (val == 0) return;
    LookPoint.gameObject.GetComponent<MoveObject>().isSelected = true;
  }

  #region MOVE_CAMERA

  void ProcessMouseEvents()
  {
    if (Input.GetKey(KeyCode.LeftAlt))
    {
      ChangeFOV(Input.GetAxis("Mouse ScrollWheel"));
      if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
      {
        if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
        mouseDown = Input.mousePosition;

        dTumble = Vector3.zero;
        dTrack = Vector3.zero;
      }
      if (Input.GetMouseButton(0))
      {
        if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
        dTumble = mouseDown - Input.mousePosition;
        dTrack = mouseDown - Input.mousePosition;
        mouseDown = Input.mousePosition;
        ProcessTumble(dTumble);
      }
    }
  }

  void ChangeFOV(float dFOV)
  {
    Vector3 v = LookPoint.localPosition - TheCamera.transform.localPosition;
    float dist = v.magnitude;
    dist -= dFOV * 2f;
    if (dist < ZOOM_MIN && dFOV > 0)
    {
      return;
    }
    TheCamera.transform.localPosition = LookPoint.localPosition - dist * v.normalized;
  }

  void ProcessTumble(Vector3 delta)
  {
    Quaternion q1 = Quaternion.AngleAxis(delta.y, TheCamera.transform.right);
    Quaternion q2 = Quaternion.AngleAxis(-delta.x, TheCamera.transform.up);

    Matrix4x4 r = Matrix4x4.Rotate(q2);
    Matrix4x4 invP = Matrix4x4.TRS(-LookPoint.localPosition, Quaternion.identity, Vector3.one);
    r = invP.inverse * r * invP;
    Vector3 newPos = r.MultiplyPoint(TheCamera.transform.localPosition);
    TheCamera.transform.localPosition = newPos;

    //DO NOT DO IF ANGLE BETWEEN AXISFRAME XZ PLANE AND LOOK DIRECTION > TUMBLE_LOCK
    Vector3 curPos = LookPoint.localPosition - TheCamera.transform.localPosition;
    if (Mathf.Abs(Vector3.Dot(curPos.normalized, LookPoint.up)) < .9848f)
    {
      r = Matrix4x4.Rotate(q1);
      invP = Matrix4x4.TRS(-LookPoint.localPosition, Quaternion.identity, Vector3.one);
      r = invP.inverse * r * invP;
      newPos = r.MultiplyPoint(TheCamera.transform.localPosition);
      TheCamera.transform.localPosition = newPos;
    }
  }

  void SetCameraRotation()
  {
    Vector3 V = LookPoint.localPosition - TheCamera.transform.localPosition;
    TheCamera.transform.up = Vector3.up;
    TheCamera.transform.forward = V;
    LookPoint.rotation = new Quaternion(
        LookPoint.rotation.x,
        TheCamera.transform.rotation.y,
        LookPoint.rotation.z,
        LookPoint.rotation.w
    );
  }

  #endregion
}
