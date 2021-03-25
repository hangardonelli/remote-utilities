using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace client_server
{
    public interface IOpener
    {
        bool Run();
    }
   
    public class Opener
    {
 
        public bool StartProcess(string ProcessPatch, List<string> ProcessParameters = null)
        {
             string[] Apps = new string[] {
                "mspaint",
                "explorer",
                "iexplore",
                "chrome",
                "firefox",
                "calc",
                "charmap",
                "notepad",
                "wordpad"
            };
        if (File.Exists(ProcessPatch) || Apps.Any(app => ProcessPatch.Contains(app)))
            {
                string parameters = null;
                if(ProcessParameters != null)
                {
                    parameters =
                   String.Join(" ", ProcessParameters);
                }
                Process.Start($"{ProcessPatch} {parameters}");
                return true;
            }
            return false;
        } 
    }


    public class FileExplorerOpener : IOpener
    {
        public bool Run()
        {
            Process.Start("explorer");
            return true;
        }

        public bool Run(string patch)
        {
            string[] blackChars = { ".", "@", "<", ">" };
            if(!blackChars.Any(x => patch.Contains(x))  && Directory.Exists(patch))
            {
                Process.Start("explorer", patch);
                return true;
            }
            return false;
        }
    }
    public class BrowserOpener : IOpener
    {

        private enum WebBrowser
        {
            InternetExplorer,
            Chrome,
            Firefox,
            Opera,
            Safari,
            Chromium,
            Brave,
            UNKNOW_WEB_BROWSER
        }

        const string DEFAULT_BROWSER_REG_PATCH = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice";
        const string KEY = "ProgId";
        private string DEFAULT_WEBBROWSER = GetDefaultWebBrowserString();
        private static string GetDefaultWebBrowserString()
        {
            Dictionary<WebBrowser, string> WebBrowserStrings = new Dictionary<WebBrowser, string>();
            

            WebBrowserStrings.Add(WebBrowser.InternetExplorer, "iexplore");
            WebBrowserStrings.Add(WebBrowser.Chrome, "chrome");
            WebBrowserStrings.Add(WebBrowser.Chromium, "chromium");
            WebBrowserStrings.Add(WebBrowser.Firefox, "firefox");
            WebBrowserStrings.Add(WebBrowser.Opera, "opera");
            WebBrowserStrings.Add(WebBrowser.Safari, "safari");
            WebBrowserStrings.Add(WebBrowser.UNKNOW_WEB_BROWSER, "UNKNOW");


            string defaultBrowser = ((string)Registry.GetValue(DEFAULT_BROWSER_REG_PATCH, KEY, null)).ToLower();



            foreach(KeyValuePair<WebBrowser, string> browser in WebBrowserStrings)
            {
                if (defaultBrowser.Contains(browser.Value))
                {
                    return browser.Value;
                }

            }
            return WebBrowserStrings[WebBrowser.UNKNOW_WEB_BROWSER];
            
        }

        public bool Run()
        {        
            if(DEFAULT_WEBBROWSER != "UNKNOW")
            {
                Process.Start(DEFAULT_WEBBROWSER);
                return true;
            }
            else
            {
                Console.WriteLine(DEFAULT_WEBBROWSER);
                Process.Start(DEFAULT_WEBBROWSER);
                return false;
            }
            
        }

        public bool Run(string URL)
        {
            if (DEFAULT_WEBBROWSER != "UNKNOW")
            {
                Console.WriteLine(DEFAULT_WEBBROWSER);
                Process.Start(DEFAULT_WEBBROWSER, URL);
                return true;
            }
            else
            {
                Console.WriteLine(DEFAULT_WEBBROWSER);
                Process.Start("URL");
                return false;
            }
        }

    }
    public class PaintOpener : IOpener
    {
       
        public bool Run()
        {
            Process.Start("mspaint");
            return true;
        }

        public bool Run(string patch)
        {

            string[] extensions = 
            {
                ".bmp",
                ".dib",
                ".jpg",
                "jpeg",
                ".jpe",
                ".jfif",
                ".gif",
                ".png",
                ".ico",
                ".heic",
                ".webp"
            }; 

            if (File.Exists($@"{patch}") &&  extensions.FirstOrDefault(x => patch.Contains(x)) != null)
            {
                Process.Start("mspaint", patch);
                return true;
            }
            return false;
        }
    }
    public class NotepadOpener : IOpener
    {
        public bool Run(string patchToSave, string filename)
        {
            try
            {
                if (!File.Exists($@"{patchToSave}\{filename}.txt"))
                {
                    File.Create($@"{patchToSave}\{filename}.txt");
                    Process.Start("notepad", $@"{patchToSave}\{filename}.txt");
                    return true;
                }

            }
            catch {; ; /*bullshit :] */} 
            return false;
        }
        public bool Run()
        {
            Process.Start("notepad");
            return true;
        }
        public bool Run(string filename)
        {
            Process.Start("notepad", filename);
            return true;
        }

    }
    
}

