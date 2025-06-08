using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestauranteApi.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        [JsonIgnore]
        public Pedido? Pedido { get; set; }

        public string NomePrato { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public double PrecoUnitario { get; set; }
    }
}