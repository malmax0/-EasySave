using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp.Model
{
    public class SaveList
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Type { get; set; }
        public string CryptoSoft { get; set; }
        public string MegaOctets { get; set; }
        public string Progress { get; set; }
    }
}
