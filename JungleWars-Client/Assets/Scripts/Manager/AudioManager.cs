using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager {

    public AudioManager(GameFacade facade) : base(facade) { }
    //構造函式，因為基類有，所以繼承基類的類也要有
    
    //下面一堆const字串，可以依據自己的資料夾分類來設定
    //這邊的範例是射箭角色會用到的音檔資料夾的前綴詞
    private const string Sound_Prefix = "Sounds/";
    public const string Sound_Alert = "Alert";
    public const string Sound_ArrowShoot = "ArrowShoot";
    public const string Sound_Bg_Fast = "Bg(fast)";
    public const string Sound_Bg_Moderate = "Bg(moderate)";
    public const string Sound_ButtonClick = "ButtonClick";
    public const string Sound_Miss = "Miss";
    public const string Sound_ShootPerson = "ShootPerson";
    public const string Sound_Timer = "Timer";

    //把AudioSource分類
    //這邊範例分成背景音效、一般音效
    private AudioSource bgAudioSource;
    private AudioSource normalAudioSource;

    public override void OnInit() //初始化
    {   //創立一個新遊戲物件，這個遊戲物件用來處理聲音
        //遊戲物件內增加Component，Component為AudioSource
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        normalAudioSource = audioSourceGO.AddComponent<AudioSource>();
        
        //初始化就執行播放聲音函式
        //這邊先自動播放背景音樂，一般音效等動作執行才發出音效
        //函式內才去找要播放哪個AudioSource的哪個AudioClip
        //注意GameFacade的函式audioMng.PlayBgSound(soundName);
        PlaySound(bgAudioSource, LoadSound(Sound_Bg_Moderate),0.5f, true);
    }
    
    //GameFacade會叫audioMng執行PlayBgSound(soundName)
    public void PlayBgSound(string soundName)
    {
        PlaySound(bgAudioSource, LoadSound(soundName), 0.5f, true);
    }
    //定義一般音效函式
    //要執行時，會先透過GameFacade，GameFacade會找audioMng來放聲音
    public void PlayNormalSound(string soundName)
    {
        PlaySound(normalAudioSource, LoadSound(soundName), 1f);
    }
    
    //定義播放聲音函式
    private void PlaySound( AudioSource audioSource,AudioClip clip,float volume, bool loop=false)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }
    //Clip是透過函式另外加載來的，回傳AudioClip
    //當作PlaySound()的參數之一
    private AudioClip LoadSound(string soundsName)
    {
        return Resources.Load<AudioClip>(Sound_Prefix + soundsName);
    }
}
