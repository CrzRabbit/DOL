using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;

public class PlayerItemPanel : BasePanel
{
    private RoomPlayerInfo roomplayerInfo;
    private Image character;
    private Text playerLevel;
    private Text playerName;
    private Text readyState;
    private int state = 0;
    private IEnumerator coroutine;
    private RoomReadyPanel roomReadyPanel;

    public void Start()
    {
        if (character == null)
        {
            GetComponent<Button>().onClick.AddListener(OnItemClick);
        }
        character = transform.Find("CharacterImg").GetComponent<Image>();
        playerLevel = transform.Find("CharacterImg/Level").GetComponent<Text>();
        playerName = transform.Find("PlayerName").GetComponent<Text>();
        readyState = transform.Find("ReadyState").GetComponent<Text>();
        //coroutine = ResumeState(1.0f);
    }

    public void SetInfo(RoomPlayerInfo roomplayerInfo, RoomReadyPanel roomReadyPanel, Dictionary<UIPanelTextType, string> dict)
    {
        this.roomplayerInfo = roomplayerInfo;
        this.roomReadyPanel = roomReadyPanel;
        ShowInfo(dict);
    }

    private void ShowInfo(Dictionary<UIPanelTextType, string> dict)
    {
        //character
        playerLevel.text = roomplayerInfo.level.ToString();
        playerName.text = roomplayerInfo.name;
        string temp;
        if(roomplayerInfo.readyState)
        {
            dict.TryGetValue(UIPanelTextType.RoomReady_Ready, out temp);
        }
        else
        {
            dict.TryGetValue(UIPanelTextType.RoomReady_NotReady, out temp);
        }
        readyState.text = temp;
    }

    private void OnItemClick()
    {
        if (state == 0)
        {
            state = 1;
            StartCoroutine(ResumeState(1.0f));
        }
        else if (state == 1)
        {
            state = 0;
            //TODO: double click

        }
    }

    private IEnumerator ResumeState(float time)
    {
        yield return new WaitForSeconds(time);
        state = 0;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        EnterAnim();
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
        ExitAnim();
    }

    private void EnterAnim()
    {
        //transform.localScale = Vector3.zero;
        //transform.DOScale(1f, 0.5f);
        //transform.localPosition = new Vector3(-1300, 100, 0);
        //transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    private void ExitAnim()
    {
        //transform.DOScale(0f, 0.5f);
        //Tweener tweener = transform.DOLocalMove(new Vector3(-1300, 100, 0), 0.5f);
    }

    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}
