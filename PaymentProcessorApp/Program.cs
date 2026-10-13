using PaymentProcessorApp;

var service = new PaymentService();
var key = Guid.NewGuid().ToString();

var req1 = new PaymentRequest(key, 150.00m, "ACC-123");
var req2 = new PaymentRequest(key, 150.00m, "ACC-123"); // Mesma chave!

Console.WriteLine("--- Primeira Chamada ---");
var res1 = service.ProcessPayment(req1);

Console.WriteLine("\n--- Segunda Chamada (Duplicada) ---");
var res2 = service.ProcessPayment(req2); // (Nota: aqui passamos req2 com a mesma chave)


// Demonstra visualmente no console que a primeira chamada executa o fluxo (gera log de processamento) e a segunda chamada intercepta a chave e retorna o resultado instantaneamente do "cache".

// Para garantir idempotência em operações críticas como pagamentos, utilizo o padrão de Idempotency Key. No serviço, mantenho uma camada de verificação (que na produção aponta para um Redis ou banco com índice unique). Quando a requisição chega, checo se a chave já existe no repositório; se existir, retorno o resultado armazenado da primeira execução sem tocar na regra de negócio principal. Isso evita cobranças duplicadas em cenários de falha de rede onde o cliente faz retentativas automáticas.