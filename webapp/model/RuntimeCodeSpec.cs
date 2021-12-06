using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webapp.model
{
    public class RuntimeCodeSpec
    {
        [Required]
        [DefaultValue("return _ => _ * 2;")]
        public String Code { get; set; }

        [Required]
        [DefaultValue("10")]
        public int Input { get; set; }
    }
}
