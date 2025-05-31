using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities.Entidades
{
    [Table("Despesa")]
    public class Despesa: Base
    {
        [Key]
        public int DespesaID { get; set; }
        [Display(Name = "Nome")]
        public string? DespesaNome { get; set; }
        public decimal Valor { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public EumTipoDespesa TipoDespesa { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public DateTime DataPagamento { get; set; } 
        public DateTime DataVencimento { get; set; }
        public bool Pago { get; set; }
        public bool DespesaAtrasada { get; set; }

        [ForeignKey(nameof(Categoria))]
        [Column(Order = 1)]
        public int CategoriaID { get; set; }
        public virtual Categoria? Categoria { get; set; }

        [JsonIgnore]
        [Display(Name = "Juros")]
        public decimal Juros { get; set; }
        [JsonIgnore]
        [Display(Name = "Valor Total")]
        public decimal ValorTotal => Valor + Juros;
    }
}
