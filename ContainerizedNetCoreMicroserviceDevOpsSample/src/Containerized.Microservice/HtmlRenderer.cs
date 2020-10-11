using System.Reflection;
using System.Runtime.InteropServices;

namespace Containerized.Microservice
{
    public class HtmlRenderer
    {
        private static readonly string s_appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        private static readonly string s_operatingSystem = GetOperatingSystem();

        public static string GetHtml() =>
            "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
            "<title>Containerized Sample Microservice</title>" +
            "<style type=\"text/css\">" +
            "html, body {" +
            "    height: 100%;" +
            "}" +
            "html {" +
            "    display: table;" +
            "    margin: auto;" +
            "}" +
            "body {" +
            "    display: table-cell;" +
            "    vertical-align: middle;" +
            "    font-family: 'varela round','HelveticaNeue','Helvetica Neue','Helvetica-Neue',Helvetica,Arial,sans-serif;" +
            "    color: #616161;" +
            "}" +
            "h3 {" +
            "    font-weight: 600;" +
            "    letter-spacing: 1px;" +
            "    font-size: 1.1em;" +
            "    margin-bottom: 0" +
            "}" +
            "h3 + p {" +
            "    margin-top: 0;" +
            "    color: #222222;" +
            "}" +
            ".small {" +
            "    font-size: 80%;" +
            "    font-weight: 400;" +
            "}" +
            "</style>" +
            "</head>" +
            "<body>" +
            "<h1>Containerized Sample Microservice</h1>" +
            "<h3>App Version</h1>" +
            $"<p>{s_appVersion}</p>" +
            $"<p class=\"small\">Powered by {RuntimeInformation.FrameworkDescription} running on {s_operatingSystem}.</p>" +
            "</body>" +
            "</html>";

        public static string GetOperatingSystem()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "Linux";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "macOS";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "Windows";

            return "an unknown operating system";
        }
    }
}
