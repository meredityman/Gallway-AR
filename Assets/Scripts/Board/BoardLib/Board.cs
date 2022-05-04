using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BoardLib
{

    [Serializable]
    public struct BoardProperties
    {
        public string name;
        public Vector2 boardSize;
        public Vector2 cardSize;
        public float siteAttachDistance;

        public BoardProperties(string name, Vector2 boardSize, Vector2 cardSize, float siteAttachDistance)
        {
            this.name = name;
            this.boardSize = boardSize;
            this.cardSize  = cardSize;
            this.siteAttachDistance = siteAttachDistance;
        }
    }

    [Serializable]
    public struct Zone
    {
        static int number;
        public int index;
        public string name;

        public Color c_docked;
        public Color c_notDocked;

        public Zone(string name, Color c_docked, Color c_notDocked)
        {
            index = number++;
            this.name = name;
            this.c_docked = c_docked;
            this.c_notDocked = c_notDocked;
        }
    }

    [Serializable]
    struct DevelopmentType
    {
        public string name;
        public Color c_docked;
        public Color c_notDocked;

        public DevelopmentType(string name, Color c_docked, Color c_notDocked)
        {
            this.name = name;
            this.c_docked = c_docked;
            this.c_notDocked = c_notDocked;
        }
    }

    [Serializable]
    struct DevelopmentImpact
    {
        public string name;

        public DevelopmentImpact(string name)
        {
            this.name = name;
        }
    }

    [Serializable]
    public struct Site
    {
        public Vector2 position;
        public string zone;

        public Site(Vector2 position, string zone)
        {
            this.position = position;
            this.zone = zone;
        }
    }

    [Serializable]
    public struct Development
    {
        public string name;
        public string type;
        public float[] scores;

        public Development(string name, string type, float[] scores) {
            this.name = name;
            this.type = type;
            this.scores = scores;
        }
    }

    [Serializable]
    public struct Card
    {
        public string name;
        public string devName;

        public Card(string devName) {
            this.devName = this.name = devName;
        }
    }

    [Serializable]
    class Board
    {
        public BoardProperties Properties;
        public Zone[] DevZones;
        public DevelopmentType[] DevTypes;
        public DevelopmentImpact[] DevImpacts;
        public Development[] DevOptions;
        public Site[] DevSites;
        public Card[] Cards;
        public float siteAttachDistance;

        // // For 6x6 Board
        // public Nullable<int> getClosestSiteIndex(Vector3 position)
        // {
        //     // Out of board
        //     if (Mathf.Abs(position.x) > this.Properties.boardSize.x * 0.5f * (float)1e-3 || Mathf.Abs(position.y) > this.Properties.boardSize.y * 0.5f * (float)1e-3)
        //     {
        //         return null;
        //     }

        //     float smallestDistance = 10000;
        //     int startIndex = 0;
        //     int closestIndex = 0;
            
        //     if (position.x < 0.0f && position.y < 0.0f) { startIndex = 0; }        // BL
        //     else if (position.x < 0.0f && position.y < 0.0f) { startIndex = 3; }   // TL
        //     else if (position.x < 0.0f && position.y < 0.0f) { startIndex = 18; }  // BR
        //     else if (position.x > 0.0f && position.y > 0.0f) { startIndex = 21; }  // TR
       
        //     for( int i = 0; i < 3; i++)
        //     {
        //         for( int j = 0; j < 3; j++) 
        //         {
        //             int index = startIndex + j + i * 3;
        //             float dist = Vector3.Distance(new Vector3this.DevSites[index].position, position);

        //             if (dist < smallestDistance)
        //             {
        //                 closestIndex = index;
        //                 smallestDistance = dist;
        //             }
        //         }
        //     }

        //     if (smallestDistance < this.Properties.siteAttachDistance) {
        //         return closestIndex;
        //     }

        //     return null;
        // }
    }

    class BoardFactory{
        public static Board getDefaultBoard()
        {
            // Board board = new Board();
            Board board = new Board();

            board.Properties = new BoardProperties(
                "Default Board",
                new Vector2(594, 841), // A1
                // new Vector2(74, 74),   // A7
                new Vector2(85, 85),   // A7
                (float)1e-3 * 85 // Site attach distance
            );

            // Rules
            const int N_Zones = 3;
            board.DevZones = new Zone[N_Zones]
            {
                new Zone( "Zone 1", new Color(0.882f, 0.078f, 0.459f), new Color(0.518f, 0f    , 0.243f) ), 
                new Zone( "Zone 2", new Color(0.071f, 0.788f, 0.424f), new Color(0f    , 0.435f, 0.216f) ), 
                new Zone( "Zone 3", new Color(1f    , 0.992f, 0.086f), new Color(0.62f , 0.616f, 0f    ) )
            };

            //    shade 2 = #E11475 = rgb(225, 20,117) = rgba(225, 20,117,1) = rgb0(0.882,0.078,0.459)
            //    shade 4 = #84003E = rgb(132,  0, 62) = rgba(132,  0, 62,1) = rgb0(0.518,0,0.243)

            //    shade 2 = #12C96C = rgb( 18,201,108) = rgba( 18,201,108,1) = rgb0(0.071,0.788,0.424)
            //    shade 4 = #006F37 = rgb(  0,111, 55) = rgba(  0,111, 55,1) = rgb0(0,0.435,0.216)

            //    shade 2 = #FFFD16 = rgb(255,253, 22) = rgba(255,253, 22,1) = rgb0(1,0.992,0.086)
            //    shade 4 = #9E9D00 = rgb(158,157,  0) = rgba(158,157,  0,1) = rgb0(0.62,0.616,0)

            const int N_DevType = 3;
            board.DevTypes = new DevelopmentType[N_DevType]
            {
                // new DevelopmentType( "Housing"       , new Color(0.588f, 0.898f ,0.455f), new Color(0.243f, 0.62f,  0.082f) ),
                new DevelopmentType( "Housing"       , new Color(0.588f, 0.898f ,0.455f), new Color(0.105f, 0.266f,  0.035f) ),
                new DevelopmentType( "Services"      , new Color(0.0f, 0.188f,  0.950f), new Color(0.722f, 0.459f, 0.094f) ),
                new DevelopmentType( "Infrastructure", new Color(0.627f, 0.408f, 0.757f), new Color(0.337f, 0.086f, 0.482f) )
            };

// *** Primary color:

//    shade 0 = #77D64F = rgb(119,214, 79) = rgba(119,214, 79,1) = rgb0(0.467,0.839,0.31)
//    shade 1 = #BAF1A3 = rgb(186,241,163) = rgba(186,241,163,1) = rgb0(0.729,0.945,0.639)
//    shade 2 = #96E574 = rgb(150,229,116) = rgba(150,229,116,1) = rgb0(0.588,0.898,0.455)
//    shade 3 = #5AC12F = rgb( 90,193, 47) = rgba( 90,193, 47,1) = rgb0(0.353,0.757,0.184)
//    shade 4 = #1B4409 = rgb( 27, 68, 9)  = rgba( 27, 68, 9,1) = rgb0(0.105,0.266,0.035)
//    shade 5 = #0030cf = rgb( 0, 48, 207)  = rgba( 0, 48, 207,1) = rgb0(0.0,0.188,0.811)
//    replaced: shade 4 = #3E9E15 = rgb( 62,158, 21) = rgba( 62,158, 21,1) = rgb0(0.243,0.62,0.082)

// *** Secondary color (1):

//    shade 0 = #F9B75C = rgb(249,183, 92) = rgba(249,183, 92,1) = rgb0(0.976,0.718,0.361)
//    shade 1 = #FFDCAD = rgb(255,220,173) = rgba(255,220,173,1) = rgb0(1,0.863,0.678)
//    shade 2 = #FFCA82 = rgb(255,202,130) = rgba(255,202,130,1) = rgb0(0.243,0.62,0.082)
//    shade 3 = #E09936 = rgb(224,153, 54) = rgba(224,153, 54,1) = rgb0(0.878,0.6,0.212)
//    shade 4 = #B87518 = rgb(184,117, 24) = rgba(184,117, 24,1) = rgb0(0.722,0.459,0.094)

// *** Secondary color (2):

//    shade 0 = #8243A6 = rgb(130, 67,166) = rgba(130, 67,166,1) = rgb0(0.51,0.263,0.651)
//    shade 1 = #C49ADD = rgb(196,154,221) = rgba(196,154,221,1) = rgb0(0.769,0.604,0.867)
//    shade 2 = #A068C1 = rgb(160,104,193) = rgba(160,104,193,1) = rgb0(0.627,0.408,0.757)
//    shade 3 = #6E2B96 = rgb(110, 43,150) = rgba(110, 43,150,1) = rgb0(0.431,0.169,0.588)
//    shade 4 = #56167B = rgb( 86, 22,123) = rgba( 86, 22,123,1) = rgb0(0.337,0.086,0.482)



            const int N_DevImpact = 3;
            board.DevImpacts = new DevelopmentImpact[N_DevImpact]
            {
                new DevelopmentImpact( "Commercial"    ),
                new DevelopmentImpact( "Social"        ),
                new DevelopmentImpact( "Environmental" )
            };

            // Cards
            board.DevOptions = new Development[]
            {
            // Housing
                //                                                                                                      Com                Soc                Ind
                //                                                                                                  1    2    3      1   2    3       1    2    3
                new Development( "Exclusive Housing"   , board.DevTypes[0].name, new float[N_Zones * N_DevImpact]{ 15,  50,  -5,    50,  40,  -5,    10,  50,  10   }),
                new Development( "Low Density Housing" , board.DevTypes[0].name, new float[N_Zones * N_DevImpact]{ 15,  15,  -25,    5,  50,  10,   -50,   0,  30   }),
                new Development( "High Density Housing", board.DevTypes[0].name, new float[N_Zones * N_DevImpact]{ 20,   0, -50,   -10,  50,   5,    10,  50,  40   }),
            // Services
                //                                                                                                      Com              Soc              Ind
                //                                                                                                  1    2    3      1   2    3       1    2    3
                new Development( "Commercial"          , board.DevTypes[1].name, new float[N_Zones * N_DevImpact]{ 10,  15, -15,    20,  40, -30,   -20,  10,  -5   }),
                new Development( "Education"           , board.DevTypes[1].name, new float[N_Zones * N_DevImpact]{ 20,  10, -20,    10,  50, -40,    10,   5,  10   }),
                new Development( "Sport"               , board.DevTypes[1].name, new float[N_Zones * N_DevImpact]{ 15,   5, -20,     2,  50, -40,   -10,  15,  10   }),
            // Infrastructure
                //                                                                                                      Com              Soc              Ind
                //                                                                                                  1    2    3      1   2    3       1    2    3
                new Development( "Public Transport "   , board.DevTypes[2].name, new float[N_Zones * N_DevImpact]{ 10,   5, -40,     5,  20, -50,   -20,   5, -10   }),
                new Development( "Energy (Renewable)"  , board.DevTypes[2].name, new float[N_Zones * N_DevImpact]{ 15,   5, -30,     0,  10, -60,    30,  25,   5   }),
                new Development( "Active transport"    , board.DevTypes[2].name, new float[N_Zones * N_DevImpact]{ 20,   5, -20,     0,  10, -70,   -20,   5, -10   })
            };

            // Board

            float xSep = board.Properties.boardSize.x / 6.0f;
            float ySep = board.Properties.boardSize.y / 6.0f;
            ySep = xSep = Mathf.Min(ySep, xSep);

            float xOff = board.Properties.boardSize.x * 0.5f;//board.Properties.cardSize.x * 0.5f;
            float yOff = board.Properties.boardSize.y * 0.5f;//board.Properties.cardSize.y * 0.5f;

            xOff -= (6.0f - 1.0f) * 0.5f * xSep;
            yOff -= (6.0f - 1.0f) * 0.5f * ySep;

            // xOff -= ((6.0f * board.Properties.cardSize.x) + (5.0f * xSep)) * 0.5f;
            // yOff -= ((6.0f * board.Properties.cardSize.y) + (5.0f * ySep)) * 0.5f;


            xSep *= (float)1e-3;
            ySep *= (float)1e-3;

            xOff *= (float)1e-3;
            yOff *= (float)1e-3;


            board.DevSites = new Site[] {
                new Site( new Vector2(0 * xSep + xOff, 0 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(0 * xSep + xOff, 1 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(0 * xSep + xOff, 2 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(0 * xSep + xOff, 3 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(0 * xSep + xOff, 4 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(0 * xSep + xOff, 5 * ySep + yOff), board.DevZones[2].name ),//
                new Site( new Vector2(1 * xSep + xOff, 0 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(1 * xSep + xOff, 1 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(1 * xSep + xOff, 2 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(1 * xSep + xOff, 3 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(1 * xSep + xOff, 4 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(1 * xSep + xOff, 5 * ySep + yOff), board.DevZones[2].name ),//
                new Site( new Vector2(2 * xSep + xOff, 0 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(2 * xSep + xOff, 1 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(2 * xSep + xOff, 2 * ySep + yOff), board.DevZones[0].name ),
                new Site( new Vector2(2 * xSep + xOff, 3 * ySep + yOff), board.DevZones[0].name ),
                new Site( new Vector2(2 * xSep + xOff, 4 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(2 * xSep + xOff, 5 * ySep + yOff), board.DevZones[2].name ),//
                new Site( new Vector2(3 * xSep + xOff, 0 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(3 * xSep + xOff, 1 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(3 * xSep + xOff, 2 * ySep + yOff), board.DevZones[0].name ),
                new Site( new Vector2(3 * xSep + xOff, 3 * ySep + yOff), board.DevZones[0].name ),
                new Site( new Vector2(3 * xSep + xOff, 4 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(3 * xSep + xOff, 5 * ySep + yOff), board.DevZones[2].name ),//
                new Site( new Vector2(4 * xSep + xOff, 0 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(4 * xSep + xOff, 1 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(4 * xSep + xOff, 2 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(4 * xSep + xOff, 3 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(4 * xSep + xOff, 4 * ySep + yOff), board.DevZones[1].name ),
                new Site( new Vector2(4 * xSep + xOff, 5 * ySep + yOff), board.DevZones[2].name ),//
                new Site( new Vector2(5 * xSep + xOff, 0 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(5 * xSep + xOff, 1 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(5 * xSep + xOff, 2 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(5 * xSep + xOff, 3 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(5 * xSep + xOff, 4 * ySep + yOff), board.DevZones[2].name ),
                new Site( new Vector2(5 * xSep + xOff, 5 * ySep + yOff), board.DevZones[2].name )
            };

            board.Cards = new Card[]{
                new Card( "Exclusive Housing"   ),
                new Card( "Exclusive Housing"   ),
                new Card( "Low Density Housing" ),
                new Card( "Low Density Housing" ),
                new Card( "Low Density Housing" ),
                new Card( "Low Density Housing" ),
                new Card( "High Density Housing"),
                new Card( "High Density Housing"),
                new Card( "High Density Housing"),
                new Card( "High Density Housing"),
                new Card( "High Density Housing"),
                new Card( "High Density Housing"),
                new Card( "High Density Housing"),
                new Card( "High Density Housing"),

                new Card( "Commercial"          ),
                new Card( "Commercial"          ),
                new Card( "Commercial"          ),
                new Card( "Commercial"          ),
                new Card( "Commercial"          ),
                new Card( "Commercial"          ),
                new Card( "Education"           ),
                new Card( "Education"           ),
                new Card( "Education"           ),
                new Card( "Sport"               ),
                new Card( "Sport"               ),
                new Card( "Sport"               ),

                new Card( "Public Transport "   ),
                new Card( "Public Transport "   ),
                new Card( "Public Transport "   ),
                new Card( "Public Transport "   ),
                new Card( "Energy (Renewable)"  ),
                new Card( "Energy (Renewable)"  ),
                new Card( "Energy (Renewable)"  ),
                new Card( "Energy (Renewable)"  ),
                new Card( "Active transport"    ),
                new Card( "Active transport"    )
            };

        return board;

        }
    }
    
}
