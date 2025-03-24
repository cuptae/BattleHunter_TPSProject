using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System;

// 파서 하나 준비하고
public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string text)
    {
        var list = new List<Dictionary<string, object>>();
        //TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}

public class Downloader : MonoBehaviour
{
    const string urlPattern = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}"; // 내 시트를 공유를 열어 놔야함

    [System.Serializable]
    public class Item
    {
        public string name;
        public int price;
        public int count;
    }
    public List<Item> itemList= new List<Item>();


    void Start()
    {
        StartCoroutine(Download());

    }
   
    IEnumerator Download()
    {
        // 주소창에 /d 다음부터~ /edit 까지
        // {0},{1} 순서대로 문저열이 적용된다.
        string url = string.Format(urlPattern, "1PUnAWsw9k4eTlAx2B35JaCS6UzROb4y5Fl_f_CNjKRA", 0);
        Debug.Log("url: " + url);
        UnityWebRequest req = UnityWebRequest.Get(url);

        while (!req.isDone)
            yield return req.SendWebRequest();

        Debug.Log("req.IsDone");

        if(string.IsNullOrEmpty(req.error))
        {
            string text = req.downloadHandler.text;
            //Debug.Log(text);
            List<Dictionary<string, object>> result = CSVReader.Read(text); //ReadCSV(text);

            //string name = result[0]["count"];
            //Debug.Log(name);

            foreach (Dictionary<string, object> data in result)
            {
                Item item = new Item();
                item.name = data["name"].ToString();
                item.price = int.Parse(data["price"].ToString());
                item.count = int.Parse(data["count"].ToString());
                itemList.Add(item);
            }
        }
    }

    List<Dictionary<string, string>> ReadCSV(string csv)
    {
        string[] lines = csv.Split('\n');
        //foreach (string line in lines)
        //{
        //    Debug.Log(line);
        //}

        string[] attribute = lines[0].Trim().Split(',');
        //foreach (string attr in attribute)
        //{
        //    Debug.Log(attr);
        //}

        List<Dictionary<string, string>> ret = new List<Dictionary<string, string>>();
        for(int i = 1; i < lines.Length; i++)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            string[] items = lines[i].Trim().Split(',');
            for(int j = 0; j< items.Length; j++)
            {
                data[attribute[j]] = items[j];
            }
            ret.Add(data);
        }

        return ret;
    }
}
