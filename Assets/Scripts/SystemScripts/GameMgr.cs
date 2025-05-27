using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr
{
    static GameMgr instance = null;
    public static GameMgr Inst
    {
        get
        {
            if (instance == null)
                instance = new GameMgr();
            return instance;
        }
    }
    //a Singleton class that can be used to load GameScene
    public void Initialize()
    {
        IsInstalled = true;
        Application.runInBackground = true;
    }

    public GameScene gameScene { get; set; }
    public GameInfo ginfo = new GameInfo();
    public bool IsInstalled { get; set; }
}
