## To-dos

### Código

- [ ] Usar Polly para conectar ao banco

- [x] Injetar config do RabbitMQ

- [x] Implementar envio em lote para o broker

- [ ] Melhorar interação com o banco de dados
    - Implementação de UnitOfWork?
    - Trabalhar com EF para abstrair?

- [x] Remover connection string do código


### Infra

- [x] Subir SQL Server com criação de base e tabelas (e dados?)

- [x] Subir tudo via docker-compose pra facilidade de execução (depende de todas as outras acima)

- [x] Subir influx configurado (usuário, senha e db)

### Dashboards

- [x] Criar painel com acompanhamento de casos de febre (se alguém atingiu >=38 dentro do período, mostrar toda a evolução da pessoa)