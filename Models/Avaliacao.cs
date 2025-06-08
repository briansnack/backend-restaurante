using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestauranteApi.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        [JsonIgnore]
        public Pedido? Pedido { get; set; }

        public int Nota { get; set; } // 1 a 5
        public string Comentario { get; set; } = string.Empty;
    }
}