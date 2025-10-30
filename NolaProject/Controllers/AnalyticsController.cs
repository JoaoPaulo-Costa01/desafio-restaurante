using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NolaProject.Models; // << Verifique se este é o namespace do seu projeto
using System; // Adicionado para DayOfWeek
using System.Collections.Generic; // Adicionado para List<>
using System.Linq; // Adicionado para LINQ methods like Where, GroupBy, Select, etc.
using System.Threading.Tasks; // Adicionado para Task<>

namespace NolaProject.Controllers // << Verifique se este é o namespace
{
    // DTO (Data Transfer Object) - A classe que representa nossa "resposta"
    public class TopProductDto {
        public string? ProductName { get; set; }
        public int TotalSold { get; set; }
    }

    // DTO para o Dataset (ex: uma linha para "iFood", outra para "Rappi")
    public class DatasetDto {
        public string Label { get; set; } = string.Empty;
        public List<decimal?> Data { get; set; } = new();
    }

    // DTO para a Resposta Completa (formato exato que o Chart.js precisa)
    public class TicketMedioPorCanalDto {
        public List<string> Labels { get; set; } = new(); // Os dias (ex: "28/10", "29/10")
        public List<DatasetDto> Datasets { get; set; } = new();
    }

    // DTO para o gráfico de Tempo Médio de Entrega
    public class TempoMedioEntregaDto {
        public List<string> Labels { get; set; } = new(); // Os dias (ex: "28/10", "29/10")
        public List<double?> Data { get; set; } = new(); // A média de tempo em minutos
    }

    // DTO para representar um Cliente Inativo
    public class ClienteInativoDto {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int TotalCompras { get; set; }
        public DateTime? UltimaCompra { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase {
        // 1. Variável para guardar a conexão com o banco
        private readonly NolaDbContext _context;

        // 2. Construtor (Injeção de Dependência)
        public AnalyticsController(NolaDbContext context) {
            _context = context;
        }

        // 3. Endpoint Top Produtos
        [HttpGet("top-products")]
        public async Task<ActionResult<IEnumerable<TopProductDto>>> GetTopProducts(
            [FromQuery] int channelId,
            [FromQuery] DayOfWeek dayOfWeek) {
            var topProducts = await _context.Sales
                .Where(s => s.SaleStatusDesc == "COMPLETED")
                .Where(s => s.ChannelId == channelId)
                .Where(s => s.CreatedAt.HasValue && s.CreatedAt.Value.DayOfWeek == dayOfWeek)
                .Join(
                    _context.ProductSales,
                    sale => sale.Id,
                    productSale => productSale.SaleId,
                    (sale, productSale) => productSale
                )
                .Join(
                    _context.Products,
                    productSale => productSale.ProductId,
                    product => product.Id,
                    (productSale, product) => new { productSale, product }
                )
                .GroupBy(x => x.product.Name)
                .Select(g => new TopProductDto {
                    ProductName = g.Key,
                    TotalSold = g.Sum(x => x.productSale.Quantity ?? 0)
                })
                .OrderByDescending(dto => dto.TotalSold)
                .Take(10)
                .ToListAsync();

            return Ok(topProducts);
        }

        // 4. Endpoint Ticket Médio por Canal
        [HttpGet("ticket-medio-por-canal")]
        public async Task<ActionResult<TicketMedioPorCanalDto>> GetTicketMedioPorCanal(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate) {
            var stats = await _context.Sales
                .Where(s => s.SaleStatusDesc == "COMPLETED" && s.CreatedAt.HasValue)
                .Where(s => s.CreatedAt.Value.Date >= startDate.Date && s.CreatedAt.Value.Date <= endDate.Date)
                .GroupBy(s => new {
                    Date = s.CreatedAt.Value.Date,
                    s.ChannelId
                })
                .Select(g => new {
                    g.Key.Date,
                    g.Key.ChannelId,
                    TicketMedio = g.Sum(s => s.TotalAmount) / g.Count() // EF Core handles division correctly
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            if (!stats.Any()) {
                return Ok(new TicketMedioPorCanalDto());
            }

            var channelIds = stats.Select(s => s.ChannelId).Distinct().ToList();
            var channels = await _context.Channels
                .Where(c => channelIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.Name);

            var allDates = stats
                .Select(s => s.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            var result = new TicketMedioPorCanalDto {
                Labels = allDates.Select(d => d.ToString("dd/MM")).ToList()
            };

            foreach (var channel in channels) {
                var dataset = new DatasetDto { Label = channel.Value ?? "Desconhecido" };

                foreach (var date in allDates) {
                    var statForDay = stats.FirstOrDefault(s => s.Date == date && s.ChannelId == channel.Key);
                    if (statForDay != null) {
                        // Explicitly cast TotalAmount to decimal? before summing if needed,
                        // but Sum typically handles nullables correctly.
                        // Ensure TicketMedio calculation handles potential division by zero if Count can be 0.
                        // EF Core division usually promotes to decimal if operands are decimal/int.
                        dataset.Data.Add(Math.Round(statForDay.TicketMedio ?? 0m, 2)); // Use 'm' for decimal literal
                    } else {
                        dataset.Data.Add(null);
                    }
                }
                result.Datasets.Add(dataset);
            }

            return Ok(result);
        } // <--- ESTA CHAVE ESTAVA FALTANDO ANTES

        // 5. NOVO Endpoint Tempo Médio de Entrega
        [HttpGet("tempo-medio-entrega")]
        public async Task<ActionResult<TempoMedioEntregaDto>> GetTempoMedioEntrega(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate) {
            var stats = await _context.Sales
                .Where(s => s.SaleStatusDesc == "COMPLETED"
                            && s.DeliverySeconds.HasValue
                            && s.CreatedAt.HasValue)
                .Where(s => s.CreatedAt.Value.Date >= startDate.Date && s.CreatedAt.Value.Date <= endDate.Date)
                .GroupBy(s => s.CreatedAt.Value.Date)
                .Select(g => new {
                    Date = g.Key,
                    // Use AverageAsync for potentially better performance on large datasets
                    TempoMedioMinutos = g.Average(s => (double?)s.DeliverySeconds / 60.0)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            if (!stats.Any()) {
                return Ok(new TempoMedioEntregaDto());
            }

            var result = new TempoMedioEntregaDto {
                Labels = stats.Select(s => s.Date.ToString("dd/MM")).ToList(),
                Data = stats.Select(s => s.TempoMedioMinutos.HasValue ? Math.Round(s.TempoMedioMinutos.Value, 1) : (double?)null).ToList()
            };

            return Ok(result);
        }

        [HttpGet("clientes-inativos")]
        public async Task<ActionResult<IEnumerable<ClienteInativoDto>>> GetClientesInativos(
    [FromQuery] int minCompras = 3,
    [FromQuery] int diasInativo = 30) {
            // A CORREÇÃO ESTÁ AQUI: Usando DateTime.Now em vez de DateTime.UtcNow
            var dataLimite = DateTime.Now.AddDays(-diasInativo).Date;

            // 1. Encontra os dados agregados (ID, Total, Ultima Compra) dos clientes que atendem aos critérios
            var clientesFieisStats = await _context.Sales
                .Where(s => s.SaleStatusDesc == "COMPLETED" && s.CustomerId.HasValue)
                .GroupBy(s => s.CustomerId.Value)
                .Select(g => new {
                    CustomerId = g.Key,
                    TotalCompras = g.Count(),
                    UltimaCompra = g.Max(s => s.CreatedAt) // Assume CreatedAt is nullable DateTime?
                })
                // A comparação agora funciona porque dataLimite não é mais UTC Kind
                .Where(x => x.TotalCompras >= minCompras &&
                            x.UltimaCompra.HasValue &&
                            x.UltimaCompra.Value.Date < dataLimite)
                .ToListAsync();

            if (!clientesFieisStats.Any()) {
                return Ok(new List<ClienteInativoDto>());
            }

            var idsClientesInativos = clientesFieisStats.Select(c => c.CustomerId).ToList();

            var customerDetails = await _context.Customers
                .Where(c => idsClientesInativos.Contains(c.Id))
                .Select(c => new { c.Id, c.CustomerName, c.Email, c.PhoneNumber })
                .ToListAsync();

            var resultado = clientesFieisStats
                .Join(
                    customerDetails,
                    stats => stats.CustomerId,
                    details => details.Id,
                    (stats, details) => new ClienteInativoDto {
                        CustomerId = stats.CustomerId,
                        CustomerName = details.CustomerName,
                        Email = details.Email,
                        PhoneNumber = details.PhoneNumber,
                        TotalCompras = stats.TotalCompras,
                        UltimaCompra = stats.UltimaCompra
                    })
                .OrderByDescending(c => c.UltimaCompra)
                .ToList();

            return Ok(resultado);
        }

    } // Fim da classe AnalyticsController
} // Fim do namespace