using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// GUID 기반 Custom ID 로그인 후,
/// 닉네임 입력 및 로그인 실패 시 경고 메시지를 표시하는 스크립트
/// </summary>
public class PlayFabGuidLoginWithNickname : MonoBehaviour
{
    private const string PREFS_CUSTOM_ID = "CUSTOM_PLAYFAB_ID";

    [Header("Nickname UI")]
    [Tooltip("닉네임 입력 UI Prefab (InputField + Button)")]
    public GameObject nicknamePanelPrefab;
    [Tooltip("UI를 붙일 부모 Canvas 또는 UI Parent")]
    public Transform uiParent;

    [Header("Login Error UI")]
    [Tooltip("로그인 실패 시 표시할 경고 메시지 Prefab")]
    public GameObject loginErrorPanelPrefab;

    [Header("Debug")]
    [Tooltip("기존 DisplayName이 있어도 UI를 띄울지 여부")]
    public bool forceShowNicknameUI = false;

    private GameObject nicknamePanelInstance;
    private GameObject loginErrorPanelInstance;
    private TMP_InputField nicknameInput;
    private Button submitButton;

    private void Start()
    {
        // Custom ID 읽기 또는 생성
        string customId = PlayerPrefs.GetString(PREFS_CUSTOM_ID, null);
        if (string.IsNullOrEmpty(customId))
        {
            customId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString(PREFS_CUSTOM_ID, customId);
            PlayerPrefs.Save();
        }

        // PlayFab 로그인 요청
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log($"PlayFab 로그인 성공: {result.PlayFabId}");
        // 계정 정보 조회
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnAccountInfoReceived, OnLoginError);
    }

    private void OnAccountInfoReceived(GetAccountInfoResult infoResult)
    {
        string currentName = infoResult.AccountInfo.TitleInfo.DisplayName;
        if (string.IsNullOrEmpty(currentName) || forceShowNicknameUI)
            ShowNicknameUI();
    }

    private void OnLoginError(PlayFabError error)
    {
        Debug.LogError("PlayFab 에러: " + error.GenerateErrorReport());
        ShowLoginError();
    }

    private void ShowNicknameUI()
    {
        if (nicknamePanelInstance != null) return;
        if (nicknamePanelPrefab == null) return;
        nicknamePanelInstance = uiParent != null
            ? Instantiate(nicknamePanelPrefab, uiParent, false)
            : Instantiate(nicknamePanelPrefab);
        nicknameInput = nicknamePanelInstance.GetComponentInChildren<TMP_InputField>();
        submitButton = nicknamePanelInstance.GetComponentInChildren<Button>();
        if (submitButton != null)
            submitButton.onClick.AddListener(OnSubmitNickname);
    }

    /// <summary>
    /// 로그인 실패 시 호출: 경고 메시지 패널을 표시
    /// </summary>
    private void ShowLoginError()
    {
        if (loginErrorPanelInstance != null) return;
        if (loginErrorPanelPrefab == null) return;
        loginErrorPanelInstance = uiParent != null
            ? Instantiate(loginErrorPanelPrefab, uiParent, false)
            : Instantiate(loginErrorPanelPrefab);
    }

    private void OnSubmitNickname()
    {
        string name = nicknameInput.text.Trim();
        if (string.IsNullOrEmpty(name)) return;
        var setNameRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = name };
        PlayFabClientAPI.UpdateUserTitleDisplayName(setNameRequest,
            _ => { if (nicknamePanelInstance != null) Destroy(nicknamePanelInstance); },
            err => Debug.LogError("닉네임 설정 실패: " + err.GenerateErrorReport()));
    }
}
