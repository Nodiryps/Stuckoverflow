using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace prid1920_g10.Models {
    public class PostTag {
        public int PostId { get; set; }
        public int TagId { get; set; } 
        public virtual Post Post { get; set; }
        public virtual Tag Tag { get; set; }
    }
}