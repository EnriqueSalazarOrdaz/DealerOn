using DealerOn.Core;
using System;
using System.Collections.Generic;

namespace DealerOn.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Wich exercise do you want to run: 1, 2, 3");
            var opc = System.Console.ReadLine();

            switch (opc)
            {
                case "1":
                    #region NASARemotely
                    //just the inputs from here, and inside the class resolving the problem.
                    NASARemotelyControl nASARemotelyControl = new NASARemotelyControl();

                    System.Console.WriteLine("Write the settup for coordinate grid. Example: 5 5");
                    var explorationGrid = nASARemotelyControl.SetGrid(System.Console.ReadLine());

                    System.Console.WriteLine("Set the start point and facing to. Example: 1 2 N");
                    nASARemotelyControl.SetInstructions(System.Console.ReadLine(), explorationGrid);
                    System.Console.WriteLine("Set Directions. Example: LMLMLMLMM");
                    var result = nASARemotelyControl.SetInstructions(System.Console.ReadLine(), explorationGrid);
                    System.Console.WriteLine(result);

                    //uncomment if you want to run directly
                    //var explorationGrid = nASARemotelyControl.SetGrid("5 5");

                    //nASARemotelyControl.SetInstructions("1 2 N", explorationGrid);
                    //var result1 = nASARemotelyControl.SetInstructions("LMLMLMLMM", explorationGrid);

                    //nASARemotelyControl.SetInstructions("3 3 E", explorationGrid);
                    //var result2 = nASARemotelyControl.SetInstructions("MMRMMRMRRM", explorationGrid);

                    //nASARemotelyControl.SetInstructions("3 3 w", explorationGrid);
                    //var result3 = nASARemotelyControl.SetInstructions("MMRMMRMRRM", explorationGrid);


                    //System.Console.WriteLine(result1);
                    //System.Console.WriteLine(result2);
                    //System.Console.WriteLine(result3);
                    #endregion
                    break;
                case "2":
                    #region Sales Taxes

                    double ibop = 0, mcd = 0, bop = 0, iboc = 0, book = 0, cb = 0, pohp = 0;
                    List<Items> items2pay = new List<Items>();
                    bool steelBuyingYes;
                    do
                    {
                        System.Console.Clear();
                        System.Console.WriteLine("Select an item from the list, example 1, then set the price if haven't yet");
                        System.Console.WriteLine("1: Music CD ");
                        System.Console.WriteLine("2: Bottle Of Perfume");
                        System.Console.WriteLine("3: Imported Box Of Chocolates");
                        System.Console.WriteLine("4: Imported Bottle Of Perfume");
                        System.Console.WriteLine("5: Book");
                        System.Console.WriteLine("6: Chocalete Bar");
                        System.Console.WriteLine("7: Packet Of Headache Pills");
                        var itemNumber = System.Console.ReadLine();
                        switch (itemNumber)
                        {
                            case "1":
                                if (mcd == 0.0)
                                {
                                    System.Console.WriteLine("set the price");
                                    mcd = Convert.ToDouble(System.Console.ReadLine());
                                }
                                items2pay.Add(Items.MusicCD);
                                break;
                            case "2":
                                if (bop == 0.0)
                                {
                                    System.Console.WriteLine("set the price");
                                    bop = Convert.ToDouble(System.Console.ReadLine());
                                }
                                items2pay.Add(Items.BottleOfPerfume);
                                break;
                            case "3":
                                if (iboc == 0.0)
                                {
                                    System.Console.WriteLine("set the price");
                                    iboc = Convert.ToDouble(System.Console.ReadLine());
                                }
                                items2pay.Add(Items.ImportedBoxOfChocolates);
                                break;
                            case "4":
                                if (ibop == 0.0)
                                {
                                    System.Console.WriteLine("set the price");
                                    ibop = Convert.ToDouble(System.Console.ReadLine());
                                }
                                items2pay.Add(Items.ImportedBottleOfPerfume);
                                break;
                            case "5":
                                if (book == 0.0)
                                {
                                    System.Console.WriteLine("set the price");
                                    book = Convert.ToDouble(System.Console.ReadLine());
                                }
                                items2pay.Add(Items.Book);
                                break;
                            case "6":
                                if (cb == 0.0)
                                {
                                    System.Console.WriteLine("set the price");
                                    cb = Convert.ToDouble(System.Console.ReadLine());
                                }
                                items2pay.Add(Items.ChocaleteBar);
                                break;
                            case "7":
                                if (pohp == 0.0)
                                {
                                    System.Console.WriteLine("set the price");
                                    pohp = Convert.ToDouble(System.Console.ReadLine());
                                }
                                items2pay.Add(Items.PacketOfHeadachePills);
                                break;
                            default:
                                System.Console.WriteLine("invalid option");
                                break;
                        }
                        System.Console.WriteLine("do you want to add another item?:\n yes(y) no(n): ");
                        var opcSteelBuying = System.Console.ReadLine();
                        steelBuyingYes = opcSteelBuying.ToLower() == "yes" || opcSteelBuying.ToLower() == "y";
                    } while (steelBuyingYes);

                    var items15Taxes = new Dictionary<Items, double>() { { Items.ImportedBottleOfPerfume, ibop } };
                    var items10Taxes = new Dictionary<Items, double>() { { Items.MusicCD, mcd }, { Items.BottleOfPerfume, bop } };
                    var items5Taxes = new Dictionary<Items, double>() { { Items.ImportedBoxOfChocolates, iboc } };
                    var items0Taxes = new Dictionary<Items, double>() { { Items.Book, book }, { Items.ChocaleteBar, cb }, { Items.PacketOfHeadachePills, pohp } };

                    SalesTaxes salesTaxes = new SalesTaxes();
                    // #1 { Items.Book, Items.Book, Items.MusicCD, Items.ChocaleteBar };
                    // #2 { Items.ImportedBoxOfChocolates, Items.ImportedBottleOfPerfume};
                    // #3 { Items.ImportedBottleOfPerfume, Items.BottleOfPerfume, Items.PacketOfHeadachePills, Items.ImportedBoxOfChocolates, Items.ImportedBoxOfChocolates };
                    var resultST = salesTaxes.PrintTicket(items2pay, items15Taxes, items10Taxes, items5Taxes, items0Taxes);
                    System.Console.WriteLine(resultST);

                    #endregion
                    break;
                case "3":
                    #region Trains
                    System.Console.WriteLine("Writre all routes separate by comma(,) example:AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7 ");
                    var routes = System.Console.ReadLine();
                    Trains trains = new Trains();
                    //var data = trains.SetInputs("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
                    var data = trains.SetInputs(routes);
                    var resultTrain = trains.Caculate(data);
                    System.Console.WriteLine(resultTrain);
                    #endregion
                    break;
                default:
                    System.Console.WriteLine("Invalid option");
                    break;
            }
            



            

            


            System.Console.ReadLine();
        }
    }
}
