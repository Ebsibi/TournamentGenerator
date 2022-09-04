using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Turniej
{
    public class Player
    {
        public string nick;
        public string address;
        public int ID;
        public int lvl;
        public string profession;
        public int charID;
        public double factor;
        public Equipment helmet;
        public Equipment ring;
        public Equipment necklace;
        public Equipment gloves;
        public Equipment rightHand;
        public Equipment armor;
        public Equipment leftHand;
        public Equipment shoes;

#pragma warning disable CS8618 // Pole niedopuszczające wartości null musi zawierać wartość inną niż null podczas kończenia działania konstruktora. Rozważ zadeklarowanie pola jako dopuszczającego wartość null.
        public Player(string nick, string address, int ID, int charID, int lvl, string profession)
#pragma warning restore CS8618 // Pole niedopuszczające wartości null musi zawierać wartość inną niż null podczas kończenia działania konstruktora. Rozważ zadeklarowanie pola jako dopuszczającego wartość null.
        {
            this.nick = nick;
            this.address = address;
            this.ID = ID;
            this.charID = charID;
            this.lvl = lvl;
            this.profession = profession;
        }

#pragma warning disable CS8618 // Pole niedopuszczające wartości null musi zawierać wartość inną niż null podczas kończenia działania konstruktora. Rozważ zadeklarowanie pola jako dopuszczającego wartość null.
        public Player(string nick, int lvl, double factor)
#pragma warning restore CS8618 // Pole niedopuszczające wartości null musi zawierać wartość inną niż null podczas kończenia działania konstruktora. Rozważ zadeklarowanie pola jako dopuszczającego wartość null.
        {
            this.nick = nick;
            this.lvl = lvl;
            this.factor = factor;
            this.address = "-";
            this.charID = 0;
            this.ID = 0;
            this.profession = "-";
        }

        public string Print()
        {
            return this.nick + " (" + this.ID + ")\n" + this.lvl + " lvl, " + this.profession + "\n" + this.address + "\nWspółczynnik postaci: " + this.factor + "\n\n";
        }

        public string FactorPrint()
        {
            return this.nick + " " + this.lvl + " lvl " + "Współczynnik postaci: " + this.factor;
        }

        public string GroupElementPrint()
        {
            return this.nick + " (" + this.ID + ") " + this.lvl + " lvl " + this.profession;
        }

        public string LongPrint()
        {
            return this.nick + " (" + this.ID + ")\n" + this.lvl + " lvl, " + this.profession + "\n" + this.address +
                "\nEQ:\n\t" + 
                "\n\tHełm " + this.helmet.Print() +
                "\n\tPierścień " + this.ring.Print() +
                "\n\tNaszyjnik " + this.necklace.Print() +
                "\n\tRękawice " + this.gloves.Print() +
                "\n\tPrawa ręka " + this.rightHand.Print() +
                "\n\tZbroja " + this.armor.Print() +
                "\n\tLewa ręka " + this.leftHand.Print() +
                "\n\tButy " + this.shoes.Print() + "\n\n";
        }

        public void CalculateFactor(string WorldName)
        {
            string EquipmentAddress = this.GenerateJSONAddress(WorldName);
            WebClient client = new WebClient();
            // init strings
            string[] Helmet = { "", "" };
            string HelmetRarity = "";
            string HelmetLvl = "0";
            int HelmetLvlInt = 0;
            string[] Ring = { "", "" };
            string RingRarity = "";
            string RingLvl = "0";
            int RingLvlInt = 0;
            string[] Necklace = { "", "" };
            string NecklaceRarity = "";
            string NecklaceLvl = "0";
            int NecklaceLvlInt = 0;
            string[] Gloves = { "", "" };
            string GlovesRarity = "";
            string GlovesLvl = "0";
            int GlovesLvlInt = 0;
            string[] RightHand = { "", "" };
            string RightHandRarity = "";
            string RightHandLvl = "0";
            string RightHandClass = "";
            int RightHandLvlInt = 0;
            string[] Armor = { "", "" };
            string ArmorRarity = "";
            string ArmorLvl = "0";
            int ArmorLvlInt = 0;
            string[] LeftHand = { "", "" };
            string LeftHandRarity = "";
            string LeftHandLvl = "0";
            int LeftHandLvlInt = 0;
            string[] Shoes = { "", "" };
            string ShoesRarity = "";
            string ShoesLvl = "0";
            int ShoesLvlInt = 0;

            try
            {
                string Page = client.DownloadString(EquipmentAddress);

                try
                {
                    Helmet = Page.Split("\"st\":1"); // taking Helmet Lvl
                    Helmet = Helmet[1].Split(";lvl=");
                    Helmet = Helmet[1].Split(";");
                    HelmetLvl = Helmet[0];
                    Helmet = Page.Split("\"st\":1"); // taking Helmet rarity
                    Helmet = Helmet[1].Split(";rarity=");
                    Helmet = Helmet[1].Split(";");
                    HelmetRarity = Helmet[0];
                    HelmetLvlInt = int.Parse(HelmetLvl);
                }
                catch
                {
                    HelmetRarity = "-";
                    HelmetLvlInt = 0;
                }

                try
                {
                    Ring = Page.Split("\"st\":2"); // taking Ring Lvl
                    Ring = Ring[1].Split(";lvl=");
                    Ring = Ring[1].Split(";");
                    RingLvl = Ring[0];
                    Ring = Page.Split("\"st\":2"); // taking Ring rarity
                    Ring = Ring[1].Split(";rarity=");
                    Ring = Ring[1].Split(";");
                    RingRarity = Ring[0];
                    RingLvlInt = int.Parse(RingLvl);
                }
                catch
                {
                    RingRarity = "-";
                    RingLvlInt = 0;
                }

                try
                {
                    Necklace = Page.Split("\"st\":3"); // taking Necklace Lvl
                    Necklace = Necklace[1].Split(";lvl=");
                    Necklace = Necklace[1].Split(";");
                    NecklaceLvl = Necklace[0];
                    Necklace = Page.Split("\"st\":3"); // taking Necklace rarity
                    Necklace = Necklace[1].Split(";rarity=");
                    Necklace = Necklace[1].Split(";");
                    NecklaceRarity = Necklace[0];
                    NecklaceLvlInt = int.Parse(NecklaceLvl);
                }
                catch
                {
                    NecklaceRarity = "-";
                    NecklaceLvlInt = 0;
                }

                try
                {
                    Gloves = Page.Split("\"st\":4"); // taking Gloves Lvl
                    Gloves = Gloves[1].Split(";lvl=");
                    Gloves = Gloves[1].Split(";");
                    GlovesLvl = Gloves[0];
                    Gloves = Page.Split("\"st\":4"); // taking Gloves rarity
                    Gloves = Gloves[1].Split(";rarity=");
                    Gloves = Gloves[1].Split(";");
                    GlovesRarity = Gloves[0];
                    GlovesLvlInt = int.Parse(GlovesLvl);
                }
                catch
                {
                    GlovesRarity = "-";
                    GlovesLvlInt= 0;
                }

                try
                {
                    RightHand = Page.Split("\"st\":5"); // taking RightHand Lvl
                    RightHand = RightHand[1].Split(";lvl=");
                    RightHand = RightHand[1].Split(";");
                    RightHandLvl = RightHand[0];
                    RightHand = Page.Split("\"st\":5"); // taking RightHand rarity
                    RightHand = RightHand[1].Split(";rarity=");
                    RightHand = RightHand[1].Split(";");
                    RightHandRarity = RightHand[0];
                    RightHand = Page.Split("\"st\":5"); // taking RightHand class
                    RightHand = RightHand[0].Split("\"cl\":");
                    RightHand = RightHand[1].Split(",");
                    RightHandClass = RightHand[0];
                    RightHandLvlInt = int.Parse(RightHandLvl);
                }
                catch
                {
                    RightHandRarity = "-"; 
                    RightHandLvlInt= 0;
                }
                try
                {
                    Armor = Page.Split("\"st\":6"); // taking Armor Lvl
                    Armor = Armor[1].Split(";lvl=");
                    Armor = Armor[1].Split(";");
                    ArmorLvl = Armor[0];
                    Armor = Page.Split("\"st\":6"); // taking Armor rarity
                    Armor = Armor[1].Split(";rarity=");
                    Armor = Armor[1].Split(";");
                    ArmorRarity = Armor[0];
                    ArmorLvlInt = int.Parse(ArmorLvl);
                }
                catch
                {
                    ArmorRarity = "-";
                    ArmorLvlInt = 0;
                }

                try
                {
                    LeftHand = Page.Split("\"st\":7"); // taking LeftHand Lvl
                    LeftHand = LeftHand[1].Split(";lvl=");
                    LeftHand = LeftHand[1].Split(";");
                    LeftHandLvl = LeftHand[0];
                    LeftHand = Page.Split("\"st\":7"); // taking LeftHand rarity
                    LeftHand = LeftHand[1].Split(";rarity=");
                    LeftHand = LeftHand[1].Split(";");
                    LeftHandRarity = LeftHand[0];
                    LeftHandLvlInt = int.Parse(LeftHandLvl);
                }
                catch
                {
                    if (this.profession == "Wojownik" && (RightHandClass == "2" || RightHandClass == "3"))
                    {
                        LeftHandRarity = RightHandRarity;
                        LeftHandLvlInt = RightHandLvlInt;
                    }
                    else
                    {
                        LeftHandRarity = "-";
                        LeftHandLvlInt = 0;
                    } 
                }

                try
                {
                    Shoes = Page.Split("\"st\":8"); // taking Shoes Lvl
                    Shoes = Shoes[1].Split(";lvl=");
                    Shoes = Shoes[1].Split(";");
                    ShoesLvl = Shoes[0];
                    Shoes = Page.Split("\"st\":8"); // taking Shoes rarity
                    Shoes = Shoes[1].Split(";rarity=");
                    Shoes = Shoes[1].Split(";");
                    ShoesRarity = Shoes[0];
                    ShoesLvlInt = int.Parse(ShoesLvl);
                }
                catch
                {
                    ShoesRarity = "-";
                    ShoesLvlInt = 0;
                }
            }
            catch
            {
                HelmetRarity = "-";
                HelmetLvlInt = 0;
                RingRarity = "-";
                RingLvlInt = 0;
                NecklaceRarity = "-";
                NecklaceLvlInt = 0;
                GlovesRarity = "-";
                GlovesLvlInt = 0;
                RightHandRarity = "-";
                RightHandLvlInt = 0;
                ArmorRarity = "-";
                ArmorLvlInt = 0;
                LeftHandRarity = "-";
                LeftHandLvlInt = 0;
                ShoesRarity = "-";
                ShoesLvlInt = 0;

            }
            
            this.helmet = new Equipment(HelmetRarity, HelmetLvlInt);
            this.ring = new Equipment(RingRarity, RingLvlInt);
            this.necklace = new Equipment(NecklaceRarity, NecklaceLvlInt);
            this.gloves = new Equipment(GlovesRarity, GlovesLvlInt);
            this.rightHand = new Equipment(RightHandRarity, RightHandLvlInt);
            this.armor = new Equipment(ArmorRarity, ArmorLvlInt);
            this.leftHand = new Equipment(LeftHandRarity, LeftHandLvlInt);
            this.shoes = new Equipment(ShoesRarity, ShoesLvlInt);

            this.helmet.CalculateFactor();
            this.ring.CalculateFactor();
            this.necklace.CalculateFactor();
            this.gloves.CalculateFactor();
            this.rightHand.CalculateFactor();
            this.armor.CalculateFactor();
            this.leftHand.CalculateFactor();
            this.shoes.CalculateFactor();

            this.factor = this.helmet.factor + this.ring.factor + this.necklace.factor + this.gloves.factor + this.rightHand.factor + this.armor.factor + this.shoes.factor + this.leftHand.factor + (this.lvl - 24);
        }

        public string GenerateJSONAddress(string WorldName)
        {
            return "https://mec.garmory-cdn.cloud/pl/" + WorldName.ToLower() +"/" + this.charID % 128 + "/" + this.charID + ".json";
        }

    }
}
