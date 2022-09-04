# Turniej
## Wstęp

Margonem to gra MMORPG, w której gracze mogą prowadzić rozgrywkę na wielu serwerach. Część z nich zarządzana jest przez administrację gry, pozostałe zaś przez graczy, którzy mają rangę zarządcy. Zarządcy mogą powoływać pomocników, którzy będą im pomagać w prowadzeniu świata. Jednym z elementów prowadzenie świata jest organizowanie atrakcji. Chyba najciekawszą z atrakcji są turnieje PvP (gracz vs gracz).

## Generator

Niedawno zostałem zarządcą jednego ze światów prywatnych. Większość światów prywatnych posiada niewiele postaci, mój z kolei ma ich bardzo dużo. Zrobienie takiego turnieju układając grupy i kolejki ręcznie zajęłoby ogrom czasu, dlatego postanowiłem zrobić generator turniejowy.

## Działanie

Program posiada kilka pól, które można uzupełniać. W polu "Nazwa świata" należy wpisać prawdziwą nazwę świata z Margonem. Lista światów znajduje się w rankingu: https://www.margonem.pl/ladder Do testów polecam wybrać światy prywatne, jak wspomniałem, są mniejsze, więc output załaduje się znacznie szybciej. Następnym polem jest pole "Przedział", gdzie wpisuje się albo własną nazwę przedziału, albo maksymalny poziom z jakimi gracze mogą wziąć udział w turnieju (przykładowo 167). Ostatnim polem do uzupełniania jest pole "Lista postaci". Przykładowe dane do generatora: świat: Legion, Przedział: 64, Lista postaci: Komisarz Czechov,Kryształek Mocy,Adraxer,Zjarany Łysy,Vixenn,Lonzzo,Lemoniadowy Lui,Nituś Emeryt,Saeliś,Szek Truskawkowy,Woluś. W ustawieniach można wybrać, czy chcemy zrzut rankingu świata w postaci pliku .txt, komendy na czat w pliku .docx oraz plik do zapisywania wyników w formacie .xlsx.