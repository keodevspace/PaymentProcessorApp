namespace PaymentProcessorApp;

public class PaymentService
{
    // Simula banco de dados ou cache distribuído (ex: Redis) em memória
    private readonly Dictionary<string, PaymentResult> _processedTransactions = new();

    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        // 1. Checa idempotência
        if (_processedTransactions.TryGetValue(request.IdempotencyKey, out var existingResult))
        {
            Console.WriteLine($"[LOG] Requisição duplicada detectada para chave {request.IdempotencyKey}. Retornando cache.");
            return existingResult;
        }

        // 2. Validação da regra de negócio
        if (request.Amount <= 0)
        {
            var errorResult = new PaymentResult(false, "Valor deve ser maior que zero", DateTime.UtcNow);

            // Decisão importante: Salvamos até o erro no cache de idempotência? 
            // Sim! Se a requisição falhou por regra de negócio, retentativas com a mesma chave devem retornar o mesmo erro.
            _processedTransactions[request.IdempotencyKey] = errorResult;
            return errorResult;
        }

        // 3. Processamento bem-sucedido (simulação)
        var successResult = new PaymentResult(true, $"Pagamento de R${request.Amount} aprovado!", DateTime.UtcNow);

        // Armazena no "banco/cache" vinculado à chave
        _processedTransactions[request.IdempotencyKey] = successResult;

        Console.WriteLine($"[LOG] Pagamento processado com sucesso para a chave {request.IdempotencyKey}.");
        return successResult;
    }
}


// private readonly Dictionary<string, PaymentResult> _processedTransactions: Em um cenário real de microsserviço, isso seria um Redis ou uma tabela no banco relacional com uma restrição de índice único (UNIQUE) na coluna IdempotencyKey. Em memória para o console, o Dictionary atende perfeitamente para simular a busca O(1) por chave.

// TryGetValue em vez de ContainsKey + Acesso por índice: O TryGetValue faz a busca e a extração do valor em uma única ida ao dicionário, o que é mais performático e evita exceções ou múltiplas varreduras na estrutura.

// Persistência do erro: Guardar o resultado de falhas na mesma chave é uma decisão sênior. Se o cliente enviar um pedido inválido repetidas vezes, a API deve responder de forma consistente (idempotente) sem reprocessar validações desnecessárias.