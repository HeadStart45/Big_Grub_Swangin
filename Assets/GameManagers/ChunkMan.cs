using UnityEngine;

namespace GameManagers {
    public class ChunkMan : MonoBehaviour {
        // Singleton instance of Chunk Manager
        private static ChunkMan Instance { get; set; }

        // Prefab for the new chunk to add
        [SerializeField]
        private GameObject chunkPrefab;
        //Initial Chunk 
        [SerializeField]
        private GameObject firstChunk;

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
            Instantiate(chunkPrefab, new Vector3(xPos, firstChunk.transform.position.y, firstChunk.transform.position.z), Quaternion.identity);
        }

        /// <summary>
        /// On start create 2 chunks
        /// </summary>
        public void Start() {
            // Create the first chunk at 0
            //CreateChunk(0);

            // Create the next map chunk at the end of the previous chunk
            CreateChunk( firstChunk.transform.position.x - (GetBounds(chunkPrefab).x - 1));
            
            
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