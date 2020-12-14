using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildCreation : MonoBehaviour
{
  public SceneNodeControl sceneNodeControl = null;
  public Dropdown childMenu = null;
  public Transform parentNode = null;
  public SceneNode sceneNodeParent = null;
  public List<GameObject> foodGOs = null;
  public List<string> foodNames = null;
  public List<Texture> foodTextures = null;
  public Material NPMaterial = null;
  public Transform PetLocation = null;
  public Transform HumanLocation = null;
  public NodePrimitive rootNP = null;
  private Dictionary<string, GameObject> foods = new Dictionary<string, GameObject>();
  // Start is called before the first frame update
  void Start()
  {
    Debug.Assert(rootNP);
    Debug.Assert(PetLocation != null);
    Debug.Assert(HumanLocation != null);
    Debug.Assert(foodTextures != null);
    Debug.Assert(sceneNodeControl != null);
    Debug.Assert(parentNode != null);
    Debug.Assert(sceneNodeParent != null);
    Debug.Assert(foodGOs != null);
    Debug.Assert(NPMaterial != null);
    Debug.Assert(foodNames != null);
    Debug.Assert(childMenu != null);
    childMenu.onValueChanged.AddListener(CreateChild);
    for (int i = 0; i < foodGOs.Count; i++)
    {
      foods.Add(foodNames[i], foodGOs[i]);
    }
  }

  public void SetParentNode(Transform parentN)
  {
    parentNode = parentN;
  }

  public void SetParentSceneNode(SceneNode parentSn)
  {
    sceneNodeParent = parentSn;
  }
  public void CreateChild(int val)
  {
    if (val == 0) return;
    string food = foodNames[val - 1];
    GameObject newFood = Instantiate<GameObject>(foods[food]);
    if (newFood == null) return;
    string foodId = RandomID(4);
    newFood.name = food + foodId;
    // create a newSceneNode GameObject
    GameObject newSceneNodeGO = new GameObject(food);
    newSceneNodeGO.name = food + foodId;
    SceneNode newSceneNode = newSceneNodeGO.AddComponent<SceneNode>();
    newSceneNode.gameObject.tag = "SceneNode";
    // Create a new nodePrimitive
    NodePrimitive newFoodNP = newFood.AddComponent<NodePrimitive>();
    newFoodNP.GetComponent<Renderer>().material = NPMaterial;
    newFoodNP.GetComponent<Renderer>().material.mainTexture = foodTextures[val - 1];
    // sets as child of parent node primitive not scene node. 
    //newFood.transform.SetParent(parentNode);
    newFood.transform.localScale = new Vector3(10, 10, 10);
    // set new scene node to child of old scene node
    newSceneNodeGO.transform.SetParent(sceneNodeParent.transform);
    newSceneNodeGO.transform.localPosition = new Vector3(0, 0.5f, 0);
    SetAllSceneNodes(newSceneNode, newFoodNP);
    childMenu.value = 0;
    // update scene node hierarchy
    sceneNodeControl.UpdateSceneNodeMenu();
    // set pet and human location
    newSceneNode.PetLocation = PetLocation;
    newSceneNode.HumanLocation = HumanLocation;
    newSceneNode.sceneNodeControl = sceneNodeControl;
    newSceneNode.rootNP = rootNP;
  }

  private void SetAllSceneNodes(SceneNode newSceneNode, NodePrimitive newNodePrim)
  {
    NodePrimitive curr = newNodePrim;
    while (curr != null)
    {
      newSceneNode.PrimitiveList.Add(curr);
      if (curr.transform.parent != null)
        curr = curr.transform.parent.GetComponent<NodePrimitive>();
      else
        curr = null;
    }
  }

  private string RandomID(int len)
  {
    string alphaNum = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()_+=-";
    StringBuilder res = new StringBuilder();
    System.Random rand = new System.Random();
    for (int i = 0; i < len; i++)
    {
      int randIdx = rand.Next(0, alphaNum.Length - 1);
      res.Append(alphaNum[randIdx]);
    }
    return res.ToString();
  }
}
