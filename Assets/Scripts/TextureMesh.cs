using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextureMesh : MonoBehaviour
{
  Vector3[] Vertices;
  int[] Tris;

  Mesh TheMesh = null;
  public int hRes = 5;
  public int wRes = 5;
  public int LENGTH = 4;

  // Start is called before the first frame update
  void Start()
  {
    TheMesh = GetComponent<MeshFilter>().mesh;
    TheMesh.Clear();
  }

  // Update is called once per frame
  void Update()
  {
    SetVerticies(wRes, hRes);
    Vector2[] uv = TheMesh.uv;
    TheMesh.uv = uv;
  }

  void SetVerticies(float width, float height)
  {
    TheMesh.Clear();
    wRes = (int)width;
    hRes = (int)height;
    Vertices = new Vector3[wRes * hRes];

    float wOffset = LENGTH / (wRes - 1);
    float hOffset = LENGTH / (hRes - 1);
    int idx = 0;

    for (int i = 0; i < hRes; i++)
    {
      for (int j = 0; j < wRes; j++)
      {
        Vertices[idx] = new Vector3((float)(-1 + (j * wOffset)), 0, (float)(-1 + (i * hOffset)));
        idx++;
      }
    }
    DrawTris(wRes, hRes);
    Recalc(TheMesh.vertices, TheMesh.normals, wRes, hRes, TheMesh);
  }
  void DrawTris(int width, int height)
  {
    Tris = new int[(width - 1) * (height - 1) * 6];
    int triangleIdx = 0;
    for (int row = 0; row < height - 1; row++)
    {
      for (int col = 0; col < width - 1; col++)
      {
        int currPt = row * width + col;
        Tris[triangleIdx++] = currPt;
        Tris[triangleIdx++] = currPt + width;
        Tris[triangleIdx++] = currPt + width + 1;

        Tris[triangleIdx++] = currPt;
        Tris[triangleIdx++] = currPt + width + 1;
        Tris[triangleIdx++] = currPt + 1;
      }
    }
    TheMesh.vertices = Vertices;
    TheMesh.triangles = Tris;
  }

  void LoadUVs(Vector3[] vertices, Mesh theMesh)
  {
    Vector2[] calcUV = new Vector2[vertices.Length];
    for (int i = 0; i < vertices.Length; i++)
    {
      calcUV[i] = new Vector2(vertices[i].x, vertices[i].z);
    }
    theMesh.uv = calcUV;
  }

  void Recalc(Vector3[] vertices, Vector3[] normals, int width, int height, Mesh theMesh)
  {
    int numTriangles = (width - 1) * (height - 1) * 2;
    Vector3[] triangleNormals = new Vector3[numTriangles];
    int[] triangleMapping = theMesh.triangles;
    int verticeIdx = 0;
    for (int tri = 0; tri < triangleNormals.Length; tri++)
    {
      triangleNormals[tri] =
          FaceNormal(vertices, triangleMapping[verticeIdx], triangleMapping[verticeIdx + 1], triangleMapping[verticeIdx + 2]);
      verticeIdx += 3;
    }

    int edgePt = 0;
    int trianglesPerRow = numTriangles / (height - 1);
    for (int tri = 0; tri < normals.Length; tri++)
    {
      int nextPt = edgePt + 1;
      // first and last triangle
      if (edgePt == 0 || edgePt == trianglesPerRow * (height) - 1)
      {
        Vector3 tri1 = edgePt == 0 ?
        (triangleNormals[edgePt] + triangleNormals[edgePt + 1]).normalized
        : (triangleNormals[edgePt - trianglesPerRow] + triangleNormals[edgePt - 1 - trianglesPerRow]);
        normals[tri] = tri1;
      }
      // top left and bottom right corner
      else if ((edgePt == trianglesPerRow - 1) || (edgePt == trianglesPerRow * (height - 1)))
      {
        int tri1 = edgePt == trianglesPerRow - 1 ?
        edgePt
        : edgePt - trianglesPerRow;
        normals[tri] = (triangleNormals[tri1]).normalized;
      }
      // left and right edges
      else if (edgePt % trianglesPerRow == 0 || nextPt % trianglesPerRow == 0)
      {
        bool isLeft = edgePt % trianglesPerRow == 0;
        normals[tri] = isLeft ?
        (triangleNormals[edgePt] + triangleNormals[edgePt + 1] + triangleNormals[edgePt - trianglesPerRow]).normalized
        : (triangleNormals[edgePt] + triangleNormals[edgePt - trianglesPerRow] + triangleNormals[edgePt - trianglesPerRow - 1]).normalized;
      }
      // top and bottom edges
      else if ((0 < edgePt && edgePt < trianglesPerRow) || ((trianglesPerRow * (height)) - trianglesPerRow < edgePt && edgePt < trianglesPerRow * height))
      {
        bool isBottom = (0 < tri && tri < width);
        int tri1 = isBottom ?
        edgePt : edgePt - trianglesPerRow;
        normals[tri] = (triangleNormals[tri1] + triangleNormals[tri1 + 1] + triangleNormals[tri1 + 2]).normalized;
        edgePt++;
      }
      // everything else
      else
      {
        normals[tri] =
        (triangleNormals[edgePt] + triangleNormals[edgePt + 1]
        + triangleNormals[edgePt + 2] + triangleNormals[edgePt - trianglesPerRow]
        + triangleNormals[edgePt - trianglesPerRow - 1]
        + triangleNormals[edgePt - trianglesPerRow + 1]).normalized;
        edgePt++;
      }
      edgePt++;
    }

    theMesh.normals = normals;
    theMesh.vertices = vertices;
    LoadUVs(vertices, theMesh);
  }

  Vector3 FaceNormal(Vector3[] vertices, int triPt1, int triPt2, int triPt3)
  {
    Vector3 a = vertices[triPt2] - vertices[triPt1];
    Vector3 b = vertices[triPt3] - vertices[triPt1];
    return Vector3.Cross(a, b).normalized;
  }
}
