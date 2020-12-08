using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetScript : MonoBehaviour
{
    NodePrimitive Myself = null;
    public bool IsPet;

    public Vector3 minSize = Vector3.one * 2f;

    void Awake()
    {
        Myself = GetComponent<NodePrimitive>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (IsPet) transform.localScale = minSize;

        Matrix4x4 i = Matrix4x4.identity;
        if (Myself != null) Myself.LoadShaderMatrix(ref i);
        SetPos();
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 i = Matrix4x4.identity;
        if (Myself != null) Myself.LoadShaderMatrix(ref i);

        FoodCheck();
        GrowthDecay();
        SetPos();
    }

    void SetPos()
    {
        Vector3 pos = new Vector3(transform.position.x, 0f, transform.position.z);
        if (IsPet) pos += new Vector3(0f, 0.5f * transform.localScale.y, 0f);
        else pos += new Vector3(0f, transform.localScale.y + .01f, 0f);
        transform.position = pos;
    }

    void FoodCheck()
    {
        //If there is a scene node in my collision
        //If scene node has no children AND scene node has no parent
        //Destroy scene node
        if (IsPet)
        {
            //Grow
        }
    }

    void GrowthDecay()
    {
        if (IsPet)
        {
            //Reduce scale
        }
    }
}
