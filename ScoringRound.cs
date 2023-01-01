using System;
using System.Collections.Generic;
using Amplitude;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Data.Simulation;
using Amplitude.Mercury.Sandbox;
using HumankindModTool;

namespace shakee.Humankind.FameByScoring
{
    public class ScoringRound
    {
        
        public static int debuglevel = 2; // 0 = none, 1 = medium, 2 = incl. details
                
        static string[,] arrState = new string[,]{
        {"CityCount","3","Expansionist"},
        {"TerritoryCount","1","Expansionist"},
        {"NumberOfEnactedCivic","1","Cultural"},
        {"ResearchNet","0.05","Sciencist"},
        {"NumberOfCulturallyControlledTerritory","1","Cultural"},
        };
        static string[,] arrEconomy = new string[,]{
        {"MoneyNet","0.06","Merchant"},
        {"MoneyStock","0.02","Merchant"},
        {"InfluenceNet","0.06","Cultural"},
        {"InfluenceStock","0.02","Cultural"},
        {"SumOfLuxuryResourceAccessCount","2","Merchant"},
        };
        static string[,] arrMilitary = new string[,]{
        {"SumOfUnits","0.2","Warmonger"},
        {"CapturedCityCount","3","Expansionist"},
        {"SumOfStrategicResourceAccessCount","1","Warmonger"},
        {"TerritoryCount","1","Expansionist"},
        };
        static string[,] arrCity = new string[,]{
        {"DistrictCount","0.25","Builder"},
        {"WonderCulturalClaimedCount","10","Builder"},
        {"SumOfPopulation","0.10","Farmer"},
        };


        public static void RoundScoring (bool scoring, int turn = -1, int empireIndex = -1) //bool scoring is for checkking if round scoring or era change scoring
        {
            turn = R.SandboxManager_Sandbox().Turn();
            MajorEmpire empire;
            MajorEmpireExtension majorSave;
            
            int calc = 0;   
            int numEmpires = Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires;
            int ranking = int.Parse(GameOptionHelper.GetGameOption(FameByScoring.FameScoringOption));
            runDebug("Scoring Type: " + ranking.ToString(),2);
            FixedPoint[,] arrFame = new FixedPoint[numEmpires,3]; // 0 = index, 1 = old famescore, 2 = fame gain
            List<MajorEmpire> listEmpires = new List<MajorEmpire>();
            
            for (int i = 0; i < numEmpires; i++)
            {
                empire = Amplitude.Mercury.Sandbox.Sandbox.Empires[i] as MajorEmpire;
                listEmpires.Add(empire);
                arrFame[i,1] = empire.GetPropertyValue("FameScore");
                arrFame[i,0] = (FixedPoint)i;
                if (empire.GetPropertyValue("EraLevel") >= 1)
                {
                    calc += 1;                                        
                }
            }
            if (calc >= numEmpires / 2 || !scoring )
            {
                foreach (MajorEmpire item in listEmpires)                
                {                  
                    FameHistory var2 = CreateHistory(item);
                    var2.turn = turn;
                    majorSave = MajorEmpireSaveExtension.GetExtension(item.Index());
                }
 
                var listCat = new List<object>();
                listCat.Add((object)arrState);
                listCat.Add((object)arrEconomy);
                listCat.Add((object)arrMilitary);
                listCat.Add((object)arrCity);
                FameHistory var;

                for (int i = 0; i < listCat.Count; i++)
                {
                    FixedPoint[,] arrRank = new FixedPoint[numEmpires,2]; //neues Array pro Category
                    for (int j = 0; j < numEmpires; j++)
                    {
                        arrRank[j,0] = j; //Initialize arrRank mit Empire Index
                    }
                    string[,] arrtmp = (string[,])listCat[i];
                    FetchStuffRound(numEmpires, arrtmp, ref arrRank, ranking);
                    if (scoring)
                    {                        
                        DistributeFameRound(numEmpires, calc, ref arrRank, ref arrFame, ranking);
                        FillCategoryHistory (numEmpires, arrRank, i);
                        // for (int j = 0; j < numEmpires; j++)
                        // {
                        //     empire = Sandbox.MajorEmpires[j];
                        //     majorSave = MajorEmpireSaveExtension.GetExtension(empire.Index());
                        //     var = GetHistory(empire);

                        //     FixedPoint[,] tmpArr = arrRank.OrderByDescending(n => n[1]);                        
                        //     for (int u = 0; u < numEmpires; u++) //check rank position
                        //     {
                        //         runDebug("Debug Empire Rank: " + tmpArr[u,0] + " with Value: " + tmpArr[u,1] + " | args: j = " + j + "; u = " + u, 3);
                        //         if (j == (int)tmpArr[u,0]) // j = empire index; u = rank index
                        //         {
                        //             var.categoryRank[i] = majorSave.listRanking[u];
                        //             runDebug("Debug Category Ranking: " + var.categoryRank[i],3);                                        
                        //         }                                    
                        //     }                                
                            
                        // }
                    }
                    else
                    {
                        DistributeFameRound(numEmpires, calc, ref arrRank, ref arrFame, ranking, empireIndex);
                        FillCategoryHistory (numEmpires, arrRank, i);
                        // for (int j = 0; j < numEmpires; j++)
                        // {
                        //     empire = Sandbox.MajorEmpires[j];
                        //     majorSave = MajorEmpireSaveExtension.GetExtension(empire.Index());
                        //     var = GetHistory(empire);

                        //     FixedPoint[,] tmpArr = arrRank.OrderByDescending(n => n[1]);                        
                        //     for (int u = 0; u < numEmpires; u++) //check rank position
                        //     {
                        //         runDebug("Debug Empire Rank: " + tmpArr[u,0] + " with Value: " + tmpArr[u,1] + " | args: j = " + j + "; u = " + u, 3);
                        //         if (j == (int)tmpArr[u,0]) // j = empire index; u = rank index
                        //         {
                        //             var.categoryRank[i] = majorSave.listRanking[u];
                        //             runDebug("Debug Category Ranking: " + var.categoryRank[i],3);                                        
                        //         }                                    
                        //     }                                
                            
                        // }
                    }                 

                }
                
                runDebug("Total Fame Gains for Turn " + turn.ToString(),1);

                for (int k = 0; k < numEmpires; k++)                
                {
                    empire = Sandbox.MajorEmpires[k];
                    var = GetHistory(empire);
                    var.fame = arrFame[k,2];
                    runDebug("Empire: "+ k + " (" + arrFame[k,1] + " Fame) +" + arrFame[k,2],2);
                    runDebug("Cat1: " + var.categoryRank[0] + "; Cat2: "+ var.categoryRank[1] + "; Cat3: "+ var.categoryRank[2] + "; Cat4: " + var.categoryRank[3] + "; Fame: " + var.fame.ToString(),2);
                }
                empire = Sandbox.MajorEmpires[0];
                majorSave = MajorEmpireSaveExtension.GetExtension(empire.Index());
                runDebug("Last 3 Fame Values of Empire 0 - List Length: " + majorSave.FameHistoryList.Count.ToString(),2);
                                
                if (majorSave.FameHistoryList.Count == 0)
                {
                    runDebug("No Fame History yet... aborting",2);                        
                }
                if (majorSave.FameHistoryList.Count >= 1)
                {
                    var = GetHistory(empire, majorSave.FameHistoryList.Count - 1);
                    runDebug("Latest Fame: " + var.fame.ToString() + " Turn: " + var.turn.ToString() + " | Cat1: " + var.categoryRank[0] + "; Cat2: "+ var.categoryRank[1] + "; Cat3: "+ var.categoryRank[2] + "; Cat4: " + var.categoryRank[3],2);
                }
                if (majorSave.FameHistoryList.Count >= 2)
                {
                    var = GetHistory(empire, majorSave.FameHistoryList.Count - 2);
                    runDebug("Turn -1 Fame: " + var.fame.ToString() + " Turn: " + var.turn.ToString() + " | Cat1: " + var.categoryRank[0] + "; Cat2: "+ var.categoryRank[1] + "; Cat3: "+ var.categoryRank[2] + "; Cat4: " + var.categoryRank[3],2);
                }
                if (majorSave.FameHistoryList.Count >= 3)
                {
                    var = GetHistory(empire, majorSave.FameHistoryList.Count - 3);
                    runDebug("Turn -2 Fame: " + var.fame.ToString() + " Turn: " + var.turn.ToString() + " | Cat1: " + var.categoryRank[0] + "; Cat2: "+ var.categoryRank[1] + "; Cat3: "+ var.categoryRank[2] + "; Cat4: " + var.categoryRank[3],2);
                }                
            }
            else
            {
                runDebug("No Scoring - " + calc.ToString() + " of required " + (numEmpires / 2).ToString() + " Empires. Total Empires: " + numEmpires.ToString(),1);
            }
            
        } 
        public static void FillCategoryHistory (int numEmpires, FixedPoint[,] arrRank, int category)
        {
            for (int j = 0; j < numEmpires; j++)
            {
                FameHistory var;
                MajorEmpire empire = Sandbox.MajorEmpires[j];
                var majorSave = MajorEmpireSaveExtension.GetExtension(empire.Index());
                var = GetHistory(empire);

                FixedPoint[,] tmpArr = arrRank.OrderByDescending(n => n[1]);                        
                for (int u = 0; u < numEmpires; u++) //check rank position
                {
                    runDebug("Debug Empire Rank: " + tmpArr[u,0] + " with Value: " + tmpArr[u,1] + " | args: j = " + j + "; u = " + u, 3);
                    if (j == (int)tmpArr[u,0]) // j = empire index; u = rank index
                    {
                        var.categoryRank[category] = majorSave.listRanking[u];
                        runDebug("Debug Category Ranking: " + var.categoryRank[category],3);                                        
                    }                                    
                }                                
                
            }
        }
        
        public static FameHistory GetHistory(MajorEmpire empire)
        {
            var majorSave = MajorEmpireSaveExtension.GetExtension(empire.Index());
            //Console.WriteLine("Get FameHistory Lenght of Empire " + empire.Index().ToString() + " | " + majorSave.FameHistoryList.Count.ToString());
            FameHistory var = majorSave.FameHistoryList[majorSave.FameHistoryList.Count - 1];            
            return var;
        }
        public static FameHistory GetHistory(MajorEmpire empire, int index)
        {
            var majorSave = MajorEmpireSaveExtension.GetExtension(empire.Index());
            FameHistory var = majorSave.FameHistoryList[index];                     
            return var;
        }
        public static FameHistory CreateHistory(MajorEmpire empire)
        {
            var majorSave = MajorEmpireSaveExtension.GetExtension(empire.Index());
            FameHistory var = new FameHistory(empire);
            majorSave.CheckDispose();
            //majorSave.FameHistoryList.Add(var);
            return var;
        }
        public static void RankingCheckRound (FixedPoint[,] arr, ref FixedPoint[,] arrRank)
        {

            FixedPoint[,] tmpArr = arr.OrderByDescending(n => n[1]);
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                runDebug("Empire " + tmpArr[i,0].ToString() + " with Value " + tmpArr[i,1].ToString(),3);
            }

            for (int i = 0; i < arrRank.GetLength(0); i++)
            {
                for (int j = 0; j < tmpArr.GetLength(0); j++)
                {
                    if (arrRank[i,0] == tmpArr[j,0]) 
                    {
                        if (j == 0)
                        {
                            arrRank[i,1] += 7;
                        }
                        else if (j == 1)
                        {
                            arrRank[i,1] += 5;
                        }
                        else if (j == 2)
                        {
                            arrRank[i,1] += 3;
                        }
                        else
                        {
                            arrRank[i,1] += 1;
                        }
                    }
                }            
            }

        }

        // ranking: 1 = ratio, 2 = ranking 1; 3 = ranking 2
        public static void FetchStuffRound (int numEmpires, string[,] arrProperty, ref FixedPoint[,] arrRank, int ranking)
        {
            runDebug("Fetchstuff for Category",2);
            
            for (int u = 0; u < arrProperty.GetLength(0);u++)
            {
                FixedPoint[,] arr = new FixedPoint[numEmpires,2];    
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    arr[i,0] = (FixedPoint)i;        
                }

                float multi = float.Parse(arrProperty[u,1]);
                string affinity = arrProperty[u,2];


                for (int i = 0; i < numEmpires; i++)
                {   
                    MajorEmpire empire = Sandbox.MajorEmpires[i];
                    FactionDefinition empireFaction = R.Utils_GameUtils().GetFactionDefinition(i);
                    float catchup = 0f;
                    float x = 0f;
                    FixedPoint vEraLevel = empire.GetPropertyValue("EraLEvel");   
                    for (int maj = 0; maj < numEmpires; maj++)
                    {
                        if (vEraLevel < (FixedPoint)Sandbox.MajorEmpires[maj].GetPropertyValue("EraLevel"))
                        {
                            catchup += 0.1f;
                        }
                    }
                    if (arrProperty[u,0] == "MoneyNet" || arrProperty[u,0] == "MoneyStock" || arrProperty[u,0] == "InfluenceNet" || arrProperty[u,0] == "InfluenceStock")
                    {
                        if ((FixedPoint)empire.GetPropertyValue(arrProperty[u,0]) < 0)
                        {
                            continue;
                        }
                    }
                    if (affinity == empireFaction.GameplayOrientation.ToString())
                    {
                        x += (float)(empire.GetPropertyValue(arrProperty[u,0]) * multi * 1.25f);
                    }
                    else
                    {
                        x += (float)(empire.GetPropertyValue(arrProperty[u,0]) * multi);
                    }
                    if (vEraLevel < 1)
                    {
                        x *= 0;
                    }
                    else
                    {
                        x *= (1 + (float)(vEraLevel - 1) * 0.05f + catchup);
                    }

                    arr[i,1] = (FixedPoint)x;
                    if (ranking != 3)
                    {                        
                        runDebug("Adding Value " + arr[i,1] + " to Empire in arrRank: " + arrRank[i,0],3);
                        arrRank[i,1] += arr[i,1]; // add raw value
                    }
                    
                }
                
                if (ranking == 3)
                {
                    runDebug("Empireranking for Property: " + arrProperty[u,0],3);
                    RankingCheckRound(arr, ref arrRank); //convert rawvalue to ranking points
                }                

            }       

        }


        // Is done per Category
        public static void DistributeFameRound (int numEmpires, int calc, ref FixedPoint[,] arrRank, ref FixedPoint[,] arrFame, int ranking, int empireIndex = -1)
        {
            runDebug("Running Fame Distribution | Scoringtype: " + ranking.ToString() + " | EmpireIndex: " + empireIndex.ToString(),1);
            FixedPoint baseFame = int.Parse(GameOptionHelper.GetGameOption(FameByScoring.FameBaseGain));
            float fameMulti = float.Parse(GameOptionHelper.GetGameOption(FameByScoring.FameGainMultiplier));
            int turn = R.SandboxManager_Sandbox().Turn();
            FixedPoint[,] tmpArr = arrRank.OrderByDescending(n => n[1]);            
            FixedPoint sum = 0;

            runDebug("EmpireRanking of Category:",2);
            for (int i = 0; i < numEmpires; i++)
            {
                sum += arrRank[i,1];
                runDebug("Empire " + tmpArr[i,0] + " with Value: " + tmpArr[i,1], 2);
                //Console.WriteLine("Empire After {0} | Value: {1}", tmpArr[k,0], tmpArr[k,1]);
            }
            if (empireIndex == -1)
            {
                runDebug("Famegain for Scoring Round", 2);
                for (int i = 0; i < numEmpires; i++)
                {
                    
                    MajorEmpire empire = Amplitude.Mercury.Sandbox.Sandbox.MajorEmpires[i];
                    FactionDefinition empireFaction = R.Utils_GameUtils().GetFactionDefinition(i);
                    string empireName = empireFaction.name;


                    FixedPoint fameGain;
                    FixedPoint oldFame = empire.GetPropertyValue("FameScore");
                    FixedPoint vEraLevel = empire.GetPropertyValue("EraLevel");
                    FixedPoint vBonusFame = empire.GetPropertyValue("FameGainBonus");
                    //runDebug("Assigned Values",2);
                    if (ranking >= 2)
                    {
                        if ((int)tmpArr[0,0] == i)
                        {
                            fameGain = baseFame * 1.75f;
                        }
                        else if ((int)tmpArr[1,0] == i)
                        {
                            fameGain = baseFame * 1.50f;                        
                        }
                        else if ((int)tmpArr[2,0] == i)
                        {
                            fameGain = baseFame * 1.25f;
                        }
                        else
                        {
                            fameGain = baseFame;
                        }
                    }
                    else
                    {
                        fameGain = fameCalc((FixedPoint)ratioCalc(arrRank[i,1],sum),calc * baseFame);
                    }
                    fameGain *= fameMulti;                
                    fameGain *= (1 + vBonusFame);
                    empire.SetEditablePropertyValue("FameScore",fameGain + oldFame);
                    arrFame[i,2] += fameGain;
                    runDebug("ScoringRound - Empire: " + i.ToString() + " | Famegain: +" + fameGain.ToString(),2); 
                         
                }
            }
            else
            {
                runDebug("Famegain for EraChange",2);
                MajorEmpire empire = Amplitude.Mercury.Sandbox.Sandbox.MajorEmpires[empireIndex];
                FactionDefinition empireFaction = R.Utils_GameUtils().GetFactionDefinition(empireIndex);
                string empireName = empireFaction.name;
                
                FixedPoint fameGain;
                FixedPoint oldFame = empire.GetPropertyValue("FameScore");
                FixedPoint vEraLevel = empire.GetPropertyValue("EraLevel");
                FixedPoint vBonusFame = empire.GetPropertyValue("FameGainBonus");
                //runDebug("Assigned Values",2);
                if (ranking >= 2)
                {
                    if ((int)tmpArr[0,0] == empireIndex)
                    {
                        fameGain = baseFame * 1.3f;
                    }
                    else if ((int)tmpArr[1,0] == empireIndex)
                    {
                        fameGain = baseFame * 1.2f;                        
                    }
                    else if ((int)tmpArr[2,0] == empireIndex)
                    {
                        fameGain = baseFame * 1.1f;
                    }
                    else
                    {
                        fameGain = baseFame;
                    }
                }
                else
                {
                    fameGain = fameCalc((FixedPoint)ratioCalc(arrRank[empireIndex,1],sum),calc * baseFame);   
                }
                fameGain *= fameMulti;                
                fameGain *= (1 + vBonusFame);
                empire.SetEditablePropertyValue("FameScore",fameGain + oldFame);
                arrFame[empireIndex,2] += fameGain;
                var majorSave = MajorEmpireSaveExtension.GetExtension(empireIndex); 
                majorSave.lastFameScoreEraChange = empire.FameScore.Value;

                runDebug("EraChange - Empire: " + empireIndex.ToString() + " | Famegain: +" + fameGain.ToString() + " of " + (calc * baseFame).ToString(),2);         
            }
        }

        static FixedPoint fameCalc (FixedPoint x, FixedPoint y)
        {
            float tmpValue = (float)x * (float)y;
            return (FixedPoint)tmpValue;
        }
        static float ratioCalc (FixedPoint x, FixedPoint y)
        {

            if(y == 0)
            {
                return (float)(x / 1);
            }
            else
            {
                return (float)(x / y);
            }
        }
        public static void runDebug(string value, int debugCheck)
        {
            if (ScoringRound.debuglevel >= debugCheck)
            {
                Console.WriteLine(value);
            }
        }
    }
}
