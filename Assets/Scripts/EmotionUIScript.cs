using UnityEngine;
using Customer;
using UnityEngine.UI;

public class EmotionUIScript : MonoBehaviour
{
    public GameObject emotionObj;
    public Image emotionImage;
    public CustomerStateMachine stateMachine;
    public CustomerInteractScript interactScript;
    public TalkRangeScript rangeScript;

    public CustomerMood mood;

    public bool talkingToCustomer = false;

    private void Start()
    {
        stateMachine.OnCustomerMoodChange += ChangeEmotionSprite;

        rangeScript.OnEnterTalkRange += TryShowEmotion;
        rangeScript.OnExitTalkRange += HideEmotion;
    }

    private void OnDestroy()
    {
        stateMachine.OnCustomerMoodChange -= ChangeEmotionSprite;

        rangeScript.OnEnterTalkRange -= TryShowEmotion;
        rangeScript.OnExitTalkRange -= HideEmotion;



    }

    public void ChangeEmotionSprite(CustomerMood newMood)
    {
        emotionImage.sprite = stateMachine.MapMoodToSprite(newMood);
    }

    public void HideEmotion()
    {
        emotionObj.SetActive(false);
    }

    public void TryShowEmotion()
    {
        if (interactScript.talkingTo)
        {
            HideEmotion();
            return;
        }
        emotionObj.SetActive(true);

    }

}
