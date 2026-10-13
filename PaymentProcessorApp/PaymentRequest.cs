namespace PaymentProcessorApp;

public record PaymentRequest(string IdempotencyKey, decimal Amount, string AccountId);