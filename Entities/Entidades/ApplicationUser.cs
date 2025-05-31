using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entidades
{
    public class ApplicationUser: IdentityUser
    {
        [Column("USR_CPF")]
        public string? CPF { get; set; }

        [Column("FirstName")]
        public string? FirstName { get; set; }

        [Column("NormalizedFirstName")]
        public string? NormalizedFirstName { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("NormalizedName")]
        public string? NormalizedName { get; set; }
    }
}
