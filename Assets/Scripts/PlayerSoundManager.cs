using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public SFXBank playerBank;

    public AudioSource walkingSrc;
    public PlayerMovement playerMoveScript;

    private void Start()
    {
        playerMoveScript.OnMove += PlayWalkingSound;
        playerMoveScript.OnStopMove += PauseWalkingSound;
        PlayerHandScript.OnHoldSomething += PlayPickupSound;

        walkingSrc.clip = playerBank.GetEntry("walking").clip;
    }

    private void OnDestroy()
    {
        playerMoveScript.OnMove -= PlayWalkingSound;
        playerMoveScript.OnStopMove -= PauseWalkingSound;
        PlayerHandScript.OnHoldSomething -= PlayPickupSound;
    }

    public void PlayWalkingSound() => walkingSrc.UnPause();
    public void PauseWalkingSound() => walkingSrc.Pause();

    public void PlayPickupSound() => GeneralSoundManager.instance.PlaySoundEffect(playerBank, "pickup");
}