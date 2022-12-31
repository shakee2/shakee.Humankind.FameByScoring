using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Amplitude;
using Amplitude.Mercury;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Data.Simulation;
using Amplitude.Mercury.Data.World;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Sandbox;
using Amplitude.Mercury.Terrain;
using Amplitude.Mercury.UI.Helpers;
using Amplitude.Framework.Simulation.Collections;
using Amplitude.Framework.Simulation.DataStructures;
using Amplitude.Serialization;
using Amplitude.Framework.Simulation;
using HumankindModTool;

namespace shakee.Humankind.FameByScoring
{
    public class ScoringRound
    {
        
        static int debuglevel = 2; // 0 = none, 1 = medium, 2 = incl. details
        public FixedPoint FameTotalGame;
        
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
        {"CapturedCityCount","3","Warmonger"},
        {"SumOfStrategicResourceAccessCount","1","Warmonger"},
        {"TerritoryCount","1","Expansionist"},
        };
        static string[,] arrCity = new string[,]{
        {"DistrictCount","0.25","Builder"},
        {"WonderCulturalClaimedCount","10","Builder"},
        {"SumOfPopulation","0.10","Farmer"},
        };


        public static void RoundScoring (int turn = -1, int empireIndex = -1)
        {
            turn = R.SandboxManager_Sandbox().Turn();
            
            int calc = 0;   
            int numEmpires = Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires;
            FixedPoint[,] arrFame = new FixedPoint[numEmpires,3];                   

            for (int i = 0; i < numEmpires; i++)
            {
                MajorEmpire empire = Amplitude.Mercury.Sandbox.Sandbox.Empires[i] as MajorEmpire;
                arrFame[i,1] = empire.GetPropertyValue("FameScore");
                arrFame[i,0] = (FixedPoint)i;
                if (empire.GetPropertyValue("EraLevel") >= 1)
                {
                    calc += 1;
                }
            }
            if (calc >= numEmpires / 2 && empireIndex == -1)
            {
                var listCat = new List<object>();
                listCat.Add((object)arrState);
                listCat.Add((object)arrEconomy);
                listCat.Add((object)arrMilitary);
                listCat.Add((object)arrCity);
                
                for (int i = 0; i < listCat.Count; i++)
                {
                    FixedPoint[,] arrRank = new FixedPoint[numEmpires,2];
                    for (int j = 0; j < numEmpires; j++)
                    {
                        arrRank[j,0] = j;
                        runDebug("Initialize arrRank: " + arrRank[i,0].ToString(),2);
                    }

                    string[,] arrtmp = (string[,])listCat[i];
                    FetchStuffRound(numEmpires, arrtmp, ref arrRank);
                    DistributeFameRound(numEmpires, calc, ref arrRank, ref arrFame);                 
                    
                }
                Console.WriteLine("Total Fame Gains for Turn " + turn.ToString());
                for (int k = 0; k < numEmpires; k++)
                {
                    
                    Console.WriteLine("Empire: {0} ({1} Fame) +{2}",k,arrFame[k,1],arrFame[k,2]);
                }

            }
            else
            {
                Console.WriteLine("No Scoring - " + calc.ToString() + " of required " + (numEmpires / 2).ToString() + " Empires. Total Empires: " + numEmpires.ToString());
            }
        } 


        public static void RankingCheckRound (FixedPoint[,] arr, ref FixedPoint[,] arrRank)
        {

            FixedPoint[,] tmpArr = arr.OrderByDescending(n => n[1]);
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                runDebug("Empire " + tmpArr[i,0].ToString() + " with Value " + tmpArr[i,1].ToString(),2);
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

        public static void DistributeFame(int numEmpires, int empireIndex, int calc, int turn, FixedPoint[,] arr, out FixedPoint[,] arr2, bool ranking = true)
        {
            FixedPoint baseFame = Convert.ToInt32(GameOptionHelper.GetGameOption(FameByScoring.FameBaseGain));
            runDebug("Distributing Fame on Turn " + turn,1);
            if(GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "false"))
            {
                ranking = false;
            }
            float fameMulti = Convert.ToSingle(GameOptionHelper.GetGameOption(FameByScoring.FameGainMultiplier));
            Console.WriteLine(fameMulti.ToString());
            
            FixedPoint[,] tmpArr = arr.OrderByDescending(n => n[1]);
            FixedPoint sum = 0;
            runDebug("EmpireRanking:",2);
            for (int k = 0; k < numEmpires; k++)
            {
                sum += tmpArr[k,1];
                runDebug("Empire " + tmpArr[k,0] + " with Value: " + tmpArr[k,1], 2);                
            }
            FixedPoint famePoints = calc * baseFame;

            // EraChange Scoring
            if (empireIndex != -1)
            {
                
                Empire empire = Amplitude.Mercury.Sandbox.Sandbox.Empires[empireIndex];
                MajorEmpire MajEmpire = empire as MajorEmpire;
                FactionDefinition empireFaction = R.Utils_GameUtils().GetFactionDefinition(empireIndex);
                string empireName = empireFaction.name;
                runDebug("Empire " + empireIndex + " gains fame due to Era Change", 1);
                
                FixedPoint fameGain = 0;
                FixedPoint oldFame = empire.GetPropertyValue("FameScore");
                FixedPoint vEraLevel = empire.GetPropertyValue("EraLevel");
                FixedPoint vBonusFame = empire.GetPropertyValue("FameGainBonus");
                if (ranking)
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

                    fameGain *= fameMulti;
                    runDebug("Empire " + empireName + " gains +" + fameGain + " Fame due to Ranking.",1);
                }
                else
                {
                fameGain = fameCalc((FixedPoint)ratioCalc(arr[empireIndex,1],sum),famePoints) * fameMulti;
                runDebug("Empire " + empireName + " | StateValue " + arr[empireIndex,1] + " / " + sum + " = " + ratioCalc(arr[empireIndex,1],sum) * 100 + " | Famegain +" + fameGain + " = " + (oldFame + fameGain) + " | FameGain Bonus " + vBonusFame.ToString(), 1);
                //Console.WriteLine("Empireindex: {0} | StateValues {1} / {2} = {3}% | Fame(" + oldFame.ToString() + "): +{4} of {5} | FameGain Bonus " + vBonusFame.ToString(),empireIndex, arr[empireIndex,1], sum, ratioCalc(arr[empireIndex,1],sum) * 100,fameRatio,famePoints);
                }
                fameGain *= (1 + vBonusFame);
                MajEmpire.SetEditablePropertyValue("FameScore",fameGain + oldFame);
                arr[empireIndex,2] += fameGain;
                
            }   
            // Turn Scoring
            else
            {
                //Console.WriteLine(debugLine);

                for (int i = 0; i < numEmpires; i++)
                {
                    MajorEmpire empire = Sandbox.MajorEmpires[i];
                    FactionDefinition empireFaction = R.Utils_GameUtils().GetFactionDefinition(i);
                    string empireName = empireFaction.name;
                    
                    FixedPoint fameGain = 0;
                    FixedPoint oldFame = empire.GetPropertyValue("FameScore");
                    FixedPoint vEraLevel = empire.GetPropertyValue("EraLevel");
                    FixedPoint vBonusFame = empire.GetPropertyValue("FameGainBonus");
                    if (ranking && vEraLevel > 0)
                    {

                        if ((int)tmpArr[0,0] == i)
                        {
                            fameGain = baseFame * 1.3f;
                        }
                        else if ((int)tmpArr[1,0] == i)
                        {
                            fameGain = baseFame * 1.2f;                            
                        }
                        else if ((int)tmpArr[2,0] == i)
                        {
                            fameGain = baseFame * 1.1f;                            
                        }
                        else
                        {
                            fameGain = baseFame;
                        }

                        fameGain *= fameMulti;
                        runDebug("Empire " + empireName + " gains +" + fameGain + " Fame due to Ranking.",1);
                    }
                    else
                    {
                        fameGain = fameCalc((FixedPoint)ratioCalc(arr[i,1],sum),famePoints) * fameMulti;                
                        runDebug("Empire " + empireName + " | StateValue " + arr[i,1] + " / " + sum + " = " + ratioCalc(arr[i,1],sum) * 100 + " | Famegain +" + fameGain + " = " + (oldFame + fameGain) + " | FameGain Bonus " + vBonusFame.ToString(), 1);
                    }
                    //Console.WriteLine("Empireindex: {0} | StateValues {1} / {2} = {3}% | Fame(" + oldFame.ToString() + "): +{4} of {5} | FameGain Bonus " + vBonusFame.ToString(),empireName, arr[i,1], sum, ratioCalc(arr[i,1],sum) * 100,fameRatio,famePoints);
                    fameGain *= (1 + vBonusFame);
                    empire.SetEditablePropertyValue("FameScore",fameGain + oldFame);
                    arr[i,2] += fameGain;
                }
                
            }
            arr2 = arr;
        }
        public static void FetchStuffRound (int numEmpires, string[,] arrProperty, ref FixedPoint[,] arrRank)
        {
            
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

                }
                runDebug("Empireranking for Property: " + arrProperty[u,0],2);
                RankingCheckRound(arr, ref arrRank);   
            }       

        }



        public static void DistributeFameRound (int numEmpires, int calc, ref FixedPoint[,] arrRank, ref FixedPoint[,] arrFame)
        {
            runDebug("Running Fame Distribution",1);
            FixedPoint baseFame = Convert.ToInt32(GameOptionHelper.GetGameOption(FameByScoring.FameBaseGain));
            float fameMulti = Convert.ToSingle(GameOptionHelper.GetGameOption(FameByScoring.FameGainMultiplier));

            FixedPoint[,] tmpArr = arrRank.OrderByDescending(n => n[1]);
            

            runDebug("EmpireRanking:",2);
            for (int i = 0; i < numEmpires; i++)
            {
                runDebug("Empire " + tmpArr[i,0] + " with Value: " + tmpArr[i,1], 2);
                //Console.WriteLine("Empire After {0} | Value: {1}", tmpArr[k,0], tmpArr[k,1]);
            }

            for (int i = 0; i < numEmpires; i++)
            {
                MajorEmpire empire = Amplitude.Mercury.Sandbox.Sandbox.Empires[i] as MajorEmpire;
                FactionDefinition empireFaction = R.Utils_GameUtils().GetFactionDefinition(i);
                string empireName = empireFaction.name;
                
                FixedPoint fameGain;
                FixedPoint oldFame = empire.GetPropertyValue("FameScore");
                FixedPoint vEraLevel = empire.GetPropertyValue("EraLevel");
                FixedPoint vBonusFame = empire.GetPropertyValue("FameGainBonus");

                if ((int)tmpArr[0,0] == i)
                {
                    fameGain = baseFame * 1.3f;
                }
                else if ((int)tmpArr[1,0] == i)
                {
                    fameGain = baseFame * 1.2f;                        
                }
                else if ((int)tmpArr[2,0] == i)
                {
                    fameGain = baseFame * 1.1f;
                }
                else
                {
                    fameGain = baseFame;
                }

                fameGain *= fameMulti;                
                fameGain *= (1 + vBonusFame);
                empire.SetEditablePropertyValue("FameScore",fameGain + oldFame);
                arrFame[i,2] += fameGain;
                
            }
            

        }

        // get Stuff from single category
        public static FixedPoint[,] FetchStuff (int numEmpires, string[,] arrProperty, object array = null)
        {
            FixedPoint[,] arr = new FixedPoint[numEmpires,3];
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                arr[i,0] = i;            
            }

            for (int i = 0; i < numEmpires; i++)
            {
                    
                MajorEmpire empire2 = Sandbox.MajorEmpires[i];
                EmpireInfo empireInfoNew = R.Utils_GameUtils().GetCurrentEmpireInfo();
                FactionDefinition newempireFaction = R.Utils_GameUtils().GetFactionDefinition(i);
                float x = 0f;
                float catchup = 0f;
                FixedPoint vEraLevel = 0;
                vEraLevel = empire2.GetPropertyValue("EraLevel");
                for (int maj = 0; maj < numEmpires; maj++)
                {
                    if (vEraLevel < (FixedPoint)Sandbox.MajorEmpires[maj].GetPropertyValue("EraLevel"))
                    {
                        catchup += 0.1f;
                    }
                }
                for (int u = 0; u < arrProperty.GetLength(0);u++)
                {
                    float multi = float.Parse(arrProperty[u,1]);
                    string affinity = arrProperty[u,2];
                    if (arrProperty[u,0] == "MoneyNet" || arrProperty[u,0] == "MoneyStock" || arrProperty[u,0] == "InfluenceNet" || arrProperty[u,0] == "InfluenceStock")
                    {
                        if ((FixedPoint)empire2.GetPropertyValue(arrProperty[u,0]) < 0)
                        {
                            continue;
                        }
                    }
                    if (affinity == newempireFaction.GameplayOrientation.ToString())
                    {
                        x += (float)(empire2.GetPropertyValue(arrProperty[u,0]) * multi * 1.25f);
                    }
                    else
                    {
                        x += (float)(empire2.GetPropertyValue(arrProperty[u,0]) * multi);
                    }
                    runDebug("Empire " + i + " | Property " + arrProperty[u,0] + "; Value: " + empire2.GetPropertyValue(arrProperty[u,0].ToString()) + " * " + multi + " = " + empire2.GetPropertyValue(arrProperty[u,0]) * multi, 2);
                    //Console.WriteLine("Debug: Empire {0} - Era {4}; Property: {1}; Value: {2} * {3} = {5}",i,arrProperty[u,0],(empire2.GetPropertyValue(arrProperty[u,0].ToString())),multi,vEraLevel,empire2.GetPropertyValue(arrProperty[u,0]) * multi);
                    
                }
                runDebug("Debug StateValue: " + x,2);
                if (vEraLevel < 1)
                {
                    x *= 0;
                }
                else
                {
                    x *= (1 + (float)(vEraLevel - 1) * 0.05f + catchup);
                }
                runDebug("Debug StateValue #2: " + x,2);
                arr[i,1] = (FixedPoint)x;               
            }
            return arr;
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
            if (debuglevel >= debugCheck)
            {
                Console.WriteLine(value);
            }
        }
    }
}
