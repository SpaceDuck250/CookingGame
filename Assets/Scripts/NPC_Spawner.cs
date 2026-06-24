using UnityEngine;

public class NPC_Spawner : MonoBehaviour
{
    public GameObject npcPrefab;

    public Transform spawnPoint;

    public float spawnInterval = 5f;

    public int maxNPC = 10;

    private int currentNPC = 0;


    void Start()
    {
        InvokeRepeating(
            "SpawnNPC",
            1f,
            spawnInterval
        );
    }


    void SpawnNPC()
    {
        if (currentNPC >= maxNPC)
            return;


        GameObject npc = Instantiate(
            npcPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );


        currentNPC++;


        // Optional: decrease count when NPC leaves later
        NPCCount tracker = npc.AddComponent<NPCCount>();
        tracker.spawner = this;
    }


    public void NPCLeft()
    {
        currentNPC--;
    }
}