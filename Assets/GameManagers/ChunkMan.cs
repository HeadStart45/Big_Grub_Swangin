using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagers {
    public class ChunkMan : MonoBehaviour {
        // Singleton instance of Chunk Manager
        public static ChunkMan Instance { get; private set; }

        // Prefab for the new chunk to add
        [SerializeField]
        private GameObject chunkPrefab;

        //Initial Chunk
        [SerializeField]
        private GameObject firstChunk;

        // List of Chunks
        [NonSerialized]
        private List<GameObject> _chunks;

        private int _totalChunkCount = 0;

        private Vector3 _initialChunkPosition;

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

            _initialChunkPosition = firstChunk.transform.position;
        }

        private GameObject CreateChunk(float xPos) {

            // Create a new chunk at the end of the previous chunk
            var newChunk = Instantiate(chunkPrefab,
                new Vector3(xPos - (GetBounds(chunkPrefab).x - 1), _initialChunkPosition.y, _initialChunkPosition.z),
                Quaternion.identity);

            // Increment chunk count and set the name
            _totalChunkCount++;
            newChunk.name = newChunk.tag + "_" + _totalChunkCount;

            return newChunk;
        }

        /// <summary>
        /// On start create 2 chunks
        /// </summary>
        public void Start() {
            // Add the first chunk to the list of chunks
            firstChunk.name = firstChunk.tag + "_" + _totalChunkCount;

            // Init the list of chunks
            _chunks = new List<GameObject> {
                firstChunk
            };

            // Increment the chunk count
            _totalChunkCount++;
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
        /// Called by chunks when the player enters them.
        /// Check to remove a chunk and if we need to add a new chunk
        /// </summary>
        /// <param name="chunkName">The name of the chunk the player has just moved into</param>
        public void UpdateChunks(string chunkName) {
            // Check if we should remove the first chunk
            for (var i = 0; i < _chunks.Count; i++) {
                if (_chunks[i].name == chunkName && i > 2) {
                    var startChunk = _chunks[0];
                    if (startChunk != null && startChunk.ToString() != chunkName) {
                        _chunks.RemoveAt(0);
                        Destroy(startChunk);
                    }
                }
            }

            // Early return if we have enough chunks
            if (_chunks.Count >= 4) {
                return;
            }

            // Add a new chunk
            var endChunk = _chunks[^1];
            if (endChunk != null) {
                _chunks.Add(CreateChunk(endChunk.transform.position.x));
            }
        }
    }
}