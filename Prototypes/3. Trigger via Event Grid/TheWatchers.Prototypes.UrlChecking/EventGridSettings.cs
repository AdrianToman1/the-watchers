using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWatchers.Prototypes.TriggerViaEventGrid
{
    /// <summary>
    ///     Configuration settings.
    /// </summary>
    public class EventGridSettings
    {
        /// <summary>
        ///     The URL to check.
        /// </summary>
        /// <remarks>
        ///     The URL may a have trailing forward slash or not.
        ///     If not defined the URL will default to <see cref="Constants.DefaultUrl" />.
        /// </remarks>
        public string Url { get; set; }

        public string Key { get; set; } 
    }
}
