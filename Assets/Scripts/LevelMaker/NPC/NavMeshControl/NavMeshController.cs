using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace NavMeshControl {
    public class NavMeshController {
        public static void BakeNavMeshLevel(NavMeshSurface navMeshSurface) {
            // Assuming we have a NavMeshSurface component in the scene
            if (navMeshSurface == null)
            {
                Debug.LogError("NavMeshSurface not assigned!");
                return;
            }

            // Bake navMesh
            navMeshSurface.BuildNavMesh();
        }
    }
}
