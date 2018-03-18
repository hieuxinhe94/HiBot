using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;

namespace HiBot.Midware
{
    public class LanguageCheckingHelper
    {
        private static readonly string LANGUAGETOOL_API_LANGUAGE = "en-US";
        private static readonly string LANGUAGETOOL_API_URL = "https://languagetool.org/api/v2/check?language=" + LANGUAGETOOL_API_LANGUAGE + "&text=";

        public LanguageCheckingHelper()
        {

        }

        public static async System.Threading.Tasks.Task<RootObject> CheckGrammarAsync(string text)
        {
            // defaut en-us
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //Assuming that the api takes the user message as a query paramater

                    HttpResponseMessage responsemMsg = await client.GetAsync(LANGUAGETOOL_API_URL + text);
                    if (responsemMsg.IsSuccessStatusCode)
                    {
                        var apiResponse = await responsemMsg.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<RootObject>(apiResponse);
                        return result;
                    }
                }
            }
            catch (Exception  )
            {
                // TODO : remove
            }

            return null;
        }
    }


    public class Software
    {
        public string name { get; set; }
        public string version { get; set; }
        public string buildDate { get; set; }
        public int apiVersion { get; set; }
        public string status { get; set; }
    }

    public class Language
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Replacement
    {
        public string value { get; set; }
    }

    public class Context
    {
        public string text { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
    }

    public class Url
    {
        public string value { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Rule
    {
        public string id { get; set; }
        public string subId { get; set; }
        public string description { get; set; }
        public List<Url> urls { get; set; }
        public string issueType { get; set; }
        public Category category { get; set; }
    }

    public class Match
    {
        public string message { get; set; }
        public string shortMessage { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public List<Replacement> replacements { get; set; }
        public Context context { get; set; }
        public string sentence { get; set; }
        public Rule rule { get; set; }
    }

    public class RootObject
    {
        public Software software { get; set; }
        public Language language { get; set; }
        public List<Match> matches { get; set; }
    }
}