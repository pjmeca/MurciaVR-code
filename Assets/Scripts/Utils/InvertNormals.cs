using UnityEngine;

/// <summary>
/// Invierte las normales de un Mesh.<br />
/// https://youtu.be/HEHn4EUUyBk
/// </summary>
public class InvertNormals : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = -1 * normals[i];
        mesh.normals = normals;
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] tris = mesh.GetTriangles(i);
            for (int j = 0; j < tris.Length; j += 3)
            {
                //swap order of tri vertices
                (tris[j + 1], tris[j]) = (tris[j], tris[j + 1]);
            }
            mesh.SetTriangles(tris, i);
        }
    }
}