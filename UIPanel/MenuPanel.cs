using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuPanel : BasePanel
{
    private void Start()
    {
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
