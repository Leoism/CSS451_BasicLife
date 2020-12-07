using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class ChildCreation : MonoBehaviour
{
  public Transform parentNode = null;
  public SceneNode sceneNodeParent = null;
  public List<GameObject> foodGOs = null;
  public List<string> foodNames = null;
  public Material NPMaterial = null;
  private Dictionary<string, GameObject> foods = new Dictionary<string, GameObject>();
  // Start is called before the first frame update
  void Start()
  {
    Debug.Assert(parentNode != null);
    Debug.Assert(sceneNodeParent != null);
    Debug.Assert(foodGOs != null);
    Debug.Assert(NPMaterial != null);
    Debug.Assert(foodNames != null);
    for (int i = 0; i < foodGOs.Count; i++)
    {
      foods.Add(foodNames[i], foodGOs[i]);
    }
  }

  public void SetParentNode(Transform parentN)
  {
    parentNode = parentN;
  }

  public void CreateChild(string food)
  {
    GameObject newFood = Instantiate<GameObject>(foods[food]);
    if (newFood == null) return;
    string foodId = RandomID(4);
    newFood.name = food + foodId;
    GameObject newSceneNodeGO = new GameObject(food);
    newSceneNodeGO.name = food + foodId;
    NodePrimitive newFoodNP = newFood.AddComponent<NodePrimitive>();
    sceneNodeParent.PrimitiveList.Add(newFoodNP);
    // sets as child of parent node primitive not scene node. 
    newFood.transform.SetParent(parentNode);
    // set new scene node to child of old scene node
    SceneNode newSceneNode = newSceneNodeGO.AddComponent<SceneNode>();
    newSceneNodeGO.transform.SetParent(sceneNodeParent.transform);
    newSceneNode.PrimitiveList.Add(newFoodNP);
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
  // Update is called once per frame
  void Update()
  {

  }
}
