
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

enum TipoPagamento
{
    Debito,
    Credito,
    Cheque,
    PIX
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

    public void Pagar(TipoPagamento tipoPagamento)
    {
        switch (tipoPagamento)
        {
            case TipoPagamento.Cheque:
                Console.WriteLine("Pagamento via cheque");
                break;
            case TipoPagamento.Credito:
                Console.WriteLine("Pagamento via crédito");
                break;
            case TipoPagamento.Debito:
                Console.WriteLine("Pagamento via débito");
                break;
            case TipoPagamento.PIX:
                Console.WriteLine("Pagamento via PIX");
                break;
            default:
                throw new Exception("Tipo de pagamento não suportado");
        }
    }

    public void ImprimirComprovante(TipoComprovante tipoComprovante)
    {
        switch (tipoComprovante)
        {
            case TipoComprovante.Online:
                Console.WriteLine("Comprovante online");
                break;
            case TipoComprovante.Impresso:
                Console.WriteLine("Comprovante impresso");
                break;
            case TipoComprovante.CupomFiscal:
                Console.WriteLine("Comprovante cupom fiscal");
                break;
            case TipoComprovante.SMS:
                Console.WriteLine("Comprovante SMS");
                break;
        }
    }
}

class Produto
{
    public Guid CodProduto { get; set; }
    public string Descricao { get; set; }
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }
}