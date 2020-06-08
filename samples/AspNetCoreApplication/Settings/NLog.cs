using ServiceSettingsValidations;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreApplication.Settings
{
    public class NLog
    {
        [Required]
        [MinLength(4)]
        public string InternalLogLevel { get; set; }

        [Required]
        [MinLength(1)]
        public string InternalLogFile { get; set; }

        [ValidateObject]
        [MinLength(1)]
        public NLogRule[] Rules { get; set; }
    }

    public class NLogRule
    {
        [Required]
        [MinLength(1)]
        public string Logger { get; set; }

        [Required]
        [MinLength(1)]
        public string MinLevel { get; set; }

        [Required]
        [MinLength(1)]
        public string WriteTo { get; set; }
    }
}