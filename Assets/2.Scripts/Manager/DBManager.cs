using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DBManager : MonoBehaviour
{
    public void TryLogin(string username, string password)
    {
        StartCoroutine(LoginCoroutine(username, password));
    }

    IEnumerator LoginCoroutine(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post("http://yourserver.com/login.php", form);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("응답: " + www.downloadHandler.text);

            if (www.downloadHandler.text.Contains("success"))
                Debug.Log("로그인 성공");
            else
                Debug.Log("로그인 실패");
        }
        else
        {
            Debug.Log("에러: " + www.error);
        }
    }
}