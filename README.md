# Crm

En liten webapp for kundebehandling (CRM). Dette er startpunktet for DI-oppgaven (1C) i Delmodul 1. Repoet inneholder hele API-et fra API-oppgaven, og du skal nå endre hvordan koden er koblet sammen, uten å endre hva API-et gjør.

## Hva den gjør

- **Kunder:** liste med søk og statusfilter, detaljside, opprett, endre og slett.
- **Aktiviteter:** registrer møter, samtaler, e-poster og oppgaver for en kunde, merk dem som ferdige, og se alle åpne aktiviteter i én oversikt. Forfalte aktiviteter er markert.
- **API (JSON):** alle endepunktene under `/api`, som beskrevet under.
- **Dependency injection (DI):** `ICustomerService` er registrert, og ett endepunkt bruker den. Resten gjør du i oppgaven.

## Krav

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Visual Studio (en nyere versjon, som støtter `.http`-filer), eller en annen editor sammen med kommandolinjeverktøyet `dotnet`

Sjekk at installasjonen fungerer:

```
dotnet --version
```

## Lag din egen kopi

1. Klikk **Use this template** øverst på denne siden, og deretter **Create a new repository**.
2. Klone det nye repoet ditt:

```
git clone <url-to-your-repository>
```

## Kjør appen

**Visual Studio:** åpne `Crm.slnx` (eller `Crm.csproj`) og trykk `F5`. Nettleseren åpner <http://localhost:5080>.

**Kommandolinjen:** i mappen som inneholder `Crm.csproj`:

```
dotnet restore
dotnet run
```

Åpne deretter <http://localhost:5080>.

> **Start alltid appen fra prosjektmappen.** Maler og datafiler leses med relative stier, så sidene feiler hvis appen startes fra en annen mappe.

## API-et

Endepunktene under `/api` returnerer data som JSON, ikke HTML.

### Kunder

| Endepunkt | Beskrivelse | Svar |
|---|---|---|
| `GET /api/customers?search=&status=` | Alle kunder, eventuelt filtrert på tekst og status | 200 |
| `GET /api/customers/{id}` | Én kunde | 200, 404 |
| `POST /api/customers` | Opprett kunde | 201, 400 |
| `PUT /api/customers/{id}` | Oppdater kunde | 200, 404, 400 |
| `DELETE /api/customers/{id}` | Slett kunde og kundens aktiviteter | 204, 404 |

### Aktiviteter

| Endepunkt | Beskrivelse | Svar |
|---|---|---|
| `GET /api/customers/{id}/activities` | Aktivitetene til en kunde, nyeste først | 200, 404 |
| `POST /api/customers/{id}/activities` | Opprett aktivitet for en kunde | 201, 404, 400 |
| `GET /api/activities?open=true` | Alle aktiviteter, eller bare åpne med `open=true` | 200 |
| `PUT /api/activities/{id}/done` | Merk som ferdig eller åpen, med `{ "done": true }` | 204, 404 |
| `DELETE /api/activities/{id}` | Slett aktivitet | 204, 404 |

### Oppslagslister

| Endepunkt | Beskrivelse | Svar |
|---|---|---|
| `GET /api/statuses` | Gyldige kundestatuser | 200 |
| `GET /api/activity-types` | Gyldige aktivitetstyper | 200 |

### Valg i løsningen

- **Validering av kunde:** `name` kan ikke være tom, `email` må inneholde `@`, og `status` må være `Lead`, `Active` eller `Inactive`.
- **Validering av aktivitet:** `title` kan ikke være tom, og `type` må være `Meeting`, `Call`, `Email` eller `Task`. Mangler `date`, brukes nåværende tidspunkt.
- **Feilsvar:** Alle 400-svar har formatet `{ "error": "melding" }`.
- **201 Created:** Svaret fra `POST /api/customers` har en `Location`-header med adressen til den nye kunden. Svaret fra `POST /api/customers/{id}/activities` har ingen, siden API-et ikke har et endepunkt for å hente én aktivitet.
- **Sletting av kunde:** Aktivitetene til kunden slettes sammen med kunden.
- **Sortering:** `GET /api/customers` sorterer på navn.
- **JSON-navn:** Egenskapene i JSON starter med liten bokstav (`name`, `email`).

### Test med `.http`-filer

- `Crm.http` inneholder de tre første kallene fra API-videoen.
- `Assignment.http` inneholder alle kallene som tester løsningen, med forventet statuskode i kommentaren over hvert kall.

Åpne filen i Visual Studio, start appen, og trykk **Send request** over et kall. Kjør kallene ovenfra og ned. Noen kall bruker variabler øverst i filen (`@newCustomerId`, `@activityId` og `@tempCustomerId`). Sett dem til id-en i svaret fra opprett-kallet før du kjører de neste. Hvis appen kjører på en annen port enn 5080, endrer du `@host`.

#### `Assignment.http` og DI

DI endrer hvordan koden er koblet sammen, ikke hva API-et gjør. Alle kallene i `Assignment.http` skal derfor gi de samme statuskodene og svarene etter DI-oppgaven som før.

- Kjør kallene før du begynner, slik at du vet at alt fungerer.
- Kjør dem på nytt etter hver endring.
- Hvis et kall som virket, slutter å virke, har du en loggføring til `Log.md`.

## DI i dette repoet

Dette er allerede gjort, og er det samme som DI-videoen viser:

- `Services/ICustomerService.cs` er et interface med metodene i `CustomerService`.
- `Program.cs` registrerer tjenesten: `builder.Services.AddScoped<ICustomerService, CustomerService>();`
- `GET /api/customers` får `ICustomerService` som parameter, i stedet for å lage tjenesten med `new`.

Alle de andre endepunktene lager fortsatt tjenesten selv med `new CustomerService()`. Det er utgangspunktet for oppgaven.

Under `GET /api/customers` i `Program.cs` står varianten fra videoen som en kommentar. Den bruker `GetAll` og har ikke søk og statusfilter.

## Prosjektstruktur

```
Program.cs          Endepunkter, hjelpefunksjoner og registrering av tjenester (DI)
Models/             Customer og Activity
Services/           ICustomerService, CustomerService og ActivityService (lagrer data i JSON-filer)
Templates/          HTML-maler med {{placeholders}}
wwwroot/            CSS og JavaScript
customers.json      Eksempelkunder JSON format (appen endrer denne filen)
customers.xml		Eksempelkunder XML format (appen endrer denne filen)
Crm.http            De tre første kallene fra API-videoen
Assignment.http     Alle kallene som tester API-et. Skal gi samme svar før og etter DI-oppgaven
Log.md              Logg og designnotater som du fører underveis (del av leveransen)
TestResults.md      Resultatene fra kallene i Assignment.http (del av leveransen)
```

## Data

- `customers.json` og `customers.xml` inneholder fem eksempelkunder. Appen skriver til disse filene når du oppretter, endrer eller sletter kunder, så de vises som endret i Git. Slik gjenoppretter du eksempeldataene:

```
git checkout customers.json
git checkout customers.xml
```

- `activities.json` opprettes første gang du legger til en aktivitet. Den følges ikke av Git. Slett filen for å fjerne alle aktiviteter.

Nullstill dataene før du kjører hele `Assignment.http` på nytt. Standardverdiene for variablene øverst i filen stemmer bare med ferske data.

## Logg og testresultater

Repoet inneholder to filer som du fyller ut underveis. Begge er en del av leveransen, sammen med koden og `Assignment.http`.

- **`Log.md`:** før logg mens du jobber med oppgaven: feil du møter, og korte designnotater om valg du tar. Malene ligger i filen. Commit loggen sammen med koden.
- **`TestResults.md`:** før inn statuskodene du faktisk får for hvert kall i `Assignment.http`. For noen kall står det også hva du skal sjekke i svaret.

## Feilsøking

| Problem | Hva du bør sjekke |
|---|---|
| Feilen `NETSDK1004` (assets file not found) | Kjør `dotnet restore` i mappen med `Crm.csproj`, og bygg på nytt. |
| Feil eller blank side rett etter oppstart | Appen er startet fra feil mappe. Start den fra mappen med `Crm.csproj`. |
| Port 5080 er opptatt | Endre `applicationUrl` i `Properties/launchSettings.json`, og `@host` i `.http`-filene. |
| `Send request` vises ikke over kallene i `.http`-filene | Filen må ha endelsen `.http`, og Visual Studio må være en nyere versjon. |
| Dataene ser rare ut etter eksperimentering | Gjenopprett `customers.json` og slett `activities.json`. |
| Appen feiler etter at du har lagt til en tjeneste som parameter i et endepunkt | Tjenesten er ikke registrert med `builder.Services` i `Program.cs`, eller parameteren har feil type. |
| Et kall i `Assignment.http` gir 404 for en id som skulle finnes | Variablene øverst i filen peker på feil id. Sett dem til id-en i svaret fra opprett-kallet. |