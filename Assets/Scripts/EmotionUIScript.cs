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
    public MealChecker mealChecker;

    public CustomerMood mood;

    //public bool talkingToCustomer = false;
    public bool active = true;

    private void Start()
    {
        stateMachine.OnCustomerMoodChange += ChangeEmotionSprite;

        rangeScript.OnEnterTalkRange += TryShowEmotion;
        rangeScript.OnExitTalkRange += HideEmotion;

        mealChecker.OnMealOrderFulfilled += SetEmotionInactive;
    }

    private void OnDestroy()
    {
        stateMachine.OnCustomerMoodChange -= ChangeEmotionSprite;

        rangeScript.OnEnterTalkRange -= TryShowEmotion;
        rangeScript.OnExitTalkRange -= HideEmotion;

        mealChecker.OnMealOrderFulfilled -= SetEmotionInactive;
    }

    public void ChangeEmotionSprite(CustomerMood newMood)
    {
        emotionImage.sprite = stateMachine.MapMoodToSprite(newMood);
        mood = newMood;
    }

    public void HideEmotion()
    {
        emotionObj.SetActive(false);
    }

    public void TryShowEmotion()
    {
        if (interactScript.talkingTo || !active)
        {
            HideEmotion();
            return;
        }
        emotionObj.SetActive(true);

    }

    public void SetEmotionInactive()
    {
        active = false;
    }
}
