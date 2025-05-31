using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entidades
{
    [Table("SistemaFinanceiro")]
    public class SistemaFinanceiro: Base
    {
        [Key]
        public int SistemaFinanceiroID { get; set; }

        [Display(Name = "Nome")]
        public string? SistemaFinanceiroNome { get; set; }
        public int Mes {  get; set; }
        public int Ano { get; set; }
        public int DiaFechamento { get; set; }
        public bool GerarCopiaDespesas { get; set; }
        public int MesCopia { get; set; }
        public int AnoCopia { get; set; }
    }
}
