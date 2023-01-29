using System;
using System.Collections.Generic;
using Amplitude;
using Amplitude.Framework.Simulation;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Data.Simulation;
using Amplitude.Mercury.Sandbox;
using HumankindModTool;

namespace shakee.Humankind.FameByScoring
{
    public class ScoringRound
    {
        public static float rankSteps = 0.10f;
        public static float catchupStep = 0.05f;
        public static float eraLevelStep = 0.05f;
        public static int debuglevel = 0; // 0 = none, 1 = low, 2 = some details, 3 = all details
                
        static string[,] arrState = new string[,]{
        {"CityCount","3","Expansionist"},
        {"TerritoryCount","1","Expansionist"},
        {"NumberOfEnactedCivic","1","Cultural"},
        {"ResearchNet","0.05","Sciencist"},
        {"NumberOfCulturallyControlledTerritory","1","Cultural"},
        };
        static string[,] arrEconomy = new string[,]{
        {"MoneyNet","0.01","Merchant"},
        //{"MoneyStock","0.02","Merchant"},
        {"InfluenceNet","0.01","Cultural"},
        //{"InfluenceStock","0.02","Cultural"},
        {"SumOfLuxuryResourceAccessCount","2","Merchant"},
        
        };
        static string[,] arrMilitary = new string[,]{
        {"SumOfUnits","0.2","Warmonger"},
        {"CapturedCityCount","3","Expansionist"},
        {"SumOfStrategicResourceAccessCount","1","Warmonger"},
        {"BattlesFought","0.2","Warmonger"},
        {"BattlesWon","0.7","Warmonger"},
        {"KilledUnits","0.05","Warmonger"},        
        // {"TerritoryCount","1","Expansionist"},
        };
        static string[,] arrCity = new string[,]{
        {"DistrictCount","0.25","Builder"},
        {"WonderCulturalClaimedCount","4","Cultural"},
        {"SumOfPopulation","0.10","Farmer"},
        {"HolySite", "3", "Builder"},
        {"CulturalWonder", "5", "Builder"},
        {"LandTradeRoadsCount", "1", "Merchant"},
        {"NavalTradeRoadsCount", "0.5", "Merchant"},
        };
        public static List<object> listCat = new List<object>()
        {
            (object)arrState,
            (object)arrEconomy,
            (object)arrMilitary,
            (object)arrCity,
        };

        public static void RoundScoring (bool scoring, int turn = -1, int empireIndex = -1, bool endGame = false) //bool scoring is for checking if round scoring or era change scoring
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
            
            if (calc >= numEmpires / 2 || !scoring)
            {
                if (scoring)
                {
                    foreach (MajorEmpire item in listEmpires)                
                    {                  
                        FameHistory var2 = CreateHistory(item);
                        var2.turn = turn;
                    }
                }
                else if (!scoring)
                {
                    majorSave = MajorEmpireSaveExtension.GetExtension(empireIndex);
                    majorSave.lastFameRankEraChange = 0;
                } 

                FameHistory var;

                for (int i = 0; i < listCat.Count; i++)
                {
                    FixedPoint[,] arrRank = new FixedPoint[numEmpires,3]; //neues Array pro Category
                    for (int j = 0; j < numEmpires; j++)
                    {
                        arrRank[j,0] = j; //Initialize arrRank mit Empire Index
                    }
                    string[,] arrtmp = (string[,])listCat[i];
                    FetchStuffRound(numEmpires, arrtmp, ref arrRank, ranking);
                    if (scoring) // Round Scoring
                    {                        
                        DistributeFameRound(numEmpires, calc, ref arrRank, ref arrFame, ranking, endGame);
                        FillCategoryHistory (numEmpires, arrRank, i);
                    }
                    else // Era Change Scoring
                    {
                        DistributeFameRound(numEmpires, calc, ref arrRank, ref arrFame, ranking, empireIndex);
                        //FillCategoryHistory (numEmpires, arrRank, i);
                    }
                }                
                if (!scoring)
                {
                    majorSave = MajorEmpireSaveExtension.GetExtension(empireIndex);
                    empire = Sandbox.MajorEmpires[empireIndex];
                    majorSave.lastFameScoreEraChange = empire.FameScore.Value;
                    FixedPoint[,] tmpArr = arrFame.OrderByDescending(n => n[2]);  
                    for (int j = 0; j < numEmpires; j++)
                    {
                        if (empireIndex == tmpArr[j,0])
                        {
                            majorSave.lastFameRankEraChange /= listCat.Count;
                            majorSave.lastFameGainEraChange = tmpArr[j,2];
                            break;
                        }
                    }
                }                
            
                if (scoring)
                {
                    runDebug("Total Fame Gains for Turn " + turn.ToString(),1);
                    for (int k = 0; k < numEmpires; k++)                
                    {
                        empire = Sandbox.MajorEmpires[k];
                        var = GetHistory(empire);
                        var.fame = arrFame[k,2];
                        
                        runDebug("Empire: "+ k + " (" + arrFame[k,1] + " Fame) +" + arrFame[k,2],2);
                        runDebug("Cat1: " + var.categoryRank[0] + " ; Cat2: "+ var.categoryRank[1] + " ; Cat3: "+ var.categoryRank[2] + " ; Cat4: " + var.categoryRank[3],4);
                    }                
                    EmpireInfo empireInfo = R.Utils_GameUtils().GetCurrentEmpireInfo();
                    
                    empire = Sandbox.MajorEmpires[Convert.ToInt32(empireInfo.EmpireIndex)];
                    majorSave = MajorEmpireSaveExtension.GetExtension(empire.Index());
                    runDebug("Last 3 Fame Values of Empire 0 - List Length: " + majorSave.FameHistoryList.Count.ToString(),4);
                                    
                    if (majorSave.FameHistoryList.Count == 0)
                    {
                        runDebug("No Fame History yet... aborting",4);                        
                    }
                    if (majorSave.FameHistoryList.Count >= 1)
                    {
                        var = GetHistory(empire, majorSave.FameHistoryList.Count - 1);
                        runDebug("Latest Fame: " + var.fame.ToString() + " Turn: " + var.turn.ToString() + " | Cat1: " + var.categoryRank[0] + "; Cat2: "+ var.categoryRank[1] + "; Cat3: "+ var.categoryRank[2] + "; Cat4: " + var.categoryRank[3],4);
                    }
                    if (majorSave.FameHistoryList.Count >= 2)
                    {
                        var = GetHistory(empire, majorSave.FameHistoryList.Count - 2);
                        runDebug("Turn -1 Fame: " + var.fame.ToString() + " Turn: " + var.turn.ToString() + " | Cat1: " + var.categoryRank[0] + "; Cat2: "+ var.categoryRank[1] + "; Cat3: "+ var.categoryRank[2] + "; Cat4: " + var.categoryRank[3],4);
                    }
                    if (majorSave.FameHistoryList.Count >= 3)
                    {
                        var = GetHistory(empire, majorSave.FameHistoryList.Count - 3);
                        runDebug("Turn -2 Fame: " + var.fame.ToString() + " Turn: " + var.turn.ToString() + " | Cat1: " + var.categoryRank[0] + "; Cat2: "+ var.categoryRank[1] + "; Cat3: "+ var.categoryRank[2] + "; Cat4: " + var.categoryRank[3],4);
                    } 
                }
                runDebug("**** Scoring Round End ****", 1);               
            }
            else
            {
                runDebug("No Scoring - " + calc.ToString() + " of required " + (numEmpires / 2).ToString() + " Empires. Total Empires: " + numEmpires.ToString(),1);
            }
            
        } 
        public static void FillCategoryHistory (int numEmpires, FixedPoint[,] arrRank, int category)
        {
            FixedPoint[,] tmpArr = arrRank.OrderByDescending(n => n[1]);  
            MajorEmpire empire;
            MajorEmpireExtension majorSave;
            for (int j = 0; j < numEmpires; j++) // for each Empire
            {
                FameHistory var;
                empire = Sandbox.MajorEmpires[j];
                majorSave = MajorEmpireSaveExtension.GetExtension(empire.Index());
                var = GetHistory(empire);
                                      
                for (int u = 0; u < numEmpires; u++) //check rank position
                {
                    runDebug("Debug Empire Rank: " + tmpArr[u,0] + " with Value: " + tmpArr[u,1] + " | args: j = " + j + "; u = " + u, 4);
                    if (j == (int)tmpArr[u,0]) // j = empire index; u = rank index
                    {
                        var.categoryRank[category] = u;
                        runDebug("Debug Category Ranking: " + var.categoryRank[category],4);
                        if (category == 3)
                        {
                            var.totalRank = (int)Math.Round((double)(var.categoryRank[0] + var.categoryRank[1] + var.categoryRank[2] + var.categoryRank[3]) / 4);
                        }
                        break;                                                            
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
            int numRanks = (int)Math.Ceiling((float)arrRank.GetLength(0) / 2);
            for (int i = 0; i < arrRank.GetLength(0); i++) // empire loop
            {
                for (int j = 0; j < tmpArr.GetLength(0); j++) // rank loop
                {
                    if (arrRank[i,0] == tmpArr[j,0]) 
                    {
                        arrRank[i,1] += (1 + (Math.Max((numRanks - j),0) * 2));
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

                float weight = float.Parse(arrProperty[u,1]);
                string affinity = arrProperty[u,2];


                for (int i = 0; i < numEmpires; i++)
                {   
                    MajorEmpire empire = Sandbox.MajorEmpires[i];
                    FactionDefinition empireFaction = R.Utils_GameUtils().GetFactionDefinition(i);
                    float catchup = 0f;
                    float x = 0f;
                    FixedPoint vEraLevel = empire.GetPropertyValue("EraLevel");  
                    if (vEraLevel < 1)
                    {
                        continue;
                    } 
                    if (arrProperty[u,0] == "MoneyNet" || arrProperty[u,0] == "MoneyStock" || arrProperty[u,0] == "InfluenceNet" || arrProperty[u,0] == "InfluenceStock")
                    {
                        if ((FixedPoint)empire.GetPropertyValue(arrProperty[u,0]) < 0)
                        {
                            continue;
                        }
                    }
                    for (int maj = 0; maj < numEmpires; maj++)
                    {
                        if (vEraLevel < (FixedPoint)Sandbox.MajorEmpires[maj].GetPropertyValue("EraLevel"))
                        {
                            catchup += catchupStep;
                        }
                    }

                    ReferenceCollection<Settlement> empSettlements;
                    int x1;
                    int x2;
                    //int x3;
                    empSettlements = empire.Settlements();                    
                    switch (arrProperty[u,0])
                    {
                        case "DistrictCount":                            
                            for (x1 = 0; x1 < empSettlements.Count; x1++)
                            {
                                x += (float)(empSettlements[x1].ExtensionDistrictsCount.Value);
                            }     
                            break;
                        case "LandTradeRoadsCount":                            
                            for (x1 = 0; x1 < empSettlements.Count; x1++)
                            {
                                x += (float)(empSettlements[x1].LandTradeRoadsCount.Value);
                            }     
                            break;
                        case "NavalTradeRoadsCount":                            
                            for (x1 = 0; x1 < empSettlements.Count; x1++)
                            {
                                x += (float)(empSettlements[x1].NavalTradeRoadsCount.Value);
                            }     
                            break;
                        case "BattlesFought":                            
                            x += (float)MajorEmpireSaveExtension.GetExtension(empire.Index()).BattlesFought;                               
                            break;
                        case "BattlesWon":                            
                            x += (float)MajorEmpireSaveExtension.GetExtension(empire.Index()).BattlesWon;                            
                            break;
                        case "KilledUnits":                            
                            x += (float)MajorEmpireSaveExtension.GetExtension(empire.Index()).killedUnits;                            
                            break;
                        case "HolySite":
                            for (x1 = 0; x1 < empSettlements.Count; x1++)
                            {
                                for (x2 = 0; x2 < (empSettlements[x1].Districts().Count); x2++)
                                {
                                    if (empSettlements[x1].Districts()[x2].ContainsDescriptor(new StaticString("Effect_Extension_HolySite")))                                    
                                    {x += 1;}
                                }
                            }     
                            break;
                        case "CulturalWonder":
                            for (x1 = 0; x1 < empSettlements.Count; x1++)
                            {
                                for (x2 = 0; x2 < (empSettlements[x1].Districts().Count); x2++)
                                {
                                    if (empSettlements[x1].Districts()[x2].ContainsDescriptor(new StaticString("Effect_ArtificialWonder_Default")))                                    
                                    {x += 1;}
                                }
                            }     
                            break;                            
                        default:
                            x = (float)(empire.GetPropertyValue(arrProperty[u,0]));
                            break;
                    } 

                    x *= weight;       

                    if (affinity == empireFaction.GameplayOrientation.ToString())
                    {
                        x *= (1 + (float)(vEraLevel - 1) * eraLevelStep + 0.25f);
                    }
                    else
                    {
                        x *= (1 + (float)(vEraLevel - 1) * eraLevelStep);
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
        public static void DistributeFameRound (int numEmpires, int calc, ref FixedPoint[,] arrRank, ref FixedPoint[,] arrFame, int ranking, bool endGame = false)
        {
            runDebug("Running Fame Distribution | Scoringtype: " + ranking.ToString(),1);
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
                if (vEraLevel == 0)
                    continue;
                    
                float catchup = 0f;
                for (int k = 0; k < numEmpires; k++)
                {
                    if (vEraLevel < (FixedPoint)Sandbox.MajorEmpires[k].GetPropertyValue("EraLevel"))
                    {
                        catchup += catchupStep;
                    }
                }
                //runDebug("Assigned Values",2);
                int numRanks = (int)Math.Ceiling((float)numEmpires / 2);
                fameGain = baseFame * fameMulti;
                if (ranking >= 2)
                {
                    for (int j = 0; j < numRanks; j++)
                    {
                        if ((int)tmpArr[j,0] == i)
                        {
                            fameGain *= (1 + (Math.Max((numRanks - j),0) * rankSteps));
                            runDebug("Found Rank: " + (j + 1) + " Multiplier: " + (1 + (Math.Max((numRanks - j),0) * rankSteps)) + " Fame: " + fameGain, 2);
                            break;
                        }                            
                    }
                }
                else
                {
                    fameGain = fameCalc((FixedPoint)ratioCalc(arrRank[i,1],sum),calc * baseFame);
                }
                //fameGain *= fameMulti;                
                fameGain *= (1 + vBonusFame + catchup);
                if (endGame == true)
                    fameGain *= 5;
                empire.SetEditablePropertyValue("FameScore",fameGain + oldFame);
                arrFame[i,2] += fameGain;
                runDebug("ScoringRound - Empire: " + i.ToString() + " | Famegain: +" + fameGain.ToString(),2);                         
            }
        }
        //Era Change Method
        public static void DistributeFameRound (int numEmpires, int calc, ref FixedPoint[,] arrRank, ref FixedPoint[,] arrFame, int ranking, int empireIndex)
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

            runDebug("Famegain for EraChange",2);
            MajorEmpire empire = Amplitude.Mercury.Sandbox.Sandbox.MajorEmpires[empireIndex];
            FactionDefinition empireFaction = R.Utils_GameUtils().GetFactionDefinition(empireIndex);
            string empireName = empireFaction.name;
            
            FixedPoint fameGain;
            FixedPoint oldFame = empire.GetPropertyValue("FameScore");
            FixedPoint vEraLevel = empire.GetPropertyValue("EraLevel");
            FixedPoint vBonusFame = empire.GetPropertyValue("FameGainBonus");
            float catchup = 0f;
            for (int i = 0; i < numEmpires; i++)
            {
                if (vEraLevel < (FixedPoint)Sandbox.MajorEmpires[i].GetPropertyValue("EraLevel"))
                {
                    catchup += catchupStep;
                }
            }
            int numRanks = (int)Math.Ceiling((float)numEmpires / 2);
            fameGain = baseFame * fameMulti;
            if (ranking >= 2)
            {
                for (int j = 0; j < numRanks; j++)
                {                    
                    if ((int)tmpArr[j,0] == empireIndex)
                    {
                        fameGain *= (1 + (Math.Max((numRanks - j),0) * rankSteps));
                        runDebug("Found Rank: " + (j + 1) + " Multiplier: " + (1 + (Math.Max((numRanks - j),0) * rankSteps)) + " Fame: " + fameGain, 2);
                        break;
                    }                            
                }
            }
            else
            {
                fameGain = fameCalc((FixedPoint)ratioCalc(arrRank[empireIndex,1],sum),calc * baseFame);   
            }
            //fameGain *= fameMulti;                
            fameGain *= (1 + vBonusFame + catchup);
            empire.SetEditablePropertyValue("FameScore",fameGain + oldFame);
            arrFame[empireIndex,2] += fameGain;
            for (int i = 0; i < numEmpires; i++)
            {
                if(empireIndex == tmpArr[i,0])
                {
                    MajorEmpireExtension majorSave = MajorEmpireSaveExtension.GetExtension(empireIndex);
                    majorSave.lastFameRankEraChange += i;
                }
            }
            runDebug("EraChange - Empire: " + empireIndex.ToString() + " | Famegain: +" + fameGain.ToString(),2);    
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
