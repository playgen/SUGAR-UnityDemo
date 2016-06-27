using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using PlayGen.SUGAR.Contracts;

public class Friend : MonoBehaviour {

    public GameObject FriendItemPrefab;
    public GameObject FriendList;
    public Text StatusText;

    void OnEnable()
    {
        UpdateFriendsList();
    }

    void OnDisable()
    {
        ClearList();
    }

    private void ClearList()
    {
        //Remove old friends list
        foreach (Transform child in FriendList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateFriendsList()
    {
        ClearList();
        var friendclient = Controller.Factory.GetUserFriendClient;
        try
        {
            var friends = friendclient.GetFriends(Controller.UserId.Value);
            int counter = 0;
            var listRect = FriendList.GetComponent<RectTransform>().rect;
            foreach (var friend in friends)
            {
                var friendItem = Instantiate(FriendItemPrefab);
                friendItem.transform.SetParent(FriendList.transform, false);
                var itemRectTransform = friendItem.GetComponent<RectTransform>();
                itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 8);
                itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 8)));
                friendItem.GetComponentInChildren<Text>().text = friend.Name;
                counter++;
                var friendCopy = friend;
                friendItem.GetComponentInChildren<Button>().onClick.AddListener(() => RemoveFriend(friendCopy.Id));
            }
        }
        catch (Exception ex)
        {
            StatusText.text = "Failed to get friends list. " + ex.Message;
        }
    }

    private void RemoveFriend(int friendId)
    {
        var friend = Controller.Factory.GetUserFriendClient;
        try
        {
            friend.UpdateFriend(new RelationshipStatusUpdate()
            {
                AcceptorId = friendId,
                RequestorId = Controller.UserId.Value,
                Accepted = true
            });
            StatusText.text = "Successfully removed friend!";
            UpdateFriendsList();
            try
            {
                // Update Achievement Progress
                Controller.SaveData("FriendsRemoved", "1", GameDataValueType.Long);
                Controller.UpdateAchievements();
            }
            catch (Exception ex)
            {
                StatusText.text = "SaveData Fail. " + ex.Message;
            }
        }
        catch (Exception ex)
        {
            StatusText.text = "Remove Friend Failed. " + ex.Message;
        }
    }
}
