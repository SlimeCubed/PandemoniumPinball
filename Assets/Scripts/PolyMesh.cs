using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

// I know this is a really inelegant solution, but it saves my time
// Find a polygon collider on this game object and replace it with visual triangles
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(PolygonCollider2D))]
[ExecuteInEditMode]
public class PolyMesh : MonoBehaviour
{
    private uint hash = 0;
    
    public void Awake()
    {
        GetComponent<MeshRenderer>().sortingLayerName = "Board Back";

        if (Application.isPlaying)
        {
            Destroy(this);
            Destroy(GetComponent<PolygonCollider2D>());
        }
    }

    public void Update()
    {
        PolygonCollider2D c = GetComponent<PolygonCollider2D>();
        uint hash = c.GetShapeHash();
        if (hash == this.hash) return;

        this.hash = hash;

        Mesh mesh = new Mesh();

        var points = new List<Vector2>();
        var verts = new List<Vector3>();
        var inds = new List<int>();
        for (int path = 0; path < c.pathCount; path++)
        {
            Vector2 center = Vector2.zero;
            points.Clear();
            c.GetPath(path, points);
            foreach (var p in points)
                center += p / points.Count;

            verts.Clear();
            verts.Add(center);
            verts.AddRange(points.Select(p => (Vector3)p));

            inds.Clear();
            for(int i = 1; i < verts.Count; i++)
            {
                inds.Add(0);
                inds.Add(i);
                if (i == verts.Count - 1)
                    inds.Add(1);
                else
                    inds.Add(i + 1);
            }

            mesh.SetVertices(verts, 0, verts.Count);
            mesh.SetIndices(inds, MeshTopology.Triangles, path);
        }

        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;
    }
}
