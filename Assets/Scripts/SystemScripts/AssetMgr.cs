using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssetWord
{
    public int id = 0;
    public string basicWord = "";
    public int wordType = 0;
    public string firstWord = "";
    public string secondWord = "";
    public int minValue = 0;
    public int maxValue = 0;
    public int extraMinValue = 0;
    public int extraMaxValue = 0;
    public string desc = "";
}
public class AssetMgr
{
    static AssetMgr _inst = null;
    public static AssetMgr Inst
    {
        get
        {
            if (_inst == null)
                _inst = new AssetMgr();

            return _inst;
        }
    }
    public bool IsInstalled { get; set; }
    public List<AssetWord> m_assetWords = new List<AssetWord>();
    private AssetMgr()
    {
        IsInstalled = false;
    }

    public AssetWord GetAssetWord(int iWord)
    {
        if (iWord >= 0 && iWord <= m_assetWords.Count)
        {
            return m_assetWords[iWord];
        }
        return null;
    }
    public AssetWord GetAssetWord(string basicWord)
    {
        foreach(AssetWord word in m_assetWords)
        {
            if(word.basicWord == basicWord)
            {
                return word;
            }
        }
        return null;
    }
    public void Initialize_Word(string m_tabledata)
    {
        List<string[]> dataList = CSVParser.Load(m_tabledata);

        for (int i = 1; i < dataList.Count; i++)
        {
            string[] data = dataList[i];
            AssetWord kitem = new AssetWord();

            kitem.id = int.Parse(data[0]);
            kitem.basicWord = data[1];
            kitem.wordType = int.Parse(data[2]);
            kitem.firstWord = data[3];
            kitem.secondWord = data[4];
            kitem.minValue = int.Parse(data[5]);
            kitem.maxValue = int.Parse(data[6]);
            kitem.extraMinValue = int.Parse(data[7]);
            kitem.extraMaxValue = int.Parse(data[8]);
            kitem.desc = data[9];

            m_assetWords.Add(kitem);
        }

    }
    public void Initialize()
    {
        Initialize_Word("TableData/word");
        IsInstalled = true;
    }

}
