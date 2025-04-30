using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class FrontendIntent
    {
        public string Intent { get; set; } = string.Empty;     
        public string Description { get; set; } = string.Empty; 
        public string TargetPath { get; set; } = string.Empty; 
        public Dictionary<string, string>? Parameters { get; set; }
    }
}

