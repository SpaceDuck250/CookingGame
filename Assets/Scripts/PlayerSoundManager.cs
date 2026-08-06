using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource walkingSrc;

    public AudioClip walkingClip;
    public AudioClip pickupSound;

    public PlayerMovement playerMoveScript;

    private void Start()
    {
        playerMoveScript.OnMove += PlayWalkingSound;

        playerMoveScript.OnStopMove += PauseWalkingSound;

        PlayerHandScript.OnHoldSomething += PlayPickupSound;

        walkingSrc.clip = walkingClip;
    }

    private void OnDestroy()
    {
        playerMoveScript.OnMove -= PlayWalkingSound;

        playerMoveScript.OnStopMove -= PauseWalkingSound;

        PlayerHandScript.OnHoldSomething -= PlayPickupSound;


    }

    public void PlayWalkingSound()
    {
        walkingSrc.UnPause();
        
    }

    public void PauseWalkingSound()
    {
        walkingSrc.Pause();
    }

    public void PlayPickupSound()
    {
        GeneralSoundManager.instance.PlaySoundEffect(pickupSound);
    }

 
}
