using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace HkkAllCardsToTCGArena
{
    public static class HkkAllCardsToTcgArena
    {
        //MAGIC CONSTANTS
        const int START_KEY = 11867;

        const int KIEG = 259;

        const int GYAKORISAG = 0;

        static void Main()
        {
            string line = DownloadUrlAsync(KIEG, GYAKORISAG).GetAwaiter().GetResult();

            JsonCreator.CreateJson(START_KEY, line);
        }


        static async Task<string> DownloadUrlAsync(int kiadas, int gyakorisag)
        {
            string url = $"https://www.beholder.hu/?m=hkk&in=hkk_lapkereso.php&KERESES=1&adv=1" +
                   $"&kiadas1={kiadas}" +
                   $"&kiadas2=0" +
                   $"&kiadascsak=0" +
                   $"&evtol=0" +
                   $"&evig=0" +
                   $"&szin1=0&szin2=0&szin3=0&szin4=0" +
                   $"&tipus1=0&tipus2=0" +
                   $"&altipus1=0&altipus2=0" +
                   $"&egyebtipus=0" +
                   $"&gyakorisag={gyakorisag}" +
                   $"&nevstr=&szovegstr=&szinesitostr=" +
                   $"&idktg11=&idktg12=&idktg13=1" +
                   $"&idktg21=&idktg22=&idktg23=1" +
                   $"&tamadas1=&tamadas2=&tamadas3=0" +
                   $"&vedekezes1=&vedekezes2=&vedekezes3=0" +
                   $"&grafikus=0&tervezo=0";

            using HttpClient client = new HttpClient();

            string html = await client.GetStringAsync(url);

            string? line = html
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .FirstOrDefault(l => l.Contains("</SCRIPT><SCRIPT>tablefejlec"));

            if (line == null)
            {
                throw new InvalidOperationException("Expected script containing '</SCRIPT><SCRIPT>tablefejlec' not found in HTML response.");
            }

            return line;
        }
    }

}
