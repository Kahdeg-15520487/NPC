using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPC.Editor
{
    static class Program
    {
        public static string fileOpenWith = null;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                fileOpenWith = args[0];
            }

            JsonConvert.DefaultSettings = () =>
            {
                return new JsonSerializerSettings { Converters = { new StringEnumConverter { NamingStrategy = new DefaultNamingStrategy() } } };
            };

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new form_editor());
        }
    }
}
