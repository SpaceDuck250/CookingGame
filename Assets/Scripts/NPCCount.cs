using UnityEngine;

public class NPCCount : MonoBehaviour
{
    public NPC_Spawner spawner;


    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.NPCLeft();
        }
    }
}