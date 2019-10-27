using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Controllers.News
{
    public class NewsController : MonoBehaviour
    {
        public static NewsController Instance;
        private const string ApiKey = "0646442c2c014e8e8bd341b29c42e715";
        public string countryCode = "tr";

        [SerializeField] private Transform parentContent;
        [SerializeField] private GameObject newsContent;

        [SerializeField] private NewsInfo newsInfo;

        private void Awake()
        {
            countryCode = "tr";
            Instance = this;
            InvokeRepeating(nameof(CheckNews), 0, 600);
        }

        public IEnumerator GetNews()
        {
            using (UnityWebRequest req = UnityWebRequest.Get(
                $"https://newsapi.org/v2/top-headlines?country={countryCode}&apiKey={ApiKey}"))
            {
                yield return req.SendWebRequest();
                while (!req.isDone)
                    yield return null;

                var result = req.downloadHandler.data;
                var newsJson = System.Text.Encoding.Default.GetString(result);
                newsInfo = JsonUtility.FromJson<NewsInfo>(newsJson);

                if (newsInfo.totalResults > 0) UpdateNewsPanel();
            }
        }
        public void CheckNews()
        {
            StartCoroutine(GetNews());
        }

        public void UpdateNewsPanel()
        {
            for (var i = 0; i < parentContent.childCount; i++)
            {
                var obj = parentContent.GetChild(i).gameObject;
                obj.SetActive(false);
                Destroy(obj);
            }
            for (int i = 0; i < newsInfo.articles.Length; i++)
            {
                var everyIndex = i;
                var content = Instantiate(newsContent, parentContent);
                var rect = parentContent.GetComponent<RectTransform>();

                var sizeDelta = rect.sizeDelta;
                sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y + 150);
                rect.sizeDelta = sizeDelta;

                Vector3 pos = content.transform.localPosition;
                pos.y -= i * 155;
                content.transform.localPosition = pos;

                TextMeshProUGUI title = content.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                Button newsButton = content.transform.GetChild(1).GetComponent<Button>();

                title.text = newsInfo.articles[i].title;
                newsButton.onClick.AddListener((() =>
                {
                    Application.OpenURL(newsInfo.articles[everyIndex].url);
                }));
            }
        }
    }
}
