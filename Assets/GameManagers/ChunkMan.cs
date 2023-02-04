using System.IO.IsolatedStorage;
using UnityEngine;

namespace GameManagers {
    public class ChunkMan : MonoBehaviour {
        // Singleton instance of Chunk Manager
        private static ChunkMan Instance { get; set; }

        // Prefab for the new chunk to add
        [SerializeField]
        private GameObject chunkPrefab;

        // Number of chunks to render, defaults to 5
        [SerializeField]
        private float chunkCount = 5;

        /// <summary>
        /// On Chunk Manager awake, create a singleton instance.
        /// </summary>
        public void Awake() {
            // If there is an existing instance of Chunk Manager, destroy this one.
            if (Instance != null && Instance != this) {
                Destroy(this);
            }
            else {
                Instance = this;
            }
        }

        private void CreateChunk(float xPos) {
            Instantiate(chunkPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
        }

        /// <summary>
        /// On start create chunks
        /// </summary>
        public void Start() {
            for (var i = 0; i < chunkCount; i++) {
                CreateChunk((GetBounds(chunkPrefab).x - 1) * i);
            }
        }

        /// <summary>
        /// Get the size bounds of a chunk
        /// </summary>
        /// <param name="chunk">The chunk to get the size</param>
        /// <returns>a Vector3 with the size of the chunk</returns>
        private static Vector3 GetBounds(GameObject chunk) {
            return chunk.GetComponent<BoxCollider>().size;
        }
    }
}