using PaymentProcessorApp;
using Xunit;

public class PaymentServiceTests
{
    [Fact]
    public void ProcessPayment_DuplicateKey_ShouldReturnCachedResult()
    {
        // Arrange (Preparação)
        var service = new PaymentService();
        var key = "KEY-TEST-01";
        var req = new PaymentRequest(key, 100m, "ACC-1");

        // Act (Ação: Primeira e segunda chamada)
        var res1 = service.ProcessPayment(req);
        var res2 = service.ProcessPayment(req);

        // Assert (Validação)
        // Garantimos que o timestamp da transação processada é exatamente o mesmo,
        // provando que a segunda chamada não recalculou nada e veio do cache.
        Assert.Equal(res1.ProcessedAt, res2.ProcessedAt);
        Assert.True(res2.Success);
    }
}


// O teste foca em provar o comportamento de negócio (idempotência). Usar o ProcessedAt como comparador é um excelente truque de teste unitário para validar que o objeto retornado na segunda chamada é literalmente o mesmo que foi salvo na primeira.