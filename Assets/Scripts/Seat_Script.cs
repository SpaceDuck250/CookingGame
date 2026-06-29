using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool occupied = false;


    void Awake()
    {
        // No SitPoint needed when using tags
    }


    public bool IsAvailable()
    {
        return !occupied;
    }


    public void TakeSeat()
    {
        occupied = true;
    }


    public void LeaveSeat()
    {
        occupied = false;
    }
}