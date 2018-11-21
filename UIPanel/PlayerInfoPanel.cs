using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.Model;

public class PlayerInfoPanel : BasePanel
{
    private Text levelText;
    private Text nameText;
    private Text expText;
    private Slider expSlider; 
    private void Start()
    {
        levelText = transform.Find("LevelImage").transform.Find("Text").GetComponent<Text>();
        nameText = transform.Find("PlayerName").GetComponent<Text>();
        expText = transform.Find("ExpInfo").transform.Find("Text").GetComponent<Text>();
        expSlider = transform.Find("ExpInfo").GetComponent<Slider>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        uiMng.InjectPlayerInfoPanel(this);
        uiMng.ShowPlayerInfo();
        EnterAnim();
    }

    public void ShowPlayerInfo(PlayerInfo info)
    {
        Start();
        this.levelText.text = info.PlayerLevel.ToString();
        this.nameText.text = info.PlayerName;
        this.expText.text = info.PlayerCurExp.ToString() + "/" + info.GetLevelExp();
        this.expSlider.value = info.PlayerCurExp / info.GetLevelExp();
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
        transform.localPosition = new Vector3(1500, 180, 0);
        transform.DOLocalMove(new Vector3(750, 180, 0), 0.5f);
    }
        
    private void ExitAnim()
    {
        //transform.DOScale(0f, 0.5f);
        Tweener tweener = transform.DOLocalMove(new Vector3(1500, 180, 0), 0.5f);
    }
}
