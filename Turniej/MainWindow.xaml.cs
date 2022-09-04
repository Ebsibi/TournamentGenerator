using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using Xceed.Document.NET;
using Xceed.Words.NET;
using SwiftExcel;

/*Thread thread = new Thread(new ThreadStart(delegate()
                {
                    Thread.Sleep(200); // this is important ...
                    try
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Send,
                            new NoArgsHandle(delegate()
                            {
                               // do something, set .Text = "some text"
                            }));
                    }
                    catch { }
                }));
                thread.Name = "thread-UpdateText";
                thread.Start();
*/

namespace Turniej
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentGenerator\\"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentGenerator\\");
            InitializeComponent();
        }

        public List<Player> TournamentListGenerator(List<string> ListOfSubmitted, string WorldName, DateTime LocalDate)
        {
            // initializing variables and creating string containing world's name + datetime stamp
            string WorldDateTimeString = LocalDate.ToString();
            WorldDateTimeString = WorldName + " " + WorldDateTimeString;
            WorldDateTimeString = WorldDateTimeString.Replace(" ", "_");
            WorldDateTimeString = WorldDateTimeString.Replace(":", "_");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentGenerator\\" + WorldDateTimeString + ".txt";

            // making list where will be placed addresses from Margonem ladder, nicks, IDs, lvls, professions
            List<string> AddressesList = LadderAddressesGenerator(WorldName);
            List<string> CharsAddressesList = new List<string>();
            List<string> IDsList = new List<string>();
            List<string> CharsIDsList = new List<string>();
            List<string> NicksList = new List<string>();
            List<string> LVLsList = new List<string>();
            List<string> ProfessionsList = new List<string>();
            WebClient client = new WebClient();
            foreach (string address in AddressesList)
            {
                string Page = client.DownloadString(address);
                string ScrappedPage = Page;
                string[] ArrayScrappedPage = ScrappedPage.Split("Przejdź do strony:");
                ArrayScrappedPage = ArrayScrappedPage[0].Split("<tbody>");
                ArrayScrappedPage = ArrayScrappedPage[1].Split("<a href=\"");
                foreach (string element in ArrayScrappedPage)
                {
                    ArrayScrappedPage = element.Split("\">");
                    CharsAddressesList.Add("https://www.margonem.pl"+ArrayScrappedPage[0]);
                }

                ScrappedPage = Page;
                ArrayScrappedPage = ScrappedPage.Split("Przejdź do strony:");
                ArrayScrappedPage = ArrayScrappedPage[0].Split("<tbody>");
                ArrayScrappedPage = ArrayScrappedPage[1].Split("<a href=\"/profile/view,");
                foreach (string element in ArrayScrappedPage)
                {
                    ArrayScrappedPage = element.Split("#char_");
                    IDsList.Add(ArrayScrappedPage[0]);
                }

                ScrappedPage = Page;
                ArrayScrappedPage = ScrappedPage.Split("Przejdź do strony:");
                ArrayScrappedPage = ArrayScrappedPage[0].Split("<tbody>");
                ArrayScrappedPage = ArrayScrappedPage[1].Split("#char_");
                foreach (string element in ArrayScrappedPage)
                {
                    ArrayScrappedPage = element.Split(","+WorldName.ToLower());
                    CharsIDsList.Add(ArrayScrappedPage[0]);
                }

                ScrappedPage = Page;
                ArrayScrappedPage = ScrappedPage.Split("Przejdź do strony:");
                ArrayScrappedPage = ArrayScrappedPage[0].Split("<tbody>");
                ArrayScrappedPage = ArrayScrappedPage[1].Split(WorldName.ToLower() + "\">");
                foreach (string element in ArrayScrappedPage)
                {
                    ArrayScrappedPage = element.Split("</a>");
                    ArrayScrappedPage[0] = ArrayScrappedPage[0].Trim();
                    NicksList.Add(ArrayScrappedPage[0]);
                }

                ScrappedPage = Page;
                ArrayScrappedPage = ScrappedPage.Split("Przejdź do strony:");
                ArrayScrappedPage = ArrayScrappedPage[0].Split("<tbody>");
                ArrayScrappedPage = ArrayScrappedPage[1].Split("<td class=\"long-level\">");
                foreach (string element in ArrayScrappedPage)
                {
                    ArrayScrappedPage = element.Split("</td>");
                    ArrayScrappedPage[0] = ArrayScrappedPage[0].Trim();
                    LVLsList.Add(ArrayScrappedPage[0]);
                }

                ScrappedPage = Page;
                ArrayScrappedPage = ScrappedPage.Split("Przejdź do strony:");
                ArrayScrappedPage = ArrayScrappedPage[0].Split("<tbody>");
                ArrayScrappedPage = ArrayScrappedPage[1].Split("<td class=\"long-players\">");
                foreach (string element in ArrayScrappedPage)
                {
                    ArrayScrappedPage = element.Split("</td>");
                    ArrayScrappedPage[0] = ArrayScrappedPage[0].Trim();
                    ProfessionsList.Add(ArrayScrappedPage[0]);
                }
                textBoxProgress.AppendText("Zebrano dane z podstrony."+Environment.NewLine);
            }

            // deleting unnecessary elements
            CharsAddressesList.RemoveAll(FirstBadListElement);
            CharsAddressesList.RemoveAll(SecondBadListElement);
            IDsList.RemoveAll(ThirdBadListElement);
            CharsIDsList.RemoveAll(ThirdBadListElement);
            NicksList.RemoveAll(FourthBadListElement);
            LVLsList.RemoveAll(FourthBadListElement);
            ProfessionsList.RemoveAll(FourthBadListElement);

            // Converting strings list to ints list
            List<int> IDsListInt = IDsList.Select(s => int.Parse(s)).ToList();
            List<int> CharsIDsListInt = CharsIDsList.Select(s => int.Parse(s)).ToList();
            List<int> LVLsListInt = LVLsList.Select(s => int.Parse(s)).ToList();


            // Creating Player Objects
            Player[] PlayerList = new Player[CharsAddressesList.Count];
            for (int i=0; i<CharsAddressesList.Count; i++)
            {
                // making objects
                PlayerList[i] = new Player(NicksList[i], CharsAddressesList[i], IDsListInt[i], CharsIDsListInt[i], LVLsListInt[i], ProfessionsList[i]);
                textBoxProgress.AppendText("Stworzono obiekt " + i + " z " + CharsAddressesList.Count + "." + Environment.NewLine);
            }

            if(running.Default.CharsList == true)
            {
                // creating file with list
                using (FileStream fs = File.Create(path))
                    fs.Close();
                for (int i = 0; i < CharsAddressesList.Count; i++)
                {
                    // saving Players list to file
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("{0,-21} {1,-70} {2,-10} {3,-5} {4}", NicksList[i], CharsAddressesList[i], IDsListInt[i], LVLsListInt[i], ProfessionsList[i]);
                    }
                }
            }

            List<Player> TournamentPlayerList = new List<Player>();

            foreach (string Submitted in ListOfSubmitted)
            {
                foreach (Player player in PlayerList)
                {
                    if(player.nick == Submitted)
                        TournamentPlayerList.Add(player);
                }
            }

            return TournamentPlayerList;
        }

        private static bool FirstBadListElement(String s)
        {
            return s.EndsWith("https://www.margonem.pl\n                                                                <tr>\n                            <td class=\"dark-cell id");
        }
        private static bool SecondBadListElement(String s)
        {
            return s.StartsWith("https://www.margonem.pl/ladder");
        }
        private static bool ThirdBadListElement(String s)
        {
            return s.StartsWith("\n                                                                <tr>");
        }
        private static bool FourthBadListElement(String s)
        {
            return s.StartsWith("<tr>");
        }


        public List<string> LadderAddressesGenerator(string WorldName)
        {
            // initializing variables
            string ScrappedPage = "";

            // making list where will be placed addresses from Margonem ladder
            List<string> AddressesList = new List<string>();
            int i = 1;
            bool IsWebsiteExisting = true;
            do
            {
                string FirstPartOfAddress = "https://www.margonem.pl/ladder/players,";
                string ThirdPartOfAddress = "?page=";
                string LadderAddress = FirstPartOfAddress + WorldName + ThirdPartOfAddress + i;

                WebClient client = new WebClient();
                try
                {
                    ScrappedPage = client.DownloadString(LadderAddress);
                    AddressesList.Add(LadderAddress);
                    textBoxProgress.AppendText("Dodano " + LadderAddress + " do listy adresów." + Environment.NewLine);
                }
                catch
                {
                    IsWebsiteExisting = false;
                }
                i++;
            } while (IsWebsiteExisting == true);

            return AddressesList;
        }

        public int CalculateHowManyGroupsNeeded(List<Player> players, string WorldName)
        {
            int HowManyNeeded = 0;
            switch (players.Count)
            {
                case int n when (players.Count < 6 && players.Count >= 1):
                    HowManyNeeded = 1;
                    break;
                case int n when (players.Count < 12 && players.Count >= 6):
                    if (running.Default.LargeGroups == true)
                        HowManyNeeded = 1;
                    else
                        HowManyNeeded = 2;
                    break;
                case int n when (players.Count < 24 && players.Count >= 12):
                    if (running.Default.LargeGroups == true)
                        HowManyNeeded = 2;
                    else
                        HowManyNeeded = 4;
                    break;
                case int n when (players.Count < 48 && players.Count >= 24):
                    if (running.Default.LargeGroups == true)
                        HowManyNeeded = 4;
                    else
                        HowManyNeeded = 8;
                    break;
                case int n when (players.Count < 96 && players.Count >= 48):
                    if (running.Default.LargeGroups == true)
                        HowManyNeeded = 8;
                    else
                        HowManyNeeded = 16;
                    break;
                case int n when (players.Count >= 96):
                    if (running.Default.LargeGroups == true)
                        HowManyNeeded = 16;
                    else
                        HowManyNeeded = 32;
                    break;

            }
            return HowManyNeeded;
        }

        public static List<List<Player>> SplitList(List<Player> locations, int nSize)
        {
            var list = new List<List<Player>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }

            return list;
        }

        public List<List<Player>> TournamentGroupGenerator(List<Player> players, string WorldName, DateTime LocalDate)
        {
            // calling function which can calculate players' factor
            foreach (Player player in players)
            {
                player.CalculateFactor(WorldName);
                textBoxProgress.AppendText("Obliczono współczynnik Gracza " + player.nick + ". Wynosi on " + player.factor + "." + Environment.NewLine);
            }

            // sorting by factor
            players.Sort(delegate (Player x, Player y) {
                return y.factor.CompareTo(x.factor);
            });
            /* foreach (Player player in players)
            {
                Console.WriteLine(player.FactorPrint());
            } */
textBoxProgress.AppendText("Uporządkowano Graczy pod względem współczynnika." + Environment.NewLine);

            int HowManyGroupsNeeded = CalculateHowManyGroupsNeeded(players, WorldName);
            List<List<Player>> TournamentPots = SplitList(players, HowManyGroupsNeeded);
            textBoxProgress.AppendText("Utworzono koszyki." + Environment.NewLine);
            // this.Dispatcher...
            var lastItem = TournamentPots.Last();
            if (lastItem.Count < HowManyGroupsNeeded)
            {
                int i = HowManyGroupsNeeded - lastItem.Count;
                for (int j = 0; j < i; j++)
                {
                    lastItem.Add(new Player("Ghost", 0, 0));
                }
            }

            foreach (List<Player> Pot in TournamentPots)
            {
                Pot.Shuffle();
            }
            textBoxProgress.AppendText("Rozlosowano grupy." + Environment.NewLine);

            List<List<Player>> Groups = new List<List<Player>>();
            for (int i = 0; i < HowManyGroupsNeeded; i++)
            {
                List<Player> Group = new List<Player>();
                for (int j = 0; j < TournamentPots.Count; j++)
                {
                    if(TournamentPots[j][i].nick != "Ghost")
                    {
                        Group.Add(TournamentPots[j][i]);
                        textBoxProgress.AppendText("Gracz " + TournamentPots[j][i].nick + " trafił do grupy " + (i + 1) + "." + Environment.NewLine);
                    }
                }
                Groups.Add(Group);
            }

            return Groups;
        }

        public string ListMatches(List<Player> ListTeam, int i)
        {
            string Text = "Grupa " + i + ":\n";
            if (ListTeam.Count % 2 != 0)
            {
                Player temp = new Player("Odpoczynek", 0, 0);
                ListTeam.Add(temp);
            }

            int numDays = (ListTeam.Count - 1);
            int halfSize = ListTeam.Count / 2;

            List<Player> teams = new List<Player>();

            teams.AddRange(ListTeam.Skip(halfSize).Take(halfSize));
            teams.AddRange(ListTeam.Skip(1).Take(halfSize - 1).ToArray().Reverse());

            int teamsSize = teams.Count;

            for (int day = 0; day < numDays; day++)
            {
                Text += "\n/nar Kolejka " + (day + 1) + "\n";
               
                int teamIdx = day % teamsSize;
                if (teams[teamIdx].nick == "Odpoczynek")
                    Text += "/nar " + ListTeam[0].nick + " odpoczywa w tej kolejce.\n";
                else if (ListTeam[0].nick == "Odpoczynek")
                    Text += "/nar " + teams[teamIdx].nick + " odpoczywa w tej kolejce.\n";
                else
                    Text += "/nar " + teams[teamIdx].nick + " vs " + ListTeam[0].nick + "\n";

                for (int idx = 1; idx < halfSize; idx++)
                {
                    int firstTeam = (day + idx) % teamsSize;
                    int secondTeam = (day + teamsSize - idx) % teamsSize;
                    if (teams[firstTeam].nick == "Odpoczynek")
                        Text += "/nar " + teams[secondTeam].nick + " odpoczywa w tej kolejce.\n";
                    else if (teams[secondTeam].nick == "Odpoczynek")
                        Text += "/nar " + teams[firstTeam].nick + " odpoczywa w tej kolejce.\n";
                    else
                        Text += "/nar " + teams[firstTeam].nick + " vs " + teams[secondTeam].nick + "\n";
                }
            }
            Text += "\n";
            return Text;
        }

        public void GenerateTxtFileContainingGroupSchedule(List<List<Player>> Groups, string WorldName, DateTime LocalDate)
        {
            // initializing variables and creating string containing world's name + datetime stamp
            
            string WorldDateTimeString = LocalDate.ToString();
            WorldDateTimeString = WorldName + " " + WorldDateTimeString;
            WorldDateTimeString = WorldDateTimeString.Replace(" ", "_");
            WorldDateTimeString = WorldDateTimeString.Replace(":", "_");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentGenerator\\" + "Losowanie_" + WorldDateTimeString + ".txt";

            // making a file
            using (FileStream fs = File.Create(path))
                fs.Close();

            int i = 1;
            foreach (List<Player> Group in Groups)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("Grupa " + i);
                }
                foreach (Player player in Group)
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(player.GroupElementPrint());
                    }
                }
                i++;
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("\n");
                }
            }
        }

        private void GenerateXlsxFile(List<List<Player>> Groups, string Interval, string path)
        {
            int baseX = 1;
            int baseY = 1;
            int X = baseX + 1;
            int Y = baseY + 1;
            int i = 1;
            using (var ew = new ExcelWriter(path))
            {
                foreach (List<Player> Group in Groups)
                {
                    Y++;
                    ew.Write("Grupa " + i, X, Y);
                    X = baseX + 1;
                    foreach (Player player in Group)
                    {
                        if (player.nick != "Odpoczynek")
                        {
                            ew.Write(player.nick, X + 1, Y);
                            X++;
                        }
                    }
                    X = baseX + 1;
                    foreach (Player player in Group)
                    {
                        if (player.nick != "Odpoczynek")
                        {
                            ew.Write(player.nick, X, Y + 1);
                            Y++;
                        }
                    }
                    Y = baseY + 1 + ((Group.Count * i) + 1);
                    i++;
                }

                ew.Save();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textBoxWorldName.Clear();
            textBoxListOfSubtmitted.Clear();
            textBoxContaingGroups.Clear();
            textBoxProgress.Clear();
            textBoxInterval.Clear();
            textBoxProgress.AppendText("Progress:");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            textBoxContaingGroups.Clear();
            textBoxProgress.Clear();
            textBoxProgress.AppendText("Progress:");
            textBoxProgress.AppendText(Environment.NewLine);
            textBoxProgress.AppendText("Rozpoczęto generowanie grup."+Environment.NewLine);
            DateTime LocalDate = DateTime.Now;
            string WorldName = textBoxWorldName.Text;
            string ListOfSubmitted = textBoxListOfSubtmitted.Text;
            string Interval = textBoxInterval.Text;
            List<string> Submitted = ListOfSubmitted.Split(',').ToList();
            List<Player> players = TournamentListGenerator(Submitted, WorldName, LocalDate);
            List<List<Player>> Groups = TournamentGroupGenerator(players, WorldName, LocalDate);
            GenerateTxtFileContainingGroupSchedule(Groups, WorldName, LocalDate);

            string WorldDateTimeString = LocalDate.ToString();
            WorldDateTimeString = WorldName + " " + WorldDateTimeString;
            WorldDateTimeString = WorldDateTimeString.Replace(" ", "_");
            WorldDateTimeString = WorldDateTimeString.Replace(":", "_");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentGenerator\\" + "Losowanie_" + WorldDateTimeString + ".txt";

            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(path);

            // Display the file contents by using a foreach loop.
            textBoxContaingGroups.AppendText("Wyniki losowania:" + Environment.NewLine);
            foreach (string line in lines)
            {
                if(line != "Ghost (0) 0 lvl -")
                    textBoxContaingGroups.AppendText(line + Environment.NewLine);
            }
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentGenerator\\" + "Rozpiska_" + Interval + "_" + WorldDateTimeString + ".docx";
            if (running.Default.DocxFile == true)
            {
                var doc = DocX.Create(path);
                doc.AddHeaders();
                doc.Headers.First.InsertParagraph("Rozpiska przedziału " + Interval).Bold();
                int i = 1;
                foreach (List<Player> group in Groups)
                {
                    doc.InsertParagraph(ListMatches(group, i));
                    i++;
                }
                doc.Save();
            }
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentGenerator\\" + "Wiadomości_na_skrzynkę_" + Interval + "_" + WorldDateTimeString + ".txt";
            if (running.Default.MessagesOnMailbox == true)
            {
                int i = 1;
                foreach (List<Player> group in Groups)
                {
                    foreach (Player player in group)
                    {
                        if (player.nick != "Odpoczynek")
                        {
                            using (StreamWriter sw = File.AppendText(path))
                            {
                                sw.WriteLine(player.nick + ", dziękujemy za zgłoszenie się do rywalizacji!");
                                sw.WriteLine("System losujący przydzielił Cię do grupy " + i + ", w której zmierzysz się z:\n");
                                foreach (Player rival in group)
                                {
                                    if ((rival.nick != player.nick) && rival.nick != "Odpoczynek")
                                    {
                                        sw.WriteLine(rival.nick + " (" + rival.lvl + " lvl, " + rival.profession + ") " + rival.address);
                                    }
                                }
                                sw.WriteLine("\nInformacje o dacie i godzinie rywalizacji w Twojej grupie znajdują się na forum (link jest również na czacie, w systemowej wiadomości dnia).\nDaj z siebie wszystko,\nPowodzenia!\n\n");
                            }
                        }
                    }
                    i++;
                }
                
            }
            if (running.Default.XlsxFile == true)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentGenerator\\" + "Rozpiska_" + Interval + "_" + WorldDateTimeString + ".xlsx";
                GenerateXlsxFile(Groups, Interval, path);
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            settigns settingsWindow = new settigns();
            settingsWindow.Show();
            bool CharListValue = running.Default.CharsList;
            bool DocxFileValue = running.Default.DocxFile;
            bool XlsxFileValue = running.Default.XlsxFile;
            bool MessagesOnMailboxValue = running.Default.MessagesOnMailbox;
            bool LargeGroupsValue = running.Default.LargeGroups;
            settingsWindow.CharListOption.IsChecked = CharListValue;
            settingsWindow.DocxOption.IsChecked = DocxFileValue;
            settingsWindow.XlsxOption.IsChecked = XlsxFileValue;
            settingsWindow.MessagesOnMailbox.IsChecked = MessagesOnMailboxValue;
            settingsWindow.LargeGroups.IsChecked = LargeGroupsValue;
        }
    }

    public static class Extensions
    {
        private static Random rand = new Random();

        public static void Shuffle<T>(this IList<T> values)
        {
            for (int i = values.Count - 1; i > 0; i--)
            {
                int k = rand.Next(i + 1);
                T value = values[k];
                values[k] = values[i];
                values[i] = value;
            }
        }
    }
}
