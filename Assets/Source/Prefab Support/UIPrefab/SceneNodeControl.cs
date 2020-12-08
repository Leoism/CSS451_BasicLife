using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneNodeControl : MonoBehaviour
{
  public Dropdown TheMenu = null;
  public SceneNode TheRoot = null;
  public NodePrimitive RootNodePrim = null;
  public XfromControl XformControl = null;
  public ChildCreation childCreator = null;

  const string kChildSpace = "  ";
  public List<Dropdown.OptionData> mSelectMenuOptions = new List<Dropdown.OptionData>();
  List<Transform> mSelectedTransform = new List<Transform>();

  // Use this for initialization
  void Start()
  {
    Debug.Assert(RootNodePrim != null);
    Debug.Assert(TheMenu != null);
    Debug.Assert(TheRoot != null);
    Debug.Assert(XformControl != null);
    Debug.Assert(childCreator != null);

    UpdateSceneNodeMenu();
    TheMenu.onValueChanged.AddListener(SelectionChange);

    XformControl.SetSelectedObject(TheRoot.transform);
  }

  public void GetChildrenNames(string blanks, Transform node)
  {
    string space = blanks + kChildSpace;
    for (int i = node.childCount - 1; i >= 0; i--)
    {
      Transform child = node.GetChild(i);
      SceneNode cn = child.GetComponent<SceneNode>();
      if (cn != null)
      {
        mSelectMenuOptions.Add(new Dropdown.OptionData(space + child.name));
        mSelectedTransform.Add(child);
        GetChildrenNames(blanks + kChildSpace, child);
      }
    }
  }

  void SelectionChange(int index)
  {
    XformControl.SetSelectedObject(mSelectedTransform[index]);
    childCreator.SetParentSceneNode(mSelectedTransform[index].gameObject.GetComponent<SceneNode>());
    NodePrimitive nodePrimEq = FindEquivalentNodePrim(mSelectedTransform[index].name, RootNodePrim.transform);
    //childCreator.SetParentNode(nodePrimEq.transform);
  }

  private NodePrimitive FindEquivalentNodePrim(string name, Transform rootNP)
  {
    if (rootNP.transform.name.Equals(name)) return rootNP.GetComponent<NodePrimitive>();
    for (int i = 0; i < rootNP.childCount; i++)
    {
      Transform child = rootNP.GetChild(i);
      NodePrimitive cn = child.GetComponent<NodePrimitive>();
      if (cn == null) continue;
      NodePrimitive foundNode = FindEquivalentNodePrim(name, child);
      if (foundNode != null) return foundNode;
    }
    return null;
  }

  public void UpdateSceneNodeMenu()
  {
    mSelectedTransform.Clear();
    mSelectMenuOptions.Clear();
    TheMenu.ClearOptions();
    mSelectMenuOptions.Add(new Dropdown.OptionData(TheRoot.transform.name));
    mSelectedTransform.Add(TheRoot.transform);
    GetChildrenNames("", TheRoot.transform);
    TheMenu.AddOptions(mSelectMenuOptions);
    childCreator.SetParentNode(RootNodePrim.transform);
    childCreator.SetParentSceneNode(TheRoot);
  }
}
