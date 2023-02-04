using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameManagers {
    public class ChunkMan : MonoBehaviour {
        // Singleton instance of Chunk Manager
        public static ChunkMan Instance { get; set; }

        // Prefab for the new chunk to add
        [SerializeField]
        private GameObject chunkPrefab;

        //Initial Chunk
        [SerializeField]
        private GameObject firstChunk;

        // List of Chunks
        public List<GameObject> chunks = new List<GameObject>();

        private int totalChunkCount = 0;

        private Vector3 initialChunkPosition;

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

            // Init the list of chunks
            // chunks = new List<GameObject>();
            initialChunkPosition = firstChunk.transform.position;
        }

        private GameObject CreateChunk(float xPos) {

            // Create a new chunk at the end of the previous chunk
            var newChunk = Instantiate(chunkPrefab,
                new Vector3(xPos - (GetBounds(chunkPrefab).x - 1), initialChunkPosition.y, initialChunkPosition.z),
                Quaternion.identity);

            // Increment chunk count and set the name
            totalChunkCount++;
            newChunk.name = newChunk.tag + "_" + totalChunkCount;

            return newChunk;
        }

        /// <summary>
        /// On start create 2 chunks
        /// </summary>
        public void Start() {
            // Add the first chunk to the list of chunks
            firstChunk.name = firstChunk.tag + "_" + totalChunkCount;

            // Add firstChunk to the list of chunks
            chunks = new List<GameObject> {
                firstChunk,
            };

            // Increment the chunk count
            totalChunkCount++;
        }

        /// <summary>
        /// Get the size bounds of a chunk
        /// </summary>
        /// <param name="chunk">The chunk to get the size</param>
        /// <returns>a Vector3 with the size of the chunk</returns>
        private static Vector3 GetBounds(GameObject chunk) {
            return chunk.GetComponent<BoxCollider>().size;
        }

        /// <summary>
        /// Called by chunks when the player enters them
        /// </summary>
        /// <param name="chunkName"></param>
        public void UpdateChunks(string chunkName) {
            Debug.Log("In update chunks, chunk count is:" + chunks.Count);

            // If the chunk count is less than 3, generate a new chunk
            if (chunks.Count < 3) {
                Debug.Log("chunksCount is less than 3, adding new chunk");
                var endChunk = chunks.LastOrDefault();
                if (endChunk != null) {
                    chunks.Add(CreateChunk(endChunk.transform.position.x));
                }
            }

            // If the chunk count is greater 3, pop the first chunk
            if (chunks.Count == 3) {
                Debug.Log("chunksCount == 3, popping first chunk");
                var startChunk = chunks.FirstOrDefault();
                if (startChunk != null && startChunk.name != chunkName) {
                    chunks.Remove(startChunk);
                    Destroy(startChunk);
                }
            }

            Debug.Log("After update chunks, chunk count is:" + chunks.Count);
        }
    }
}