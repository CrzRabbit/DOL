using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuPanel : BasePanel
{
    private LogoutRequest logoutRequest;
    private void Start()
    {
        GameObject obj = GameObject.Find("RequestProxy(Clone)");

        logoutRequest = obj.GetComponent<LogoutRequest>();

        uiMng.PushPanel(UIPanelType.Room);
        uiMng.PushPanel(UIPanelType.PlayerInfo);
        uiMng.ShowPlayerInfo();

        transform.Find("LobbyButton").GetComponent<Button>().onClick.AddListener(OnLobbyClick);
        transform.Find("WarehouseButton").GetComponent<Button>().onClick.AddListener(OnStoreClick);
        transform.Find("SettingButton").GetComponent<Button>().onClick.AddListener(OnSettingClick);
        transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(OnExitClick);
    }

    private void OnLobbyClick()
    {

    }

    private void OnStoreClick()
    {

    }

    private void OnSettingClick()
    {

    }

    private void OnExitClick()
    {
        logoutRequest.SendRequest(facade.GetPlayerInfo().PlayerIndex);
        uiMng.ClearPanel();
        uiMng.PushPanel(UIPanelType.Login);
        SceneManager.LoadScene(1);
    }

    public override void OnEnter()
    {
        uiMng.PushPanel(UIPanelType.Room);
        uiMng.PushPanel(UIPanelType.PlayerInfo);
        uiMng.ShowPlayerInfo();

        base.OnEnter();
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    public override void OnSetLanguage(Dictionary<UIPanelTextType, string> panelTextDict)
    {
        string temp;
        panelTextDict.TryGetValue(UIPanelTextType.Menu_ExitBtn, out temp);
        transform.Find("ExitButton/Text").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Menu_SettingBtn, out temp);
        transform.Find("SettingButton/Text").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Menu_LobbyBtn, out temp);
        transform.Find("LobbyButton/Text").GetComponent<Text>().text = temp;

        panelTextDict.TryGetValue(UIPanelTextType.Menu_WarehouseBtn, out temp);
        transform.Find("WarehouseButton/Text").GetComponent<Text>().text = temp;
    }

    private void EnterAnim()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.5f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    private void ExitAnim()
    {
        transform.DOScale(0f, 0.5f);
        Tweener tweener = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.5f);
    }
}
