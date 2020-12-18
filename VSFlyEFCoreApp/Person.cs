using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace VSFlyEFCoreApp
{
    public class Person
    {
        [Key]
        public int PersonID { get; set; }

        public string Surname { get; set; }

        public string GivenName { get; set; }
    }
}
