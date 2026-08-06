using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource walkingSrc;
    public AudioClip walkingClip;

    public PlayerMovement playerMoveScript;

    private void Start()
    {
        playerMoveScript.OnMove += PlayWalkingSound;

        playerMoveScript.OnStopMove += PauseWalkingSound;

        walkingSrc.clip = walkingClip;
    }

    private void OnDestroy()
    {
        playerMoveScript.OnMove -= PlayWalkingSound;

        playerMoveScript.OnStopMove -= PauseWalkingSound;

    }

    public void PlayWalkingSound()
    {
        walkingSrc.UnPause();
        
    }

    public void PauseWalkingSound()
    {
        walkingSrc.Pause();
    }
}
