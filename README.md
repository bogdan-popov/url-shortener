# UrlShortener API

**UrlShortener API** – это высокопроизводительный бэкенд-сервис на ASP.NET Core для сокращения URL-адресов, построенный с использованием принципов чистой архитектуры. Проект включает в себя JWT-аутентификацию, полный CRUD-цикл для управления ссылками, а также кэширование на Redis для обеспечения быстрых редиректов.

---

## 🚀 Стек технологий

*   **Фреймворк:** .NET 8, ASP.NET Core
*   **Архитектура:** Многослойная архитектура (Controllers, Services, Repositories), Dependency Injection, Unit of Work, SOLID
*   **База данных:** PostgreSQL
*   **ORM:** Entity Framework Core (Code-First)
*   **Кэширование:** Redis (для распределенного кэширования горячих ссылок)
*   **Аутентификация и Авторизация:** JWT (JSON Web Tokens)
*   **Маппинг:** AutoMapper
*   **Валидация:** FluentValidation
*   **Обработка ошибок:** Custom Middleware для централизованной обработки исключений
*   **Контейнеризация:** Docker
*   **Тестирование:** xUnit, Moq, FluentAssertions

---

## ✨ Основные возможности (API Endpoints)

### Аутентификация (`/api/auth`)

*   `POST /register`: Регистрация нового пользователя.
*   `POST /login`: Аутентификация и получение JWT-токена.

### Управление ссылками (`/api/url`)

*   `POST /shorten` **(🔒 Защищено)**: Создание новой сокращенной ссылки.
*   `GET /my-urls` **(🔒 Защищено)**: Получение списка всех ссылок, созданных текущим пользователем.
*   `DELETE /{shortCode}` **(🔒 Защищено)**: Удаление ссылки, принадлежащей текущему пользователю.

### Редирект

*   `GET /{shortCode}` **(Публичный)**: Перенаправление на оригинальный URL. Использует кэш Redis для минимизации задержек.

---

## ⚙️ Как запустить локально

1.  **Клонируйте репозиторий:**
    ```bash
    git clone https://github.com/bogdan-popov/UrlShortener.git
    cd UrlShortener
    ```

2.  **Настройте `appsettings.Development.json`:**
    Укажите вашу строку подключения к PostgreSQL и секретный ключ для JWT в секции `JwtSettings`.

3.  **Запустите Redis в Docker:**
    ```bash
    docker run --name some-redis -p 6379:6379 -d redis
    ```

4.  **Примените миграции EF Core:**
    В окне Package Manager Console в Visual Studio выполните:
    ```bash
    Update-Database
    ```

5.  **Запустите приложение** из Visual Studio (F5) или через команду `dotnet run`.
