using System.ComponentModel.DataAnnotations;
using ConfigurationManager;

namespace DataManager.Options
{
    public class PathOptions
    {
        [Required]
        [Name("Templog")]
        public string Templog { get; set; }
        [Required]
        [Name("SourceDirectory")]
        public string SourceDirectory { get; set; }
    }
}