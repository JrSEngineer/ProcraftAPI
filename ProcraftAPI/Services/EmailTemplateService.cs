namespace ProcraftAPI.Services
{
    public class EmailTemplateService
    {
        public string SetHtmlTemplateValues(string user, string recoveryCode)
        {
            var templatesPath = $"/Templates/index.html";

            string htmlFile = File.ReadAllText(templatesPath) ?? "";

            htmlFile = htmlFile.Replace("--User--", user).Replace("--Code--", recoveryCode);

            return htmlFile;
        }
    }
}
