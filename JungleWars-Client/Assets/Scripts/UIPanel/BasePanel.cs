using UnityEngine;
using System.Collections;

public class BasePanel : MonoBehaviour {
    protected UIManager uiMng;//聲明相關的Mng
    protected GameFacade facade;//聲明facade單例

    public UIManager UIMng 
    {
        set { uiMng = value; }
    }//定義相關的Mng屬性

    public GameFacade Facade
    {
        set { facade = value; }
    }//定義facade單例屬性


    protected void PlayClickSound()//按下按鈕，播放聲音
    {
        facade.PlayNormalSound(AudioManager.Sound_ButtonClick);
    }

    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPause()
    {

    }

    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume()
    {

    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public virtual void OnExit()
    {

    }
}
