# Testresultater

Denne filen dokumenterer at kallene i `Assignment.http` gir samme resultat med alle tre implementasjonene av `ICustomerService`. Den er en del av leveransen.

Slik fyller du den ut:

1. Velg implementasjon i `Program.cs`, og nullstill dataene (se under).
2. Start appen og kjør kallene i `Assignment.http` ovenfra og ned.
3. Skriv statuskoden du faktisk fikk, i kolonnen for implementasjonen du tester.
4. Der kolonnen «Sjekk også» har tekst, holder ikke statuskoden alene. Se på innholdet i svaret, og sett `x` bak statuskoden når innholdet stemmer.
5. Gjenta for de to andre implementasjonene.

Slik nullstiller du dataene:

- **JSON:** `git checkout customers.json`
- **XML:** `git checkout customers.xml`
- **Minne:** start appen på nytt
- **Aktiviteter (alle tre):** slett `activities.json`

Hvis et kall ikke gir forventet resultat, har du en loggføring til `Log.md`.

| Implementasjon | Dato for siste hele kjøring |
|---|---|
| JSON | |
| Minne | |
| XML | |

## Del 1: Oppvarming

| Nr | Kall i `Assignment.http` | Forventet | JSON | Minne | XML | Sjekk også |
|---|---|---|---|---|---|---|
| 1 | Get all customers | 200 | | | |  |
| 2 | Get one customer | 200 | | | |  |
| 3 | Get a customer that does not exist | 404 | | | |  |
| 4 | Filter by status | 200 | | | | Bare kunder med status `Lead` |
| 5 | Search by text | 200 | | | | Bare kunder der navn, e-post eller firma inneholder «berg» |
| 6 | Search and filter combined | 200 | | | | Bare kunder som oppfyller begge kravene |
| 7 | Get valid statuses | 200 | | | |  |
| 8 | Get valid activity types | 200 | | | |  |

## Del 2: Kunder

| Nr | Kall i `Assignment.http` | Forventet | JSON | Minne | XML | Sjekk også |
|---|---|---|---|---|---|---|
| 9 | Create customer | 201 | | | | Svaret inneholder den nye kunden med `id` |
| 10 | Create customer without name | 400 | | | |  |
| 11 | Create customer with invalid email | 400 | | | |  |
| 12 | Create customer with invalid status | 400 | | | |  |
| 13 | Update customer | 200 | | | | Svaret inneholder de oppdaterte verdiene |
| 14 | Update customer that does not exist | 404 | | | |  |
| 15 | Update customer with invalid data | 400 | | | |  |
| 16 | Delete customer | 204 | | | |  |
| 17 | Delete the same customer again | 404 | | | |  |

## Del 3: Aktiviteter for en kunde

| Nr | Kall i `Assignment.http` | Forventet | JSON | Minne | XML | Sjekk også |
|---|---|---|---|---|---|---|
| 18 | Get activities for a customer | 200 | | | |  |
| 19 | Get activities for a customer that does not exist | 404 | | | |  |
| 20 | Create activity | 201 | | | | Svaret inneholder den nye aktiviteten med `id` |
| 21 | Create activity without date | 201 | | | | `date` i svaret er nåværende tidspunkt |
| 22 | Create activity without title | 400 | | | |  |
| 23 | Create activity with invalid type | 400 | | | |  |
| 24 | Create activity for a customer that does not exist | 404 | | | |  |

## Del 4: Aktiviteter

| Nr | Kall i `Assignment.http` | Forventet | JSON | Minne | XML | Sjekk også |
|---|---|---|---|---|---|---|
| 25 | Get all activities | 200 | | | |  |
| 26 | Get open activities | 200 | | | | Bare aktiviteter som ikke er ferdige, nærmeste dato først |
| 27 | Mark activity as done | 204 | | | |  |
| 28 | Open activities no longer contain the activity | 200 | | | | Aktiviteten fra kall 27 er ikke i listen |
| 29 | Reopen activity | 204 | | | |  |
| 30 | Mark activity that does not exist as done | 404 | | | |  |
| 31 | Delete activity | 204 | | | |  |
| 32 | Delete the same activity again | 404 | | | |  |

## Del 5: Utfordring

| Nr | Kall i `Assignment.http` | Forventet | JSON | Minne | XML | Sjekk også |
|---|---|---|---|---|---|---|
| 33 | Step 1: Create a customer to delete | 201 | | | |  |
| 34 | Step 2: Add an activity to that customer | 201 | | | |  |
| 35 | Step 3: Delete the customer | 204 | | | |  |
| 36 | Step 4: The activity from step 2 must not be in the list | 200 | | | | Aktiviteten fra steg 2 er ikke i listen |