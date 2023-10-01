using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q;
public class LSystemMesh: MonoBehaviour {
  private List<Vector3> vertices = new List<Vector3> ();
  private List<int> triangles = new List<int> ();
  private List<Vector2> uvs = new List<Vector2> ();
  private List<int> lastCircleIndices = new List<int> ();

  private Material material;

  //some way to store normal information, maybe a vector 3?

  [SerializeField]
  private float tubeRadius = 0.1f;
  [SerializeField]
  private int tubeSides = 8;

  //private void Awake () { material = GetComponent<MeshRenderer> ().material; }

  // Compute a circle of vertices around a point in the given direction
  public Vector3[] ComputeCircle (Vector3 center, Vector3 direction) {
    Vector3[] circle = new Vector3[tubeSides];
    Quaternion rotation = Quaternion.LookRotation (direction);

    for (int i = 0;i < tubeSides;i ++) {
      float angle = i * 360f / tubeSides;
      Vector3 offset
          = new Vector3 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad), 0)
          * tubeRadius;
      circle[i] = center + rotation * offset;
    }

    return circle;
  }


  // Add a tube segment between two circles of vertices
  public void AddTubeSegment (Vector3[] startCircle, Vector3[] endCircle) {
    Debug.Log ("Generating Mesh Segment");
    Debug.Log ("Vert Count: " + vertices.Count + ", Tri Count: " + triangles.Count);
    for (int i = 0;i < tubeSides;i ++) {
      int nextIndex = (i + 1) % tubeSides;

      // Indices for this quad
      int v1, v2, v3, v4;

      // Check if we can reuse vertices from the last circle for the start of this segment
      if (lastCircleIndices.Count > 0) {
        v1 = lastCircleIndices[i];
        v2 = lastCircleIndices[nextIndex];
      } else {
        vertices.Add (startCircle[i]);
        vertices.Add (startCircle[nextIndex]);
        v1 = vertices.Count - 2;
        v2 = vertices.Count - 1;
      }

      // Add vertices for the end of the segment
      vertices.Add (endCircle[i]);
      vertices.Add (endCircle[nextIndex]);
      v3 = vertices.Count - 2;
      v4 = vertices.Count - 1;

      // Add triangles for this quad
      triangles.Add (v1);
      triangles.Add (v2);
      triangles.Add (v3);
      triangles.Add (v2);
      triangles.Add (v4);
      triangles.Add (v3);

      // Add UVs (this is a simple wrap, can be customized later)
      uvs.Add (new Vector2 (i / (float)tubeSides, 0));
      uvs.Add (new Vector2 ((i + 1) / (float)tubeSides, 0));
      uvs.Add (new Vector2 (i / (float)tubeSides, 1));
      uvs.Add (new Vector2 ((i + 1) / (float)tubeSides, 1));
    }

    // Store the indices of the end circle to be used for the next segment
    lastCircleIndices.Clear ();
    for (int i = vertices.Count - tubeSides * 2;i < vertices.Count;i += 2) {
      lastCircleIndices.Add (i);
    }
  }


  public void FinalizeMesh () {
    Debug.Log ("Finalize Mesh");
    Mesh mesh = GetComponent<MeshFilter> ().mesh;
    if (mesh == null) {
      mesh = new Mesh ();
      GetComponent<MeshFilter> ().mesh = mesh;
    }

    mesh.Clear ();
    mesh.vertices = vertices.ToArray ();
    mesh.triangles = triangles.ToArray ();
    mesh.uv = uvs.ToArray ();

    mesh.RecalculateNormals ();

    // Ensure MeshRenderer is attached and has a material.
    var meshRenderer = GetComponent<MeshRenderer> ();
    if (meshRenderer == null) {
      meshRenderer = gameObject.AddComponent<MeshRenderer> ();
    }
    if (meshRenderer.sharedMaterial == null) {
      meshRenderer.material = new Material (Shader.Find ("Standard"));
    }
  }
}
