using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
   public string meshName;
   public Vector2 planeSize;
   public int planeResolution;

   private List<Vector3> vertices;
   private List<int> triangles;

   private Mesh currentMesh;

   [Button("Generate A Plane")]
   public void GenerateMesh()
   {
      GeneratePlane(planeSize, planeResolution);
      SaveMesh();
   }

   void GeneratePlane(Vector2 size, int resolution)
   {
      vertices = new List<Vector3>();
      float xPerStep = size.x / resolution;
      float yPerStep = size.y / resolution;

      for (int y = 0; y < resolution + 1; y++)
      {
         for (int x = 0; x < resolution + 1; x++)
         {
            vertices.Add(new Vector3(x*xPerStep, 0, y*yPerStep));
         }  
      }

      triangles = new List<int>();
      for (int row = 0; row < resolution; row++)
      {
         for (int column = 0; column < resolution; column++)
         {
            int i = (row * resolution) + row + column;
            
            triangles.Add(i);
            triangles.Add(i + resolution + 1);
            triangles.Add(i + resolution + 2);
            
            triangles.Add(i);
            triangles.Add(i+resolution+2);
            triangles.Add(i + 1);
         }
      }
   }

   void SaveMesh()
   {
      currentMesh = new Mesh();
      currentMesh.Clear();
      currentMesh.vertices = vertices.ToArray();
      currentMesh.triangles = triangles.ToArray();

      if (meshName != "")
      {
         Mesh findMesh = (Mesh)AssetDatabase.LoadAssetAtPath($"Assets/_Dev/DJN/DJN_Assets/{meshName}.mesh", typeof(Mesh));
         
         if (findMesh == null)
         {
            AssetDatabase.CreateAsset(currentMesh, $"Assets/_Dev/DJN/DJN_Assets/{meshName}.mesh");

            meshName = "";
         }
         else
         {
            Debug.LogError("This asset name already exist.");
         }
      }
   }
}
