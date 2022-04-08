namespace OsuBotNews.Common
{
    using Discord;

    /// <summary>
    /// themes for embed.
    /// </summary>
    internal class BotEmbedBuilder : EmbedBuilder
    {
        public BotEmbedBuilder()
        {
            this.WithColor(new Color(255, 138, 0));
            this.WithDescription("Тут пока ничего нет.");
        }
    }
}
