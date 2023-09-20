using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class FeedbacksManager : MonoBehaviour
{
    private Player player;
    public MMFeedbacks jumpFeedback;
    public MMFeedbacks landingFeedback;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    public void PlayJumpFeedback(Component sender, object data)
    {
        if(sender is  Player)
        {
            jumpFeedback.PlayFeedbacks();
        }
    }
}
