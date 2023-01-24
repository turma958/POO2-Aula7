var carrinho = new CarrinhoDeCompra();
carrinho.Add(new Produto
{
    CodProduto = Guid.NewGuid(),
    Descricao = "Puma Slipstram",
    ValorUnitario = 100,
    Quantidade = 1
});

carrinho.Pagar(new PagamentoDebito());

var compra = new Compra
{
    CarrinhoDeCompra = carrinho
};
compra.CalcularImpostos();
compra.ImprimirComprovante(new ComprovanteSMS());

Console.ReadLine();


class Compra
{
    private readonly CalculadoraImpostos _calculadoraImpostos;

    public CarrinhoDeCompra CarrinhoDeCompra { get; set; } = new CarrinhoDeCompra();

    public Compra()
    {
        _calculadoraImpostos = new CalculadoraImpostos();
    }

    public decimal CalcularImpostos()
    {
        return _calculadoraImpostos.CalcularImpostos(CarrinhoDeCompra.ValorTotal);
    }

    public void ImprimirComprovante(Comprovante comprovante)
    {
        comprovante.Imprimir(CarrinhoDeCompra.ValorTotal);
    }
}

class CalculadoraImpostos
{
    private readonly IEnumerable<IImposto> _impostos;

    public CalculadoraImpostos()
    {
        _impostos = new List<IImposto> {
            new PIS(),
            new COFINS(),
            new ICMS()
        };
    }

    public decimal CalcularImpostos(decimal valor)
    {
        return _impostos.Sum(x => x.CalcularImposto(valor));
    }
}

interface IImposto
{
    decimal CalcularImposto(decimal valor);
}

abstract class Imposto : IImposto
{
    public abstract decimal Porcentagem { get; }

    public virtual decimal CalcularImposto(decimal valor)
    {
        return Porcentagem * valor;
    }
}

class PIS : Imposto, IImposto
{
    public override decimal Porcentagem => 0.1m;
}
class COFINS : Imposto, IImposto
{
    public override decimal Porcentagem => 0.1m;
}
class ICMS : Imposto, IImposto
{
    public override decimal Porcentagem => 0.15m;
}

enum TipoComprovante
{
    Online,
    Impresso,
    CupomFiscal,
    SMS
}

class CarrinhoDeCompra
{
    public decimal ValorTotal { get; set; }
    public List<Produto> Produtos { get; set; } = new List<Produto>();

    public void Add(Produto produto)
    {
        Produtos.Add(produto);
        CalcularValorTotal();
    }

    public void CalcularValorTotal()
    {
        ValorTotal = Produtos.Sum(x =>
        {
            return x.ValorUnitario * x.Quantidade;
        });
    }

    public void Pagar(IPagamento pagamento)
    {
        pagamento.Pagar(ValorTotal);
    }
}

interface IPagamento
{
    void Pagar(decimal valor);
}

class PagamentoCredito : IPagamento
{
    public void Pagar(decimal valor)
    {
        Console.WriteLine($"Pagamento de {valor} no crédito realizado com sucesso");
    }
}

class PagamentoDebito : IPagamento
{
    public void Pagar(decimal valor)
    {
        Console.WriteLine($"Pagamento de {valor} no débito realizado com sucesso");
    }
}

class PagamentoCheque : IPagamento
{
    public void Pagar(decimal valor)
    {
        Console.WriteLine($"Pagamento de {valor} no cheque realizado com sucesso");
    }
}

class PagamentoPIX : IPagamento
{
    public void Pagar(decimal valor)
    {
        Console.WriteLine($"Pagamento de {valor} no PIX realizado com sucesso");
    }
}

abstract class Comprovante
{
    public abstract void Imprimir(decimal valor);
}

class ComprovanteImpresso : Comprovante
{
    public override void Imprimir(decimal valor)
    {
        Console.WriteLine($"Comprovante impresso {valor}");
    }
}

class ComprovanteOnline : Comprovante
{
    public override void Imprimir(decimal valor)
    {
        Console.WriteLine($"Comprovante online de {valor}");
    }
}

class ComprovanteSMS : Comprovante
{
    public override void Imprimir(decimal valor)
    {
        Console.WriteLine($"Comprovante SMS {valor}");
    }
}

class Produto
{
    public Guid CodProduto { get; set; }
    public string Descricao { get; set; }
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }
}