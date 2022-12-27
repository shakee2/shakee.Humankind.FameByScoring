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
        static FixedPoint famePoints;
        static int debuglevel = 1; // 0 = none, 1 = medium, 2 = incl. details
        
        static string[,] arrState = new string[6,3]{
        {"CityCount","3","Expansionist"},
        {"TerritoryCount","1","Expansionist"},
        {"SumOfPopulation","0.25","Farmer"},
        {"NumberOfEnactedCivic","1","Cultural"},
        {"ResearchNet","0.05","Sciencist"},
        {"NumberOfCulturallyControlledTerritory","1","Cultural"},
        };
        static string[,] arrEconomy = new string[5,3]{
        {"MoneyNet","0.10","Merchant"},
        {"MoneyStock","0.02","Merchant"},
        {"InfluenceNet","0.10","Cultural"},
        {"InfluenceStock","0.02","Cultural"},
        {"SumOfLuxuryResourceAccessCount","1","Merchant"},
        };
        static string[,] arrMilitary = new string[4,3]{
        {"SumOfUnits","0.2","Warmonger"},
        {"CapturedCityCount","3","Warmonger"},
        {"SumOfStrategicResourceAccessCount","1","Warmonger"},
        {"TerritoryCount","1","Expansionist"},
        };
        static string[,] arrBuilding = new string[2,3]{

        {"DistrictCount","0.25","Builder"},
        {"WonderCulturalClaimedCount","4","Builder"},
        };


        public static void RoundScoring (int turn = -1, int empireIndex = -1)
        {

            int calc = 0;
            int numEmpires = Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires;
            for (int i = 0; i < numEmpires; i++)
            {
                MajorEmpire empire = Amplitude.Mercury.Sandbox.Sandbox.Empires[i] as MajorEmpire;
                if (empire.GetPropertyValue("EraLevel") >= 1)
                {
                    calc += 1;
                }
            }
            if (calc >= numEmpires / 2 || empireIndex > -1)
            {
                FixedPoint[,] arr;
                var listCat = new List<object>();
                listCat.Add((object)arrState);
                listCat.Add((object)arrEconomy);
                listCat.Add((object)arrMilitary);
                listCat.Add((object)arrBuilding);
                FixedPoint[,] arrFame = new FixedPoint[10,3];
                for (int i = 0; i < 4; i++)
                {
                    string[,] arrtmp = (string[,])listCat[i];
                    arr = FetchStuff(numEmpires, arrtmp);
                    //object arr2;
                    DistributeFame(numEmpires,empireIndex,calc,turn,arr, out FixedPoint[,] arr2);

                    for (int k = 0; k <= arr.GetUpperBound(0); k++)
                    {
                        arrFame[k,2] += arr2[k,2];
                    }
                    
                    
                }
                Console.WriteLine("Total Fame Gains");
                for (int k = 0; k < numEmpires; k++)
                {
                    Console.WriteLine("Empire: {0} | Fame +{1}",k,arrFame[k,2]);
                }
            }
            else
            {
                Console.WriteLine("No Scoring - " + calc.ToString() + " of required " + (numEmpires / 2).ToString() + " Empires. Total Empires: " + numEmpires.ToString());
            }
        } 

        public static void DistributeFame(int numEmpires, int empireIndex, int calc, int turn, FixedPoint[,] arr, out FixedPoint[,] arr2, bool ranking = true)
        {
            //arr2 = null;
            StringBuilder debugLine = new StringBuilder();
            runDebug("Distributing Fame on Turn " + turn,1,ref debugLine);
            if(GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "false"))
            {
                ranking = false;
            }
            //debugLine.AppendLine("Used Fame Score Method: " + GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "false").ToString() + " | Ranking = " + ranking.ToString());
            FixedPoint[,] tmpArr = arr.OrderByDescending(n => n[1]);
            FixedPoint sum = 0;
            runDebug("EmpireRanking:",2,ref debugLine);
            for (int k = 0; k <= tmpArr.GetUpperBound(0); k++)
            {
                sum += tmpArr[k,1];
                runDebug("Empire " + tmpArr[k,0] + " with Value: " + tmpArr[k,1], 2, ref debugLine);
                //Console.WriteLine("Empire After {0} | Value: {1}", tmpArr[k,0], tmpArr[k,1]);
            }

            // EraChange Scoring
            if (empireIndex != -1)
            {
                famePoints = 20 + calc * 20;
                Empire empire = Amplitude.Mercury.Sandbox.Sandbox.Empires[empireIndex];
                MajorEmpire MajEmpire = empire as MajorEmpire;
                FactionDefinition empireFaction = R.Utils_GameUtils().GetFactionDefinition(empireIndex);
                string empireName = empireFaction.name;
                runDebug("Empire " + empireIndex + " gains fame due to Era Change", 1, ref debugLine);
                //Console.WriteLine("Empire {1} gains fame due to Era Change - Index: {0}", empireIndex, empireFaction.Name.ToString());           
                FixedPoint fameRatio = 0;
                FixedPoint oldFame = empire.GetPropertyValue("FameScore");
                FixedPoint vEraLevel = empire.GetPropertyValue("EraLevel");
                FixedPoint vBonusFame = empire.GetPropertyValue("FameGainBonus");
                if (ranking)
                {
                    for (int i = 0; i <= tmpArr.GetUpperBound(0); i++)
                    {
                        if ((int)tmpArr[0,0] == empireIndex)
                        {
                            fameRatio = 120;
                            
                        }
                        else if ((int)tmpArr[1,0] == empireIndex)
                        {
                            fameRatio = 110;
                            
                        }
                        else if ((int)tmpArr[2,0] == empireIndex)
                        {
                            fameRatio = 100;
                            
                        }
                        else
                        {
                            fameRatio = 95;
                            
                        }
                    }
                    runDebug("Empire " + empireName + " gains +" + fameRatio + " Fame due to Ranking.",1, ref debugLine);
                }
                else
                {
                fameRatio = fameCalc((FixedPoint)ratioCalc(arr[empireIndex,1],sum),famePoints);
                runDebug("Empire " + empireName + " | StateValue " + arr[empireIndex,1] + " / " + sum + " = " + ratioCalc(arr[empireIndex,1],sum) * 100 + " | Famegain +" + fameRatio + " = " + (oldFame + fameRatio) + " | FameGain Bonus " + vBonusFame.ToString(), 1, ref debugLine);
                //Console.WriteLine("Empireindex: {0} | StateValues {1} / {2} = {3}% | Fame(" + oldFame.ToString() + "): +{4} of {5} | FameGain Bonus " + vBonusFame.ToString(),empireIndex, arr[empireIndex,1], sum, ratioCalc(arr[empireIndex,1],sum) * 100,fameRatio,famePoints);
                }
                MajEmpire.SetEditablePropertyValue("FameScore",fameRatio * (1 + vBonusFame) + oldFame);
                arr[empireIndex,2] += fameRatio;
                
            }   
            // Turn Scoring
            else
            {
                
                famePoints = 20 + calc * 20;
                //Console.WriteLine(debugLine);

                for (int i = 0; i < numEmpires; i++)
                {
                    MajorEmpire empire = Sandbox.MajorEmpires[i];
                    FactionDefinition empireFaction = R.Utils_GameUtils().GetFactionDefinition(i);
                    string empireName = empireFaction.name;
                    FixedPoint fameRatio = 0;
                    FixedPoint oldFame = empire.GetPropertyValue("FameScore");
                    FixedPoint vEraLevel = empire.GetPropertyValue("EraLevel");
                    FixedPoint vBonusFame = empire.GetPropertyValue("FameGainBonus");
                    if (ranking && vEraLevel > 0)
                    {
                        for (int j = 0; j <= tmpArr.GetUpperBound(0); j++)
                        {
                            if ((int)tmpArr[0,0] == i)
                            {
                                fameRatio = 120;
                                
                            }
                            else if ((int)tmpArr[1,0] == i)
                            {
                                fameRatio = 110;
                                
                            }
                            else if ((int)tmpArr[2,0] == i)
                            {
                                fameRatio = 100;
                                
                            }
                            else
                            {
                                fameRatio = 95;
                                
                            }
                        }
                        runDebug("Empire " + empireName + " gains +" + fameRatio + " Fame due to Ranking.",1, ref debugLine);
                    }
                    else
                    {
                        fameRatio = fameCalc((FixedPoint)ratioCalc(arr[i,1],sum),famePoints);                
                        runDebug("Empire " + empireName + " | StateValue " + arr[i,1] + " / " + sum + " = " + ratioCalc(arr[i,1],sum) * 100 + " | Famegain +" + fameRatio + " = " + (oldFame + fameRatio) + " | FameGain Bonus " + vBonusFame.ToString(), 1, ref debugLine);
                    }
                   //Console.WriteLine("Empireindex: {0} | StateValues {1} / {2} = {3}% | Fame(" + oldFame.ToString() + "): +{4} of {5} | FameGain Bonus " + vBonusFame.ToString(),empireName, arr[i,1], sum, ratioCalc(arr[i,1],sum) * 100,fameRatio,famePoints);
                    empire.SetEditablePropertyValue("FameScore",fameRatio * (1 + vBonusFame) + oldFame);
                    arr[i,2] += fameRatio;
                }
                
            }
            arr2 = arr;
            Console.Write(debugLine);
            debugLine.Clear();
        }

        public static FixedPoint[,] FetchStuff (int numEmpires, string[,] arrProperty, object array = null)
        {
            StringBuilder debugLine = new StringBuilder();
            FixedPoint[,] arr = new FixedPoint[10,3]{{0,0,0},{1,0,0},{2,0,0},{3,0,0},{4,0,0},{5,0,0},{6,0,0},{7,0,0},{8,0,0},{9,0,0}};

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
                    for (int u = 0; u <= arrProperty.GetUpperBound(0);u++)
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
                        runDebug("Empire " + i + " | Property " + arrProperty[u,0] + "; Value: " + empire2.GetPropertyValue(arrProperty[u,0].ToString()) + " * " + multi + " = " + empire2.GetPropertyValue(arrProperty[u,0]) * multi, 2, ref debugLine);
                        //Console.WriteLine("Debug: Empire {0} - Era {4}; Property: {1}; Value: {2} * {3} = {5}",i,arrProperty[u,0],(empire2.GetPropertyValue(arrProperty[u,0].ToString())),multi,vEraLevel,empire2.GetPropertyValue(arrProperty[u,0]) * multi);
                        
                    }
                    runDebug("Debug StateValue: " + x,2,ref debugLine);
                    if (vEraLevel < 1)
                    {
                        x *= 0;
                    }
                    else
                    {
                        x *= (1 + (float)(vEraLevel - 1) * 0.05f + catchup);
                    }
                    runDebug("Debug StateValue #2: " + x,2,ref debugLine);
                    arr[i,1] = (FixedPoint)x;               
                }
                Console.Write(debugLine);
                debugLine.Clear();
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
        public static void runDebug(string value, int debugCheck, ref StringBuilder debugLine)
        {
            if (debuglevel >= debugCheck)
            {
                debugLine.AppendLine(value);
            }
        }
    }
}
