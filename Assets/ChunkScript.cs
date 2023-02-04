using GameManagers;
using UnityEngine;

public class ChunkScript : MonoBehaviour {

    /// <summary>
    /// If the player collides with the chunk, Update Chunks
    /// </summary>
    /// <param name="other">Other object colliding</param>
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name != "Player") {
            return;
        }

        ChunkMan.Instance.UpdateChunks(gameObject.name);
    }
}