using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
  public Light lightSource = null;
  public bool isDecrease = true;
  // Start is called before the first frame update
  void Start()
  {
    Debug.Assert(lightSource != null);
  }

  // Update is called once per frame
  void Update()
  {
    DoLighting();
    MouseControl();
  }

  void DoLighting()
  {
    if (lightSource.color.r < 0.4f)
      isDecrease = false;
    else if (lightSource.color.r > 0.98f)
      isDecrease = true;
    if (Time.frameCount % 300 == 0)
      lightSource.color *= isDecrease ? 0.95f : 1.05f;

  }

  private Vector3 track = Vector3.zero;
  private Vector3 mouseDown = Vector3.zero;
  private Vector3 prevPos;
  private int fingerID = -1;

  KindGetter kindGetter = null;
  SceneNode attachedNode = null;

  void MouseControl()
  {
    /*if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1)) return;
        if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
        RaycastHit hit;
    if (selectedCam == null) return;
    Ray ray = selectedCam.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit))
    {
      KindGetter kindGetter = hit.transform.gameObject.GetComponent<KindGetter>();
      if (kindGetter == null) return;
      SceneNode attachedSceneNode = kindGetter.nodeType;
      if (attachedSceneNode == null) return;
      Vector3 offset = hit.point - attachedSceneNode.transform.position;
      Vector3 direction =
      (Input.mousePosition.x - prevPos.x > 0 ?
        selectedCam.transform.right : -selectedCam.transform.right) +
        (Input.mousePosition.y - prevPos.y > 0 ?
        selectedCam.transform.up : -selectedCam.transform.up);
      direction.Normalize();
            if(Input.GetMouseButton(0)) attachedSceneNode.transform.Translate(offset * 5f * Time.deltaTime);
            if(Input.GetMouseButton(1)) attachedSceneNode.transform.Translate(-offset * 5f * Time.deltaTime);
            prevPos = Input.mousePosition;
    }*/

    if (!Input.GetKey(KeyCode.LeftAlt))
    {
      Camera selectedCam = null;
      foreach (Camera cam in Camera.allCameras)
      {
        if (cam != null && !cam.name.Equals("SelectMiniCamera")) selectedCam = cam;
      }
      if (Input.GetMouseButtonDown(0))
      {
        if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
        RaycastHit hit;

        if (selectedCam == null) return;
        Ray ray = selectedCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
          kindGetter = hit.transform.gameObject.GetComponent<KindGetter>();
          if (kindGetter == null) return;
          attachedNode = kindGetter.nodeType;
          if (attachedNode == null) return;

          mouseDown = Input.mousePosition;
          track = Vector3.zero;
        }
      }
      if (Input.GetMouseButton(0))
      {
        if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
        track = Input.mousePosition - mouseDown;
        mouseDown = Input.mousePosition;

        if (attachedNode == null) return;
        track *= selectedCam.transform.right.x >= 0 ? 1 : -1;
        track.y = selectedCam.transform.right.x >= 0 ? track.y : track.y * -1;
        if (-0.4f < selectedCam.transform.right.x && selectedCam.transform.right.x < 0.4)
        {
          track.z = selectedCam.transform.right.z < 0 ? track.x * -1 : track.x;
          track.z *= selectedCam.transform.right.x > 0 ? 1 : -1;
          track.x = 0;
        }
        attachedNode.transform.Translate(track * Time.deltaTime, Space.World);
        //I want to move based on camera position
      }
      if (Input.GetMouseButtonUp(0))
      {
        attachedNode = null;
        kindGetter = null;
      }
    }
  }
}

