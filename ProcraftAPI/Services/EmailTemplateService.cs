namespace ProcraftAPI.Services
{
    public class EmailTemplateService
    {
        public string SetHtmlTemplateValues(string user, string recoveryCode)
        {
            //var templatesPath = $"Templates/index.html";

            //string htmlFile = File.ReadAllText(templatesPath) ?? "";

            //htmlFile = htmlFile.Replace("--User--", user).Replace("--Code--", recoveryCode);

            return 
                $"< img src = 'https://procraft-assets.s3.sa-east-1.amazonaws.com/procraft_logo_for_email.png' alt = 'procraft_logo' width = '180' > <h2 style = 'text-align: center; padding-bottom: 2rem;'> Olá, {user}!</ h2 > < br > < h3 style = 'text-align: center; padding-bottom: 2rem;' > Seu código de recuperação é:</ h3 > < br > < p style = 'text-align: center; font-size:2rem;' > {recoveryCode} </ p >";
        }
    }
}
