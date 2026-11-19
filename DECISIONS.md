# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
am ales r8 , anularea unei rezervari. in starter era in serviciul de rezervari, iar acum este in metoda de anulare a entitatii booking. regula sta in domeniu pentru ca rezervarea sa nu poata fi anulata de doua ori.

_(your answer here)_

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

andler-ul intoarce un rezultat cu eroare de conflict, fara exceptie de business. mapper-ul transforma eroarea intr-un raspuns http 409, fara salvarea rezervarii.

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

ef core este folosit de contextul bazei, configurari, repository-uri, unit of work si configurarea aplicatiei. domeniul si handler-ele nu depind direct de el. castig separarea responsabilitatilor si testarea mai usoara, dar adaug mai multe clase si interfete.

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

endpoint -> command -> handler
-> unit of work: incepe tranzactia
-> repository: cauta sala
-> domeniu: valideaza rezervarea
-> repository: citeste suprapunerile si numara rezervarile
-> domeniu: verifica limitele
-> repository: pregateste adaugarea
-> unit of work -> context ef core -> sqlite
-> commit -> result -> mapper -> http 201

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

nu am respins sau corectat personal o propunere din implementarea primita. am pastrat modificarile celor trei etape. nu pot prezenta sincer un exemplu de respingere pe care nu am facut-o.

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

as adauga migrari ef core pentru modificarile viitoare ale bazei. momentan, aplicatia creeaza schema initiala, dar nu actualizeaza automat o baza existenta. riscul este ca modelul modificat sa nu mai corespunda structurii bazei.
