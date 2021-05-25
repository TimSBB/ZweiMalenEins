using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackController : MonoBehaviour
{
    public GameObject FeedbackPrefab;
    private string LabelTag;
    public int playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        LabelTag = gameObject.tag;
        GameController.current.onObjectTriggerEnter += OnRightLabel;
    }

    private void OnRightLabel(string LabelTag, int playerNumber)
    {
        if (LabelTag == this.LabelTag  && playerNumber == this.playerNumber) {
            var pos = gameObject.transform.position + new Vector3(0, 1, 0);
            Instantiate(FeedbackPrefab, pos, Quaternion.identity);
        }

    }

    private void OnDestroy()
    {
        GameController.current.onObjectTriggerEnter -= OnRightLabel;
    }
}
