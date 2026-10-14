Para garantir idempotência em operações críticas como pagamentos, utilizo o padrão de Idempotency Key. No serviço, mantenho uma camada de verificação (que na produção aponta para um Redis ou banco com índice unique). Quando a requisição chega, checo se a chave já existe no repositório; se existir, retorno o resultado armazenado da primeira execução sem tocar na regra de negócio principal. Isso evita cobranças duplicadas em cenários de falha de rede onde o cliente faz retentativas automáticas.

Processador Idempotente de Pagamentos
- O Problema: Duplo clique do usuário ou retentativa automática de rede (F5) cobrando o cartão e entregando o item duas vezes.
- A Solução: Garantir Idempotency Key. Se a mesma chave vier de novo, a API devolve o cache da primeira resposta sem reprocessar.

📝 Passo a Passo & Decisões no Código
- Solution & Separação: dotnet new sln separando Console App de Testes (xUnit). Padrão de mercado.

- DTOs Imutáveis (record): Uso de record em vez de class para gerar payloads imutáveis com value equality (essencial para rastreabilidade e testes).

- Cache de Memória (Dictionary): Simula um Redis ou banco relacional com índice UNIQUE.

- Busca Performática (TryGetValue): Busca e extração em uma única ida ao dicionário (evita varreduras duplas e exceções).

- Persistência de Erro: Validações de negócio inválidas também são salvas no dicionário de idempotência. Se o cliente insistir no erro, recebe a mesma resposta consistente.
