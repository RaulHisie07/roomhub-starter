# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|

| R7| services/bookingservice.cs - metoda create| testare cu ai si confirmare cu verificari http | comparatiile cu <= initial au fost inlocuite cu<  |
| R8 | bookingservice.cs metoda create | analiza ai si verificare http executate de ai | a fost adaugata conditia ca doar rezervarile active sa fie verificate in suprapunere |
| r8 | bookingservice.cs metoda cancel |  | analiza ai |
| anularea repetata intorcea 204. a fost adaugata verificarea statusului , deci anularea unei rezervari deja anulate intorce 409 
| r6 | bookingservice.cs metoda create | analiza codului cu ai, test,reading | lipsea limita duratei, acum sunt respinse rezervarile mai lungi de 4 ore|
| r5 | services/bookingservice.cs metoda create | cod analizat prin ai , inspectie prin citirea codului si sesizarea problemelor | starterul permitea rezervari cu inceputul in trecut . a fost adaugata verificarea in booking.create,folosind timpul primit de handler in timeprovider |
| r7 si r9 | Features/Bookings/CreateBooking/CreateBookingHandler.cs| analiza cu ai , inspectarea personala , reading ,testing | doua cereri puteau verifica aceleasi date si accepta impreuna rezervari nepermise . acum verificarea si salvarea se fac intr o tranzactie  |
| _(fill in)_ | _(fill in)_ | _(fill in)_ | _(fill in)_ |
