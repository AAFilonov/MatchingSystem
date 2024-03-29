﻿using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.Entities
{
    #nullable enable
    public class MatchingInfo
    {
        [Key]
        public int MatchingID { get; set; }
        public string? MatchingName { get; set; }
    }
}