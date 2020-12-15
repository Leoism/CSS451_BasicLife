using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
