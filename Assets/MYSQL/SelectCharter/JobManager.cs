using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Job
{
    public int id;
    public string name;
}

public class JobManager : MonoBehaviour
{
    public GameObject[] jobObjects;     // Button 또는 Toggle을 포함한 GameObject 배열
    public Text jobText;                // 선택된 직업명을 표시할 텍스트
    public SkillLoader skillLoader;

    private int selectedCharacterId = -1;
    private List<Job> jobList;

    void Start()
    {
        StartCoroutine(GetJobsFromServer());
    }

    IEnumerator GetJobsFromServer()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://192.168.0.24:8080/get_characters.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            jobList = new List<Job>(JsonHelper.FromJson<Job>(json));

            for (int i = 0; i < jobList.Count && i < jobObjects.Length; i++)
            {
                int index = i;
                GameObject obj = jobObjects[i];

                // Toggle 처리
                if (obj.TryGetComponent(out Toggle toggle))
                {
                    // 텍스트 세팅
                    Text toggleText = toggle.GetComponentInChildren<Text>();
                    if (toggleText != null)
                        toggleText.text = jobList[index].name;

                    toggle.onValueChanged.AddListener((isOn) =>
                    {
                        if (isOn) OnJobSelected(index);
                    });
                }
                // Button 처리
                else if (obj.TryGetComponent(out Button button))
                {
                    Text buttonText = button.GetComponentInChildren<Text>();
                    if (buttonText != null)
                        buttonText.text = jobList[index].name;

                    button.onClick.AddListener(() => OnJobSelected(index));
                }
                else
                {
                    Debug.LogWarning($"{obj.name} 에 Button 또는 Toggle 컴포넌트가 없습니다.");
                }
                if (jobList.Count > 0)
                {
                    OnJobSelected(0); // 첫 번째 직업 선택 실행

                    if (jobObjects[0].TryGetComponent(out Toggle firstToggle))
                    {
                        firstToggle.isOn = true; // 첫 번째 토글이 있다면 활성화
                    }
                }
            }
        }
    }

    private void OnJobSelected(int index)
    {
        selectedCharacterId = jobList[index].id;
        jobText.text = jobList[index].name;

        Debug.Log($"[JobManager] 선택된 직업: {jobList[index].name} (ID: {jobList[index].id})");

        if (skillLoader != null)
            skillLoader.SetCharacterId(jobList[index].id);
        else
            Debug.LogWarning("SkillLoader가 연결되지 않았습니다!");
    }

    public int GetSelectedCharacterId()
    {
        return selectedCharacterId;
    }

    // JsonHelper 내부 포함
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    private static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"Items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.Items;
        }
    }
}
