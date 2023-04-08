## To-dos

### Código

- [ ] Implementar envio em lote para o broker

- [ ] Melhorar interação com o banco de dados
    - Implementação de UnitOfWork?
    - Trabalhar com EF para abstrair?

- [x] Remover connection string do código


### Infra

- [ ] Subir SQL Server com criação de base e tabelas (e dados?)

- [x] Subir influx configurado (usuário, senha e db)

- [ ] Subir tudo via docker-compose pra facilidade de execução (depende de todas as outras acima)

### Dashboards

- [x] Criar painel com acompanhamento de casos de febre (se alguém atingiu >=38 dentro do período, mostrar toda a evolução da pessoa)