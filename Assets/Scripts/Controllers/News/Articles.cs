using System;

namespace Controllers.News
{
    [Serializable]
    public class Articles
    {
        public Source source;
        public string author;
        public string title;
        public string description;
        public string url;
        public string urlToImage;
        public string publishedAt;
    }
}
