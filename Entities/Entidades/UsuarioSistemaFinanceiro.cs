using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entidades
{
    [Table("UsuarioSistemaFinanceiro")]
    public class UsuarioSistemaFinanceiro
    {
        [Key]
        public int UsuarioSistemaFinanceiroID { get; set; }
        public string? EmailUsuario { get; set; }
        public bool Administrador { get; set; }
        public bool SistemaAtual { get; set; }

        [ForeignKey("SistemaFinanceiro")]
        [Column(Order = 1)]
        public int SistemaID { get; set; }
        public SistemaFinanceiro? SistemaFinanceiro { get; set; }
    }
}
