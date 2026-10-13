namespace PaymentProcessorApp;
public record PaymentResult(bool Success, string Message, DateTime ProcessedAt);

// public record ...: Usamos record em vez de class tradicional porque objetos de transferência de dados (DTOs) ou payloads de requisição devem ser imutáveis. Um record gera propriedades init-only por padrão e implementa igualdade baseada em valores (value equality), o que é excelente para testes e rastreabilidade.

// Campos da Requisição: O IdempotencyKey é o coração do padrão. É um identificador único (geralmente um UUID gerado pelo cliente/frontend) que diz: "Estou tentando pagar isso aqui. Se você já recebeu essa chave, não cobre de novo".