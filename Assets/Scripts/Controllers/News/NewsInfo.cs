using System;

namespace Controllers.News
{
    [Serializable]
    public class NewsInfo
    {
        public string status;
        public int totalResults;
        public Articles[] articles;
    }
}
