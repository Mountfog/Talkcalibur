using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class CSVParser
{
    // sPathName :  ������ Resources ���� ���� �����ؾ� �Ѵ�. "TableData/test" 
    public static List<string[]> Load(string sPathName)
    {
        List<string[]> listData = new List<string[]>();

        //TextAsset kTextAsset2 = Resources.Load(sPathName, typeof(TextAsset)) as TextAsset;
        //TextAsset kTextAsset3 = (TextAsset)Resources.Load(sPathName, typeof(TextAsset));
        TextAsset kTextAsset = Resources.Load<TextAsset>(sPathName);

        if (kTextAsset == null)
            return null;

        StringReader _reader = new StringReader(kTextAsset.text);

        string inputData = _reader.ReadLine();
        while (inputData != null)
        {
            string[] datas = inputData.Split('\t');
            if (datas.Length == 0)
                continue;

            listData.Add(datas);

            inputData = _reader.ReadLine();
        }
        _reader.Close();

        return listData;
    }


    // sPathName :  ����) ������ Assets���� �����ؾ� �Ѵ�. "Assets/Resources/TableData/test.csv" 
    public static List<string[]> Load2(string sPathName)
    {
        List<string[]> listData = new List<string[]>();

        StreamReader sr = new StreamReader(sPathName, Encoding.UTF8);
        if (sr == null) return null;

        while (!sr.EndOfStream)                         // ��Ʈ���� ������ �б�
        {
            string line = sr.ReadLine();                // �� �پ� �о�´�.

            string[] datas = line.Split('\t');           // ���� �������� ���ڸ� �и��Ѵ�.
            if (datas.Length == 0)
                continue;

            listData.Add(datas);
        }
        sr.Close();

        return listData;
    }


    // sPathName :  ����) ������ Assets���� �����ؾ� �Ѵ�. "Assets/Resources/TableData/test.csv" 
    public static List<string[]> Load3(string sPathName)
    {
        List<string[]> listData = new List<string[]>();

        //StreamReader sr = new StreamReader(sPathName, Encoding.UTF8);
        //if (sr == null) return null;
        //string strAll = sr.ReadToEnd();
        //sr.Close();

        string strAll = File.ReadAllText(sPathName, Encoding.UTF8);

        StringReader _reader = new StringReader(strAll);

        string inputData = _reader.ReadLine();
        while (inputData != null)
        {
            string[] datas = inputData.Split('\t');
            if (datas.Length == 0)
                continue;

            listData.Add(datas);

            inputData = _reader.ReadLine();
        }
        _reader.Close();
        return listData;
    }

}
