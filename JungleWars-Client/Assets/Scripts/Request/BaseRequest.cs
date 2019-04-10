using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class BaseRequest : MonoBehaviour { //掛載在場景上，是各種Request的基類
//Request其實就是傳遞Msg並做某些事情，先定義Msg的形式
//要傳遞的Msg內有ActionCode跟RequestCode，預設一開始都是None
//要傳遞或是呼叫Mng去做事情需要透過單例，也就是GameFacade
    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    protected GameFacade _facade;

//取用GameFacade的實例，因為要透過GameFacade實例去叫其他Manager做事
    protected GameFacade facade
    {
        get
        {
            if (_facade == null)
                _facade = GameFacade.Instance;
            return _facade;
        }
    }
	// Use this for initialization
	public virtual void Awake () {
        facade.AddRequest(actionCode, this);
    }

    protected void SendRequest(string data)
    {
        facade.SendRequest(requestCode, actionCode, data);
    }
//寫傳送、回應的虛方法給其他繼承此基類Msg的去複寫。
    public virtual void SendRequest() { }
    public virtual void OnResponse(string data) { }

    public virtual void OnDestroy()
    {
        if(facade != null)
            facade.RemoveRequest(actionCode);
    }
}
