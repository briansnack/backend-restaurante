using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestauranteApi.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int RestauranteId { get; set; }
        [JsonIgnore]
        public Restaurante? Restaurante { get; set; }

        public List<ItemPedido> Itens { get; set; } = [];

        public double ValorTotal { get; set; } = 0;

        [JsonIgnore]
        public Avaliacao? Avaliacao { get; set; }

        public void CalcularValorTotal()
        {
            ValorTotal = Itens.Sum(i => i.PrecoUnitario * i.Quantidade);
        }
    }
}