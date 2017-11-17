using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Assignment
{
    public class Oilseeds
    {
        public string particulars { get; set; }
        public double values { get; set; }
        public string year { get; set; }
        public string values1 { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader(new FileStream("C:/Users/Training/Downloads/Agriculture.csv", FileMode.Open, FileAccess.Read));
            StreamWriter ASSOil = new StreamWriter(new FileStream("C:/Users/Training/Downloads/ASSOilseeds.json", FileMode.OpenOrCreate, FileAccess.Write));
            StreamWriter ASSFood = new StreamWriter(new FileStream("C:/Users/Training/Downloads/ASSFoodgrains.json", FileMode.OpenOrCreate, FileAccess.Write));
            StreamWriter ASSAgg = new StreamWriter(new FileStream("C:/Users/Training/Downloads/ASSAggregate.json", FileMode.OpenOrCreate, FileAccess.Write));
            StreamWriter ASSRi = new StreamWriter(new FileStream("C:/Users/Training/Downloads/ASSRice.json", FileMode.OpenOrCreate, FileAccess.Write));
            List<Oilseeds> oil = new List<Oilseeds>();
            List<Oilseeds> food = new List<Oilseeds>();
            List<Oilseeds> ri = new List<Oilseeds>();
            double[] tempsum = new double[25];
            double[] xyz = new double[25];
            string[] values = new string[25];
            string[] riceval = new string[22];
            for (int i = 0; i < 25; i++)
            {
                xyz[i] = 0;
                tempsum[i] = 0;
            }
            var Header = sr.ReadLine();
            string[] Heading = Header.Split(',');
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] val = line.Split('"');
                if (val.Length > 1)
                    val[1] = val[1].Replace(",", "*");
                line = "";
                foreach (var a in val)
                    line += a;
                values = line.Split(',');
                for (int m = 0; m < values.Length; m++)
                    values[m] = values[m].Replace("*", ",");
                if (values[0].Contains("Oilseeds"))
                {
                    double.TryParse(values[24], out double abc);
                    oil.Add(new Oilseeds() { particulars = values[0], values = abc });
                }
                if (values[0].Contains("Foodgrains"))
                {
                    double.TryParse(values[24], out double abc);
                    food.Add(new Oilseeds() { particulars = values[0], values = abc });
                }
                if (values[0].Contains("Commercial"))
                {
                    for (int i = 0; i < 25; i++)
                    {
                        if (values[i] == "NA")
                            xyz[i] = 0;
                        else
                            double.TryParse(values[i], out xyz[i]);
                        tempsum[i] = tempsum[i] + xyz[i];
                    }
                }
                if (values[0].Contains("Rice") && (values[0].Contains("Karnataka") || values[0].Contains("Kerala") || values[0].Contains("Tamil") || values[0].Contains("Andhra")))
                {
                    for (int i = 3; i < Heading.Length; i++)
                    {
                        if (values[i] == "NA")
                            values[i] = "0";
                        ri.Add(new Oilseeds() { particulars = values[0], values1 = values[i], year = Heading[i] });
                    }
                }
            }
            ASSOil.WriteLine("[");
            var temp = oil.OrderByDescending(m => m.values);
            foreach (var i in temp)
                ASSOil.WriteLine("{" + "\n" + "\t" + "\"" + Heading[0] + "\"" + ":" + "\"" + i.particulars + "\"" + "," + "\n" + "\t" + "\"" + "3-2013" + "\"" + ":" + "\"" + i.values + "\"\n" + ((i.particulars == "Agriculture Production Foodgrains Yield Oilseeds Nine Oilseeds Sunflower") ? "}" : "},"));
            ASSOil.WriteLine("]");
            ASSOil.Flush();
            ASSFood.WriteLine("[");
            var temp1 = food.OrderByDescending(m => m.values);
            foreach (var i in temp1)
                ASSFood.WriteLine("{" + "\n" + "\t" + "\"" + Heading[0] + "\"" + ":" + "\"" + i.particulars + "\"" + "," + "\n" + "\t" + "\"" + "3-2013" + "\"" + ":" + "\"" + i.values + "\"\n" + ((i.particulars == "Agricultural Production Foodgrains Coarse Cereals Yield West Bengal") ? "}" : "},"));
            ASSFood.WriteLine("]");
            ASSFood.Flush();
            ASSAgg.WriteLine("[");
            for (int x = 3; x < 25; x++)
                ASSAgg.WriteLine((tempsum[x] == 407.685) ? "{" + "\n" + "\t" + "\"" + "Year" + "\"" + ":" + "\"" + Heading[x] + "\"" + "," + "\n" + "\t" + "\"" + "Value" + "\"" + ":" + "\"" + tempsum[x] + "\"\n}" : "{" + "\n" + "\t" + "\"" + "Year" + "\"" + ":" + "\"" + Heading[x] + "\"" + "," + "\n" + "\t" + "\"" + "Value" + "\"" + ":" + "\"" + tempsum[x] + "\"\n},");
            ASSAgg.WriteLine("]");
            ASSAgg.Flush();
            ASSRi.WriteLine("[");
            //int count = 0;
            var x1 = ri.GroupBy(m => m.year);
            foreach (var ric in x1)
            {
                ASSRi.WriteLine("{\"year\" : \"" + ric.Key + "\",");
                foreach (var item in ric)
                {
                    if (item.particulars == "Agricultural Production Foodgrains Rice Yield Tamil Nadu")
                        ASSRi.WriteLine("\t \"" + item.particulars + "\" : \"" + item.values1 + "\"");
                    else
                        ASSRi.WriteLine("\t \"" + item.particulars + "\" : \"" + item.values1 + "\",");
                }
                if (ric.Key == " 3-2014")
                    ASSRi.WriteLine("}");
                else
                    ASSRi.WriteLine("},");
            }
            ASSRi.WriteLine("]");
            ASSRi.Flush();
        }
    }
}