namespace OsuBotNews
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Discord.Commands;
    using OsuBotNews.Common;

    public class Program : ModuleBase<SocketCommandContext>
    {
        [Command("OsuNews")]
        [Alias("o-n")]
        public async Task OsuNewsAsync()
        {
            WebClient wc = new();
            string url = "https://osu.ppy.sh/api/v2/news";
            string siteUrl = "https://osu.ppy.sh/home/news/";
            string html = wc.DownloadString(url);
            string text = html.Remove(0, html.IndexOf('['));

            string substringImg = "\"first_image\":\"";
            string substringTtl = "\"title\":\"";
            string substringPreview = "\"preview\":\"";
            string substringUrlNews = "\"slug\":\"";

            List<int> ttlIndexs = WordIndex(text, substringTtl);
            List<int> imgIndexs = WordIndex(text, substringImg);
            List<int> previewIndexs = WordIndex(text, substringPreview);
            List<int> urlNewsIndexs = WordIndex(text, substringUrlNews);

            string imageUrl = text.Substring(imgIndexs[0] + substringImg.Length, imgIndexs[0] + 4);
            imageUrl = imageUrl.Replace("\\", null);

            string title = text[(ttlIndexs[0] + substringTtl.Length)..];
            title = title.Substring(0, title.IndexOf('\"'));

            string preview = text[(previewIndexs[0] + substringPreview.Length)..];
            preview = preview.Substring(0, preview.IndexOf('\"'));

            string urlNews = text[(urlNewsIndexs[0] + substringUrlNews.Length)..];
            urlNews = siteUrl + urlNews.Substring(0, urlNews.IndexOf('\"'));

            var embed = new BotEmbedBuilder()
                .WithUrl(urlNews)
                .WithTitle(title)
                .WithImageUrl(imageUrl)
                .WithDescription(preview)
                .Build();
            await this.ReplyAsync(embed: embed);

            ConsoleReport.Report("Была опубликована одна новость.");
        }
        private static List<int> WordIndex(string str, string findString)
        {
            var indices = new List<int>();
            int index = str.IndexOf(findString, 0);

            while (index > -1)
            {
                indices.Add(index);
                index = str.IndexOf(findString, index + findString.Length);
            }

            return indices;
        }
    }
}