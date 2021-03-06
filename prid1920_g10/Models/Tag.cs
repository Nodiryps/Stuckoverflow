using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace prid1920_g10.Models {
    public class Tag {
        [Key]
        [Required(ErrorMessage = "Required")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        public int NbOcc {
            get => (from pt in this.PostTags
                    where pt.TagId == this.Id
                    select pt.TagId).Count();
        }

        public virtual IList<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}