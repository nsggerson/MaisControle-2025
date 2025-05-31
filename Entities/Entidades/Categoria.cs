using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entidades
{
    [Table("Categoria")]
    public class Categoria: Base
    {
        [Key]
        public int CategoriaID { get; set; }
        [Display(Name = "Nome")]
        public string? CategoriaNome { get; set; }

        [ForeignKey("SistemaFinanceiro")]
        [Column(Order = 1)]
        public int SistemaID { get; set; }  
        public virtual SistemaFinanceiro? SistemaFinanceiro { get; set; }
    }
}
