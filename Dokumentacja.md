# QuizMaster - dokumentacja projektu

## Wprowadzenie

**QuizMaster** to aplikacja wspierająca naukę z wykorzystaniem fiszek. System umożliwia tworzenie własnych zestawów fiszek, edytowanie ich zawartości, udostępnianie zestawów publicznie oraz prowadzenie sesji nauki, w których użytkownik odpowiada na kolejne pytania i ocenia poprawność swojej odpowiedzi.

Aplikacja została zrealizowana w języku **C#** z wykorzystaniem platformy **.NET**. Część serwerowa została przygotowana jako **REST API** w technologii **ASP.NET Core**, natomiast część kliencka została wykonana jako aplikacja desktopowa w technologii **WPF**. Do obsługi bazy danych wykorzystano **Entity Framework Core** oraz bazę danych **SQL Server**.

Rozwiązanie składa się z dwóch głównych części:

- **QuizMaster REST API** — backend odpowiedzialny za logikę biznesową, autoryzację, obsługę danych i komunikację z bazą danych.
- **QuizMaster WPF** — desktopowy klient graficzny, który komunikuje się z API i udostępnia użytkownikowi interfejs aplikacji.

System został zaprojektowany jako aplikacja klient-serwer. Aplikacja WPF nie komunikuje się bezpośrednio z bazą danych. Wszystkie operacje wykonywane są przez REST API, które pośredniczy pomiędzy interfejsem użytkownika a warstwą danych.

Takie podejście pozwala oddzielić logikę biznesową od interfejsu użytkownika, zwiększa czytelność projektu oraz umożliwia potencjalne dodanie innych klientów aplikacji w przyszłości, na przykład aplikacji webowej lub mobilnej.

---

## Cel aplikacji

Celem aplikacji jest umożliwienie użytkownikowi wygodnej nauki przy pomocy fiszek. Użytkownik może tworzyć zestawy tematyczne, dodawać do nich pytania i odpowiedzi, a następnie rozpoczynać sesje nauki.

Aplikacja pozwala również na korzystanie z publicznych zestawów innych użytkowników. Publiczny zestaw można skopiować na własne konto i dalej używać go jako prywatnego lub własnoręcznie modyfikowanego materiału do nauki.

System ma również na celu pokazanie praktycznego zastosowania architektury warstwowej, komunikacji REST API z aplikacją desktopową, autoryzacji użytkownika oraz obsługi danych w bazie relacyjnej.

---

## Zakres aplikacji

Aplikacja obejmuje następujące obszary funkcjonalne:

- rejestrację i logowanie użytkowników,
- obsługę autoryzacji z wykorzystaniem tokenów JWT,
- zarządzanie własnymi zestawami fiszek,
- dodawanie, edytowanie i usuwanie fiszek,
- oznaczanie zestawów jako publiczne lub prywatne,
- przeglądanie i filtrowanie publicznych zestawów,
- kopiowanie publicznych zestawów na konto użytkownika,
- rozpoczynanie i kontynuowanie sesji nauki,
- ocenianie odpowiedzi użytkownika jako poprawnych lub błędnych,
- prezentację podstawowych statystyk na dashboardzie.

Aplikacja nie jest jedynie prostym edytorem fiszek. System posiada mechanizm sesji nauki, dzięki któremu użytkownik może rozpocząć naukę wybranego zestawu, przechodzić przez kolejne fiszki, korzystać z podpowiedzi oraz śledzić postęp bieżącej sesji.

---

## Wykorzystanie zagadnień z laboratoriów

Projekt **QuizMaster** został zrealizowany z wykorzystaniem zagadnień omawianych podczas laboratoriów z przedmiotu **Programowanie zaawansowane**. Poszczególne mechanizmy zostały użyte w praktycznych elementach aplikacji, a nie wyłącznie jako przykłady demonstracyjne.


| Laboratorium | Główne zagadnienie | Wykorzystanie w projekcie |
|---|---|---|
| 1. Polimorfizm | klasy, dziedziczenie, metody wirtualne, praca na obiektach | Projekt opiera się na modelach domenowych reprezentujących użytkowników, zestawy fiszek, fiszki, kategorie i sesje nauki. Klasy posiadają jasno określone odpowiedzialności i są wykorzystywane w wielu warstwach aplikacji. |
| 2. Wyjątki | własne klasy wyjątków i obsługa sytuacji błędnych | W aplikacji zdefiniowano własne wyjątki, np. dla braku zestawu, braku dostępu, zakończonej sesji nauki lub przekroczenia limitu aktywnych sesji. Wyjątki są rzucane w warstwie aplikacyjnej i mapowane na odpowiedzi API. |
| 3. Interfejsy | abstrakcja i baza wspólnego kodu | Interfejsy opisują kontrakty serwisów, klienta API oraz usług pomocniczych. Dzięki temu kontrolery i widoki WPF korzystają z abstrakcji, a nie bezpośrednio z konkretnych implementacji. |
| 4. Generyczność | typy i metody generyczne | Generyczność została wykorzystana m.in. w uniwersalnym kliencie HTTP. Metody typu `GetAsync<T>`, `PostAsync<TRequest, TResponse>` i `PutAsync<TRequest>` pozwalają obsługiwać różne typy DTO jednym wspólnym mechanizmem. |
| 5. Delegaty | przekazywanie metod i zdarzenia | Delegaty i zdarzenia zostały użyte w aplikacji WPF do komunikacji pomiędzy oknami. Przykładowo okno tworzenia lub edycji fiszki informuje widok nadrzędny o zakończeniu operacji, co pozwala odświeżyć listę danych. |
| 6. Zaawansowane konstrukcje C# | metody rozszerzeniowe, inicjalizatory, zdarzenia, praca z obiektami | W projekcie wykorzystano inicjalizatory obiektów przy tworzeniu komend i DTO, zdarzenia w WPF oraz metody pomocnicze rozszerzające działanie aplikacji, np. pobieranie identyfikatora aktualnego użytkownika w kontrolerach. |
| 7. LINQ | filtrowanie, sortowanie, projekcje i agregacje | LINQ jest używany głównie po stronie backendu razem z Entity Framework Core. Zapytania służą do filtrowania publicznych zestawów, sortowania danych, liczenia fiszek w zestawie, pobierania aktywnych sesji oraz przygotowywania danych dashboardu. |
| 8. Aplikacja desktopowa | graficzny interfejs użytkownika | Część kliencka została wykonana jako aplikacja desktopowa WPF. Zawiera okna logowania, rejestracji, dashboard, widoki zestawów, publicznych zestawów oraz okno sesji nauki. |
| 10. ASP.NET Core MVC / aplikacja webowa | kontrolery, routing, modele, Entity Framework, CRUD | Backend został wykonany jako REST API w ASP.NET Core. Aplikacja zawiera kontrolery, routing, autoryzację JWT, operacje CRUD, obsługę bazy danych przez Entity Framework Core oraz komunikację JSON z klientem WPF. |

Wymagania projektu zaliczeniowego wskazywały między innymi konieczność użycia warstw abstrakcji, konkretnych klas implementujących logikę, własnych wyjątków, delegatów, LINQ oraz przejrzystego interfejsu graficznego. Wszystkie te elementy zostały wykorzystane w projekcie QuizMaster.

---

## Podział odpowiedzialności projektów(🔴Ukryte wymaganie)

Rozwiązanie zostało podzielone na kilka osobnych projektów. Każdy z nich pełni określoną rolę w aplikacji i odpowiada za inny obszar systemu. Taki podział pozwala oddzielić logikę biznesową od interfejsu użytkownika, warstwy dostępu do danych oraz kontraktów komunikacyjnych.

Głównym założeniem było to, aby aplikacja desktopowa WPF nie zawierała logiki biznesowej ani nie komunikowała się bezpośrednio z bazą danych. Klient WPF odpowiada wyłącznie za prezentację danych i obsługę interakcji użytkownika, natomiast wszystkie operacje na danych wykonywane są po stronie REST API.

### QuizMaster.Core

Projekt `QuizMaster.Core` zawiera podstawowe elementy domenowe aplikacji. Znajdują się w nim klasy reprezentujące główne obiekty systemu, typy wyliczeniowe oraz obiekty DTO wykorzystywane do przesyłania danych pomiędzy warstwami aplikacji.

Do tego projektu należą między innymi modele:

- `User`,
- `FlashcardSet`,
- `Flashcard`,
- `Category`,
- `LearningSession`,
- `LearningSessionItem`,
- `UserFlashcardProgress`.

Projekt ten zawiera również typy pomocnicze, takie jak poziom trudności fiszki, oraz obiekty DTO wykorzystywane przez API i aplikację WPF, na przykład:

- `FlashcardSetListItemDto`,
- `LearningSessionDto`,
- `LearningFlashcardDto`,
- `AnswerFlashcardResultDto`.

`QuizMaster.Core` jest projektem bazowym, który nie powinien zawierać logiki zależnej od interfejsu użytkownika, kontrolerów HTTP ani szczegółów technicznych związanych z bazą danych.

### QuizMaster.Contracts

Projekt `QuizMaster.Contracts` zawiera kontrakty wykorzystywane przez pozostałe warstwy aplikacji. Umieszczono w nim przede wszystkim komendy, interfejsy serwisów oraz wyjątki domenowe.

Komendy reprezentują dane wejściowe przekazywane do operacji biznesowych. Przykładami takich komend są:

- `CreateFlashcardSetCommand`,
- `UpdateFlashcardSetCommand`,
- `CreateFlashcardCommand`,
- `UpdateFlashcardCommand`,
- `StartLearningSessionCommand`,
- `AnswerFlashcardCommand`.

W projekcie znajdują się również interfejsy określające dostępne operacje serwisów aplikacyjnych, na przykład serwisu zestawów fiszek, serwisu fiszek, serwisu sesji nauki lub serwisu autoryzacji.

Dodatkowo `QuizMaster.Contracts` zawiera wyjątki wykorzystywane do sygnalizowania błędów domenowych, takich jak brak dostępu do zasobu, brak odnalezienia fiszki, zakończona sesja nauki lub przekroczenie limitu aktywnych sesji.

Oddzielenie kontraktów od implementacji pozwala zachować czytelny podział pomiędzy tym, co system udostępnia, a tym, w jaki sposób dana funkcjonalność została zaimplementowana.

### QuizMaster.Infrastructure

Projekt `QuizMaster.Infrastructure` odpowiada za dostęp do danych oraz konfigurację bazy danych. Znajduje się w nim kontekst Entity Framework Core, czyli `QuizMasterDbContext`.

Warstwa infrastruktury definiuje:

- zbiory danych `DbSet`,
- relacje pomiędzy encjami,
- konfigurację kluczy głównych i obcych,
- sposób usuwania powiązanych danych,
- mapowanie modeli domenowych na tabele bazy danych.

Ten projekt jest jedyną warstwą, która bezpośrednio komunikuje się z bazą danych. Pozostałe projekty korzystają z danych za pośrednictwem serwisów aplikacyjnych.

Dzięki temu szczegóły techniczne związane z bazą danych są odseparowane od kontrolerów API oraz aplikacji WPF.

### QuizMaster.Application

Projekt `QuizMaster.Application` zawiera właściwą logikę biznesową aplikacji. To w tej warstwie znajdują się implementacje serwisów odpowiedzialnych za wykonywanie operacji systemowych.

Warstwa aplikacyjna odpowiada między innymi za:

- tworzenie zestawów fiszek,
- edycję zestawów fiszek,
- usuwanie zestawów,
- kopiowanie publicznych zestawów na konto użytkownika,
- dodawanie i edytowanie fiszek,
- rozpoczynanie sesji nauki,
- pobieranie kolejnych fiszek w sesji,
- zapisywanie odpowiedzi użytkownika,
- kończenie sesji nauki,
- przygotowywanie danych dashboardu.

Serwisy aplikacyjne korzystają z `QuizMasterDbContext`, wykonują walidację dostępu do danych oraz rzucają odpowiednie wyjątki domenowe w przypadku błędów.

Przykładowo, jeżeli użytkownik próbuje edytować zestaw fiszek, który do niego nie należy, warstwa aplikacyjna wykrywa taki przypadek i zgłasza błąd dostępu. Dzięki temu kontrolery API nie muszą zawierać szczegółowej logiki biznesowej.

### QuizMaster.WebApi

Projekt `QuizMaster.WebApi` jest backendem aplikacji. Udostępnia REST API, z którego korzysta aplikacja WPF.

Warstwa API odpowiada za:

- odbieranie żądań HTTP,
- przekazywanie danych do serwisów aplikacyjnych,
- pobieranie identyfikatora aktualnie zalogowanego użytkownika,
- zwracanie odpowiedzi HTTP,
- obsługę autoryzacji JWT,
- obsługę wyjątków,
- konfigurację Swaggera,
- rejestrację zależności.

Kontrolery znajdujące się w tym projekcie nie powinny zawierać rozbudowanej logiki biznesowej. Ich zadaniem jest przyjęcie żądania, przekazanie go do odpowiedniego serwisu oraz zwrócenie wyniku.

Przykładowo `FlashcardSetController` udostępnia endpointy do tworzenia, pobierania, edytowania, kopiowania i usuwania zestawów fiszek. Sama logika tych operacji znajduje się jednak w serwisie aplikacyjnym.

### QuizMaster.Wpf

Projekt `QuizMaster.Wpf` jest aplikacją desktopową stanowiącą graficzny interfejs użytkownika. Odpowiada za prezentowanie danych oraz obsługę interakcji użytkownika.

W projekcie znajdują się między innymi:

- okno logowania,
- okno rejestracji,
- główne okno aplikacji,
- widok dashboardu,
- widok własnych zestawów fiszek,
- widok publicznych zestawów,
- okno tworzenia zestawu,
- okno edycji zestawu,
- okno edycji fiszki,
- okno sesji nauki.

Aplikacja WPF komunikuje się z backendem za pomocą klienta HTTP. Nie wykonuje bezpośrednich zapytań do bazy danych i nie posiada dostępu do `QuizMasterDbContext`.

Odpowiedzialnością projektu WPF jest:

- wyświetlanie danych otrzymanych z API,
- wysyłanie żądań do API,
- obsługa formularzy,
- prezentacja komunikatów błędów,
- przechowywanie informacji o aktualnej sesji użytkownika,
- obsługa nawigacji pomiędzy widokami.

Dzięki temu interfejs użytkownika jest oddzielony od logiki biznesowej i może zostać rozwijany niezależnie od backendu.

---


## Definicje warstw abstrakcji i bazy wspólnego kodu(🔴Pierwsze wymaganie)

Jednym z wymagań projektu było zastosowanie warstw abstrakcji oraz opartego na nich wspólnego kodu. W projekcie **QuizMaster** zostało to zrealizowane głównie poprzez interfejsy, klasy bazowe oraz komendy wejściowe.

Celem takiego podejścia było oddzielenie miejsc, które korzystają z danej funkcjonalności, od szczegółów jej implementacji. Dzięki temu kontrolery API, widoki WPF oraz serwisy aplikacyjne nie muszą znać szczegółów technicznych działania konkretnych klas. Korzystają one z abstrakcji, a właściwe implementacje są dostarczane przez mechanizm wstrzykiwania zależności.

---

### Klasa bazowa dla komend

W projekcie zastosowano komendy, czyli klasy opisujące dane wejściowe przekazywane do operacji biznesowych. Przykładem jest komenda tworzenia fiszki, edycji fiszki, tworzenia zestawu lub rozpoczęcia sesji nauki.

Wiele operacji wykonywanych w systemie wymaga informacji o użytkowniku, który daną operację wykonuje. Z tego powodu przygotowano wspólną klasę bazową `CommandBase`.

```csharp
namespace QuizMaster.Contracts.Abstracts
{
    public abstract class CommandBase
    {
        public int UserId { get; set; }
    }
}
```

Klasa znajduje się w projekcie `QuizMaster.Contracts`, ponieważ stanowi wspólny kontrakt używany przez API oraz warstwę aplikacyjną. Nie zawiera logiki biznesowej ani szczegółów technicznych. Jej zadaniem jest dostarczenie wspólnego pola `UserId` dla komend, które wymagają identyfikacji aktualnego użytkownika.

Przykładem klasy dziedziczącej po `CommandBase` jest `CreateFlashcardCommand`.

```csharp
using QuizMaster.Contracts.Abstracts;
using QuizMaster.Core.Enums;
using QuizMaster.Core.Models;

namespace QuizMaster.Contracts.Commands.Flashcards
{
    public sealed class CreateFlashcardCommand : CommandBase
    {
        public int FlashcardSetId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string? Hint { get; set; }

        public DifficultyLevel Difficulty { get; set; }

        public CreateFlashcardCommand()
        {
        }
    }
}
```

Dzięki dziedziczeniu po `CommandBase` komenda tworzenia fiszki posiada zarówno dane potrzebne do utworzenia fiszki, jak i identyfikator użytkownika wykonującego operację. Identyfikator użytkownika nie jest podawany przez aplikację kliencką, lecz uzupełniany po stronie API na podstawie tokenu JWT.

---

### Interfejs serwisu zestawów fiszek

Jedną z najważniejszych abstrakcji w projekcie jest interfejs `IFlashcardSetService`. Definiuje on operacje związane z zestawami fiszek, ale nie określa sposobu ich wykonania.

```csharp
using QuizMaster.Contracts.Commands.FlashcardSets;
using QuizMaster.Contracts.Dto;
using QuizMaster.Core.Dto;
using QuizMaster.Core.Models;

namespace QuizMaster.Contracts.Interfaces
{
    public interface IFlashcardSetService
    {
        Task<FlashcardSet> CreateFlashcardSet(
            CreateFlashcardSetCommand command,
            CancellationToken cancellationToken = default);

        Task<List<FlashcardSetListItemDto>> GetFlashcardSets(
            int userId,
            CancellationToken cancellationToken = default);

        Task<List<FlashcardSetListItemDto>> GetPublicFlashcardSets(
            string? userName,
            string? categoryName,
            CancellationToken cancellationToken);

        Task<FlashcardSet> GetFlashcardSetDetails(
            int id,
            int userId,
            CancellationToken cancellationToken = default);

        Task UpdateFlashcardSet(
            int id,
            UpdateFlashcardSetCommand command,
            CancellationToken cancellationToken);

        Task<CopiedFlashcardSetDto> CopyFlashcardSet(
            int id,
            int userId,
            CancellationToken cancellationToken);

        Task DeleteFlashcardSet(
            int id,
            int userId,
            CancellationToken cancellationToken = default);
    }
}
```

Interfejs znajduje się w projekcie `QuizMaster.Contracts`, ponieważ jest kontraktem pomiędzy kontrolerami API a warstwą aplikacyjną. Kontroler nie powinien wiedzieć, w jaki sposób dane są pobierane z bazy, jak sprawdzany jest dostęp użytkownika ani jak kopiowany jest publiczny zestaw fiszek.

Implementacja interfejsu znajduje się w projekcie `QuizMaster.Application`, ponieważ tam umieszczona jest logika biznesowa aplikacji.

```csharp
using Microsoft.EntityFrameworkCore;
using QuizMaster.Contracts.Commands.FlashcardSets;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;
using QuizMaster.Infrastructure.Data;

namespace QuizMaster.Application.Services
{
    public sealed class FlashcardSetService : IFlashcardSetService
    {
        private readonly QuizMasterDbContext _context;

        public FlashcardSetService(QuizMasterDbContext context)
        {
            _context = context;
        }

        public async Task<FlashcardSet> CreateFlashcardSet(
            CreateFlashcardSetCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new EmptyFieldException("Nazwa");

            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == command.CategoryId, cancellationToken);

            if (category == null)
                throw new CategoryNotFoundException(command.CategoryId);

            var flashcardSet = new FlashcardSet
            {
                Name = command.Name,
                Description = command.Description,
                CategoryId = command.CategoryId,
                UserId = command.UserId,
                IsPublic = command.IsPublic,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _context.FlashcardSets.AddAsync(
                flashcardSet,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }
    }
}
```

Klasa `FlashcardSetService` znajduje się w projekcie `QuizMaster.Application`, ponieważ odpowiada za reguły biznesowe. To tutaj sprawdzane jest, czy nazwa zestawu nie jest pusta, czy wskazana kategoria istnieje oraz czy użytkownik może wykonać daną operację.

Kontroler korzysta wyłącznie z interfejsu:

```csharp
[ApiController]
[Authorize]
[Route("api/flashcardset")]
public sealed class FlashcardSetController : ControllerBase
{
    private readonly IFlashcardSetService _flashcardSetService;

    public FlashcardSetController(IFlashcardSetService flashcardSetService)
    {
        _flashcardSetService = flashcardSetService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFlashcardSets(
        CancellationToken cancellationToken)
    {
        var result = await _flashcardSetService.GetFlashcardSets(
            this.GetCurrentUserId(),
            cancellationToken);

        return Ok(result);
    }
}
```

Dzięki temu kontroler nie tworzy klasy `FlashcardSetService` samodzielnie. Otrzymuje ją przez konstruktor, a jej implementacja jest dostarczana przez kontener wstrzykiwania zależności.

---

### Abstrakcja generowania tokenów JWT

Logowanie użytkownika wymaga wygenerowania tokenu JWT. Sama czynność budowania tokenu jest szczegółem technicznym, dlatego została ukryta za interfejsem `IJwtTokenService`.

```csharp
using QuizMaster.Core.Models;

namespace QuizMaster.Contracts.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
```

Interfejs znajduje się w projekcie `QuizMaster.Contracts`, ponieważ opisuje kontrakt usługi, z której korzysta logika autoryzacji. Serwis logowania nie musi wiedzieć, jak dokładnie tworzony jest token, jakie claimy są dodawane i jaki algorytm podpisu jest używany.

Implementacja znajduje się w projekcie `QuizMaster.Infrastructure`, ponieważ generowanie tokenu JWT jest szczegółem technicznym zależnym od konfiguracji aplikacji.

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizMaster.Infrastructure.Services
{
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
```

Dzięki zastosowaniu interfejsu możliwa jest zmiana sposobu generowania tokenu bez modyfikowania kodu, który z tej funkcjonalności korzysta.

---

### Abstrakcja haszowania haseł

Podobny mechanizm zastosowano dla haseł użytkowników. Haszowanie i weryfikacja hasła zostały ukryte za interfejsem `IPasswordHasher`.

```csharp
namespace QuizMaster.Contracts.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(
            string password,
            string passwordHash);
    }
}
```

Interfejs znajduje się w projekcie `QuizMaster.Contracts`, ponieważ opisuje operacje potrzebne w procesie rejestracji i logowania. Kod odpowiedzialny za autoryzację nie musi znać szczegółów algorytmu haszowania.

Implementacja znajduje się w projekcie `QuizMaster.Infrastructure`, ponieważ jest to szczegół techniczny.

```csharp
using QuizMaster.Contracts.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace QuizMaster.Infrastructure.Services
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(
            string password,
            string passwordHash)
        {
            var hash = HashPassword(password);

            return hash == passwordHash;
        }
    }
}
```

Dzięki temu rejestracja i logowanie korzystają z jednego wspólnego mechanizmu. Logika haszowania nie jest powielana w kilku miejscach aplikacji.

---

### Podsumowanie

W projekcie zastosowano kilka warstw abstrakcji, które pełnią rolę wspólnej bazy kodu dla różnych części aplikacji.

| Element | Projekt | Rola |
|---|---|---|
| `CommandBase` | `QuizMaster.Contracts` | wspólna baza dla komend wejściowych |
| `CreateFlashcardCommand` | `QuizMaster.Contracts` | dane wejściowe dla operacji tworzenia fiszki |
| `IFlashcardSetService` | `QuizMaster.Contracts` | kontrakt operacji na zestawach fiszek |
| `FlashcardSetService` | `QuizMaster.Application` | implementacja logiki biznesowej zestawów |
| `IJwtTokenService` | `QuizMaster.Contracts` | abstrakcja generowania tokenów JWT |
| `JwtTokenService` | `QuizMaster.Infrastructure` | techniczna implementacja generowania tokenu |
| `IPasswordHasher` | `QuizMaster.Contracts` | abstrakcja haszowania i weryfikacji haseł |
| `PasswordHasher` | `QuizMaster.Infrastructure` | techniczna implementacja haszowania haseł |
| `IApiClient` | `QuizMaster.Wpf` | wspólna baza komunikacji WPF z REST API |

Zastosowanie interfejsów i klas bazowych pozwoliło uporządkować projekt oraz jasno rozdzielić odpowiedzialności. Kontrolery API korzystają z kontraktów serwisów, serwisy aplikacyjne zawierają logikę biznesową, infrastruktura odpowiada za szczegóły techniczne, a aplikacja WPF korzysta ze wspólnego klienta API. Dzięki temu kod jest łatwiejszy w utrzymaniu i dalszej rozbudowie.

---

---

## Własne wyjątki w aplikacji(🔴Drugie wymaganie)

W projekcie **QuizMaster** zastosowano własne klasy wyjątków. Ich zadaniem jest sygnalizowanie sytuacji błędnych, które wynikają z logiki działania aplikacji, a nie z awarii technicznej programu.

Wyjątki pozwalają przerwać wykonywanie operacji w momencie, gdy dalsze działanie nie ma sensu. Przykładem może być próba pobrania nieistniejącej sesji nauki, rozpoczęcie nauki dla pustego zestawu fiszek albo dostęp do zasobu należącego do innego użytkownika.

Zamiast zwracać z metod wartości typu `null`, `false` albo specjalne kody błędów, aplikacja zgłasza konkretny wyjątek. Dzięki temu kod serwisów jest czytelniejszy, a obsługa błędów może zostać przeniesiona do jednego wspólnego miejsca, na przykład middleware w REST API lub mappera błędów po stronie aplikacji WPF.

---

### Główna klasa bazowa wyjątku

W projekcie przygotowano bazową klasę wyjątku `QuizMasterException`. Pełni ona rolę wspólnego typu dla błędów związanych bezpośrednio z działaniem aplikacji.

```csharp
using System;

namespace QuizMaster.Contracts.Exceptions
{
    public abstract class QuizMasterException : Exception
    {
        protected QuizMasterException(string message)
            : base(message)
        {
        }
    }
}
```

Klasa znajduje się w projekcie `QuizMaster.Contracts`, ponieważ wyjątki są częścią kontraktu aplikacji. Są wykorzystywane przez warstwę aplikacyjną, REST API oraz klienta WPF. Dzięki temu każda część systemu może rozpoznać ten sam typ błędu.

---

### Przykład wyjątku dla nieistniejącej sesji

Jednym z przykładów jest wyjątek `LearningSessionNotFoundException`. Jest on rzucany wtedy, gdy użytkownik próbuje odwołać się do sesji nauki, która nie istnieje.

```csharp
namespace QuizMaster.Contracts.Exceptions
{
    public sealed class LearningSessionNotFoundException : QuizMasterException
    {
        public LearningSessionNotFoundException(int id)
            : base($"Nie znaleziono sesji nauki o identyfikatorze {id}.")
        {
        }
    }
}
```

Przykład użycia w serwisie:

```csharp
public async Task FinishSession(
    int id,
    int userId,
    CancellationToken cancellationToken)
{
    var session = await _context.LearningSessions
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    if (session == null)
        throw new LearningSessionNotFoundException(id);

    if (session.UserId != userId)
        throw new LearningSessionAccessDeniedException();

    if (session.IsFinished)
        throw new LearningSessionFinishedException();

    session.FinishedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync(cancellationToken);
}
```

Dzięki temu metoda nie musi zwracać informacji o błędzie jako wartości. Jeżeli sesja nie istnieje, wykonywanie operacji zostaje przerwane, a wyjątek zostaje obsłużony wyżej.

---

### Przykład wyjątku dla zakończonej sesji

W aplikacji istnieją operacje, których nie można wykonać na zakończonej sesji nauki. Przykładem jest próba ponownego pobrania kolejnej fiszki albo odpowiedzenia na fiszkę po zakończeniu sesji.

Do obsługi takiego przypadku służy wyjątek `LearningSessionFinishedException`.

```csharp
namespace QuizMaster.Contracts.Exceptions
{
    public sealed class LearningSessionFinishedException : QuizMasterException
    {
        public LearningSessionFinishedException()
            : base("Sesja nauki została już zakończona.")
        {
        }
    }
}
```

Taki wyjątek jasno opisuje przyczynę błędu. Jest to lepsze niż ogólny `Exception`, ponieważ od razu wiadomo, że problem dotyczy stanu sesji nauki.

---

### Przykład wyjątku dla pustej sesji

Kolejnym przykładem jest `LearningSessionExhaustedException`. Wyjątek ten oznacza, że w sesji nie ma już kolejnych fiszek do wyświetlenia.

```csharp
namespace QuizMaster.Contracts.Exceptions
{
    public sealed class LearningSessionExhaustedException : QuizMasterException
    {
        public LearningSessionExhaustedException()
            : base("W sesji nauki nie ma już kolejnych fiszek.")
        {
        }
    }
}
```

Ten wyjątek jest wykorzystywany również po stronie WPF. Gdy aplikacja desktopowa otrzyma informację, że sesja została wyczerpana, może pokazać użytkownikowi ekran zakończenia nauki zamiast zwykłego komunikatu błędu.

---

### Przykład wyjątku dla limitu aktywnych sesji

W projekcie wprowadzono także ograniczenie liczby aktywnych sesji nauki. Ma to zapobiec sytuacji, w której użytkownik tworzy wiele rozpoczętych sesji i nie kończy żadnej z nich.

Do obsługi takiego przypadku służy wyjątek `TooManyActiveLearningSessionsException`.

```csharp
namespace QuizMaster.Contracts.Exceptions
{
    public sealed class TooManyActiveLearningSessionsException : QuizMasterException
    {
        public TooManyActiveLearningSessionsException(int limit)
            : base($"Przekroczono limit aktywnych sesji nauki. Maksymalna liczba aktywnych sesji: {limit}.")
        {
        }
    }
}
```

Przykład użycia:

```csharp
var activeSessionsCount = await _context.LearningSessions
    .AsNoTracking()
    .CountAsync(x =>
        x.UserId == command.UserId &&
        x.FinishedAt == null,
        cancellationToken);

const int activeSessionsLimit = 5;

if (activeSessionsCount >= activeSessionsLimit)
{
    throw new TooManyActiveLearningSessionsException(activeSessionsLimit);
}
```

Dzięki osobnemu wyjątkowi system może zwrócić użytkownikowi konkretny komunikat, zamiast ogólnej informacji o błędzie.

---

### Wyjątki związane z autoryzacją i użytkownikami

W projekcie znajdują się również wyjątki związane z kontem użytkownika oraz autoryzacją.

Przykładowe wyjątki:

| Wyjątek | Znaczenie |
|---|---|
| `UserAlreadyExistsException` | użytkownik o podanej nazwie lub adresie e-mail już istnieje |
| `UserNotExistsException` | użytkownik nie istnieje |
| `TokenExpiredException` | token autoryzacyjny wygasł |
| `ServerResponseIsEmptyException` | odpowiedź z serwera była pusta |

Takie wyjątki są szczególnie istotne w komunikacji pomiędzy REST API a aplikacją WPF. Backend może zwrócić informację o konkretnym błędzie, a aplikacja desktopowa może pokazać użytkownikowi odpowiedni komunikat albo przekierować go z powrotem do logowania.

---

### Dlaczego wyjątków jest dużo

W projekcie znajduje się wiele klas wyjątków, ponieważ każdy z nich opisuje inną sytuację domenową. Zamiast używać jednego ogólnego wyjątku dla wszystkich błędów, aplikacja rozróżnia konkretne przypadki.

Takie podejście ma kilka zalet.

Po pierwsze, kod jest bardziej czytelny. Widząc instrukcję:

```csharp
throw new LearningSessionFinishedException();
```

od razu wiadomo, dlaczego operacja została przerwana.

Po drugie, łatwiej obsługiwać błędy. REST API może mapować różne wyjątki na różne kody HTTP, na przykład `404 Not Found`, `403 Forbidden`, `400 Bad Request` albo `409 Conflict`.

Po trzecie, aplikacja WPF może reagować inaczej na różne sytuacje. Wygaśnięcie tokenu powinno spowodować powrót do okna logowania, natomiast zakończenie sesji nauki powinno pokazać ekran podsumowania.

Po czwarte, własne wyjątki pomagają utrzymać logikę biznesową w serwisach. Serwis nie musi zwracać specjalnych kodów błędów. Jeżeli wystąpi sytuacja niepoprawna, zgłasza konkretny wyjątek i kończy operację.

---





## Wykorzystanie delegatów i zdarzeń(🔴Trzecie wymaganie)

W projekcie **QuizMaster** wykorzystano delegaty zarówno w postaci własnych typów delegatów, jak i w formie standardowych zdarzeń dostępnych w WPF. Delegaty zostały użyte głównie tam, gdzie jedna część aplikacji musi poinformować inną część o wykonaniu określonej akcji, ale bez tworzenia silnego powiązania pomiędzy klasami.

Delegat w C# określa sygnaturę metody, czyli typ zwracany oraz listę parametrów. Oznacza to, że do zmiennej typu delegata można przypisać każdą metodę, która posiada zgodną sygnaturę. Dzięki temu kod wywołujący delegata nie musi wiedzieć, jaka dokładnie metoda zostanie uruchomiona. Wystarczy, że metoda pasuje do wymaganego typu.

W projekcie mechanizm ten został wykorzystany między innymi do komunikacji pomiędzy oknami WPF. Okno podrzędne może poinformować okno nadrzędne, że użytkownik zakończył operację, na przykład utworzył konto, dodał zestaw fiszek albo edytował fiszkę. Dzięki temu okno nadrzędne może odświeżyć dane lub przejść do kolejnego widoku, bez konieczności przekazywania do okna podrzędnego całej logiki aplikacji.

---

### Własny delegat zakończenia rejestracji

Jednym z prostszych przykładów jest delegat wykorzystywany po zakończeniu rejestracji użytkownika.

```csharp
namespace QuizMaster.Wpf.Delegates
{
    public delegate void RegistrationFinishedHandler();
}
```

Delegat ten opisuje metodę, która nie przyjmuje żadnych parametrów i nie zwraca wartości. Został zastosowany w oknie rejestracji, aby poinformować inne okno, że proces rejestracji zakończył się powodzeniem.

Przykład pola delegata w oknie rejestracji:

```csharp
public RegistrationFinishedHandler OnRegistrationFinished;
```

Po poprawnym utworzeniu konta wywoływany jest delegat:

```csharp
OnRegistrationFinished?.Invoke();
```

Zastosowanie operatora `?.Invoke()` oznacza, że metoda zostanie wywołana tylko wtedy, gdy do delegata została wcześniej przypisana jakaś obsługa. Dzięki temu nie trzeba ręcznie sprawdzać, czy delegat ma wartość `null`.

Takie rozwiązanie pozwala odseparować okno rejestracji od dalszego zachowania aplikacji. Okno rejestracji nie musi wiedzieć, czy po rejestracji ma zostać otwarte okno logowania, pokazany komunikat, czy wykonana inna akcja. Ono jedynie zgłasza fakt zakończenia rejestracji.

---

### Delegat informujący o wygaśnięciu sesji

Kolejnym przykładem jest delegat odpowiedzialny za informowanie aplikacji o wygaśnięciu sesji użytkownika.

```csharp
namespace QuizMaster.Wpf.Delegates
{
    public delegate void SessionExpiredHandler();
}
```

Ten delegat również nie przyjmuje parametrów i nie zwraca wartości. Jego zadaniem jest przekazanie informacji, że aktualna sesja użytkownika przestała być ważna, na przykład z powodu wygaśnięcia tokenu JWT.

Na podstawie tego delegata przygotowano interfejs zdarzeń sesji:

```csharp
using QuizMaster.Wpf.Delegates;

namespace QuizMaster.Wpf.Interfaces
{
    public interface ISessionEvents
    {
        event SessionExpiredHandler OnSessionExpired;

        void InvokeSessionExpired();
    }
}
```

W tym przypadku delegat został użyty razem ze słowem kluczowym `event`. Dzięki temu inne klasy mogą podpinać obsługę zdarzenia, ale nie mogą wywołać go bezpośrednio z zewnątrz. Wywołanie zdarzenia pozostaje pod kontrolą klasy implementującej `ISessionEvents`.

Przykład użycia w głównym oknie aplikacji:

```csharp
_sessionEvents.OnSessionExpired += SessionEvents_OnSessionExpired;
```

Metoda obsługująca zdarzenie może wyglądać następująco:

```csharp
private void SessionEvents_OnSessionExpired()
{
    Dispatcher.Invoke(() =>
    {
        _appSession.Clear();

        _messageDialogService.ShowError(
            "Sesja wygasła",
            "Twoja sesja wygasła. Zaloguj się ponownie.",
            this);

        var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
        loginWindow.Show();

        Close();
    });
}
```

W tym przykładzie główne okno aplikacji reaguje na zdarzenie wygaśnięcia sesji. Czyści dane aktualnej sesji, pokazuje komunikat użytkownikowi, otwiera okno logowania i zamyka bieżące okno.

Delegat pozwala więc oddzielić miejsce wykrycia problemu od miejsca reakcji interfejsu użytkownika. Klient API może wykryć błąd autoryzacji, zgłosić zdarzenie, a główne okno aplikacji podejmuje decyzję, co zrobić dalej.

---

### Delegat tworzenia zestawu fiszek

Własny delegat został użyty również podczas tworzenia zestawu fiszek.

```csharp
namespace QuizMaster.Wpf.Delegates
{
    public delegate void CreateFlashcardSetHandler(
        object sender,
        int flashcardSetId);
}
```

Ten delegat przyjmuje dwa parametry:

- `sender` — obiekt, który zgłasza zdarzenie,
- `flashcardSetId` — identyfikator utworzonego zestawu fiszek.

Został on użyty w oknie tworzenia zestawu fiszek. Po poprawnym utworzeniu zestawu okno może poinformować widok nadrzędny, jaki zestaw został dodany.

Przykład użycia:

```csharp
public event CreateFlashcardSetHandler OnCreatedFlashcardSet;
```

Po zapisaniu zestawu:

```csharp
OnCreatedFlashcardSet?.Invoke(this, createdSet.Id);
```

Dzięki temu widok listy zestawów może odświeżyć dane po zamknięciu okna tworzenia.

```csharp
window.OnCreatedFlashcardSet += async (_, flashcardSetId) =>
{
    await LoadFlashcardSetsAsync();
};
```

W tym miejscu zastosowano wyrażenie lambda, które również jest zgodne z sygnaturą delegata. Delegat oczekuje metody przyjmującej `object` oraz `int`, więc lambda z parametrami `(_, flashcardSetId)` może zostać do niego przypisana.

---

### Delegaty przy tworzeniu i edycji fiszek

Delegaty zostały wykorzystane także w oknie edycji fiszki. Okno to może działać w dwóch trybach: dodawania nowej fiszki albo edycji istniejącej. Z tego powodu zostały przygotowane osobne delegaty dla obu operacji.

Przykładowa idea delegatów wygląda następująco:

```csharp
public delegate bool CreateFlashcardHandler(
    object sender,
    CreateFlashcardCommand command,
    int flashcardSetId);

public delegate bool EditFlashcardHandler(
    object sender,
    UpdateFlashcardCommand command,
    int flashcardId);
```

Delegaty zwracają wartość logiczną. Dzięki temu metoda obsługująca zapis może poinformować okno, czy operacja zakończyła się powodzeniem. Jeżeli wynik jest równy `true`, okno może zostać zamknięte. Jeżeli wynik jest równy `false`, okno pozostaje otwarte.

Fragment okna edycji fiszki:

```csharp
public CreateFlashcardHandler OnCreatedFlashcard;
public EditFlashcardHandler OnEditedFlashcard;
```

Podczas zapisu okno sprawdza, czy działa w trybie dodawania czy edycji.

```csharp
if (Context == WindowContext.Adding)
{
    var command = new CreateFlashcardCommand
    {
        FlashcardSetId = _flashcardSetId!.Value,
        Question = question,
        Answer = answer,
        Hint = hint,
        Difficulty = difficulty
    };

    var result = OnCreatedFlashcard(
        this,
        command,
        _flashcardSetId!.Value);

    if (result)
        Close();
}
else
{
    var command = new UpdateFlashcardCommand
    {
        Question = question,
        Answer = answer,
        Hint = hint,
        Difficulty = difficulty
    };

    var result = OnEditedFlashcard(
        this,
        command,
        _flashcardId!.Value);

    if (result)
        Close();
}
```

Dzięki delegatom okno `EditFlashcardWindow` nie musi znać szczegółów komunikacji z API. Nie musi wiedzieć, jaki endpoint zostanie wywołany ani w jaki sposób zostanie odświeżona lista fiszek. Okno odpowiada wyłącznie za zebranie danych od użytkownika i przekazanie ich dalej za pomocą delegata.

---

### Standardowe zdarzenia WPF jako delegaty

W projekcie wykorzystywane są również standardowe zdarzenia WPF, takie jak `Click`, `Loaded`, `Closed` czy `MouseLeftButtonUp`. One również bazują na mechanizmie delegatów. Różnica polega na tym, że typy delegatów zostały zdefiniowane przez bibliotekę WPF.

Przykład obsługi kliknięcia przycisku:

```csharp
private async void LearnButton_Click(
    object sender,
    RoutedEventArgs e)
{
    var command = new StartLearningSessionCommand
    {
        FlashcardSetId = flashcardSetId
    };

    var session = await _apiClient
        .PostAsync<StartLearningSessionCommand, LearningSessionDto>(
            "api/learning-session/start",
            command);
}
```

Metoda może zostać podpięta do zdarzenia `Click`, ponieważ jej sygnatura pasuje do oczekiwanej przez WPF sygnatury obsługi zdarzenia. W pliku XAML wygląda to następująco:

```xml
<Button Content="Ucz się"
        Click="LearnButton_Click"/>
```

Podobnie działa zdarzenie `Loaded`, które jest wykorzystywane do załadowania danych po otwarciu widoku:

```csharp
Loaded += DashboardView_Loaded;
```

Metoda obsługująca zdarzenie:

```csharp
private async void DashboardView_Loaded(
    object sender,
    RoutedEventArgs e)
{
    Loaded -= DashboardView_Loaded;

    await LoadDashboardAsync();
}
```

W tym przypadku metoda zostaje odpięta po pierwszym uruchomieniu, aby uniknąć wielokrotnego ładowania tych samych danych.

---

### Delegaty a luźne powiązanie elementów aplikacji

Najważniejszym powodem użycia delegatów w projekcie było ograniczenie powiązań pomiędzy klasami. Okno podrzędne nie powinno bezpośrednio znać logiki widoku nadrzędnego. Nie powinno też samodzielnie decydować, jak odświeżyć listę danych albo jak zmienić aktualny widok aplikacji.

Delegaty pozwalają odwrócić tę zależność. Okno podrzędne definiuje moment, w którym chce poinformować o zdarzeniu, natomiast kod tworzący okno decyduje, jaka metoda zostanie wtedy wykonana.

Przykładowo okno edycji zestawu fiszek może zgłosić zapis:

```csharp
public event EventHandler Saved;
```

Po wykonaniu zapisu:

```csharp
Saved?.Invoke(this, EventArgs.Empty);
```

A widok nadrzędny może zdecydować, że po zapisie należy odświeżyć listę zestawów:

```csharp
window.Saved += async (_, _) =>
{
    await LoadFlashcardSetsAsync();
};
```

Dzięki temu okno edycji nie posiada bezpośredniej zależności od widoku listy. Jest bardziej uniwersalne i może zostać użyte w innym miejscu aplikacji.

---



## Wykorzystanie zapytań LINQ(🔴 Czwarte wymaganie)

Kolejnym wymaganiem projektu było wykorzystanie zapytań LINQ. W projekcie **QuizMaster** LINQ został użyty przede wszystkim po stronie backendu, w warstwie aplikacyjnej, podczas pracy z bazą danych przez **Entity Framework Core**.

LINQ pozwala zapisywać zapytania do kolekcji i bazy danych bezpośrednio w języku C#. W przypadku Entity Framework Core zapytania LINQ nie są wykonywane od razu jako zwykły kod na obiektach, lecz są tłumaczone na zapytania SQL wykonywane na bazie danych. Dzięki temu możliwe jest filtrowanie, sortowanie, agregowanie i projektowanie danych jeszcze po stronie SQL Servera.

W projekcie LINQ został wykorzystany między innymi do:

- filtrowania danych,
- wyszukiwania pojedynczych rekordów,
- sortowania wyników,
- liczenia powiązanych elementów,
- sprawdzania istnienia rekordów,
- przygotowywania obiektów DTO,
- pobierania danych z relacjami,
- obsługi aktywnych sesji nauki.

---

### LINQ w Entity Framework Core

Najczęściej LINQ pojawia się w serwisach aplikacyjnych, które korzystają z `QuizMasterDbContext`. Przykładowo zestawy fiszek użytkownika są pobierane z bazy na podstawie identyfikatora aktualnie zalogowanego użytkownika.

```csharp
public async Task<List<FlashcardSetListItemDto>> GetFlashcardSets(
    int userId,
    CancellationToken cancellationToken = default)
{
    return await _context.FlashcardSets
        .AsNoTracking()
        .Where(x => x.UserId == userId)
        .OrderByDescending(x => x.CreatedAt)
        .Select(x => new FlashcardSetListItemDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CategoryId = x.CategoryId,
            CategoryName = x.Category.Name,
            UserId = x.UserId,
            UserName = x.User.UserName,
            FlashcardsCount = x.Flashcards.Count,
            IsPublic = x.IsPublic,
            CreatedAt = x.CreatedAt
        })
        .ToListAsync(cancellationToken);
}
```

W tym przykładzie wykorzystano kilka metod LINQ:

- `Where()` — filtruje zestawy tylko do tych należących do danego użytkownika,
- `OrderByDescending()` — sortuje zestawy od najnowszych,
- `Select()` — tworzy obiekty DTO zamiast zwracać encje bazodanowe,
- `Count` — liczy liczbę fiszek w zestawie,
- `ToListAsync()` — wykonuje zapytanie i zwraca wynik jako listę.

Ważne jest to, że dzięki `Select()` aplikacja nie pobiera pełnych encji wraz ze wszystkimi danymi, tylko konkretne pola potrzebne do wyświetlenia listy zestawów. Entity Framework Core tłumaczy takie zapytanie LINQ na odpowiednie zapytanie SQL.

---

### Filtrowanie publicznych zestawów fiszek

LINQ został wykorzystany również przy wyszukiwaniu publicznych zestawów fiszek. Użytkownik może filtrować zestawy po nazwie autora oraz kategorii.

```csharp
public async Task<List<FlashcardSetListItemDto>> GetPublicFlashcardSets(
    string? userName,
    string? categoryName,
    CancellationToken cancellationToken)
{
    var setsQuery = _context.FlashcardSets
        .AsNoTracking()
        .Where(x => x.IsPublic);

    if (!string.IsNullOrWhiteSpace(userName))
    {
        setsQuery = setsQuery.Where(x =>
            EF.Functions.Like(x.User.UserName, $"%{userName}%"));
    }

    if (!string.IsNullOrWhiteSpace(categoryName))
    {
        setsQuery = setsQuery.Where(x =>
            EF.Functions.Like(x.Category.Name, $"%{categoryName}%"));
    }

    return await setsQuery
        .OrderByDescending(x => x.CreatedAt)
        .Select(x => new FlashcardSetListItemDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CategoryId = x.CategoryId,
            CategoryName = x.Category.Name,
            UserId = x.UserId,
            UserName = x.User.UserName,
            FlashcardsCount = x.Flashcards.Count,
            IsPublic = x.IsPublic,
            CreatedAt = x.CreatedAt
        })
        .ToListAsync(cancellationToken);
}
```

W tym przypadku zapytanie jest budowane stopniowo. Na początku pobierane są tylko zestawy publiczne. Następnie, jeżeli użytkownik poda filtr po autorze lub kategorii, do zapytania dodawane są kolejne warunki `Where()`.

Zmienna `setsQuery` nie wykonuje zapytania od razu. Jest to zapytanie typu `IQueryable`, które zostanie wykonane dopiero po wywołaniu `ToListAsync()`. Dzięki temu można dynamicznie budować zapytanie zależnie od wybranych filtrów.

Do filtrowania tekstowego użyto `EF.Functions.Like()`, ponieważ taka konstrukcja może zostać przetłumaczona na SQL-owy operator `LIKE`.

---

### Pobieranie pojedynczych rekordów

LINQ jest również używany do pobierania pojedynczych rekordów z bazy danych. Przykładem jest sprawdzenie, czy istnieje kategoria wskazana podczas tworzenia zestawu fiszek.

```csharp
var category = await _context.Categories
    .AsNoTracking()
    .FirstOrDefaultAsync(
        x => x.Id == command.CategoryId,
        cancellationToken);

if (category == null)
{
    throw new CategoryNotFoundException(command.CategoryId);
}
```

W tym przykładzie metoda `FirstOrDefaultAsync()` zwraca pierwszy rekord spełniający warunek albo `null`, jeżeli taki rekord nie istnieje. Dzięki temu można w prosty sposób sprawdzić poprawność danych wejściowych.

Podobny mechanizm jest wykorzystywany przy pobieraniu użytkowników, fiszek, zestawów oraz sesji nauki.

---

### Sprawdzanie istnienia danych

W projekcie LINQ służy również do sprawdzania, czy dana sytuacja występuje w bazie. Przykładem jest kontrola liczby aktywnych sesji nauki użytkownika.

```csharp
var activeSessionsCount = await _context.LearningSessions
    .AsNoTracking()
    .CountAsync(x =>
        x.UserId == command.UserId &&
        x.FinishedAt == null,
        cancellationToken);

const int activeSessionsLimit = 5;

if (activeSessionsCount >= activeSessionsLimit)
{
    throw new TooManyActiveLearningSessionsException(activeSessionsLimit);
}
```

Tutaj metoda `CountAsync()` zlicza aktywne sesje użytkownika. Dzięki temu aplikacja może zablokować tworzenie kolejnej sesji, jeżeli użytkownik przekroczył ustalony limit.

Innym przykładem jest sprawdzanie, czy w sesji pozostały jeszcze nieodpowiedziane fiszki.

```csharp
var hasUnansweredItems = await _context.LearningSessionItems
    .AsNoTracking()
    .AnyAsync(x =>
        x.LearningSessionId == sessionId &&
        x.Id != sessionItem.Id &&
        !x.IsAnswered,
        cancellationToken);

var isSessionFinished = !hasUnansweredItems;
```

Metoda `AnyAsync()` jest używana wtedy, gdy nie trzeba pobierać pełnych danych, a wystarczy sprawdzić, czy istnieje przynajmniej jeden rekord spełniający warunek.

---

### Pobieranie danych powiązanych

Entity Framework Core pozwala korzystać z LINQ również przy pobieraniu danych powiązanych relacjami. Przykładem jest pobranie szczegółów sesji nauki razem z zestawem fiszek.

```csharp
var session = await _context.LearningSessions
    .AsNoTracking()
    .Include(x => x.FlashcardSet)
    .FirstOrDefaultAsync(x =>
        x.Id == sessionId &&
        x.UserId == userId,
        cancellationToken);

if (session == null)
{
    throw new LearningSessionNotFoundException(sessionId);
}
```

Metoda `Include()` informuje Entity Framework Core, że razem z sesją należy pobrać powiązany zestaw fiszek. Dzięki temu można później wykorzystać nazwę zestawu podczas budowania DTO zwracanego do aplikacji WPF.

---

### Projekcja danych do DTO

W projekcie bardzo często używana jest metoda `Select()`, która pozwala zamienić encje bazodanowe na obiekty DTO. Jest to ważne, ponieważ aplikacja kliencka nie powinna otrzymywać pełnych encji Entity Framework Core.

Przykład przygotowania DTO dla sesji nauki:

```csharp
return new LearningSessionDto
{
    Id = session.Id,
    FlashcardSetId = session.FlashcardSetId,
    FlashcardSetName = session.FlashcardSet.Name,
    TotalFlashcardsCount = session.TotalFlashcardsCount,
    ReviewedFlashcardsCount = session.ReviewedFlashcardsCount,
    IsFinished = session.IsFinished,
    StartedAt = session.StartedAt,
    FinishedAt = session.FinishedAt
};
```

W innych miejscach projekcja wykonywana jest bezpośrednio w zapytaniu LINQ:

```csharp
var activeSessions = await _context.LearningSessions
    .AsNoTracking()
    .Where(x =>
        x.UserId == userId &&
        x.FinishedAt == null)
    .OrderByDescending(x => x.StartedAt)
    .Select(x => new LearningSessionDto
    {
        Id = x.Id,
        FlashcardSetId = x.FlashcardSetId,
        FlashcardSetName = x.FlashcardSet.Name,
        TotalFlashcardsCount = x.TotalFlashcardsCount,
        ReviewedFlashcardsCount = x.ReviewedFlashcardsCount,
        IsFinished = x.IsFinished,
        StartedAt = x.StartedAt,
        FinishedAt = x.FinishedAt
    })
    .ToListAsync(cancellationToken);
```

Takie podejście ogranicza ilość pobieranych danych i pozwala zwrócić do WPF tylko informacje potrzebne do wyświetlenia interfejsu.

---

### LINQ w obsłudze sesji nauki

LINQ jest szczególnie istotny w module sesji nauki. To tam aplikacja musi pobierać kolejne fiszki, sprawdzać postęp i oznaczać odpowiedzi użytkownika.

Przykład pobrania kolejnej nieodpowiedzianej fiszki:

```csharp
var sessionItem = await _context.LearningSessionItems
    .Include(x => x.Flashcard)
    .Where(x =>
        x.LearningSessionId == sessionId &&
        !x.IsAnswered)
    .OrderBy(x => x.Id)
    .FirstOrDefaultAsync(cancellationToken);

if (sessionItem == null)
{
    throw new LearningSessionExhaustedException();
}
```

W tym przykładzie:

- `Where()` ogranicza dane do elementów z danej sesji,
- `!x.IsAnswered` wybiera tylko fiszki, na które użytkownik jeszcze nie odpowiedział,
- `OrderBy()` ustala kolejność fiszek,
- `FirstOrDefaultAsync()` pobiera pierwszą dostępną fiszkę,
- `Include()` pobiera powiązaną encję fiszki.

Dzięki temu aplikacja WPF może wyświetlić kolejną fiszkę w oknie nauki.

---

### LINQ w dashboardzie

LINQ jest wykorzystywany także do przygotowywania danych na dashboard. Dashboard prezentuje między innymi liczbę zestawów, liczbę fiszek, ostatnie zestawy oraz aktywne sesje.

Przykładowe operacje LINQ wykorzystywane przy dashboardzie:

```csharp
var flashcardSetsCount = await _context.FlashcardSets
    .AsNoTracking()
    .CountAsync(x => x.UserId == userId, cancellationToken);

var flashcardsCount = await _context.Flashcards
    .AsNoTracking()
    .CountAsync(x => x.FlashcardSet.UserId == userId, cancellationToken);

var recentSets = await _context.FlashcardSets
    .AsNoTracking()
    .Where(x => x.UserId == userId)
    .OrderByDescending(x => x.CreatedAt)
    .Take(5)
    .Select(x => new FlashcardSetListItemDto
    {
        Id = x.Id,
        Name = x.Name,
        Description = x.Description,
        CategoryName = x.Category.Name,
        FlashcardsCount = x.Flashcards.Count,
        IsPublic = x.IsPublic,
        CreatedAt = x.CreatedAt
    })
    .ToListAsync(cancellationToken);
```

W tym fragmencie wykorzystano między innymi:

- `CountAsync()` do liczenia danych,
- `Where()` do filtrowania po użytkowniku,
- `OrderByDescending()` do sortowania od najnowszych,
- `Take()` do ograniczenia liczby wyników,
- `Select()` do przygotowania DTO.

---

### AsNoTracking

W wielu zapytaniach użyto metody `AsNoTracking()`.

```csharp
var sets = await _context.FlashcardSets
    .AsNoTracking()
    .Where(x => x.UserId == userId)
    .ToListAsync(cancellationToken);
```

Metoda ta informuje Entity Framework Core, że pobrane obiekty nie będą modyfikowane. Dzięki temu EF Core nie musi śledzić zmian tych encji. Jest to przydatne w zapytaniach tylko do odczytu, na przykład podczas pobierania listy zestawów, publicznych zestawów albo danych dashboardu.

---

### Podsumowanie

Zapytania LINQ są jednym z najczęściej wykorzystywanych mechanizmów w projekcie **QuizMaster**. Są używane głównie w połączeniu z Entity Framework Core, gdzie pozwalają zapisywać zapytania do bazy danych w języku C#.

W projekcie LINQ służy do filtrowania danych, sortowania wyników, liczenia rekordów, sprawdzania istnienia danych, pobierania rekordów powiązanych oraz budowania DTO zwracanych do aplikacji WPF.

Najważniejsze użyte metody LINQ to:

| Metoda | Zastosowanie w projekcie |
|---|---|
| `Where()` | filtrowanie zestawów, fiszek, sesji i użytkowników |
| `Select()` | tworzenie obiektów DTO |
| `OrderBy()` / `OrderByDescending()` | sortowanie zestawów i sesji |
| `FirstOrDefaultAsync()` | pobieranie pojedynczego rekordu |
| `AnyAsync()` | sprawdzanie, czy istnieją dane spełniające warunek |
| `CountAsync()` | liczenie zestawów, fiszek i aktywnych sesji |
| `Take()` | ograniczanie liczby wyników |
| `Include()` | pobieranie danych powiązanych relacjami |
| `ToListAsync()` | wykonanie zapytania i pobranie listy wyników |

Dzięki zastosowaniu LINQ kod odpowiedzialny za pobieranie danych jest czytelny, spójny z językiem C# i łatwiejszy w utrzymaniu niż ręcznie składane zapytania SQL.

---


## Przejrzysty Interfejs graficzny aplikacji(🔴 Piąte wymaganie)

Jednym z wymagań projektu było przygotowanie przejrzystego interfejsu graficznego. W projekcie **QuizMaster** część kliencka została wykonana jako aplikacja desktopowa WPF.

Interfejs aplikacji został podzielony na osobne widoki i okna. Dzięki temu użytkownik nie pracuje w jednym dużym formularzu, tylko przechodzi pomiędzy logicznie wydzielonymi częściami aplikacji.

W aplikacji przygotowano między innymi:

- okno logowania,
- okno rejestracji,
- główne okno aplikacji,
- dashboard użytkownika,
- widok własnych zestawów fiszek,
- widok publicznych zestawów,
- okno tworzenia zestawu,
- okno edycji zestawu,
- okno dodawania i edycji fiszki,
- okno sesji nauki.

Celem było stworzenie interfejsu, który jest czytelny i wygodny w użyciu. Widoki zostały oparte o jasny układ, karty, przyciski akcji oraz osobne sekcje danych. Przykładowo zestawy fiszek są prezentowane jako lista kart zawierających nazwę, opis, kategorię, liczbę fiszek oraz dostępne akcje.

Ważnym elementem interfejsu jest również okno sesji nauki. Fiszka została przedstawiona jako karta, którą użytkownik może kliknąć, aby zobaczyć odpowiedź. Pod kartą znajdują się przyciski oceny odpowiedzi oraz pasek postępu informujący, ile fiszek zostało już przerobionych.

---

## Metody rozszerzeniowe(🔴Wspomniane)

W projekcie wykorzystano metody rozszerzeniowe. Pozwalają one dodać dodatkowe metody do istniejących typów bez konieczności modyfikowania ich kodu źródłowego.

W aplikacji zastosowano je między innymi przy pobieraniu identyfikatora aktualnie zalogowanego użytkownika z kontrolera API.

```csharp
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace QuizMaster.WebApi.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static int GetCurrentUserId(this ControllerBase controller)
        {
            var userId = controller.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException();

            return int.Parse(userId);
        }
    }
}
```

Dzięki tej metodzie każdy kontroler dziedziczący po `ControllerBase` może w prosty sposób pobrać identyfikator użytkownika:

```csharp
command.UserId = this.GetCurrentUserId();
```

Bez metody rozszerzeniowej podobny kod trzeba byłoby powtarzać w wielu kontrolerach. Zastosowanie rozszerzenia pozwoliło przenieść tę logikę do jednego miejsca.

Drugim przykładem są metody rozszerzeniowe używane przy mapowaniu odpowiedzi błędów zwracanych przez API na wyjątki używane po stronie aplikacji WPF.

```csharp
namespace QuizMaster.Wpf.Extensions
{
    public static class ExceptionResponseExtensions
    {
        public static Exception Map(this ExceptionResponse response)
        {
            return response.Type switch
            {
                nameof(TokenExpiredException) => new TokenExpiredException(),
                nameof(UserAlreadyExistsException) => new UserAlreadyExistsException(),
                nameof(UserNotExistsException) => new UserNotExistsException(),
                _ => new Exception(response.Message)
            };
        }
    }
}
```

Dzięki temu klient API może w prosty sposób zamienić odpowiedź błędu z serwera na konkretny wyjątek:

```csharp
throw error.Map();
```

Metody rozszerzeniowe zostały więc użyte tam, gdzie pewna operacja logicznie pasuje do istniejącego typu, ale nie powinna być wpisywana bezpośrednio w jego kod.

---

## Typy i metody generyczne(🔴Wspomniane)

W projekcie wykorzystano również typy i metody generyczne. Najważniejszym przykładem jest uniwersalny klient API używany przez aplikację WPF do komunikacji z backendem.

Zamiast pisać osobną metodę dla każdego endpointu, przygotowano wspólne metody generyczne. Pozwalają one określić typ danych wysyłanych do API oraz typ danych zwracanych z API.

Przykład interfejsu klienta API:

```csharp
namespace QuizMaster.Wpf.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> GetAsync<TResponse>(
            string path,
            CancellationToken cancellationToken = default);

        Task<TResponse> PostAsync<TRequest, TResponse>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default);

        Task PutAsync<TRequest>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            string path,
            CancellationToken cancellationToken = default);
    }
}
```

Dzięki generyczności ta sama metoda może zostać wykorzystana do pobierania różnych typów danych.

Przykład pobrania listy publicznych zestawów:

```csharp
var publicSets = await _apiClient
    .GetAsync<List<FlashcardSetListItemDto>>(
        "api/flashcardset/public");
```

Przykład rozpoczęcia sesji nauki:

```csharp
var session = await _apiClient
    .PostAsync<StartLearningSessionCommand, LearningSessionDto>(
        "api/learning-session/start",
        command);
```

W pierwszym przypadku odpowiedzią jest lista zestawów fiszek, a w drugim obiekt sesji nauki. Mechanizm komunikacji HTTP pozostaje jednak wspólny.

Generyczność została więc wykorzystana do ograniczenia powielania kodu. Klient API obsługuje różne typy żądań i odpowiedzi, ale jego implementacja pozostaje jedna.

---

## Inicjalizatory obiektów(🔴Wspomniane)

W projekcie często wykorzystywano inicjalizatory obiektów. Pozwalają one tworzyć obiekty i od razu przypisywać wartości ich właściwościom w czytelny sposób.

Przykładem jest tworzenie komendy rozpoczęcia sesji nauki w aplikacji WPF:

```csharp
var command = new StartLearningSessionCommand
{
    FlashcardSetId = flashcardSetId
};
```

Inny przykład to tworzenie obiektu DTO lub encji po stronie backendu:

```csharp
var flashcardSet = new FlashcardSet
{
    Name = command.Name,
    Description = command.Description,
    CategoryId = command.CategoryId,
    UserId = command.UserId,
    IsPublic = command.IsPublic,
    CreatedAt = DateTime.UtcNow
};
```

Takie podejście zwiększa czytelność kodu, ponieważ od razu widać, jakie wartości są przypisywane do tworzonego obiektu. Jest to szczególnie przydatne przy komendach, DTO oraz encjach tworzonych na podstawie danych wejściowych.

---

## Niejawne typowanie zmiennych lokalnych(🔴Wspomniane)

W projekcie używano niejawnego typowania lokalnych zmiennych za pomocą słowa kluczowego `var`, ale tylko w miejscach, gdzie typ wynikał jasno z prawej strony przypisania.

Przykład:

```csharp
var command = new CreateFlashcardCommand
{
    FlashcardSetId = _flashcardSetId!.Value,
    Question = question,
    Answer = answer,
    Hint = hint,
    Difficulty = difficulty
};
```

W takim przypadku użycie `var` nie pogarsza czytelności, ponieważ od razu widać, że tworzony jest obiekt `CreateFlashcardCommand`.

Unikano natomiast stosowania `var` w miejscach, gdzie mogłoby to utrudniać zrozumienie kodu. Dzięki temu kod pozostaje czytelny, a jednocześnie nie jest niepotrzebnie rozwlekły.

---

## Asynchroniczność(🔴Nie wspomniane, ale fajne)

W projekcie szeroko zastosowano programowanie asynchroniczne z użyciem `async` i `await`. Mechanizm ten pojawia się zarówno po stronie REST API, jak i aplikacji WPF.

Po stronie backendu operacje bazodanowe wykonywane są asynchronicznie:

```csharp
var sets = await _context.FlashcardSets
    .AsNoTracking()
    .Where(x => x.UserId == userId)
    .ToListAsync(cancellationToken);
```

Po stronie WPF asynchronicznie wykonywane są żądania HTTP:

```csharp
var dashboard = await _apiClient
    .GetAsync<MainDashboardDto>("api/dashboard");
```

Dzięki temu aplikacja nie blokuje interfejsu użytkownika podczas komunikacji z API. Użytkownik nie ma wrażenia, że program się zawiesił w czasie pobierania danych lub wykonywania operacji sieciowych.

---

## Podsumowanie dodatkowych mechanizmów

Wymaganie dotyczące przejrzystego interfejsu graficznego oraz wykorzystania dodatkowych mechanizmów języka C# zostało zrealizowane w kilku obszarach projektu.

| Mechanizm | Przykład użycia w projekcie |
|---|---|
| Interfejs graficzny | aplikacja WPF z osobnymi widokami i oknami |
| Metody rozszerzeniowe | pobieranie ID użytkownika z kontrolera, mapowanie wyjątków |
| Generyczność | uniwersalny klient API `GetAsync<T>`, `PostAsync<TRequest, TResponse>` |
| Inicjalizatory obiektów | tworzenie komend, DTO i encji |
| Niejawne typowanie | użycie `var` tam, gdzie typ jest oczywisty |
| Asynchroniczność | operacje HTTP i zapytania EF Core z `async`/`await` |

Dzięki zastosowaniu tych mechanizmów kod aplikacji jest krótszy, bardziej czytelny i mniej podatny na powielanie tych samych fragmentów w wielu miejscach.


---


## Rejestracja kontenera DI(🔴Nie wymagane, ale fajne)

W projekcie **QuizMaster** wykorzystano mechanizm **Dependency Injection**, czyli wstrzykiwania zależności. Nie było to wymaganie obowiązkowe, ale mechanizm ten bardzo dobrze pasuje do architektury warstwowej zastosowanej w projekcie.

Dependency Injection pozwala zarejestrować klasy i interfejsy w jednym miejscu, a następnie automatycznie przekazywać je przez konstruktory do klas, które ich potrzebują. Dzięki temu kontrolery, serwisy i okna WPF nie tworzą ręcznie swoich zależności za pomocą `new`, tylko otrzymują gotowe obiekty z kontenera.

Takie podejście poprawia czytelność kodu, zmniejsza powiązania pomiędzy klasami i ułatwia późniejszą rozbudowę aplikacji.

---

### Rejestracja zależności w REST API

Po stronie backendu kontener DI jest konfigurowany w pliku `Program.cs`. To tam rejestrowane są kontrolery, serwisy aplikacyjne, kontekst bazy danych, obsługa autoryzacji JWT oraz middleware.

Przykład rejestracji serwisów aplikacyjnych:

```csharp
builder.Services.AddScoped<ITransactionManager, EfTransactionManager>();

builder.Services.AddScoped<IQuizMasterDbContext>(provider =>
    provider.GetRequiredService<QuizMasterDbContext>());

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ILearningSessionService, LearningSessionService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<IFlashcardService, FlashcardService>();
builder.Services.AddScoped<IFlashcardSetService, FlashcardSetService>();
```

W tym fragmencie widać, że aplikacja rejestruje interfejsy razem z ich implementacjami. Przykładowo, gdy kontroler potrzebuje `IFlashcardSetService`, kontener DI dostarczy obiekt klasy `FlashcardSetService`.

Dzięki temu kontroler nie musi wiedzieć, jak utworzyć serwis, jakie zależności są potrzebne w jego konstruktorze ani jak długo ten obiekt powinien żyć.

Przykład użycia w kontrolerze:

```csharp
public sealed class FlashcardSetController : ControllerBase
{
    private readonly IFlashcardSetService _flashcardSetService;

    public FlashcardSetController(IFlashcardSetService flashcardSetService)
    {
        _flashcardSetService = flashcardSetService;
    }
}
```

Kontroler deklaruje wyłącznie, że potrzebuje obiektu implementującego `IFlashcardSetService`. Konkretna implementacja jest ustalana w konfiguracji kontenera.

---

### Rejestracja kontekstu bazy danych

W `Program.cs` zarejestrowano również kontekst Entity Framework Core.

```csharp
builder.Services.AddDbContext<QuizMasterDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});
```

Dzięki temu `QuizMasterDbContext` może być automatycznie wstrzykiwany do serwisów aplikacyjnych. Konfiguracja połączenia z bazą danych znajduje się w jednym miejscu, a serwisy nie muszą samodzielnie tworzyć połączenia do SQL Servera.

Dodatkowo zarejestrowano interfejs `IQuizMasterDbContext`:

```csharp
builder.Services.AddScoped<IQuizMasterDbContext>(provider =>
    provider.GetRequiredService<QuizMasterDbContext>());
```

Oznacza to, że klasy mogą zależeć od abstrakcji `IQuizMasterDbContext`, a nie bezpośrednio od konkretnej klasy `QuizMasterDbContext`. Jest to zgodne z podziałem aplikacji na warstwy abstrakcji i implementacji.

---

### Rejestracja autoryzacji JWT

W kontenerze skonfigurowano również mechanizm uwierzytelniania JWT.

```csharp
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
```

Dzięki temu ASP.NET Core automatycznie obsługuje sprawdzanie tokenów JWT dla endpointów zabezpieczonych atrybutem `[Authorize]`.

W konfiguracji tokenu ustawiono między innymi sprawdzanie wystawcy, odbiorcy, czasu życia tokenu oraz klucza podpisu. Oznacza to, że kontrolery nie muszą ręcznie weryfikować tokenów. Ten obowiązek przejmuje middleware ASP.NET Core.

---

### Kontener DI w aplikacji WPF

Mechanizm Dependency Injection został zastosowany również po stronie aplikacji desktopowej WPF. Jest to szczególnie przydatne, ponieważ klasy okien i widoków również posiadają zależności, takie jak klient API, sesja aplikacji albo serwis komunikatów.

Konfiguracja znajduje się w pliku `App.xaml.cs`.

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);

    var services = new ServiceCollection();

    ConfigureServices(services);

    Services = services.BuildServiceProvider();

    var loginWindow = Services.GetRequiredService<LoginWindow>();
    MainWindow = loginWindow;
    loginWindow.Show();
}
```

Podczas startu aplikacji tworzona jest kolekcja usług `ServiceCollection`, następnie wywoływana jest metoda `ConfigureServices`, a na końcu budowany jest kontener usług.

Pierwsze okno, czyli `LoginWindow`, nie jest tworzone ręcznie przez `new LoginWindow()`, tylko pobierane z kontenera:

```csharp
var loginWindow = Services.GetRequiredService<LoginWindow>();
```

Dzięki temu, jeżeli `LoginWindow` potrzebuje w konstruktorze dodatkowych zależności, kontener dostarczy je automatycznie.

---

### Rejestracja usług w aplikacji WPF

W metodzie `ConfigureServices` zarejestrowano usługi wykorzystywane przez aplikację WPF.

```csharp
private void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IMessageDialogService, MessageDialogService>();

    services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7237");
    });

    services.AddHttpClient<IApiClient, ApiClient>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7237");
    });

    services.AddSingleton<IAppSession, AppSession>();
    services.AddSingleton<IAppSettings, AppSettings>();

    services.AddTransient<LoginWindow>();
    services.AddTransient<MainWindow>();
    services.AddTransient<RegisterWindow>();
    services.AddTransient<DashboardView>();
    services.AddTransient<FlashcardSetsView>();
    services.AddTransient<PublicFlashcardSetsView>();
    services.AddTransient<LearningSessionWindow>();
}
```

W tym miejscu rejestrowane są zarówno usługi aplikacyjne klienta, jak i okna oraz widoki WPF.

Dzięki temu widok może w konstruktorze otrzymać zależności, których potrzebuje:

```csharp
public LoginWindow(
    IAuthApiClient authApiClient,
    IMessageDialogService messageDialogService,
    IServiceProvider serviceProvider)
{
    _authApiClient = authApiClient;
    _messageDialogService = messageDialogService;
    _serviceProvider = serviceProvider;

    InitializeComponent();
}
```

Okno logowania nie tworzy samodzielnie klienta API ani serwisu komunikatów. Otrzymuje je z kontenera DI.

---

### Rejestracja klientów HTTP

W projekcie WPF zastosowano również `AddHttpClient`, czyli mechanizm oparty o `HttpClientFactory`.

```csharp
services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7237");
});

services.AddHttpClient<IApiClient, ApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7237");
});
```

Dzięki temu klienty API są rejestrowane jako zależności, a ich konfiguracja znajduje się w jednym miejscu. Widoki WPF nie muszą ustawiać adresu bazowego API ani tworzyć obiektu `HttpClient`.

Przykład użycia w widoku:

```csharp
private readonly IApiClient _apiClient;

public PublicFlashcardSetsView(IApiClient apiClient)
{
    _apiClient = apiClient;

    InitializeComponent();
}
```

Widok wie tylko, że potrzebuje klienta API. Nie interesuje go, jak klient został utworzony ani jak skonfigurowano adres backendu.

---

### Cykle życia usług

Podczas rejestracji usług zastosowano różne cykle życia obiektów.

`AddScoped` został użyty głównie po stronie REST API. Oznacza to, że obiekt żyje w ramach jednego żądania HTTP. Jest to naturalne rozwiązanie dla serwisów aplikacyjnych oraz `DbContext`.

```csharp
builder.Services.AddScoped<IFlashcardSetService, FlashcardSetService>();
builder.Services.AddScoped<IFlashcardService, FlashcardService>();
builder.Services.AddScoped<IAuthService, AuthService>();
```

`AddSingleton` został użyty w WPF dla usług, które powinny istnieć przez cały czas działania aplikacji, na przykład sesja użytkownika lub ustawienia aplikacji.

```csharp
services.AddSingleton<IAppSession, AppSession>();
services.AddSingleton<IAppSettings, AppSettings>();
services.AddSingleton<IMessageDialogService, MessageDialogService>();
```

`AddTransient` został użyty dla okien i widoków WPF. Oznacza to, że przy każdym pobraniu z kontenera tworzona jest nowa instancja.

```csharp
services.AddTransient<LoginWindow>();
services.AddTransient<MainWindow>();
services.AddTransient<RegisterWindow>();
services.AddTransient<LearningSessionWindow>();
```

Taki podział jest logiczny, ponieważ okna powinny być tworzone na nowo, natomiast sesja użytkownika powinna być wspólna dla całej aplikacji.

---

### Dlaczego warto było zastosować DI

Zastosowanie kontenera DI pozwoliło uporządkować tworzenie obiektów w aplikacji. Zależności są rejestrowane w jednym miejscu, a klasy deklarują swoje potrzeby przez konstruktor.

Najważniejsze korzyści z użycia DI w projekcie to:

| Korzyść | Znaczenie w projekcie |
|---|---|
| Mniejsze powiązanie klas | kontrolery i widoki zależą od interfejsów, a nie konkretnych klas |
| Czytelniejsze konstruktory | od razu widać, jakich zależności wymaga dana klasa |
| Jedno miejsce konfiguracji | serwisy są rejestrowane w `Program.cs` i `App.xaml.cs` |
| Łatwiejsza rozbudowa | można dodać nową implementację interfejsu bez zmiany klas korzystających |
| Wspólna konfiguracja HTTP | adres API jest ustawiony przy rejestracji klienta |
| Lepszy podział warstw | API, aplikacja, infrastruktura i WPF korzystają z jasno zdefiniowanych zależności |

---

## Podsumowanie

Projekt **QuizMaster** jest aplikacją klient-serwer służącą do nauki z wykorzystaniem fiszek. System umożliwia tworzenie zestawów, dodawanie fiszek, korzystanie z publicznych materiałów oraz prowadzenie sesji nauki z oceną odpowiedzi.

Podczas realizacji projektu wykorzystano najważniejsze zagadnienia omawiane na laboratoriach, między innymi interfejsy, własne wyjątki, delegaty, zdarzenia, LINQ, typy generyczne, metody rozszerzeniowe oraz programowanie asynchroniczne.

Aplikacja została podzielona na kilka warstw, dzięki czemu logika biznesowa, dostęp do danych, kontrakty, REST API oraz interfejs WPF są od siebie oddzielone. Klient desktopowy nie komunikuje się bezpośrednio z bazą danych, lecz korzysta z przygotowanego API.

W projekcie zastosowano również Dependency Injection, co pozwoliło uporządkować tworzenie obiektów i ograniczyć powiązania pomiędzy klasami. Dzięki temu kod jest bardziej czytelny, łatwiejszy w utrzymaniu i prostszy do dalszej rozbudowy.

Całość stanowi praktyczne wykorzystanie poznanych mechanizmów języka C# i platformy .NET w działającej aplikacji desktopowej połączonej z backendem REST API.

