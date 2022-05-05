namespace OsuBotNews
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Newtonsoft.Json.Linq;
    using OsuBotNews.Common;

    public class Program : ModuleBase<SocketCommandContext>
    {
        [Command("OsuNews")]
        [Alias("news")]
        public async Task OsuNewsAsync()
        {
            WebClient wc = new();

            string url = "https://osu.ppy.sh/api/v2/news";
            string siteUrl = "https://osu.ppy.sh/home/news/";

            string html = wc.DownloadString(url);
            var json = JObject.Parse(html);

            var urlNews = siteUrl + json["news_posts"][0]["slug"].ToString();
            var title = json["news_posts"][0]["title"].ToString();
            var imageUrl = json["news_posts"][0]["first_image"].ToString();
            var preview = json["news_posts"][0]["preview"].ToString();

            var embed = new BotEmbedBuilder()
                .WithUrl(urlNews)
                .WithTitle(title)
                .WithImageUrl(imageUrl)
                .WithDescription(preview)
                .Build();
            await this.ReplyAsync(embed: embed);

            ConsoleReport.Report("Была опубликована новость.");
        }
    }
}