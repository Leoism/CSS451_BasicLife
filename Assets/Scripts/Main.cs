using System.Collections;
using System.Collections.Generic;
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
        if (Camera.main == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
          KindGetter kindGetter = hit.transform.gameObject.GetComponent<KindGetter>();
          if (kindGetter == null) return;
          SceneNode attachedSceneNode = kindGetter.nodeType;
          if (attachedSceneNode == null) return;
          Vector3 offset = hit.point - attachedSceneNode.transform.position;
          Debug.Log(hit.point);
          Vector3 direction =
          (Input.mousePosition.x - prevPos.x > 0 ?
            Camera.main.transform.right : -Camera.main.transform.right) +
            (Input.mousePosition.y - prevPos.y > 0 ?
            Camera.main.transform.up : -Camera.main.transform.up);
          direction.Normalize();
                if(Input.GetMouseButton(0)) attachedSceneNode.transform.Translate(offset * 5f * Time.deltaTime);
                if(Input.GetMouseButton(1)) attachedSceneNode.transform.Translate(-offset * 5f * Time.deltaTime);
                prevPos = Input.mousePosition;
        }*/
        
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
                RaycastHit hit;
                if (Camera.main == null) return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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

