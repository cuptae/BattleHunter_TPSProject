using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string text)
    {
        var list = new List<Dictionary<string, object>>();
        var lines = Regex.Split(text, LINE_SPLIT_RE);

        if (lines.Length < 3) return list; // 최소한 3줄 필요 (Header, Type, Data)

        var headers = Regex.Split(lines[0], SPLIT_RE);
        var types = Regex.Split(lines[2], SPLIT_RE);

        for (var i = 3; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < headers.Length && j < values.Length; j++)
            {
                string value = values[j].Trim(TRIM_CHARS);
                entry[headers[j]] = ConvertType(value, types[j]);
            }
            list.Add(entry);
        }
        return list;
    }

    private static object ConvertType(string value, string type)
    {
        if (string.IsNullOrEmpty(value)) return null;
        
        switch (type.ToLower())
        {
            case "int":
                if (int.TryParse(value, out int intValue)) return intValue;
                break;
            case "float":
                if (float.TryParse(value, out float floatValue)) return floatValue;
                break;
            case "bool":
                return value.Equals("TRUE", StringComparison.OrdinalIgnoreCase);
            case "string":
            default:
                return value;
        }
        return value;
    }
}
