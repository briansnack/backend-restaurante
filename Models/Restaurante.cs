using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestauranteApi.Models
{
    public class Restaurante
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        [JsonIgnore]

        public List<Pedido> Pedidos { get; set; } = [];
    }
}