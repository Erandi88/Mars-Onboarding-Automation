using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qa_dotnet_cucumber.Contexts
{
    public class TestDataContext
    {
        public List<string> CreatedLanguages { get; } = new List<string>();

        public List<string> CreatedSkills { get; } = new();

        public int LanguageRowCountBeforeAction { get; set; }

        public string CurrentLanguage { get; set; } = string.Empty;
    }
}
