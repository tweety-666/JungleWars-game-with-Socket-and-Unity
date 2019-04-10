using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager{
    //Mng基類給各個Mng繼承，抓到上頭的GameFacade，然後有三個方法:初始化、更新、銷毀
    protected GameFacade facade;
    public BaseManager(GameFacade facade) //構造函式，找上頭的GameFacade
    {
        this.facade = facade;
    }
    public virtual void OnInit() { }
    public virtual void Update(){}
    public virtual void OnDestroy() { }
}
