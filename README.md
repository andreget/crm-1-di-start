# Crm: løsning for API-oppgavene

Dette er løsningsforslaget til API-oppgavene i modul 1. Det bygger på startpunktet `crm-1-api-start`, og har alle endepunktene implementert. Neste trinn i modulen er dependency injection.

## Hva den gjør

- **Kunder:** liste med søk og statusfilter, detaljside, opprett, endre og slett (HTML-sidene).
- **Aktiviteter:** møter, samtaler, e-poster og oppgaver per kunde, med oversikt over åpne aktiviteter (HTML-sidene).
- **API (JSON):** alle endepunktene under `/api`, som beskrevet under.

## Krav

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Visual Studio (en nyere versjon, som støtter `.http`-filer), eller en annen editor sammen med kommandolinjeverktøyet `dotnet`

## Kjør appen

**Visual Studio:** åpne `Crm.sln` (eller `Crm.csproj`) og trykk `F5`. Nettleseren åpner <http://localhost:5080>.

**Kommandolinjen:** i mappen som inneholder `Crm.csproj`:

```
dotnet restore
dotnet run
```

> **Start alltid appen fra prosjektmappen.** Maler og datafiler leses med relative stier.

## API-et

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

## Test med `.http`-filer

- `Crm.http` inneholder de tre første kallene fra API-videoen.
- `Assignment.http` inneholder alle kallene som tester løsningen, med forventet statuskode i kommentaren over hvert kall.

Åpne filen i Visual Studio, start appen, og trykk **Send request** over et kall. Kjør kallene ovenfra og ned. Noen kall bruker variabler øverst i filen (`@newCustomerId`, `@activityId` og `@tempCustomerId`). Sett dem til id-en i svaret fra opprett-kallet før du kjører de neste. Hvis appen kjører på en annen port enn 5080, endrer du `@host`.

## Prosjektstruktur

```
Program.cs          Endepunkter (HTML og API) og hjelpefunksjoner
Models/             Customer og Activity
Services/           CustomerService og ActivityService (lagrer data i JSON-filer)
Templates/          HTML-maler med {{placeholders}}
wwwroot/            CSS og JavaScript
customers.json      Eksempelkunder (appen endrer denne filen)
Crm.http            Kall fra API-videoen
Assignment.http     Tester for alle endepunktene
```

## Data

- `customers.json` inneholder fem eksempelkunder, og appen skriver til den. Gjenopprett den med `git checkout customers.json`.
- `activities.json` opprettes første gang du legger til en aktivitet, og følges ikke av Git. Slett filen for å fjerne alle aktiviteter.

## Feilsøking

| Problem | Hva du bør sjekke |
|---|---|
| Feilen `NETSDK1004` (assets file not found) | Kjør `dotnet restore` i mappen med `Crm.csproj`. |
| 404 på et endepunkt som skal finnes | Appen kjører med gammel kode. Stopp den og start på nytt. |
| Feil eller blank side rett etter oppstart | Appen er startet fra feil mappe. |
| Port 5080 er opptatt | Endre `applicationUrl` i `Properties/launchSettings.json`, og `@host` i `.http`-filene. |
| `Send request` vises ikke over kallene | Filen må ha endelsen `.http`, og Visual Studio må være en nyere versjon. |