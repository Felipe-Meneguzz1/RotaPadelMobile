# 🎾 Rota Padel - Sistema de Agendamento de Quadras

Sistema mobile de agendamento de horários para quadras de padel desenvolvido com .NET MAUI e SQLite.

## 📋 Sobre o Projeto

O **Rota Padel** é uma aplicação mobile multiplataforma que permite aos usuários reservar horários em quadras de padel de forma simples e intuitiva. O sistema oferece gerenciamento completo de reservas, autenticação de usuários e visualização de disponibilidade em tempo real.

## ✨ Funcionalidades

### Autenticação
- ✅ Cadastro de novos usuários
- ✅ Login seguro
- ✅ Recuperação de senha
- ✅ Perfil de usuário

### Quadras
- 🎯 Visualização de quadras disponíveis
- 📸 Galeria de fotos das quadras
- ℹ️ Informações detalhadas (tipo de piso, iluminação, cobertura)
- 💰 Valores por horário

### Reservas
- 📅 Seleção de data e horário
- ⏰ Escolha da duração (1h, 1h30, 2h)
- 🔍 Verificação de disponibilidade em tempo real
- 📋 Histórico completo de reservas
- ❌ Cancelamento de reservas
- 🔔 Notificações de confirmação

### Gerenciamento
- 👤 Painel do usuário com suas reservas
- 🔄 Sincronização automática de dados

## 🛠️ Tecnologias Utilizadas

### Frontend
- **.NET MAUI** - Framework multiplataforma para Android e iOS
- **XAML** - Interface de usuário declarativa

### Backend & Banco de Dados
- **C#** - Linguagem de programação
- **SQLite** - Banco de dados local leve e eficiente
- **SQLite-net** - ORM para facilitar operações com banco

### Arquitetura
- **MVVM** - Model-View-ViewModel pattern
- **Dependency Injection** - Injeção de dependências nativa do MAUI
- **Repository Pattern** - Abstração da camada de dados

## 📱 Plataformas Suportadas

- ✅ Android 5.0+ (API 21+)

## 🔐 Segurança

- Senhas criptografadas com hash SHA256
- Validação de dados no cliente e servidor
- Sessão de usuário com token local
- Proteção contra SQL Injection via ORM

## 🎨 Design

- Interface intuitiva e moderna
- Tema claro/escuro adaptável
- Animações suaves
- Responsivo para diferentes tamanhos de tela

## 📈 Próximas Melhorias

- [ ] Integração com gateway de pagamento
- [ ] Sistema de avaliação das quadras
- [ ] Chat entre usuários para formar grupos
- [ ] Notificações push
- [ ] Modo offline com sincronização
- [ ] Compartilhamento de reservas
- [ ] Sistema de pontos/fidelidade

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests.

1. Faça um Fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFuncionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/NovaFuncionalidade`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👥 Autores

- **Felipe Meneguzzi** - *Desenvolvimento inicial*
- **Anderson Antonio Cagnini** - *Desenvolvimento inicial* 

---

⭐ Se este projeto te ajudou, considere dar uma estrela no repositório!
