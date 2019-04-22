using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class RoomPanel : BasePanel
{
    private ListRoomRequest listRoomRequest;
    private void Start()
    {
        transform.Find("CreateButton").GetComponent<Button>().onClick.AddListener(OnCreateClick);
        transform.Find("SearchButton").GetComponent<Button>().onClick.AddListener(OnSearchClick);
        listRoomRequest = GetComponent<ListRoomRequest>();
    }

    private void OnCreateClick()
    {
        uiMng.ClearPanel();
        uiMng.PushPanel(UIPanelType.CreateRoom);
    }

    private void OnSearchClick()
    {
        listRoomRequest.SendRequest();
        uiMng.ClearPanelSync();
        uiMng.PushPanelSync(UIPanelType.ListRoom);
    }

    public void OnSearchResponse(ReturnCode retCode)
    {
        if (ReturnCode.Success == retCode)
        {
            //uiMng.ClearPanelSync();
            //uiMng.PushPanelSync(UIPanelType.ListRoom);
        }
        else if (ReturnCode.Fail == retCode)
        {
            List<UIPanelTextType> msgs = new List<UIPanelTextType>();
            msgs.Add(UIPanelTextType.Room_Msg0);
            uiMng.ShowMessageSync(msgs);
        }
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

    public override void OnSetLanguage(Dictionary<UIPanelTextType, string> panelTextDict)
    {
        string temp;
        panelTextDict.TryGetValue(UIPanelTextType.Room_CreateBtn, out temp);
        transform.Find("CreateButton/Text").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Room_SearchBtn, out temp);
        transform.Find("SearchButton/Text").GetComponent<Text>().text = temp;
    }

    private void EnterAnim()
    {
        //transform.localScale = Vector3.zero;
        //transform.DOScale(1f, 0.5f);
        transform.localPosition = new Vector3(-1300, 100, 0);
        transform.DOLocalMove(new Vector3(-700, 100, 0), 0.5f);
    }

    private void ExitAnim()
    {
        //transform.DOScale(0f, 0.5f);
        Tweener tweener = transform.DOLocalMove(new Vector3(-1300, 100, 0), 0.5f);
    }
}
