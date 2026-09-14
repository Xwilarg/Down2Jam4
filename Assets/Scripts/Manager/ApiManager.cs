using Down2Jam.Manager.Persistency;
using Newtonsoft.Json;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Down2Jam.Manager
{
    public class ApiManager : MonoBehaviour
    {
        public static ApiManager Instance { private set; get; }

        [SerializeField]
        private TMP_Text _connectionText;

        [SerializeField]
        private Button _validateLoginButton;

        private string _deviceCode;

        private void Awake()
        {
            Instance = this;

            var token = PersistencyManager.Instance.SaveData.Token;
            if (token != null)
            {
                _connectionText.text = $"Connected";
            }
        }

        public void GetToken()
        {
            StartCoroutine(GetTokenCoroutine());
        }

        public void ValidateToken()
        {
            StartCoroutine(ValidateTokenCoroutine());
        }

        public void UnlockAchievement(int id)
        {
            if (PersistencyManager.Instance.SaveData.Token != null)
            {
                StartCoroutine(UnlockAchievementCoroutine(id));
            }
        }


        private IEnumerator UnlockAchievementCoroutine(int id)
        {
            Debug.Log($"Sending achievement ID {id} to API");

            using UnityWebRequest request = new("https://d2jam.com/api/v1/achievement", "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(
                "{\"achievementId\": " + id + "}"
            );
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {PersistencyManager.Instance.SaveData.Token}");
            yield return request.SendWebRequest();

            var res = JsonConvert.DeserializeObject<ApiResponse<AchievementData>>(request.downloadHandler.text);

            if (!res.success)
            {
                Debug.Log("Failed to send achievement");
            }
        }

        private IEnumerator GetTokenCoroutine()
        {
            using UnityWebRequest request = new("https://d2jam.com/api/v1/device/code", "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(
                "{\"clientName\": \"Minipis' Normal Adventure\", \"gameSlug\": \"minipis-normal-adventure\"}"
            );
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            var res = JsonConvert.DeserializeObject<ApiResponse<LoginData>>(request.downloadHandler.text);

            if (res.success)
            {
                Application.OpenURL(res.data.verificationUri);
                var userCode = res.data.userCode;

                _deviceCode = res.data.deviceCode;

                _connectionText.text = $"Code: {userCode}";
                _validateLoginButton.interactable = true;
            }
            else Debug.LogError("Failed to login");
        }

        private IEnumerator ValidateTokenCoroutine()
        {
            using UnityWebRequest request = new("https://d2jam.com/api/v1/device/token", "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(
                "{\"deviceCode\": \"" + _deviceCode + "\"}"
            );
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            var res = JsonConvert.DeserializeObject<ApiResponse<TokenData>>(request.downloadHandler.text);

            if (res.success)
            {
                if (res.data.status == "approved")
                {
                    PersistencyManager.Instance.SaveData.Token = res.data.token;
                    PersistencyManager.Instance.Save();
                    _connectionText.text = $"Connected";
                }
                else
                {
                    Debug.LogError($"Token validation failed with status {res.data.status}");
                }
                _validateLoginButton.interactable = false;
            }
            else Debug.LogError("Failed to validate token");
        }
    }

    public class ApiResponse<T>
    {
        public bool success { set; get; }
        public T data { set; get; }
    }

    public class LoginData
    {
        public string deviceCode { set; get; }
        public string verificationUri { set; get; }
        public string userCode { set; get; }
    }

    public class TokenData
    {
        public string status { set; get; }
        public string token { set; get; }
    }

    public class AchievementData
    { }
}
