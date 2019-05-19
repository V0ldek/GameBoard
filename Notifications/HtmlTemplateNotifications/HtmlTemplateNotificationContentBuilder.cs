using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GameBoard.Notifications.NotificationContentBuilder;

namespace GameBoard.Notifications.HtmlTemplateNotifications
{
    internal class HtmlTemplateNotificationContentBuilder : INotificationContentBuilder
    {
        private string _title = "";

        private readonly List<string> _content = new List<string>();

        private const string TemplateDirectoryRelativePath = "HtmlTemplateNotifications/Html/Inlined";

        private const string BaseTemplateName = "layout-template.html";

        private const string TextTemplateName = "text-content-template.html";

        private const string LinkTemplateName = "link-content-template.html";

        private static readonly Regex TemplateInjectionRegex = new Regex(@"\$\{([^}]*)}");

        public INotificationContentBuilder AddTitle(string title)
        {
            _title = title;
            return this;
        }

        public INotificationContentBuilder AddText(string text)
        {
            var template = LoadTemplateFromFile(TextTemplateName);
            var content = InjectContentIntoTemplate(template, ("Text-Content", text));
            _content.Add(content);
            return this;
        }

        public INotificationContentBuilder AddLink(string href, string text)
        {
            var template = LoadTemplateFromFile(LinkTemplateName);
            var content = InjectContentIntoTemplate(template, ("Link-Href", href), ("Link-Content", text));
            _content.Add(content);
            return this;
        }

        public string Build()
        {
            var template = LoadTemplateFromFile(BaseTemplateName);
            var content = string.Join('\n', _content);
            return InjectContentIntoTemplate(template, ("Layout-Title", _title), ("Layout-Content", content));
        }

        private static string LoadTemplateFromFile(string templateName)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, TemplateDirectoryRelativePath, templateName);
            return File.ReadAllText(filePath);
        }

        private static string InjectContentIntoTemplate(
            string template,
            params (string key, string content)[] injectedContent)
        {
            var injectedContentDictionary = injectedContent.ToDictionary(t => t.key, t => t.content);
            return TemplateInjectionRegex.Replace(
                template, 
                m =>
                {
                    var key = m.Groups[1].Value;
                    return injectedContentDictionary.ContainsKey(key) ? injectedContentDictionary[key] : "";
                });
        }
    }
}