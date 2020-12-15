using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour
{

  protected Matrix4x4 mCombinedParentXform;
  public Vector3 NodeOrigin = Vector3.zero;
  public List<NodePrimitive> PrimitiveList = new List<NodePrimitive>();
  public Transform AxisFrame = null;
  public Transform SceneCollider = null;
  public Transform SmallCamera = null;
  public Vector3 kDefaultTreeTip = new Vector3(0.19f, 12.69f, 3.88f);
  public Vector3 CamPosTip = Vector3.zero;
  public bool isLeaf = false;
  public bool isRoot = false;
  public NodePrimitive rootNP = null;
  public Transform PetLocation = null;
  public Transform HumanLocation = null;
  public SceneNodeControl sceneNodeControl = null;
  private int childrenCount = 0;
  // Use this for initialization
  protected void Start()
  {
    Debug.Assert(SceneCollider != null);
    InitializeSceneNode();
  }

  // Update is called once per frame
  void Update()
  {
    SceneCollider.transform.position = transform.position;
    childrenCount = transform.childCount;
    isLeaf = childrenCount == 0;
    isRoot = transform.parent == null;
    if (!isLeaf || isRoot) return;
    float petDist = (PetLocation.localPosition - transform.position).magnitude;
    petDist -= PetLocation.localScale.x / 2;
    float humanDist = (HumanLocation.localPosition - transform.position).magnitude;
    if (Mathf.Abs(petDist) < 1.75f || Mathf.Abs(humanDist) < 1.75f)
    {
      PetScript theThing = Mathf.Abs(petDist) < 1.75f ? PetLocation.gameObject.GetComponent<PetScript>() :
        HumanLocation.gameObject.GetComponent<PetScript>();
      foreach (NodePrimitive np in PrimitiveList)
      {
        Destroy(np.gameObject);
      }
      theThing.Grow();
      sceneNodeControl.runUpdate = true;
      // Go back to looking at banana
      sceneNodeControl.SelectionChange(0);
      Destroy(SceneCollider.gameObject);
      Destroy(gameObject);
    }
  }

  private void InitializeSceneNode()
  {
    mCombinedParentXform = Matrix4x4.identity;
  }

  // This must be called _BEFORE_ each draw!! 
  public Transform GetCollider()
  {
    return SceneCollider;
  }
  public void CompositeXform(ref Matrix4x4 parentXform)
  {
    Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
    Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

    mCombinedParentXform = parentXform * orgT * trs;

    // propagate to all children
    foreach (Transform child in transform)
    {
      SceneNode cn = child.GetComponent<SceneNode>();
      if (cn != null)
      {
        cn.CompositeXform(ref mCombinedParentXform);
      }
    }

    // disenminate to primitives
    foreach (NodePrimitive p in PrimitiveList)
    {
      p.LoadShaderMatrix(ref mCombinedParentXform);
    }

    // Compute AxisFrame 
    if (AxisFrame != null)
    {
      AxisFrame.localPosition = mCombinedParentXform.MultiplyPoint(kDefaultTreeTip);

      // What is going on in the next two lines of code?
      Vector3 up = mCombinedParentXform.GetColumn(1).normalized;
      Vector3 forward = mCombinedParentXform.GetColumn(2).normalized;

      // First align up direction, remember that the default AxisFrame.up is simply the y-axis
      float angle = Mathf.Acos(Vector3.Dot(Vector3.up, up)) * Mathf.Rad2Deg;
      Vector3 axis = Vector3.Cross(Vector3.up, up);
      AxisFrame.localRotation = Quaternion.AngleAxis(angle, axis);

      // Now, align the forward axis
      angle = Mathf.Acos(Vector3.Dot(AxisFrame.transform.forward, forward)) * Mathf.Rad2Deg;
      axis = Vector3.Cross(AxisFrame.transform.forward, forward);
      AxisFrame.localRotation = Quaternion.AngleAxis(angle, axis) * AxisFrame.localRotation;
    }

    if (SmallCamera != null)
    {
      SmallCamera.localPosition = mCombinedParentXform.MultiplyPoint(CamPosTip);
      // What is going on in the next two lines of code?
      Vector3 up = mCombinedParentXform.GetColumn(2).normalized;
      Vector3 forward = mCombinedParentXform.GetColumn(1).normalized;

      // First align up direction, remember that the default SmallCamera.up is simply the y-axis
      float angle = Mathf.Acos(Vector3.Dot(Vector3.up, up)) * Mathf.Rad2Deg;
      Vector3 axis = Vector3.Cross(Vector3.up, up);
      SmallCamera.localRotation = Quaternion.AngleAxis(angle, axis);

      // Now, align the forward axis
      angle = Mathf.Acos(Vector3.Dot(SmallCamera.transform.forward, forward)) * Mathf.Rad2Deg;
      axis = Vector3.Cross(SmallCamera.transform.forward, forward);
      SmallCamera.localRotation = Quaternion.AngleAxis(angle, axis) * SmallCamera.localRotation;

    }
  }

  public void SetSmallCam(Transform cam)
  {
    SmallCamera = cam;
  }
}