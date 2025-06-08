using RestauranteApi.Models;

public static class DbInitializer
{
    public static void Seed(this AppDbContext context)
    {
        if (context.Restaurantes.Any()) return;

        var restaurantes = Enumerable.Range(1, 10).Select(i => new Restaurante
        {
            Nome = $"Restaurante {i}",
            Categoria = "Categoria " + ((i % 3) + 1),
            Localizacao = $"Localização {i}"
        }).ToList();

        context.Restaurantes.AddRange(restaurantes);
        context.SaveChanges();

        var pedidos = new List<Pedido>();
        var itensPedido = new List<ItemPedido>();
        var avaliacoes = new List<Avaliacao>();

        foreach (var restaurante in restaurantes)
        {
            for (int i = 1; i <= 10; i++)
            {
                var pedido = new Pedido
                {
                    RestauranteId = restaurante.Id
                };

                var itens = Enumerable.Range(1, 3).Select(j => new ItemPedido
                {
                    NomePrato = $"Prato {j} do Pedido {i} do {restaurante.Nome}",
                    Quantidade = j,
                    PrecoUnitario = 10 + j * 2,
                    Pedido = pedido
                }).ToList();

                pedido.Itens.AddRange(itens);
                pedido.CalcularValorTotal();

                var avaliacao = new Avaliacao
                {
                    Nota = (i % 5) + 1,
                    Comentario = $"Comentário {i} do {restaurante.Nome}",
                    Pedido = pedido
                };

                pedidos.Add(pedido);
                itensPedido.AddRange(itens);
                avaliacoes.Add(avaliacao);
            }
        }

        context.Pedidos.AddRange(pedidos);
        context.ItensPedido.AddRange(itensPedido);
        context.Avaliacoes.AddRange(avaliacoes);

        context.SaveChanges();
    }
}