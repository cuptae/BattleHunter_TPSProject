using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    public Dropdown resolutionDropdown; //드롭박스
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum; //드롭박수 내 변수 선언
    FullScreenMode screenMode; //풀스크린 모드
    public Toggle fullscreenBtn; //풀스크린 버튼 변수
    // Start is called before the first frame update
    void Start()
    {
        InitUI();
    }

    void InitUI(){
        for(int i=0; i<Screen.resolutions.Length; i++){
            if(Screen.resolutions[i].refreshRate==60){
            resolutions.Add(Screen.resolutions[i]);
            }
        }
        resolutionDropdown.options.Clear();

        foreach(Resolution item in resolutions){
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text=item.width+"x"+item.height+" "+item.refreshRateRatio+"hz";
            resolutionDropdown.options.Add(option);
        }
        resolutionDropdown.RefreshShownValue();

        fullscreenBtn.isOn=Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow)?true:false;
    }

    public void DropBoxOptionChange(int x){
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull){
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OkBtnClick(){
        Screen.SetResolution(resolutions[resolutionNum].width,
        resolutions[resolutionNum].height,
        screenMode);
    }
}
